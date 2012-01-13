\ vim: sw=2 ts=2 sta et

\ `JorthVM', a Java VM implemented in Forth
\ 
\ Copyright (C) 2012 Sebastian Rumpl <e0828489@student.tuwien.ac.at>
\ Copyright (C) 2012 Josef Eisl <zapster@zapster.cc>
\ Copyright (C) 2012 Bernhard Urban <lewurm@gmail.com>
\ 
\ This program is free software: you can redistribute it and/or modify
\ it under the terms of the GNU General Public License as published by
\ the Free Software Foundation, either version 3 of the License, or
\ (at your option) any later version.
\ 
\ This program is distributed in the hope that it will be useful,
\ but WITHOUT ANY WARRANTY; without even the implied warranty of
\ MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
\ GNU General Public License for more details.
\ 
\ You should have received a copy of the GNU General Public License
\ along with this program.  If not, see <http://www.gnu.org/licenses/>.
\ this file implements functionality that is needed to read class files
\ this file contains the implementaion used for the creation and management 
\ of jvm stack frames

require class.fs
require classfile.fs

\ ========
\ *! frame
\ *T Method Frame
\ ========

\ *S Method Frame Structure
\ *E    MethodFrame {
\ **        Class class;
\ **        addr_md method;
\ **        MethodFrame dynamic_link;
\ **        addr return_addr;
\ **        addr local_table;
\ **        addr exception_table;
\ **    }

 0 cells constant jvm_frame.class
 1 cells constant jvm_frame.method
 2 cells constant jvm_frame.dynamic_link     \ previous frame pointer
 3 cells constant jvm_frame.return_addr
 4 cells constant jvm_frame.local_table
\ 5 cells constant jvm_frame.code_start      \ offset of method
 5 cells constant jvm_frame.exception_table

 6 cells constant jvm_frame.size()

: jvm_frame.new() { class method dynamic_link return_addr -- addr }
\ *G create a new method frame
  jvm_frame.size() allocate throw
  dup jvm_frame.size() erase

  class over ( jvm_frame.class + ) !
  method over jvm_frame.method + !
  dynamic_link over jvm_frame.dynamic_link + !
  return_addr over jvm_frame.return_addr + !

  \ TODO reserve local memory
  class method jvm_class.getMethodCodeAttr()
  ( addr_code_attr )

  jvm_code_attr_max_locals 
  \ ." max_locals: " dup . CR
  cells dup
  allocate throw
  ( addr_fm size addr_locals )
  dup rot
  ( addr_fm addr_locals addr_locals size )
  erase  
  ( addr_fm addr_locals )
  over jvm_frame.local_table + ! 
  
  \ TODO exception table
;


: jvm_frame.getClass() ( addr_fm -- addr_cl )
\ *G get the class
  jvm_frame.class + @
;

: jvm_frame.getMethod() ( addr_fm -- addr_md )
\ *G get the method
  jvm_frame.method + @
;

: jvm_frame.getDynamicLink() ( addr_fm1 -- addr_fm2 )
\ *G get the dynamic link
  jvm_frame.dynamic_link + @
;

: jvm_frame.getReturnAddr() ( addr_fm -- addr )
\ *G get the retunr address
  jvm_frame.return_addr + @
;

: jvm_frame.getLocal() { addr_fm local -- value }
\ *G get the value of the local
  addr_fm jvm_frame.local_table + @ local cells + @ 
;

: jvm_frame.setLocal() { val addr_fm local -- }
\ *G set the value of the local
  val addr_fm jvm_frame.local_table + @ local cells + !
;

: jvm_frame.numberOfParamters() ( c-addr n -- count )
  over c@ 
  [CHAR] ( <> IF
    abort
  ENDIF
  \ NOTE we do not check for n because we assume that the classfile is 
  \ correct and that there is eventually a ')' in the string
  drop 1+ 
  0 swap
  BEGIN
    ( count c-addr )
    dup c@ dup 
    [CHAR] ) <> 
  WHILE
    ( count c-addr c )
    CASE
      [CHAR] D OF 
        swap 2 + swap 1+
      ENDOF
      [CHAR] J OF
        swap 2 + swap 1+
      ENDOF
      [CHAR] L OF
        swap 1+ swap 
        \ increment addr
        BEGIN 1+ dup c@ [CHAR] ; <> WHILE REPEAT
        1+ \ next char
      ENDOF
      [CHAR] [ OF
        swap 1+ swap
        BEGIN 1+ dup c@ [CHAR] [ = WHILE REPEAT
        dup c@
        CASE
          [CHAR] L OF
            BEGIN 1+ dup c@ [CHAR] ; <> WHILE REPEAT
            1+ \ next char
          ENDOF
            >r 1+ r>
        ENDCASE
      ENDOF
      \ default
        ( count c-addr c )
        >r swap 1+ swap 1+ r>
        ( count' c-addr' c )
    ENDCASE
  REPEAT
  2drop \ drop addr, c
;

: jvm_frame.setParameters() ( [arg1, [arg2 ... ]] addr_fm -- )
\ *G set the parameters
  dup jvm_frame.getClass()
  over jvm_frame.getMethod()
  dup ACC_STATIC jvm_md_?flags invert 1 and >r ( add 1 for this if not static )
  rot jvm_frame.local_table + @ -rot
  ( [arg1, [arg2 ... ]] addr_lt addr_cl addr_md )
  jvm_md_desc_idx
  ( [arg1, [arg2 ... ]] addr_lt addr_cl desc_idx )
  swap jvm_class.getRTCP() swap 
  ( [arg1, [arg2 ... ]] addr_lt rtcp desc_idx )
  jvm_rtcp.getConstpoolByIdx()
  jvm_cp_utf8_c-ref 
  ( [arg1, [arg2 ... ]] addr_lt c-addr n )
  jvm_frame.numberOfParamters() r> +
  ( [arg1, [arg2 ... ]] addr_lt index )
  tuck cells + 
  BEGIN
    ( [arg1, [arg2 ... ]] index addr_lt )
    1 cells -
    ( [arg1, [arg2 ... ]] index addr_lt' )
    swap 1- tuck
    ( [arg1, [arg2 ... ]] index' addr_lt' index' )
    0 >= WHILE
    ( [arg1, [arg2 ... ]] index' addr_lt' )
    rot swap 
    ( [arg1, [arg2 ... ]] index' arg_n addr_lt' )
    tuck !
  REPEAT
  2drop
;

\ ======
\ *> ###
\ ======

\ vim: sw=2 ts=2 sta et


\ Copyright (C) 2011-2012 Sebastian Rumpl, Josef Eisl, Bernhard Urban

\ This file is part of JorthVM

\ JorthVM is free software: you can redistribute it and/or modify
\ it under the terms of the GNU General Public License as published by
\ the Free Software Foundation, either version 3 of the License, or
\ (at your option) any later version.

\ This program is distributed in the hope that it will be useful,
\ but WITHOUT ANY WARRANTY; without even the implied warranty of
\ MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
\ GNU General Public License for more details.

\ You should have received a copy of the GNU General Public License
\ along with this program.  If not, see <http://www.gnu.org/licenses/>.
\ this file implements functionality that is needed to read class files
\ this file contains the implementaion of the class loader 

require exception.fs
require util.fs
require classfile.fs

\ ========
\ *! classloader
\ *T Class Loader
\ ========

0xCAFEBABE constant jvm_default_loader

\ ========
\ *S Classpath
\ ========
\ 
\ *E classpath_entry {
\ **    addr string;
\ **    addr string_size;
\ **    addr next_entry;
\ **  }
\ 
\ *P The classpath list is implemented as linked list. The
\ ** next_entry pointer of the last entry is 0. 


variable jvm_classpath_list
0 jvm_classpath_list !

0 cells constant jvm_classpath.string
1 cells constant jvm_classpath.string_size
2 cells constant jvm_classpath.next
3 cells constant jvm_classpath.size()


: jvm_classpath_entry.getNext() { addr1 -- addr2 }
\ *G get the next classpath entry
  addr1 jvm_classpath.next + @
;

: jvm_classpath_entry.getName() { addr -- c-addr n }
\ *G get the string of the classpath entry
  addr jvm_classpath.string + @
  addr jvm_classpath.string_size + @
;

\ FIXME rename to jvm_classpath.search()
: jvm_search_classpath { c-addr n -- addr wior }
\ *G Search for a class file and return the address to the memory location.
\ *P If the file is not found woir != 0
  jvm_classpath_list @ dup 
  0= IF
    drop
    c-addr n 
    s" .class" 
    strcat
    jvm_read_classfile 
    \ TODO IO exception vs class not found exception
  ELSE
    0 swap \ dummy result
    BEGIN
      ( addr1 addr2 -- ) 
      \ addr1 result of read_classfile
      \ addr2 next classpath entry
      nip
      dup jvm_classpath_entry.getName()
      c-addr n
      \ TODO package 
      strcat
      s" .class" 
      strcat
      ( c-addr n -- )
      try
        jvm_read_classfile
      iferror
        drop
        2drop
        0
        true
      then
      endtry
      rot
      ( -- addr )
      
      jvm_classpath_entry.getNext() dup
      0<> rot 
      and
    WHILE
    REPEAT
    drop
    dup 
    0= IF
      JVM_CLASSNOTFOUND_EXCEPTION throw
    ELSE
      0
    ENDIF
  ENDIF
;

: jvm_classpath.new() ( -- addr )
\ *G return memory for a new classpath entry
  jvm_classpath.size() allocate throw
  dup jvm_classpath.next +
  0 swap !
  jvm_classpath_list dup @
  0= IF
    over swap !
  ELSE
    @
    BEGIN
      \ ( addr -- )
      dup
      jvm_classpath_entry.getNext()
    0<> WHILE
      jvm_classpath_entry.getNext()
    REPEAT
    jvm_classpath.next + 
    over -rot
    !
  ENDIF
;

: jvm_classpath.add() { c-addr n -- }
\ *G Add a location to the classpath
\ FIXME check for slash
  jvm_classpath.new() 
  dup jvm_classpath.string +
  c-addr swap !
  jvm_classpath.string_size +
  n swap !
;

\ default classpath (TODO: path always okay?)
s" ../" jvm_classpath.add()

\ ======
\ *> ###
\ ======


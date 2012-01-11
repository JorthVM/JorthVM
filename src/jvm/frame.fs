\ vim: sw=2 ts=2 sta et
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
\ *G get the retunr address
  addr_fm jvm_frame.local_table + @ local cells + @ 
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
  rot jvm_frame.local_table + @ -rot
  ( [arg1, [arg2 ... ]] addr_lt addr_cl addr_md )
  jvm_md_desc_idx
  ( [arg1, [arg2 ... ]] addr_lt addr_cl desc_idx )
  swap jvm_class.getRTCP() swap 
  ( [arg1, [arg2 ... ]] addr_lt rtcp desc_idx )
  jvm_rtcp.getConstpoolByIdx()
  jvm_cp_utf8_c-ref 
  ( [arg1, [arg2 ... ]] addr_lt c-addr n )
  jvm_frame.numberOfParamters()
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

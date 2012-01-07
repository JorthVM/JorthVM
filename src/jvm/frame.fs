\ vim: sw=2 ts=2 sta et
\ this file contains the implementaion used for the creation and management 
\ of jvm stack frames

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
  \ TODO exception table
;

: jvm_frame.getClass() ( addr_fm -- addr_cl )
\ *G get the class
  jvm_frame.class + @
;

\ ======
\ *> ###
\ ======

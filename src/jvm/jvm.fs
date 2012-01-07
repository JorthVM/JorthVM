\ vim: sw=2 ts=2 sta et

\ ========
\ *! jvm
\ *T JVM
\ ========


require decode.fs
require frame.fs
require classfile.fs
require classloader.fs
require rtconstpool.fs
require class.fs
require frame.fs
require execute.fs
require stack.fs

: jvm_java { c-addr n -- }
  \ add class to jvm stack
  c-addr n jvm_stack.newClass()

  c-addr n 
  jvm_stack.invokeInitial()

;

: java ( "classname" -- )
\ *G start the following class by invoking public static void main(String[])
\ FIXME do cmd input handling (create string array etc.)
  parse-name 
  jvm_java
;


\ ======
\ *> ###
\ ======

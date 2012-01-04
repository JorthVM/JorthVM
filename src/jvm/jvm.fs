\ vim: sw=2 ts=2 sta et
require decode.fs
require fetch.fs
require frame.fs
require classfile.fs
require classloader.fs
require rtconstpool.fs
require class.fs
require frame.fs
require execute.fs
require stack.fs

jvm_stack.new() constant jvm_stack
\ *G this is the heart of the JVM

: jvm_init
\ does nothing atm
;

: jvm_java { c-addr n -- }
  \ add class to jvm stack
  jvm_stack jvm_class.new() c-addr n 
  jvm_stack.addClass()

  jvm_stack c-addr n 
  jvm_stack.invokeInitial()

\  c-addr n 
\  jvm_class_lookup throw
\  \ FIXME Store it somehow 
\  jvm_classentry.getClassfile()
\  dup 
\  s" main" s" ([Ljava/lang/String;)V" 
\  jvm_get_method_by_nametype
\  \ TODO check for public and static 
\  invert IF
\    JVM_MAINNOTFOUND_EXCEPTION throw
\  ENDIF
\  
\  jvm_md_get_code_attr
\  jvm_set_pc  \ TODO hardcoded
\  jvm_run 
;

: java ( "classname" -- )
\ *G start the following class by invoking public static void main(String[])
\ FIXME do cmd input handling (create string array etc.)
  parse-name 
  jvm_java
;


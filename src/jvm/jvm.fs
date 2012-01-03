\ vim: sw=2 ts=2 sta et
require decode.fs
require fetch.fs
require frame.fs
require classfile.fs
require classloader.fs
require rtconstpool.fs

\ register operation
require execute.fs

: jvm_init
\ does nothing atm
;

: ?debug_trace true ;

: show_insn ( opcode -- )
  dup jvm_mnemonic CR type
  jvm_mnemonic_imm 0 ?DO
    ." , " jvm_pc @ i + c@ hex.
  LOOP
;

: jvm_next
  POSTPONE jvm_fetch_instruction 
  [ ?debug_trace ] [IF] POSTPONE dup POSTPONE show_insn [ENDIF]
  POSTPONE jvm_execute
; immediate

: jvm_run
  begin 
    jvm_next
  again
;

: jvm_java ( c-addr n -- )
  jvm_class_lookup throw
  \ FIXME Store it somehow 
  jvm_classentry.getClassfile()
  dup 
  s" main" s" ([Ljava/lang/String;)V" 
  jvm_get_method_by_nametype
  \ TODO check for public and static 
  invert IF
    JVM_MAINNOTFOUND_EXCEPTION throw
  ENDIF
  
  jvm_md_get_code_attr
  jvm_set_pc  \ TODO hardcoded
  jvm_run 
;

: java ( "classname" -- )
\ *G start the following class by invoking public static void main(String[])
\ FIXME do cmd input handling (create string array etc.)
  parse-name 
  jvm_java
;


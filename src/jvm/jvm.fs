require decode.fs
require fetch.fs
require frame.fs
require classfile.fs

\ register operation
require execute.fs

: jvm_init
\ does nothing atm
;

: jvm_next
  POSTPONE jvm_fetch_instruction 
  POSTPONE jvm_execute
; immediate

: jvm_run
  begin 
    jvm_next
  again
;

: LoadAndRun ( c-addr n -- ) \ load a classfile and run public static void main(String[])
  jvm_read_classfile drop \ drop file size
  filebuffer @
  dup \ for get code attr
  s" main" s" ([Ljava/lang/String;)V" jvm_get_method_by_nametype
  invert IF
    s" public static main(String[]) not found" exception throw 
  ENDIF
  \ TODO check for public and static 
  jvm_md_get_code_attr

  jvm_set_pc  \ TODO hardcoded
  jvm_run 
;


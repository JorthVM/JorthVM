include decode.fs
include fetch.fs

\ register operation
include execute.fs

: jvm_init
  jvm_init_op_tbl
  jvm_init_ops
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

require decode.fs

: jvm_fetch_instruction ( ... -- opcode ... )
  jvm_pc @ dup 1 + jvm_pc ! \ swap
;

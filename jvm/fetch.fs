
: jvm_fetch_instruction ( ... -- opcode ... )
  jvm_pc @ dup 1 + jvm_pc ! @ 0xff and \ swap
;

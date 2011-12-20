\ vim: sw=2 ts=2 sta et
require decode.fs

: jvm_fetch_instruction ( ... -- opcode ... )
  POSTPONE jvm_pc POSTPONE @ \ load pc 
  POSTPONE c@ \ load instruction
  1 POSTPONE literal POSTPONE jvm_pc POSTPONE +! \ increment pc
; immediate

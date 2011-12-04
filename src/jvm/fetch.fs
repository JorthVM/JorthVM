
: jvm_fetch_instruction ( ... -- opcode ... )
  POSTPONE jvm_pc POSTPONE @ \ load pc 
  POSTPONE c@ \ load instruction
  1 POSTPONE literal POSTPONE jvm_pc POSTPONE +! \ increment pc
; immediate

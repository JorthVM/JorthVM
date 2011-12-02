
: jvm_fetch_instruction ( ... -- opcode ... )
  POSTPONE jvm_pc POSTPONE @ 
  POSTPONE dup 1 POSTPONE literal
  POSTPONE + POSTPONE jvm_pc 
  POSTPONE ! POSTPONE @ 0xff POSTPONE literal 
  POSTPONE and \ swap
; immediate

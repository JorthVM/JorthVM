\ vim: sw=2 ts=2 sta et

\ ========
\ *! decode
\ *T Instruction Decode
\ ========

\ -------------------------------------------------------- \
\ OPCODE TABLE                                             \
\ -------------------------------------------------------- \

256 CONSTANT jvm_opcode_count

CREATE jvm_opcode_table jvm_opcode_count cells allot
CREATE jvm_mnemonic_table jvm_opcode_count 2* cells allot

\ general:
\ (1) define instruction (opcode, number of immediates and mnemonic):
\     `0x32 0 s" aaload" ^> : <^ ; >[ mycode ]<'
\ (2) use word (will emit opcode on stack):
\     `aaload'
\ (3) execute instruction (as defined in `mycode'):
\     <[ aaload ]>

\ FIXME: ugly workaround...
: ^> { opcode imm mnemonic len -- }
  opcode mnemonic len imm opcode mnemonic len nextname ;

\ FIXME: wtf @ interpret mode
: <^
  POSTPONE [ >r 2over drop ]
  POSTPONE literal
  POSTPONE [ r> ]
; immediate

: >[ ( opcode mnemonic len imm opcode -- )
  2* cells jvm_mnemonic_table + >r ( opcode mnemonic len imm ; R: table )
  over 8 lshift or r@ ! ( opcode mnemonic len ; R: table )
  dup allocate throw >r r@ swap ( opcode mnemonic addr len ; R: table addr )
  cmove ( opcode ; R: table addr )
  r> r> cell+ ! ( opcode )
  cells jvm_opcode_table + :noname
;

: jvm_mnemonic ( opcode -- mnemonic len )
  2* cells jvm_mnemonic_table + >r r@ @ 8 rshift
  r> cell+ @ swap
;

: jvm_mnemonic_imm ( opcode -- amounts of immediates )
  2* cells jvm_mnemonic_table + @ 0xff and
;

: ]<
  POSTPONE ; swap !
; IMMEDIATE

\ `<[ 0x42 ]>' executes the xt token stored at offest 0x42 of the opcode-table
: <[
  jvm_opcode_table
;

: ]>
  cells + @ EXECUTE
;

: jvm_execute { opcode -- [result of operation] }
  <[ opcode ]>
;

\ show the implementeation of opcode
: jvm_opcode_see ( ... opcode - )
  cells jvm_opcode_table + @ xt-see
;


\ ======
\ *> ###
\ ======

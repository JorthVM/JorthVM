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

\ ========
\ *S Defining Instructions
\ ========

\ *(
\ *B define instruction (opcode, number of immediates and mnemonic):
\ *C 0x32 0 s" aaload" >[ mycode ]<
\ *B use word (emits opcode on stack):
\ *C aaload
\ *B execute instruction (as defined with `mycode'):
\ *C <[ aaload ]>
\ *)

: >[ { opcode imm mnemonic len -- }
\ *G read: "to opcode table". see above for a explanation
  \ define word which pushes the opcode to stack
  mnemonic len nextname
  : opcode POSTPONE literal POSTPONE ;

  opcode 2* cells jvm_mnemonic_table + >r ( ; R: table )
  len 8 lshift imm or r@ ! \ pack length and immediate value ( ; R: table )
  len allocate throw >r ( ; R: table addr )
  mnemonic r@ len cmove ( ; R: table addr )
  r> r> ( addr table )
  cell+ ! \ save pointer for mnemonic-str
  opcode cells jvm_opcode_table + \ addr for xt
  :noname
;

: ]>
\ *G close instruction implementation
  cells + @ EXECUTE
;

: jvm_mnemonic ( opcode -- mnemonic len )
  2* cells jvm_mnemonic_table + >r r@ @ 8 rshift
  r> cell+ @ swap
;

: jvm_mnemonic_imm ( opcode -- amounts of immediates )
  2* cells jvm_mnemonic_table + @ 0xff and
;

: <[
\ *G executes the xt token stored at the given offset of the opcode-table, e.g.
\ *C <[ 0x42 ]>
\ *P reads the xt from offset 0x42 and executes it
  jvm_opcode_table
;

: ]<
\ *G see <[
  POSTPONE ; swap !
; IMMEDIATE

\ show the implementeation of opcode
: jvm_opcode_see ( ... opcode - )
  cells jvm_opcode_table + @ xt-see
;


\ ======
\ *> ###
\ ======

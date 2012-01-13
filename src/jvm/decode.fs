\ vim: sw=2 ts=2 sta et

\ `JorthVM', a Java VM implemented in Forth
\ 
\ Copyright (C) 2012 Sebastian Rumpl <e0828489@student.tuwien.ac.at>
\ Copyright (C) 2012 Josef Eisl <zapster@zapster.cc>
\ Copyright (C) 2012 Bernhard Urban <lewurm@gmail.com>
\ 
\ This program is free software: you can redistribute it and/or modify
\ it under the terms of the GNU General Public License as published by
\ the Free Software Foundation, either version 3 of the License, or
\ (at your option) any later version.
\ 
\ This program is distributed in the hope that it will be useful,
\ but WITHOUT ANY WARRANTY; without even the implied warranty of
\ MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
\ GNU General Public License for more details.
\ 
\ You should have received a copy of the GNU General Public License
\ along with this program.  If not, see <http://www.gnu.org/licenses/>.
\ this file implements functionality that is needed to read class files

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

: jvm_decode.mnemonic() ( opcode -- mnemonic len )
\ *G converts opcode into readable string
  2* cells jvm_mnemonic_table + >r r@ @ 8 rshift
  r> cell+ @ swap
;

: jvm_decode.mnemonic_imm() ( opcode -- amount of immediates )
\ *G returns the number of immediates for the given opcode
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
: jvm_decode.opcode_see() ( ... opcode - )
  cells jvm_opcode_table + @ xt-see
;


\ ======
\ *> ###
\ ======

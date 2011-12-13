
\ -------------------------------------------------------- \
\ OPCODE TABLE                                             \
\ -------------------------------------------------------- \

256 CONSTANT jvm_opcode_count

CREATE jvm_opcode_table jvm_opcode_count cells allot

\ `0x42 >[ bye ]<' stores xt of bye at offset 0x42 of the opcode-table
: >[
  cells jvm_opcode_table + :noname
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

\ -------------------------------------------------------- \
\ PROGRAM COUNTER                                          \
\ -------------------------------------------------------- \

variable jvm_pc_start
variable jvm_pc

: jvm_set_pc ( ... addr -- ... )
  dup
  jvm_pc_start ! \ store start address
  jvm_pc !
;


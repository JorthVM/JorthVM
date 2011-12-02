\ create lookuptable

: jvm_op_unsupported s" This Opcode is not (yet) supported" exception throw ;

\ opcode table size
variable jvm_opcodetbl_size

\ opcode table
variable jvm_opcodetbl 

\ program counter
variable jvm_pc_start
variable jvm_pc

: jvm_set_op ( ... xt opcode - )
cells jvm_opcodetbl @ + ! \ calculate opcode-addr and store xt 
;

: jvm_execute (  ... opcode - ... [result of operation] )
POSTPONE cells POSTPONE jvm_opcodetbl POSTPONE @ POSTPONE + POSTPONE @  POSTPONE execute
; immediate

\ show the implementeation of opcode
: jvm_opcode_see ( ... opcode - ) 
  cells jvm_opcodetbl @ + @ xt-see
; 

: jvm_init_op_tbl ( ... - ... )
\ store opcode table size
  256 jvm_opcodetbl_size !
\ allocate opcode table cells
  jvm_opcodetbl_size @ cells allocate throw \ xt for opcodes 
  jvm_opcodetbl ! \ store table pointer
\ set default handler
  ['] jvm_op_unsupported
  jvm_opcodetbl_size @ 0 +DO
  dup i jvm_set_op 
  LOOP
  drop
;

: jvm_set_pc ( ... addr -- ... )
  dup
  jvm_pc_start ! \ store start address
  jvm_pc ! 
;


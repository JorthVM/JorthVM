\ create lookuptable

: unsupported s" This Opcode is not (yet) supported" type 100 throw ;

variable opcodetbl 
256 cells allocate throw \ xt for opcodes 
opcodetbl ! \ store table pointer

: jvm_set_op ( ... xt opcode - )
cells opcodetbl + ! \ calculate opcode-addr and store xt 
;

: jvm_execute (  ... opcode - ... [result of operation] )
cells opcodetbl + @ execute
;

: jvm_init_op_tbl ( ... - ... )
opcodetbl 256 erase 
;

\ program counter

variable jvm_pc

: jvm_set_pc ( ... addr -- ... )
  jvm_pc ! 
;


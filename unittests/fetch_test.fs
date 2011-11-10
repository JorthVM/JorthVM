require ../jvm/jvm.fs

\ test fetch_instruction
: fetch_instruction-test
create program 
0x04030201 , 
program jvm_set_pc

jvm_fetch_instruction
assert( 1 = )
jvm_fetch_instruction
assert( 2 = )
jvm_fetch_instruction
assert( 3 = )
jvm_fetch_instruction
assert( 4 = )
\ TODO check stack size
;

fetch_instruction-test

bye

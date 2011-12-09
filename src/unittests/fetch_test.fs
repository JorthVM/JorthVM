require ../jvm/jvm.fs

create program 4 cells allot
0x01020304 program l!-be
program jvm_set_pc

\ test fetch_instruction
: fetch_instruction-test
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

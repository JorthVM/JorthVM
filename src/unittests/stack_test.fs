\ vim: sw=2 ts=2 sta et
require ../jvm/stack.fs

: fetch_instruction-test
  assert( depth 0 = )
  6 allocate throw
  0x01 over c!
  0x02 over 1 + c!
  0x03 over 2 + c!
  0x04 over 3 + c!
  0x05 over 4 + c!
  0x06 over 5 + c!
  dup jvm_stack.setPC()

  jvm_stack.getPC()
  jvm_stack.getPC_next()
  assert( = )

  jvm_stack.fetchByte()
  assert( 0x01 = )
  jvm_stack.getPC()
  jvm_stack.getPC_next()
  assert( 1- = )

  jvm_stack.fetchByte()
  assert( 0x02 = )
  jvm_stack.getPC()
  jvm_stack.getPC_next()
  assert( 2 - = )

  jvm_stack.fetchByte()
  assert( 0x03 = )
  jvm_stack.getPC()
  jvm_stack.getPC_next()
  assert( 3 - = )
  
  jvm_stack.incPC()
  jvm_stack.getPC()
  jvm_stack.getPC_next()
  assert( = )

  jvm_stack.fetchByte()
  assert( 0x04 = )
  jvm_stack.getPC()
  jvm_stack.getPC_next()
  assert( 1- = )

  jvm_stack.fetchByte()
  assert( 0x05 = )
  jvm_stack.getPC()
  jvm_stack.getPC_next()
  assert( 2 - = )

  jvm_stack.fetchByte()
  assert( 0x06 = )
  jvm_stack.getPC()
  jvm_stack.getPC_next()
  assert( 3 - = )
  
  free throw
  
  assert( depth 0 = )
;

create program2 4 cells allot
0x01020304 program2 l!-be
program2 jvm_stack.setPC()

\ test fetch_instruction
: fetch_instruction-test2
  program2 jvm_stack.setPC()
  jvm_stack.fetchByte()
  assert( 1 = )
  jvm_stack.fetchByte()
  assert( 2 = )
  jvm_stack.fetchByte()
  assert( 3 = )
  jvm_stack.fetchByte()
  assert( 4 = )
  \ TODO check stack size
;

: test
  fetch_instruction-test
  fetch_instruction-test2
;


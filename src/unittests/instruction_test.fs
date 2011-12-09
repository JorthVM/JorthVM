include ../jvm/jvm.fs

\ initialize jvm
jvm_init

\ bipush test program
create program_bipush 4 cells allot
0x102a10ff program_bipush l!-be

\ sipush test program
create program_sipush 6 cells allot
0x112a2a11 program_sipush l!-be
0xffff program_sipush 4 + w!-be

: test_bipush
  program_bipush jvm_set_pc

\ no sign ext
  jvm_fetch_instruction
  jvm_execute
  assert( 42 = ) 
\ sign ext
  jvm_fetch_instruction
  jvm_execute
  assert( -1 = ) 
;

: test_sipush
  program_sipush jvm_set_pc

\ no sign ext
  jvm_fetch_instruction
  jvm_execute
  assert( 0x2A2A = ) 
\ sign ext
  jvm_fetch_instruction
  jvm_execute
  assert( -1 = ) 

;

: test_dup
  42  \ value
  0x59  \ dup
  jvm_execute
  assert( 42 = ) 
  assert( 42 = )
;

: test_iadd
  42 42
  0x60 \ iadd
  jvm_execute
  assert( 84 = )
;

: test_siadd
  
;

test_bipush
test_sipush
test_dup
test_iadd

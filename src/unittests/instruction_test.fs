\ vim: sw=2 ts=2 sta et
include ../jvm/jvm.fs

: write-programm ( ... addr -- )
  BEGIN depth 1 > WHILE
    dup 1+ >r
    depth 1- roll
    swap c! r>
  REPEAT
  drop
;

\ bipush test program
bipush 0x2a bipush 0xff
create program_bipush depth allot
program_bipush write-programm

\ sipush test program
sipush 0x2a 0x2a sipush 0xff 0xff
create program_sipush depth allot
program_sipush write-programm

: test_bipush
  program_bipush jvm_stack.setPC()

\ no sign ext
  jvm_stack.fetchByte()
  jvm_execute
  assert( 42 = ) 
\ sign ext
  jvm_stack.fetchByte()
  jvm_execute
  assert( -1 = ) 
;

: test_sipush
  program_sipush jvm_stack.setPC()

\ no sign ext
  jvm_stack.fetchByte()
  jvm_execute
  assert( 0x2A2A = ) 
\ sign ext
  jvm_stack.fetchByte()
  jvm_execute
  assert( -1 = ) 

;

: test_dup
  42  \ value
  jdup
  jvm_execute
  assert( 42 = ) 
  assert( 42 = )
;

: test_iadd
  42 42
  iadd
  jvm_execute
  assert( 84 = )
;

: test_siadd
  \ TODO ???
;

: test_mnemonic
  0x10 jvm_mnemonic s" bipush" compare
  assert( invert )
  0x10 jvm_mnemonic_imm
  assert( 1 = )
;

: putstatic_test
\ NOTE this testcase also ensures that jvm_class.{getStatic()|setStatic()} 
\ are working
  assert( depth 0 = )
  s" ./" jvm_classpath.add()
  s" StaticInt" jvm_java
  jvm_stack.getCurrentFrame()
  jvm_frame.getClass()
  dup s" foo|I" jvm_class.getStatic() 
  throw
  assert( 0x42 = )
  s" bar|I" jvm_class.getStatic() throw
  assert( 0x84 = )
  
  assert( depth 0 = )
;

: test
  test_bipush
  test_sipush
  test_dup
  test_iadd
  test_mnemonic
  putstatic_test
;

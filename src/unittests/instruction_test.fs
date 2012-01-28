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
  <[ jvm_stack.fetchByte() ]>
  assert( 42 = ) 
\ sign ext
  <[ jvm_stack.fetchByte() ]>
  assert( -1 = ) 
;

: test_sipush
  program_sipush jvm_stack.setPC()

\ no sign ext
  <[ jvm_stack.fetchByte() ]>
  assert( 0x2A2A = ) 
\ sign ext
  <[ jvm_stack.fetchByte() ]>
  assert( -1 = ) 

;

: test_dup
  42  \ value
  <[ jdup ]>
  assert( 42 = ) 
  assert( 42 = )
;

: test_iadd
  42 42
  <[ iadd ]>
  assert( 84 = )
;

: test_siadd
  \ TODO ???
;

: test_mnemonic
  0x10 jvm_decode.mnemonic() s" bipush" compare
  assert( invert )
  0x10 jvm_decode.mnemonic_imm()
  assert( 1 = )
;

: putstatic_test
\ NOTE this testcase also ensures that jvm_class.{getStatic()|setStatic()} 
\ are working
  assert( depth 0 = )
  s" ../testfiles/" jvm_classpath.add()
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

: putstatic_test2
\ NOTE this testcase also ensures that jvm_class.{getStatic()|setStatic()} 
\ are working
  assert( depth 0 = )
  s" ../testfiles/" jvm_classpath.add()
  s" StaticIntOther" jvm_java
  s" StaticIntOtherStore" jvm_stack.findClass() throw
  dup s" foo|I" jvm_class.getStatic() 
  throw
  assert( 0x42 = )
  s" bar|I" jvm_class.getStatic() throw
  assert( 0x84 = )
  
  assert( depth 0 = )
;

: static_invocation_test1 
  assert( depth 0 = )
  s" ../testfiles/" jvm_classpath.add()
  s" StaticInvocation" jvm_java
  s" StaticInvocation" jvm_stack.findClass() throw
  dup s" foo|I" jvm_class.getStatic() 
  throw
  assert( 0x42 = )
  s" bar|I" jvm_class.getStatic() throw
  assert( 0x84 = )
  
  assert( depth 0 = )
;

: static_invocation_test2 
  assert( depth 0 = )
  s" ../testfiles/" jvm_classpath.add()
  s" StaticInvocation2" jvm_java
  s" StaticInvocation2" jvm_stack.findClass() throw
  dup s" foo|I" jvm_class.getStatic() 
  throw
  assert( 0x42 = )
  s" bar|I" jvm_class.getStatic() throw
  assert( 0x84 = )
  
  assert( depth 0 = )
;

: parameter_test1 
  assert( depth 0 = )
  s" ../testfiles/" jvm_classpath.add()
  s" StaticInvocationParameter" jvm_java
  s" StaticInvocationParameter" jvm_stack.findClass() throw
  dup s" foo|I" jvm_class.getStatic() 
  throw
  assert( 0x11 = )
  s" bar|I" jvm_class.getStatic() throw
  assert( 0x11 3 * = )
  
  assert( depth 0 = )
;

: invoke_inheritance_test1
  \ non-static field access and {non-,}static method calls!
  assert( depth 0 = )
  s" ../testfiles/" jvm_classpath.add()
  s" ObjectCreation" jvm_java
  s" ObjectCreation" jvm_stack.findClass() throw
  s" checkMe|I" jvm_class.getStatic() throw
  assert( 0x1337 = )
  assert( depth 0 = )
;

: static_inititilazer_test1
  assert( depth 0 = )
  s" ../testfiles/" jvm_classpath.add()
  s" StaticFieldInit" 2dup 
  jvm_java
  jvm_stack.findClass() throw
  dup 
  s" foo|I" jvm_class.getStatic() throw
  assert( 0x42 = )
  s" bar|I" jvm_class.getStatic() throw
  assert( 0x84 = )
  assert( depth 0 = )
;

: static_inititilazer_test2
  assert( depth 0 = )
  s" ../testfiles/" jvm_classpath.add()
  s" StaticInitializer" 2dup 
  jvm_java
  jvm_stack.findClass() throw
  dup 
  s" foo|I" jvm_class.getStatic() throw
  assert( 0x42 = )
  s" bar|I" jvm_class.getStatic() throw
  assert( 0x84 = )
  assert( depth 0 = )
;

: ldc_integer_test
  assert( depth 0 = )
  s" ../testfiles/" jvm_classpath.add()
  s" LdcInteger" 2dup
  jvm_java
  jvm_stack.findClass() throw
  s" babe|I" jvm_class.getStatic() throw
  assert( 0xBABE = )
  assert( depth 0 = )
;

: if_icmp_test1
  assert( depth 0 = )
  s" ../testfiles/" jvm_classpath.add()
  s" IfIntegerCompare" 2dup
  jvm_java
  jvm_stack.findClass() throw
  dup 
  s" res1|I" jvm_class.getStatic() throw
  assert( 0xDEAD = )
  dup
  s" res2|I" jvm_class.getStatic() throw
  assert( 0xBABE = )
  dup
  s" res3|I" jvm_class.getStatic() throw
  assert( 0xBEEF = )
  dup
  s" res4|I" jvm_class.getStatic() throw
  assert( 0xAFFE = )
  drop
  \ TODO test: eq ne lt gt ifeq ifne iflt ifge ifgt ifle
  assert( depth 0 = )
;

: ireturn_test 
  assert( depth 0 = )
  s" ../testfiles/" jvm_classpath.add()
  s" IReturn" 2dup
  jvm_java
  jvm_stack.findClass() throw
  dup 
  s" res|I" jvm_class.getStatic() throw
  assert( 0xDEAD = )
  drop
  assert( depth 0 = )
;

: fibonacci_test 
  assert( depth 0 = )
  s" ../testfiles/" jvm_classpath.add()
  s" StaticFibonacci" 2dup
  jvm_java
  jvm_stack.findClass() throw
  dup 
  s" res|I" jvm_class.getStatic() throw
  assert( 55 = )
  drop
  assert( depth 0 = )
;

: test
  test_bipush
  test_sipush
  test_dup
  test_iadd
  test_mnemonic
  putstatic_test
  putstatic_test2
  static_invocation_test1 
  static_invocation_test2 
  parameter_test1
  invoke_inheritance_test1
  static_inititilazer_test1
  static_inititilazer_test2
  if_icmp_test1
  ldc_integer_test
  ireturn_test
  fibonacci_test
;

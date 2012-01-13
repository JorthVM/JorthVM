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

: fetch_short-test1
  program2 jvm_stack.setPC()
  jvm_stack.fetchShort()
  assert( 0x0102 = )
;

: test
  fetch_instruction-test
  fetch_instruction-test2
  fetch_short-test1
;


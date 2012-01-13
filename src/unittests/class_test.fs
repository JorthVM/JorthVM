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
require ../jvm/class.fs


: get_field_desc_size_test
  assert( depth 0 = )
  \ TODO find better names
  s" Bblblblbl" jvm_field_desc_size assert( 1 cells = )
  s" Cblblblbl" jvm_field_desc_size assert( 1 cells = )
  s" Dblblblbl" jvm_field_desc_size assert( 2 cells = )
  s" Fblblblbl" jvm_field_desc_size assert( 1 cells = )
  s" Iblblblbl" jvm_field_desc_size assert( 1 cells = )
  s" Jblblblbl" jvm_field_desc_size assert( 2 cells = )
  s" Lblblblbl" jvm_field_desc_size assert( 1 cells = )
  s" Sblblblbl" jvm_field_desc_size assert( 1 cells = )
  s" Sblblblbl" jvm_field_desc_size assert( 1 cells = )
  s" [blblblbl" jvm_field_desc_size assert( 1 cells = )
  assert( depth 0 = )
;

: field_load_test
  assert( depth 0 = )
  1 cells allocate throw
  dup >r
  jvm_field_@
  assert( depth 1 = )
  r> free throw
  clearstack

  assert( depth 0 = )
  
  2 cells allocate throw
  dup >r
  jvm_field_2@
  assert( depth 2 = )
  r> free throw
  clearstack
  assert( depth 0 = )
;

: field_store_test
  assert( depth 0 = )
  1 cells allocate throw
  dup >r
  42 over jvm_field_!
  jvm_field_@
  assert( depth 1 = )
  assert( 42 = )
  r> free throw

  assert( depth 0 = )

  2 cells allocate throw
  dup >r
  dup 42 0x42 rot
  jvm_field_2!
  jvm_field_2@
  assert( depth 2 = )
  assert( 0x42 = )
  assert( 42 = )
  r> free throw
  assert( depth 0 = )
;

: test
  get_field_desc_size_test
  field_load_test
  field_store_test
;

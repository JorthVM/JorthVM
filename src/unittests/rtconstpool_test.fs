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
include ../jvm/rtconstpool.fs

: createConstPoolTable_test
  assert( depth 0= )
  s" bin/Test.class" jvm_read_classfile throw 
  dup jvm_rtcp.createConstPoolTable()
  swap dup
  jvm_cf_constpool_addr
  swap
  jvm_cf_constpool_count
  1 ?DO
    ( addr1 addr2 --)
    \ addr1 address of the current table entry
    \ addr2 address of the current constpool entry
    over @
    over assert( = )
    swap 1 cells + swap           \ imcrement table
    dup jvm_constpool_type_size + \ increment constpool entry
  LOOP
  2drop
  assert( depth 0= )
;

: rtcp_new_test
  assert( depth 0= )
  s" bin/Test.class" jvm_read_classfile throw 
  dup jvm_rtcp.new()
  tuck
  1 jvm_rtcp.getConstpoolByIdx() 
  swap jvm_cf_constpool_addr
  assert( = )
  \ TODO jvm_rtcp.getClass_info() >order
  drop \ drop rtcp addr
  
 \  assert( depth 0= )
;

: test
  createConstPoolTable_test
  rtcp_new_test
;

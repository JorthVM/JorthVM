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
require ../jvm/classloader.fs

: classpath_add_test
  assert( depth 0 = )
  0 jvm_classpath_list ! \ reset classpath
  s" testtest" jvm_classpath.add()
  jvm_classpath_list @ 
  jvm_classpath_entry.getName()
  s" testtest"
  compare
  assert( 0= )
  s" test2" jvm_classpath.add()
  jvm_classpath_list @ jvm_classpath_entry.getNext()
  jvm_classpath_entry.getName()
  s" test2"
  compare
  assert( 0= )
  assert( depth 0 = )
;

: test
  classpath_add_test
;


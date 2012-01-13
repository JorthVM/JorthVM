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
require ../jvm/util.fs

: strcat_test
  assert( depth 0 = )
  s" hello " s" world" strcat
  s" hello world" assert( str= )
  assert( depth 0 = )
;

: char_replace_test
  s" test.test.test" [CHAR] . [CHAR] / 
  strreplacec
  s" test/test/test"
  assert( str= )
;

: add_find_word_test
  depth assert( 0= )
  wordlist dup
  42 swap
  s" forty-two" rot
  jvm_add_word
  s" forty-two" drop \ no count!?
  
  \ negativ test
  find
  assert( 0= )
  drop \ c-addr
  
  \ positiv test
  s" forty-two" rot 
  jvm_find_word throw
  assert( 42 = )
  depth assert( 0= )
;

: replace_word_test
  depth assert( 0= )
  wordlist dup >r
  42 swap
  s" forty-two" rot
  jvm_add_word
  
  \ negativ test
  s" forty-two" drop \ no count!?
  find
  assert( 0= )
  drop \ c-addr
  
  \ positiv test
  s" forty-two" 
  r> dup >r                \ wid
  jvm_find_word throw
  assert( 42 = )
  
  
  0x42 s" forty-two"
  r> dup >r                \ wid
  jvm_replace_word throw
  
  \ negativ test
  s" forty-two" drop \ no count!?
  find
  assert( 0= )
  drop \ c-addr

  \ positiv test
  s" forty-two"
  r>                       \ wid 
  jvm_find_word throw
  assert( 0x42 = )

  depth assert( 0= )

  
;

: test
  strcat_test
  char_replace_test
  add_find_word_test
  replace_word_test
;

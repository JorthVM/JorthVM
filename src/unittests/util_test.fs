\ vim: sw=2 ts=2 sta et
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

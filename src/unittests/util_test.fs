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

strcat_test
char_replace_test

\ vim: sw=2 ts=2 sta et
require ../jvm/util.fs

: strcat_test
  assert( depth 0 = )
  s" hello " s" world" strcat
  s" hello world" assert( str= )
  assert( depth 0 = )
;

strcat_test

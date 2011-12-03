require ../jvm/classfile.fs

create const_utf8_Test
0x74736554040001 , \ 01 (tag) 0004 (legth) 54 (T) 65 (e) 73 (s) 74 (t)

\ test utf8 compare
: cmp_utf8_test 
  s" Test" const_utf8_Test jvm_constpool_cmp_utf8
  assert( ) \ assert true
  s" Test!" const_utf8_Test jvm_constpool_cmp_utf8
  assert( invert ) \ assert false
  s" tEST" const_utf8_Test jvm_constpool_cmp_utf8
  assert( invert ) \ assert false
;

cmp_utf8_test

bye

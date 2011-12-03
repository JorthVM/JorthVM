require ../jvm/classfile.fs

\ test utf8 compare
: cmp_utf8_test 
  \ NOTE: this is somehow cumbersome but
  \ it work no matter what the cell size is
  7 allocate throw
  dup     0x01 swap ! \ u1 tag
  dup 1 + 0x00 swap ! \ u2 legth high
  dup 2 + 0x04 swap ! \ u2 legth low
  dup 3 + 0x54 swap ! \ T
  dup 4 + 0x65 swap ! \ e
  dup 5 + 0x73 swap ! \ s
  dup 6 + 0x74 swap ! \ t

  dup s" Test" jvm_constpool_cmp_utf8
  assert( ) \ assert true
  dup s" Test!" jvm_constpool_cmp_utf8
  assert( invert ) \ assert false
  dup s" tEST" jvm_constpool_cmp_utf8
  assert( invert ) \ assert false
  drop
;

cmp_utf8_test

bye

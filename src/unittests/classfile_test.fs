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

\ test utf8 compare
: constpool_idx_test 
  \ NOTE: this is somehow cumbersome but
  \ it work no matter what the cell size is
  12 allocate throw
  dup      0x00 swap ! \ u2 constant pool count high
  dup 1  + 0x02 swap ! \ u2 constant pool count low
  dup 2  + 0x07 swap ! \ u1 tag (class) 
  dup 3  + 0x00 swap ! \ u2 name idx high
  dup 4  + 0x02 swap ! \ u2 name idx low
  dup 5  + 0x01 swap ! \ u1 tag (utf8)
  dup 6  + 0x00 swap ! \ u2 length high
  dup 7  + 0x04 swap ! \ u2 length low
  dup 8  + 0x74 swap ! \ t
  dup 9  + 0x74 swap ! \ t
  dup 10 + 0x74 swap ! \ t
  dup 11 + 0x74 swap ! \ t

  2 + \ get the start of the const pool
  dup dup 1 jvm_constpool_idx
  assert( over = )
  dup dup 2 jvm_constpool_idx
  assert( over 3 + = )
  dup dup 1 jvm_constpool_idx
  assert( over 3 + <> )
  drop
;

cmp_utf8_test
constpool_idx_test
bye

require ../jvm/classfile.fs

\ test utf8 compare
: cmp_utf8_test 
  \ NOTE: this is somehow cumbersome but
  \ it work no matter what the cell size is
  7 allocate throw
  dup     0x01 swap c! \ u1 tag
  dup 1 + 0x0400 swap w! \ u2 legth
  dup 3 + 0x74736554 swap l! \ Test

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
  dup      0x0200 swap w! \ u2 constant pool count
  dup 2  + 0x07 swap c! \ u1 tag (class) 
  dup 3  + 0x0200 swap w! \ u2 name idx
  dup 5  + 0x01 swap c! \ u1 tag (utf8)
  dup 6  + 0x0400 swap w! \ u2 length
  dup 8  + 0x74747474 swap l! \ tttt

  2 + \ get the start of the const pool
  dup 1 jvm_constpool_idx
  assert( over = )
  dup 2 jvm_constpool_idx
  assert( over 3 + = )
  dup 1 jvm_constpool_idx
  assert( over 3 + <> )
  drop
;

: get_tag_test
  5 allocate throw
  dup      0x07 swap c! \ u1 tag
  dup 1  + 0xAAAAAAAA swap l! \ arbitrary data
  jvm_cp_tag 
  assert( 7 = )
;

: get_class_name_idx_test 
  12 allocate throw
  dup      0x0200 swap w! \ u2 constant pool count
  dup 2  + 0x07 swap c! \ u1 tag (class) 
  dup 3  + 0x0200 swap w! \ u2 name idx
  dup 5  + 0x01 swap c! \ u1 tag (utf8)
  dup 6  + 0x0400 swap w! \ u2 length
  dup 8  + 0x74747474 swap l! \ tttt

  2 + \ get the start of the const pool
  jvm_cp_class_name_idx
  assert( 2 = )
;

: ref_getter_test
  32 allocate throw
  dup      0x0700 swap w! \ u2 constant pool count
  \ idx 1
  dup 2  + 0x07 swap c! \ u1 tag (class) 
  dup 3  + 0x0200 swap w! \ u2 name idx
  \ idx 2
  dup 5  + 0x01 swap c! \ u1 tag (utf8)
  dup 6  + 0x0400 swap w! \ u2 length
  dup 8  + 0x74747474 swap l! \ tttt
  \ idx 3
  dup 12  + 0x09 swap c! \ u1 tag (fieldref) 
  dup 13 + 0x0100 swap w! \ u2 class idx
  dup 15 + 0x0600 swap w! \ u2 nametype idx
  \ idx 4
  dup 17  + 0x10 swap c! \ u1 tag (methoderef)
  dup 18 + 0x0100 swap w! \ u2 class idx
  dup 20 + 0x0600 swap w! \ u2 nametype idx
  \ idx 5
  dup 22  + 0x11 swap c! \ u1 tag (interfacemethodref) 
  dup 23 + 0x0100 swap w! \ u2 class idx
  dup 25 + 0x0600 swap w! \ u2 nametype idx
  \ idx 6
  dup 27  + 0x12 swap c! \ u1 tag (nametype) 
  dup 28 + 0xAAAA swap w! \ u2 name_idx idx (nonsens)
  dup 30 + 0x5555 swap w! \ u2 descriptor idx (nonsens)

  dup 12 + dup \ get the start of the entry
  jvm_cp_fieldref_class_idx
  assert( 1 = )
  jvm_cp_fieldref_nametype_idx
  assert( 6 = )

  dup 17 + dup \ get the start of the entry
  jvm_cp_methodref_class_idx
  assert( 1 = )
  jvm_cp_methodref_nametype_idx
  assert( 6 = )

  dup 22 + dup \ get the start of the entry
  jvm_cp_interfacemethodref_class_idx
  assert( 1 = )
  jvm_cp_interfacemethodref_nametype_idx
  assert( 6 = )

  drop

;

cmp_utf8_test
constpool_idx_test
get_tag_test
get_class_name_idx_test

bye

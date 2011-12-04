require ../jvm/classfile.fs

: big_endian_load_test
  8 allocate throw
  dup 0x01020304 swap l! 
  dup 4 + 0xf4f3f2f1 swap l! 
  dup jvm_uw@
  assert( 0x0403 = )

  dup jvm_ul@
  assert( 0x04030201 = )
  
  dup 3 + jvm_uw@
  assert( 0x01f1 = )
  
  dup 2 + jvm_ul@
  assert( 0x0201f1f2 = )
  \ drop 
  free throw
;

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
  \ drop 
  free throw
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

  dup 2 + \ get the start of the const pool
  dup 1 jvm_constpool_idx
  assert( over = )
  dup 2 jvm_constpool_idx
  assert( over 3 + = )
  dup 1 jvm_constpool_idx
  assert( over 3 + <> )

  drop 
  free throw
;

: get_tag_test
  5 allocate throw
  dup      0x07 swap c! \ u1 tag
  dup 1  + 0xAAAAAAAA swap l! \ arbitrary data
  dup jvm_cp_tag 
  assert( 7 = )
  \ drop 
  free throw
;

: get_class_name_idx_test 
  12 allocate throw
  dup      0x0200 swap w! \ u2 constant pool count
  dup 2  + 0x07 swap c! \ u1 tag (class) 
  dup 3  + 0x0200 swap w! \ u2 name idx
  dup 5  + 0x01 swap c! \ u1 tag (utf8)
  dup 6  + 0x0400 swap w! \ u2 length
  dup 8  + 0x74747474 swap l! \ tttt

  dup 2 + \ get the start of the const pool
  jvm_cp_class_name_idx
  assert( 2 = )
  \ drop 
  free throw
;

: get_string_idx_test 
  12 allocate throw
  dup      0x0200 swap w! \ u2 constant pool count
  dup 2  + 0x08 swap c! \ u1 tag (string) 
  dup 3  + 0x0200 swap w! \ u2 name idx
  dup 5  + 0x01 swap c! \ u1 tag (utf8)
  dup 6  + 0x0400 swap w! \ u2 length
  dup 8  + 0x74747474 swap l! \ tttt

  dup 2 + \ get the start of the const pool
  jvm_cp_string_idx
  assert( 2 = )
  \ drop 
  free throw
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

  \ drop 
  free throw
;

: get_integer_bytes_test
  5 allocate throw
  dup     0x03 swap c! \ u1 tag (integer) 
  dup 1 + 0x0103070f swap l! \ u4 bytes

  dup jvm_cp_integer_bytes
  assert( 0x0f070301 = )
  \ drop 
  free throw
;

: get_float_bytes_test
  5 allocate throw
  dup     0x04 swap c! \ u1 tag (float) 
  dup 1 + 0x0103070f swap l! \ u4 bytes

  dup jvm_cp_float_bytes
  assert( 0x0f070301 = )
  \ drop 
  free throw
;

: get_long_bytes_test
  9 allocate throw
  dup     0x05 swap c! \ u1 tag (long) 
  dup 1 + 0x103070f0 swap l! \ u4 bytes high
  dup 5 + 0x0103070f swap l! \ u4 bytes low
  
  dup jvm_cp_long_bytes
  assert( 0x0f070301 = )
  assert( 0xf0703010 = )
  \ drop 
  free throw
;
: get_double_bytes_test
  9 allocate throw
  dup     0x06 swap c! \ u1 tag (long) 
  dup 1 + 0x103070f0 swap l! \ u4 bytes high
  dup 5 + 0x0103070f swap l! \ u4 bytes low
  
  dup jvm_cp_double_bytes
  assert( 0x0f070301 = )
  assert( 0xf0703010 = )
  \ drop 
  free throw
;

: get_utf8_test
  7 allocate throw
  dup     0x01 swap c! \ u1 tag (long) 
  dup 1 + 0x0400 swap w! \ u2 length  
  dup 3 + 0x65656565 swap l! \ bytes tttt
  
  dup jvm_cp_utf8_length
  assert( 4 = )

  dup jvm_cp_utf8_ref
  assert( over 3 + = )

  dup jvm_cp_utf8_c-ref
  assert( 4 = )
  assert( over 3 + = )

  free throw
;


cmp_utf8_test
constpool_idx_test
get_tag_test
get_class_name_idx_test
get_string_idx_test
ref_getter_test
get_integer_bytes_test
get_float_bytes_test
get_long_bytes_test
get_double_bytes_test
get_utf8_test

bye

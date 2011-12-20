\ vim: sw=2 ts=2 sta et
require ../jvm/classfile.fs

: big_endian_load_test
  8 allocate throw
  dup 0x01020304 swap l! 
  dup 4 + 0xf4f3f2f1 swap l! 
  dup jvm_uw@ dup
  assert( 0x0403 = ?bigendian or )
  assert( 0x0102 = ?littleendian or )

  dup jvm_ul@ dup
  assert( 0x04030201 = ?bigendian or )
  assert( 0x01020304 = ?littleendian or )
  
  dup 3 + jvm_uw@ dup
  assert( 0x01f1 = ?bigendian or )
  assert( 0x04f4 = ?littleendian or )
  
  dup 2 + jvm_ul@ dup
  assert( 0x0201f1f2 = ?bigendian or )
  assert( 0x0304f4f3 = ?littleendian or )
  \ drop 
  free throw
;

\ test utf8 compare
: cmp_utf8_test 
  \ NOTE: this is somehow cumbersome but
  \ it work no matter what the cell size is
  7 allocate throw
  dup     0x01 swap c! \ u1 tag
  dup 1 + 0x0004 swap w!-be \ u2 legth
  dup 3 + 0x54657374 swap l!-be \ Test

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
  dup      0x0002 swap w!-be \ u2 constant pool count
  dup 2  + 0x07 swap c! \ u1 tag (class) 
  dup 3  + 0x0002 swap w!-be \ u2 name idx
  dup 5  + 0x01 swap c! \ u1 tag (utf8)
  dup 6  + 0x0004 swap w!-be \ u2 length
  dup 8  + 0x74747474 swap l!-be \ tttt

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
  dup 1  + 0xAAAAAAAA swap l!-be \ arbitrary data
  dup jvm_cp_tag 
  assert( 7 = )
  \ drop 
  free throw
;

: get_class_name_idx_test 
  12 allocate throw
  dup      0x0002 swap w!-be \ u2 constant pool count
  dup 2  + 0x07 swap c! \ u1 tag (class) 
  dup 3  + 0x0002 swap w!-be \ u2 name idx
  dup 5  + 0x01 swap c! \ u1 tag (utf8)
  dup 6  + 0x0004 swap w!-be \ u2 length
  dup 8  + 0x74747474 swap l!-be \ tttt

  dup 2 + \ get the start of the const pool
  jvm_cp_class_name_idx
  assert( 2 = )
  \ drop 
  free throw
;

: get_string_idx_test 
  12 allocate throw
  dup      0x0002 swap w!-be \ u2 constant pool count
  dup 2  + 0x08 swap c! \ u1 tag (string) 
  dup 3  + 0x0002 swap w!-be \ u2 name idx
  dup 5  + 0x01 swap c! \ u1 tag (utf8)
  dup 6  + 0x0004 swap w!-be \ u2 length
  dup 8  + 0x74747474 swap l!-be \ tttt

  dup 2 + \ get the start of the const pool
  jvm_cp_string_idx
  assert( 2 = )
  \ drop 
  free throw
;


: ref_getter_test
  32 allocate throw
  dup      0x0007 swap w!-be \ u2 constant pool count
  \ idx 1
  dup 2  + 0x07 swap c! \ u1 tag (class) 
  dup 3  + 0x0002 swap w!-be \ u2 name idx
  \ idx 2
  dup 5  + 0x01 swap c! \ u1 tag (utf8)
  dup 6  + 0x0004 swap w!-be \ u2 length
  dup 8  + 0x74747474 swap l!-be \ tttt
  \ idx 3
  dup 12  + 0x09 swap c! \ u1 tag (fieldref) 
  dup 13 + 0x0001 swap w!-be \ u2 class idx
  dup 15 + 0x0006 swap w!-be \ u2 nametype idx
  \ idx 4
  dup 17  + 0x10 swap c! \ u1 tag (methoderef)
  dup 18 + 0x0001 swap w!-be \ u2 class idx
  dup 20 + 0x0006 swap w!-be \ u2 nametype idx
  \ idx 5
  dup 22  + 0x11 swap c! \ u1 tag (interfacemethodref) 
  dup 23 + 0x0001 swap w!-be \ u2 class idx
  dup 25 + 0x0006 swap w!-be \ u2 nametype idx
  \ idx 6
  dup 27  + 0x12 swap c! \ u1 tag (nametype) 
  dup 28 + 0xAAAA swap w!-be \ u2 name_idx idx (nonsens)
  dup 30 + 0x5555 swap w!-be \ u2 descriptor idx (nonsens)

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
  dup 1 + 0x0f070301 swap l!-be \ u4 bytes

  dup jvm_cp_integer_bytes
  assert( 0x0f070301 = )
  \ drop 
  free throw
;

: get_float_bytes_test
  5 allocate throw
  dup     0x04 swap c! \ u1 tag (float) 
  dup 1 + 0x0f070301 swap l!-be \ u4 bytes

  dup jvm_cp_float_bytes
  assert( 0x0f070301 = )
  \ drop 
  free throw
;

: get_long_bytes_test
  9 allocate throw
  dup     0x05 swap c! \ u1 tag (long) 
  dup 1 + 0xf0703010 swap l!-be \ u4 bytes high
  dup 5 + 0x0f070301 swap l!-be \ u4 bytes low
  
  dup jvm_cp_long_bytes
  assert( 0x0f070301 = )
  assert( 0xf0703010 = )
  \ drop 
  free throw
;
: get_double_bytes_test
  9 allocate throw
  dup     0x06 swap c! \ u1 tag (long) 
  dup 1 + 0xf0703010 swap l!-be \ u4 bytes high
  dup 5 + 0x0f070301 swap l!-be \ u4 bytes low
  
  dup jvm_cp_double_bytes
  assert( 0x0f070301 = )
  assert( 0xf0703010 = )
  \ drop 
  free throw
;

: get_utf8_test
  7 allocate throw
  dup     0x01 swap c! \ u1 tag (long) 
  dup 1 + 0x0004 swap w!-be \ u2 length
  dup 3 + 0x65656565 swap l!-be \ bytes tttt
  
  dup jvm_cp_utf8_length
  assert( 4 = )

  dup jvm_cp_utf8_ref
  assert( over 3 + = )

  dup jvm_cp_utf8_c-ref
  assert( 4 = )
  assert( over 3 + = )

  free throw
;



\ test classfile access stuff

: load_class_file
   s" Test.class" jvm_read_classfile throw 
   assert( filebuffer @ = )
;

: get_cf_magic_test
  filebuffer @ jvm_cf_magic
  assert( 0xCAFEBABE = )
;

: get_cf_minor_test
  filebuffer @ jvm_cf_minor_version
  assert( 0x0000 = )
;

: get_cf_major_test
  filebuffer @ jvm_cf_major_version
  assert( 0x0032 = )
;

: get_cf_constpool_count_test
  filebuffer @ jvm_cf_constpool_count
  assert( 28 = )
;

: get_cf_constpool_addr_test
  filebuffer @ jvm_cf_constpool_addr
  assert( filebuffer @ 10 + = )
;

: get_cf_access_flags_addr_test
  filebuffer @ jvm_cf_access_flags_addr
  assert( filebuffer @ 227 + = )
;

: get_cf_access_flags_test
  filebuffer @ jvm_cf_access_flags
  assert( 0x21 = )
;

: get_cf_this_class_test
  filebuffer @ jvm_cf_this_class
  assert( 5 = )
;

: get_cf_super_class_test
  filebuffer @ jvm_cf_super_class
  assert( 6 = )
;

: get_cf_interface_count_test
  filebuffer @ jvm_cf_interfaces_count
  assert( 1 = )
;

: get_cf_interface_addr_test
  filebuffer @ jvm_cf_interfaces_addr
  assert( filebuffer @ 235 + = )
;

: get_cf_fields_count_addr_test
  filebuffer @ jvm_cf_fields_count_addr
  assert( filebuffer @ 237 + = )
;

: get_cf_fields_count_test
  filebuffer @ jvm_cf_fields_count_addr
  assert( 2 = )
;

: get_cf_fields_addr_test
  filebuffer @ jvm_cf_fields_addr
  assert( filebuffer @ 239 + = )
;

: get_cf_fields_size_test
  filebuffer @ jvm_cf_fields_size
  assert( 16 = )
;

: get_cf_methods_count_addr_test
  filebuffer @ jvm_cf_methods_count_addr
  assert( filebuffer @ 255 + = )
;

: get_cf_methods_count_test
  filebuffer @ jvm_cf_methods_count
  assert( 3 = )
;

: get_cf_methods_addr_test
  filebuffer @ jvm_cf_methods_addr
  assert( filebuffer @ 257 + = )
;

: get_cf_methods_size_test
  filebuffer @ jvm_cf_methods_size
  assert( 138 = )
;

: get_cf_attr_count_addr_test
  filebuffer @ dup jvm_cf_attr_count_addr
  assert( filebuffer @ 395 + = )
;

: get_cf_attr_count_test
  filebuffer @ jvm_cf_attr_count
  assert( 1 = )
;

: get_cf_attr_addr_test
  filebuffer @ jvm_cf_attr_addr
  assert( filebuffer @ 397 + = )
;

: get_cf_attr_size_test
  filebuffer @ jvm_cf_attr_size
  assert( = 8 )
;



\ attr test

: get_attr_name_index
  filebuffer @ jvm_cf_fields_addr
  8 + 8 + \ 2 field, no attributes
  2 + \ methods count
  8 + \ first method first attribute
  jvm_attr_name_idx
  assert( 14 = ) \ Code
;

: get_attr_length
  filebuffer @ jvm_cf_fields_addr
  8 + 8 + \ 2 field, no attributes
  2 + \ methods count
  8 + \ first method first attribute
  jvm_attr_length
  assert( 29 = ) \ Code
;

: get_attr_info_addr
  filebuffer @ jvm_cf_fields_addr
  8 + 8 + \ 2 field, no attributes
  2 + \ methods count
  8 + \ first method first attribute
  dup jvm_attr_info_addr
  assert( swap 6 + = )
;

: get_attr_size
  filebuffer @ jvm_cf_fields_addr
  8 + 8 + \ 2 field, no attributes
  2 + \ methods count
  8 + \ first method first attribute
  jvm_attr_size
  assert( 35 = ) \ Code
;

\ field tests

: get_fd_access_flags
  filebuffer @ jvm_cf_fields_addr
  jvm_fd_access_flags
  assert( 9 = )
;

: get_fd_name_idx
  filebuffer @ jvm_cf_fields_addr
  jvm_fd_name_idx
  assert( 8 = )
;

: get_fd_desc_idx
  filebuffer @ jvm_cf_fields_addr
  jvm_fd_desc_idx
  assert( 9 = )
;

: get_fd_attr_count
  filebuffer @ jvm_cf_fields_addr
  jvm_fd_attr_count
  assert( 0 = )
;

: get_fd_size
  filebuffer @ jvm_cf_fields_addr
  jvm_fd_size
  assert( 8 = )
;

\ method tests

: get_md_access_flags
  filebuffer @ jvm_cf_methods_addr
  jvm_md_access_flags
  assert( 1 = )
;

: get_md_name_idx
  filebuffer @ jvm_cf_methods_addr
  jvm_md_name_idx
  assert( 12 = )
;

: get_md_desc_idx
  filebuffer @ jvm_cf_methods_addr
  jvm_md_desc_idx
  assert( 13 = )
;

: get_md_attr_count
  filebuffer @ jvm_cf_methods_addr
  jvm_md_attr_count
  assert( 1 = )
;

: get_md_size
  filebuffer @ jvm_cf_methods_addr
  jvm_md_size
  assert( 43 = )
;

: get_method_by_nametype_test
  filebuffer @ 
  s" test" s" (I)I" 
  jvm_get_method_by_nametype
  assert( ) \ assert found
  assert( filebuffer @ 300 + = )

  filebuffer @ 
  s" test" s" ()I"
  jvm_get_method_by_nametype
  assert( invert ) \ assert found
  assert( filebuffer @ jvm_cf_attr_count_addr = )

  filebuffer @ 
  s" test2" s" (I)I"
  jvm_get_method_by_nametype
  assert( invert ) \ assert found
  assert( filebuffer @ jvm_cf_attr_count_addr = )
  
  filebuffer @ 
  s" test2" s" ()I"
  jvm_get_method_by_nametype
  assert( invert ) \ assert found
  assert( filebuffer @ jvm_cf_attr_count_addr = )
;

: get_md_get_code_attr_test
  filebuffer @ 
  dup 
  s" test" s" (I)I" 
  jvm_get_method_by_nametype
  assert( ) \ assert found
  jvm_md_get_code_attr
  assert( filebuffer @ 322 + = )
;


\ --------------------------------------------------------------


big_endian_load_test
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

load_class_file
\ filebuffer @ is the address of the classfile buffer
get_cf_magic_test
get_cf_minor_test
get_cf_major_test
get_cf_constpool_count_test
get_cf_constpool_addr_test
get_cf_access_flags_addr_test
get_cf_access_flags_test
get_cf_this_class_test
get_cf_super_class_test
get_cf_interface_count_test
get_cf_interface_addr_test
get_cf_fields_count_addr_test
get_cf_fields_addr_test
get_cf_fields_size_test
get_cf_methods_count_addr_test
get_cf_methods_count_test
get_cf_methods_addr_test
get_cf_methods_size_test
get_cf_attr_count_addr_test
get_cf_attr_count_test
get_cf_attr_addr_test
get_cf_attr_size_test

get_attr_name_index
get_attr_length
get_attr_info_addr
get_attr_size

get_fd_access_flags
get_fd_name_idx
get_fd_desc_idx
get_fd_attr_count
get_fd_size

get_md_access_flags
get_md_name_idx
get_md_desc_idx
get_md_attr_count
get_md_size

get_method_by_nametype_test
get_md_get_code_attr_test

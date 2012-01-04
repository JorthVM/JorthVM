\ vim: sw=2 ts=2 sta et
require ../jvm/class.fs


: get_field_desc_size_test
  .s
  assert( depth 0 = )
  \ TODO find better names
  s" Bblblblbl" jvm_field_desc_size assert( 1 cells = )
  s" Cblblblbl" jvm_field_desc_size assert( 1 cells = )
  s" Dblblblbl" jvm_field_desc_size assert( 2 cells = )
  s" Fblblblbl" jvm_field_desc_size assert( 1 cells = )
  s" Iblblblbl" jvm_field_desc_size assert( 1 cells = )
  s" Jblblblbl" jvm_field_desc_size assert( 2 cells = )
  s" Lblblblbl" jvm_field_desc_size assert( 1 cells = )
  s" Sblblblbl" jvm_field_desc_size assert( 1 cells = )
  s" Sblblblbl" jvm_field_desc_size assert( 1 cells = )
  s" [blblblbl" jvm_field_desc_size assert( 1 cells = )
  assert( depth 0 = )
;

: field_load_test
  assert( depth 0 = )
  1 cells allocate throw
  dup >r
  jvm_field_@
  assert( depth 1 = )
  r> free throw
  clearstack

  assert( depth 0 = )
  
  2 cells allocate throw
  dup >r
  jvm_field_2@
  assert( depth 2 = )
  r> free throw
  clearstack
  assert( depth 0 = )
;

: field_store_test
  assert( depth 0 = )
  1 cells allocate throw
  dup >r
  42 over jvm_field_!
  jvm_field_@
  assert( depth 1 = )
  assert( 42 = )
  r> free throw

  assert( depth 0 = )

  2 cells allocate throw
  dup >r
  dup 42 0x42 rot
  jvm_field_2!
  jvm_field_2@
  assert( depth 2 = )
  assert( 0x42 = )
  assert( 42 = )
  r> free throw
  assert( depth 0 = )
;

: test
  get_field_desc_size_test
  field_load_test
  field_store_test
;

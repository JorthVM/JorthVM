\ vim: sw=2 ts=2 sta et
require ../jvm/classloader.fs

: classentry_new_entry_test
  assert( depth 0 = )
  jvm_classentry_list @ 
  assert( 0= )
  jvm_classentry.new() dup
  jvm_classentry_list @
  assert( = )
  jvm_classentry.new()
  swap
  jvm_classentry.getNext()
  assert( = )
  assert( depth 0 = )
;


classentry_new_entry_test

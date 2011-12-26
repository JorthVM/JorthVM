\ vim: sw=2 ts=2 sta et
require ../jvm/classloader.fs

: classentry_new_entry_test
  jvm_classentry_list @ 
  assert( 0= )
  jvm_classentry_new_entry
  jvm_classentry_list @
  assert( = )
;


classentry_new_entry_test

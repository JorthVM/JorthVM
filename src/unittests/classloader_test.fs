\ vim: sw=2 ts=2 sta et
require ../jvm/classloader.fs

: classpath_add_test
  s" testtest" jvm_classpath.add()
  jvm_classpath_list @ 
  jvm_classpath_entry.getName()
  s" testtest"
  compare
  assert( 0= )
  s" test2" jvm_classpath.add()
  jvm_classpath_list @ jvm_classpath_entry.getNext()
  jvm_classpath_entry.getName()
  s" test2"
  compare
  assert( 0= )
;

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

: class_lookup_test
  s" Test1" 0 jvm_class_add 
  s" Test2" 0 jvm_class_add 
  s" Test3" 0 jvm_class_add 
  
  s" Test1" 2dup jvm_class_lookup throw
  jvm_classentry.getName() 
  compare
  assert( 0= )
  
  try
    s" TestX" jvm_class_lookup throw
    restore
  endtry

  JVM_CLASSNOTFOUND_EXCEPTION
  assert( = )

  s" Test3" 2dup jvm_class_lookup throw
  jvm_classentry.getName() 
  compare
  assert( 0= )
  
  s" Test2" 2dup jvm_class_lookup throw
  jvm_classentry.getName() 
  compare
  assert( 0= )
  
;


classpath_add_test
classentry_new_entry_test
class_lookup_test

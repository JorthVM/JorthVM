\ vim: sw=2 ts=2 sta et
require ../jvm/classloader.fs

: classpath_add_test
  assert( depth 0 = )
  0 jvm_classpath_list ! \ reset classpath
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
  assert( depth 0 = )
;

: test
  classpath_add_test
;


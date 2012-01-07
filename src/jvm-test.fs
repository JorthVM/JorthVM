\ vim: sw=2 ts=2 sta et
include jvm/jvm.fs  \ include the jvm

: RunDemo ( -- )
  s" testfiles/" jvm_classpath.add()
  s" StaticIntOther" jvm_java
  \ show static values
  s" StaticIntOtherStore" jvm_stack.findClass() throw
  dup s" foo|I" 
  jvm_class.getStatic() throw
  ." StaticIntOtherStore.foo: " hex. CR
  s" bar|I" jvm_class.getStatic() throw
  ." StaticIntOtherStore.bar: " hex. CR
;


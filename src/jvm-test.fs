\ vim: sw=2 ts=2 sta et
include jvm/jvm.fs  \ include the jvm

: RunDemo ( -- )
  s" testfiles/" jvm_classpath.add()
  s" ./" jvm_classpath.add()
  s" StaticIntOther" jvm_java
  \ show static values
  s" StaticIntOtherStore" jvm_stack.findClass() throw
  dup s" foo|I" 
  jvm_class.getStatic() throw
  ." StaticIntOtherStore.foo: " hex. CR
  s" bar|I" jvm_class.getStatic() throw
  ." StaticIntOtherStore.bar: " hex. CR
;

: RunDemoObj ( -- )
  s" testfiles/" jvm_classpath.add()
  s" ./" jvm_classpath.add()
  s" ObjectCreation" jvm_java
  s" ObjectCreation" jvm_stack.findClass() throw
  s" checkMe|I"
  jvm_class.getStatic() throw
  ." ObjectCreation.checkMe: " hex. CR
;

: RunDemoNative ( -- )
  s" testfiles/" jvm_classpath.add()
  s" ./" jvm_classpath.add()
  s" NativeTest1" jvm_java
  s" NativeTest1" jvm_stack.findClass() throw
  s" a|I" jvm_class.getStatic() throw
  ." NativeTest1.a: " hex. CR
;

: RunDemoPrintln ( -- )
  s" testfiles/" jvm_classpath.add()
  s" ./" jvm_classpath.add()
  s" StringTest1" jvm_java
;

: RunDemoString ( -- )
  s" testfiles/" jvm_classpath.add()
  s" ./" jvm_classpath.add()
  s" StringTest3" jvm_java
;

: RunDemoStringBuilder ( -- )
  s" testfiles/" jvm_classpath.add()
  s" ./" jvm_classpath.add()
  s" StringTest2" jvm_java
;

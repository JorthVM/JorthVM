\ vim: sw=2 ts=2 sta et
include jvm/jvm.fs  \ include the jvm

: RunDemo ( -- )
  s" testfiles/" jvm_classpath.add()
  s" Main" jvm_java
  \ show static values
  jvm_stack.getCurrentFrame()
  jvm_frame.getClass()
  dup s" foo|I" 
  jvm_class.getStatic() throw
  ." Main.foo: " hex. CR
  s" bar|I" jvm_class.getStatic() throw
  ." Main.bar: " hex. CR
;


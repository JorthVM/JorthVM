\ vim: sw=2 ts=2 sta et
include jvm/jvm.fs  \ include the jvm

jvm_init            \ initialize the jvm

: RunDemo ( -- )
  s" testfiles/" jvm_classpath.add()
  s" Main" jvm_java
;


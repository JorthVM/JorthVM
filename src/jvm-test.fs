include jvm/jvm.fs  \ include the jvm

jvm_init            \ initialize the jvm

: RunDemo ( -- )
  s" testfiles/Main.class" LoadAndRun
;


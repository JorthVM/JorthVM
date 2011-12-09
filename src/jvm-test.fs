include jvm/jvm.fs  \ include the jvm

jvm_init            \ initialize the jvm

: RunDemo ( -- )
   s" testfiles/Main.class" jvm_read_classfile drop \ drop file size
   filebuffer @ 278 + jvm_set_pc  \ TODO hardcoded
   jvm_run 
;


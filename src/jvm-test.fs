include jvm/jvm.fs  \ include the jvm

jvm_init            \ initialize the jvm

: RunDemo ( -- )
   s" testfiles/Main.class" jvm_read_classfile drop
   \ filebuffer @ jvm_print_classfile  
   filebuffer @ 278 + jvm_set_pc  \ TODO set jvm program counter to program
   jvm_run 
;


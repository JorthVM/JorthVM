include jvm/jvm.fs

jvm_init

create program 
0xCA60592A10 , \ bipush 42, dup, iadd (bipush=0x10)
0xCA , \ breakpoint
program jvm_set_pc

\ jvm_run

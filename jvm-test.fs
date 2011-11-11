include jvm/jvm.fs

jvm_init_ops

create program 
0x60606060 , \ iadd
program jvm_set_pc

5 5

jvm_fetch_instruction 
\ jvm_execute

.s

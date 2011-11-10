require jvm/jvm.fs

create program 
0x60 , \ iadd
program jvm_set_pc

5 5

jvm_fetch_instruction 
jvm_execute

.s

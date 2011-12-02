include jvm/jvm.fs  \ include the jvm

jvm_init            \ initialize the jvm

create program 
0xCA60592A10 ,      \ bipush 42, dup, iadd (bipush=0x10)
0xCA ,              \ breakpoint
program jvm_set_pc  \ set jvm program counter to program
                    \ (first opcode 0x10)

jvm_run             \ start the execution

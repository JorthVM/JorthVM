include jvm/jvm.fs  \ include the jvm

jvm_init            \ initialize the jvm

create program 5 cells allot
0x102a5960 program l!-be \ bipush 42, dup, iadd (bipush=0x10)
0xca program 4 + c!      \ breakpoint
program jvm_set_pc  \ set jvm program counter to program
                    \ (first opcode 0x10)

\ jvm_run             \ start the execution

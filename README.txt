Forth JVM - A Java Virtual Machine written in Forth

GENERAL INFORMATION
===================
Forth JVM is an implementation of the Java Virual Machine written in Forth.


SPECIFICATION
=============
- [JVMspec][] The Java&trade; Virtual Machine Specification

[JVMspec]: http://java.sun.com/docs/books/jvms/second_edition/html/VMSpecTOC.doc.html 

INSTALL
=======
TODO

Requirements
------------
- Gforth

Compile & install
-----------------
FIXME

Run unittests
-------------
Run `make test`.


USAGE
=====
Example:

    include jvm/jvm.fs  \ include the jvm

    jvm_init            \ initialize the jvm

    create program 
    0xCA60592A10 ,      \ bipush 42, dup, iadd (bipush=0x10)
    0xCA ,              \ breakpoint
    program jvm_set_pc  \ set jvm program counter to program
                        \ (first opcode 0x10)

    jvm_run             \ start the execution


WARNINGS
========
The program is distributed WITHOUT ANY WARRANTY.


LICENSING INFORMATION
=====================
Copyright (C) 2011, 2012 Sebastian Rumpl, Bernhard Urban, Josef Eisl


Forth JVM is written by 

- Sebastian Rumpl <e0828489@student.tuwien.ac.at>
- Bernhard Urban <lewurm@gmail.com>
- Josef Eisl <josef.eisl@student.tuwien.ac.at>

Copyright (C) 2011, 2012 Sebastian Rumpl, Bernhard Urban, Josef Eisl


Forth JVM - A Java Virtual Machine written in Forth

GENERAL INFORMATION
===================
Forth JVM is an implementation of the Java Virual Machine written in Forth.


SPECIFICATION
=============
- [The Java Virtual Machine Specification](http://java.sun.com/docs/books/jvms/second_edition/html/ClassFile.doc.html)
- [Forth JVM implementation documenation](implementation.html)


DOCUMENTATION
=============

The Forth JVM documentation is contained in _this file_ as well as in [HEADER.txt](HEADER.txt). These
files are formated using the [markdown](http://daringfireball.net/projects/markdown/) markup language.
A HTML version of the documentation can be created by `make doc`.


INSTALL
=======
TODO

Requirements
------------
- Gforth
- markdown (optional, for HTML documentation)

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


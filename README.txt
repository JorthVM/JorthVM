JorthVM - A Java Virtual Machine written in Forth

GENERAL INFORMATION
===================
JorthJVM is an implementation of the Java Virual Machine written in Forth.


USAGE
=====
go into src/ and type `make', then you get the gforth prompt

look in ./src/jvm-test.fs for usage examples. (e.g. RunDemo)

IMPLEMENTED
===========
- class loading, classfile parsing
- static field access and calling static methods
	o static initializer
- object instantiation: field access and method calling
- inheritance
- native calls, using forth words

TODO
====
- exception handling
- interfaces
- implmement more instructions
- more java.*;

SPECIFICATION
=============
- [The Java Virtual Machine Specification](http://java.sun.com/docs/books/jvms/second_edition/html/ClassFile.doc.html)
- [Forth JVM implementation documentation](implementation.html)


DOCUMENTATION
=============

JorthVM documentation is contained in _this file_ as well as in [HEADER.txt](HEADER.txt). These
files are formated using the [markdown](http://daringfireball.net/projects/markdown/) markup language.
A HTML version of the documentation can be created by `make doc`.


INSTALL
=======
TODO

Requirements
------------
- Gforth
- javac
- markdown (optional, for HTML documentation)
- qemu-kvm-extra (ubuntu; optional, for cross testing)

Compile & install
-----------------
FIXME

Run unittests
-------------
Run `make test`.

WARNINGS
========
The program is distributed WITHOUT ANY WARRANTY.


LICENSING INFORMATION
=====================
Copyright (C) 2011, 2012 Sebastian Rumpl, Bernhard Urban, Josef Eisl


Forth JVM is written by 

- Sebastian Rumpl <e0828489@student.tuwien.ac.at>
- Bernhard Urban <lewurm@gmail.com>
- Josef Eisl <zapster@zapster.cc>


JorthVM - A Java Virtual Machine written in Forth

GENERAL INFORMATION
===================
JorthVM is an implementation of the Java Virual Machine written in Forth.
Join us at #JorthVM on irc.oftc.net !

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

FIXME's
-------
- mnemonics -> dictionary
- documentation -> gforth docs
- License headers (This program -> JorthVM, remove email addresses)
- recursion
- regression test clean up
- long support (testing?)
- float/double
- documentation (rename HEADER)
- GNU classpath integration

Features
--------
- string stuff
- array stuff
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

JorthVM documentation is contained in _this file_ as well as in [HEADER.md](HEADER.md). These
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

JorthVM is distributed under the GNU General Public license (see COPYING).

JorthVM is written by 

- Sebastian Rumpl
- Bernhard Urban
- Josef Eisl


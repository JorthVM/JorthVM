JorthVM - A Java Virtual Machine written in Forth

GENERAL INFORMATION
===================
JorthVM is an implementation of the Java Virtual Machine written in Forth.
Join us at #JorthVM on irc.oftc.net !

USAGE
=====
go into `src/` and type `make`, then you get the gforth prompt

look in `./src/jvm-test.fs` for usage examples. (e.g. `RunDemo`)

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

See [TODOs](TODO.md).

SPECIFICATION
=============
- [The Java Virtual Machine Specification](http://java.sun.com/docs/books/jvms/second_edition/html/ClassFile.doc.html)
- [Forth JVM implementation documentation](implementation.md)


DOCUMENTATION
=============

JorthVM documentation is contained in [_this file_](README.md) as well as in [implementation.md](implementation.md). These
files are formated using the [markdown](http://daringfireball.net/projects/markdown/) markup language.

INSTALL
=======

Installation is not neccessary. Just run the code as stated in the usage section.

Requirements
============

- Gforth
- javac
- qemu-kvm-extra (ubuntu; optional, for cross testing)

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


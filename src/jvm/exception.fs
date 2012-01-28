\ vim: sw=2 ts=2 sta et


\ Copyright (C) 2011-2012 Sebastian Rumpl, Josef Eisl, Bernhard Urban

\ This file is part of JorthVM

\ JorthVM is free software: you can redistribute it and/or modify
\ it under the terms of the GNU General Public License as published by
\ the Free Software Foundation, either version 3 of the License, or
\ (at your option) any later version.

\ This program is distributed in the hope that it will be useful,
\ but WITHOUT ANY WARRANTY; without even the implied warranty of
\ MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
\ GNU General Public License for more details.

\ You should have received a copy of the GNU General Public License
\ along with this program.  If not, see <http://www.gnu.org/licenses/>.
\ this file implements functionality that is needed to read class files

\ ========
\ *! exception
\ *T Exceptions
\ ========


\ *S JVM Exceptions
s" JVM Breakpoint Exception" exception constant JVM_BREAKPOINT_EXCEPTION
\ *G
s" JVM Not Yet Implemented Exception" exception constant JVM_NOTIMPLEMENTED_EXCEPTION
\ *G
s" JVM public static main(String[]) not found Exception" exception constant JVM_MAINNOTFOUND_EXCEPTION
\ *G
s" JVM class not found Exception" exception constant JVM_CLASSNOTFOUND_EXCEPTION
\ *G

s" JVM word not found Exception" exception constant JVM_WORDNOTFOUND_EXCEPTION
s" JVM native word not found Exception" exception constant JVM_NATIVENOTFOUND_EXCEPTION
\ *G
s" JVM Classfile: unknown Constant Pool Type Exception" exception constant JVM_UNKNOWNCONSTPOOLTYPE_EXCEPTION
\ *G

\ *S Specified exceptions
s" LinkerError" exception constant JVM_LINKERERROR
\ *G
s" ClassFormatError" exception constant JVM_CLASSFORMATERROR
\ *G
s" UnsupportedClassVersionError" exception constant JVM_UNSUPPORTEDCLASSVERSIONERROR
\ *G
s" NoClassDefError" exception constant JVM_NOCLASSDEFERROR
\ *G

\ *S Temporary Exceptions
s" JVM Return Exception" exception constant JVM_RETURN_EXCEPTION
\ *G

\ ======
\ *> ###
\ ======


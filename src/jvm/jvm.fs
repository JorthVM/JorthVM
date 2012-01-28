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
\ *! jvm
\ *T JVM
\ ========


require decode.fs
require frame.fs
require classfile.fs
require classloader.fs
require rtconstpool.fs
require class.fs
require frame.fs
require execute.fs
require stack.fs

: jvm_java { c-addr n -- }
  \ add class to jvm stack
\ FIXME bug: if the classfile is not on the classpath before running jvm_java
\ it is never found, even if the correct classpath is added and jvm_java is restarted
  c-addr n jvm_stack.newClass()

  c-addr n 
  jvm_stack.invokeInitial()

;

: java ( "classname" -- )
\ *G start the following class by invoking public static void main(String[])
\ FIXME do cmd input handling (create string array etc.)
  parse-name 
  jvm_java
;

: classpath ( "classpath" -- )
\ *G add a directory to the classpath
  parse-name
  jvm_classpath.add()
;

\ ======
\ *> ###
\ ======


\ vim: sw=2 ts=2 sta et

\ `JorthVM', a Java VM implemented in Forth
\ 
\ Copyright (C) 2012 Sebastian Rumpl <e0828489@student.tuwien.ac.at>
\ Copyright (C) 2012 Josef Eisl <zapster@zapster.cc>
\ Copyright (C) 2012 Bernhard Urban <lewurm@gmail.com>
\ 
\ This program is free software: you can redistribute it and/or modify
\ it under the terms of the GNU General Public License as published by
\ the Free Software Foundation, either version 3 of the License, or
\ (at your option) any later version.
\ 
\ This program is distributed in the hope that it will be useful,
\ but WITHOUT ANY WARRANTY; without even the implied warranty of
\ MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
\ GNU General Public License for more details.
\ 
\ You should have received a copy of the GNU General Public License
\ along with this program.  If not, see <http://www.gnu.org/licenses/>.
require ../jvm/frame.fs


: numberOfParameters_test
  \ single parameter
  s" (B)" jvm_frame.numberOfParamters() assert( 1 = )
  s" (C)" jvm_frame.numberOfParamters() assert( 1 = )
  s" (D)" jvm_frame.numberOfParamters() assert( 2 = )
  s" (F)" jvm_frame.numberOfParamters() assert( 1 = )
  s" (I)" jvm_frame.numberOfParamters() assert( 1 = )
  s" (J)" jvm_frame.numberOfParamters() assert( 2 = )
  s" (Ljava/lang/String;)" jvm_frame.numberOfParamters() assert( 1 = )
  s" (S)" jvm_frame.numberOfParamters() assert( 1 = )
  s" (Z)" jvm_frame.numberOfParamters() assert( 1 = )
  s" ([D)" jvm_frame.numberOfParamters() assert( 1 = )
  s" ([[[[[[[[[[D)" jvm_frame.numberOfParamters() assert( 1 = )
  s" ([[[[[[[[[[Ljava/lang/String;)" jvm_frame.numberOfParamters() assert( 1 = )

  \ multiparameter
  s" (IDLjava/lang/Thread;)Ljava/lang/Object;" jvm_frame.numberOfParamters() assert( 4 = )
  s" (I[[DLjava/lang/Thread;)Ljava/lang/Object;" jvm_frame.numberOfParamters() assert( 3 = )

  \ negativ test
  try
    s" B)" jvm_frame.numberOfParamters() assert( 1 = )
    -1
    iferror 
      dup -1 = IF drop 0 ENDIF
    endif
  endtry
  throw
;

: test
  numberOfParameters_test
;

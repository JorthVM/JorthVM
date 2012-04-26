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

\ ========
\ *! array
\ *T JVM representation of a Arrays
\ 
\ *P This file contains the array represenation in the JVM
\ ========

require exception.fs
require classfile.fs
require rtconstpool.fs

\ ========
\ *S Array Types
\ ========
\ 
\ *C T_BOOLEAN   4
\ *C T_CHAR      5
\ *C T_FLOAT     6
\ *C T_DOUBLE    7
\ *C T_BYTE      8
\ *C T_SHORT     9
\ *C T_INT      10
\ *C T_LONG     11

 4 constant T_BOOLEAN
 5 constant T_CHAR
 6 constant T_FLOAT
 7 constant T_DOUBLE
 8 constant T_BYTE
 9 constant T_SHORT
10 constant T_INT
11 constant T_LONG

: jvm_array_element_size ( atype -- u )
\ *G Get the size of an array element (in Bytes)

\ TODO be more efficient (e.g. on 64 bit systems)
  CASE
    T_BOOLEAN OF 1 cells ENDOF
    T_CHAR    OF 1 cells ENDOF
    T_FLOAT   OF 1 cells ENDOF
    T_DOUBLE  OF 1 cells ENDOF
    T_BYTE    OF 1 cells ENDOF
    T_SHORT   OF 1 cells ENDOF
    T_INT     OF 1 cells ENDOF
    T_LONG    OF 1 cells ENDOF
    \ default
    abort
  ENDCASE
;
 
 0 cells constant jvm_array.t_atype
 1 cells constant jvm_array.t_count
 2 cells constant jvm_array.t_data

 2 cells constant jvm_array.header_size()

: jvm_array.new() { count atype -- addr }
\ *G Create a new (uninitialized) array of type atype with count elements
  atype jvm_array_element_size
  ( el_size -- type )
  count *
  ( array_data_size -- type )
  jvm_array.header_size() +
  ( array_size -- type )
  allocate throw
  ( addr )
  atype over
  ( addr atype addr )
  jvm_array.t_atype +
  ( addr atype addr_atype )
  !
  ( addr )
  count over
  ( addr count addr )
  jvm_array.t_count +
  ( addr count addr_count )
  !
  ( addr )
  dup jvm_array.header_size() + count atype jvm_array_element_size
  ( addr addr_data count el_size )
  * 0
  ( addr addr_data array_data_size 0 )
  fill
  ( addr )
;

: jvm_array.store() { arrayref index val -- }
\ FIXME If arrayref is null, iastore throws a NullPointerException.
\ FIXME  Otherwise, if index is not within the bounds of the array 
\ referenced by arrayref, the iastore instruction throws an ArrayIndexOutOfBoundsException. 

  arrayref 
  jvm_array.t_atype + @ 
  jvm_array_element_size
  ( el_size )
  index over *
  ( el_size offset )
  arrayref jvm_array.header_size() + +
  val
  ( el_size el_address val )
  swap rot
  ( val el_address el_size )
  CASE
    1 cells OF ! ENDOF
    \ default
     ." Stack: " .s CR
    abort
  ENDCASE
;

: jvm_array.load() { arrayref index -- val }
\ FIXME If arrayref is null, iaload throws a NullPointerException.
\ FIXME Otherwise, if index is not within the bounds of the array referenced 
\ by arrayref, the iaload instruction throws an ArrayIndexOutOfBoundsException.
  arrayref 
  jvm_array.t_atype + @ 
  jvm_array_element_size
  ( el_size )
  index over *
  ( el_size offset )
  arrayref jvm_array.header_size() + +
  ( el_size el_address )
  swap
  ( el_address el_size )
  CASE
    1 cells OF @ ENDOF
    \ default
     ." Stack: " .s CR
    abort
  ENDCASE
;




\ ======
\ *> ###
\ ======


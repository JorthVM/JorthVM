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
\ this file implements functionality that is needed to read class files

\ ========
\ *! execute
\ *T JVM Instruction Implementation
\ ========

require stack.fs
require exception.fs

: jvm_exec.getClassAndNameForField() ( addr_rtcp idx -- addr_cl c-addr n )
  dup rot
  ( addr_rtcp addr_rtcp idx )
  jvm_rtcp.getConstpoolByIdx()
  ( addr_rtcp addr_fd )
  over -rot
  ( addr_rtcp addr_rtcp addr_fd )
  dup jvm_cp_tag assert( CONSTANT_Fieldref = )
  ( addr_rtcp addr_rtcp addr_fieldref )
  \ check class
  dup
  ( addr_rtcp addr_rtcp addr_fieldref addr_fieldref )
  jvm_cp_fieldref_nametype_idx
  swap
  jvm_cp_fieldref_class_idx
  ( addr_rtcp addr_rtcp nametype_idx class_idx)
  rot swap
  ( addr_rtcp nametype_idx addr_rtcp class_idx)
  jvm_rtcp.getClassName()
  \ 2dup type CR
  ( addr_rtcp nametype_idx c-addr1 n1)
  2swap
  ( c-addr1 n1 addr_rtcp nametype_idx)
  jvm_rtcp.getNameType()
  ( c-addr1 n1 c-addr2 n2)
  \ 2dup type CR
  ( c-addr1 n1 c-addr2 n2)
  2swap
  jvm_stack.findAndInitClass() throw
  -rot
;


: jvm_not_implemented ( -- wior )
\ *G instruction not implemented
  CR CR 
  ." The following instruction not yet implemented: " 
  jvm_stack.currentInstruction()
  jvm_decode.mnemonic() type 
  CR 
  ." Stack: " .s 
  CR CR
  JVM_NOTIMPlEMENTED_EXCEPTION throw
;

0x32 0 s" aaload" \ ( arrayref, index -- value )
\ load onto the stack a reference from an array
>[ jvm_not_implemented ]<

0x53 0 s" aastore" \ ( arrayref, index, value -- )
\ store into a reference in an array
>[ jvm_not_implemented ]<

0x01 0 s" aconst_null" \ ( -- null )
\ push a null reference onto the stack
>[
  0
]<

0x19 1 s" aload" \ 1[index] ( -- objectref )
\ load a reference onto the stack from a local variable #index
>[ jvm_not_implemented ]<

0x2A 0 s" aload_0" \ ( -- objectref )
\ load a reference onto the stack from local variable 0
>[
  jvm_stack.getCurrentFrame()
  0 jvm_frame.getLocal()
]<

0x2B 0 s" aload_1" \ ( -- objectref )
\ load a reference onto the stack from local variable 1
>[
  jvm_stack.getCurrentFrame()
  1 jvm_frame.getLocal()
]<

0x2C 0 s" aload_2" \ ( -- objectref )
\ load a reference onto the stack from local variable 2
>[ jvm_not_implemented ]<

0x2D 0 s" aload_3" \ ( -- objectref )
\ load a reference onto the stack from local variable 3
>[ jvm_not_implemented ]<

0xBD 2 s" anewarray" \  2[indexbyte1, indexbyte2] ( count -- arrayref )
\ create a new array of references of length count and component type
\ identified by the class reference index (indexbyte1 ^> 8 + indexbyte2)
\ in the constant pool
>[ jvm_not_implemented ]<

0xB0 0 s" areturn" \ ( objectref -- [empty] )
\ return a reference from a method
>[ jvm_not_implemented ]<

0xBE 0 s" arraylength" \ ( arrayref -- length )
\ get the length of an array
>[ jvm_not_implemented ]<

0x3A 1 s" astore" \ 1[index] ( objectref -- )
\ store a reference into a local variable #index
>[ jvm_not_implemented ]<

0x4B 0 s" astore_0" \ ( objectref -- )
\ store a reference into local variable 0
>[ jvm_not_implemented ]<

0x4C 0 s" astore_1" \ ( objectref -- )
\ store a reference into local variable 1
>[
  jvm_stack.getCurrentFrame()
  1 jvm_frame.setLocal()
]<

0x4D 0 s" astore_2" \ ( objectref -- )
\ store a reference into local variable 2
>[ jvm_not_implemented ]<

0x4E 0 s" astore_3" \ ( objectref -- )
\ store a reference into local variable 3
>[ jvm_not_implemented ]<

0xBF 0 s" athrow" \ ( objectref -- [empty], objectref )
\ throws an error or exception (notice that the rest of the stack is cleared,
\ leaving only a reference to the Throwable)
>[ jvm_not_implemented ]<

0x33 0 s" baload" \ ( arrayref, index -- value )
\ load a byte or Boolean value from an array
>[ jvm_not_implemented ]<

0x54 0 s" bastore" \ ( arrayref, index, value -- )
\ store a byte or Boolean value into an array
>[ jvm_not_implemented ]<

0x10 1 s" bipush" \ 1[byte] ( -- value )
\ push a byte onto the stack as an integer value
>[
  jvm_stack.fetchByte() \ load byte
  dup 0x80 and \ sign ext
  if
  [ -1 8 lshift ] literal or
  endif
]<

0x34 0 s" caload" \ ( arrayref, index -- value )
\ load a char from an array
>[ jvm_not_implemented ]<

0x55 0 s" castore" \ ( arrayref, index, value -- )
\ store a char into an array
>[
  -rot + c!
]<

0xC0 2 s" checkcast" \ 2[indexbyte1, indexbyte2] ( objectref -- objectref )
\ checks whether an objectref is of a certain type, the class reference of
\ which is in the constant pool at index (indexbyte1 ^> 8 + indexbyte2)
>[ jvm_not_implemented ]<

0x90 0 s" d2f" \ ( value -- result )
\ convert a double to a float
>[ jvm_not_implemented ]<

0x8E 0 s" d2i" \ ( value -- result )
\ convert a double to an int
>[ jvm_not_implemented ]<

0x8F 0 s" d2l" \ ( value -- result )
\ convert a double to a long
>[ jvm_not_implemented ]<

0x63 0 s" dadd" \ ( value1, value2 -- result )
\ add two doubles
>[ jvm_not_implemented ]<

0x31 0 s" daload" \ ( arrayref, index -- value )
\ load a double from an array
>[ jvm_not_implemented ]<

0x52 0 s" dastore" \ ( arrayref, index, value -- )
\ store a double into an array
>[ jvm_not_implemented ]<

0x98 0 s" dcmpg" \ ( value1, value2 -- result )
\ compare two doubles
>[ jvm_not_implemented ]<

0x97 0 s" dcmpl" \ ( value1, value2 -- result )
\ compare two doubles
>[ jvm_not_implemented ]<

0x0E 0 s" dconst_0" \ ( -- 0.0 )
\ push the constant 0.0 onto the stack
>[ jvm_not_implemented ]<

0x0F 0 s" dconst_1" \ ( -- 1.0 )
\ push the constant 1.0 onto the stack
>[ jvm_not_implemented ]<

0x6F 0 s" ddiv" \ ( value1, value2 -- result )
\ divide two doubles
>[ jvm_not_implemented ]<

0x18 1 s" dload" \ 1[index] ( -- value )
\ load a double value from a local variable #index
>[ jvm_not_implemented ]<

0x26 0 s" dload_0" \ ( -- value )
\ load a double from local variable 0
>[ jvm_not_implemented ]<

0x27 0 s" dload_1" \ ( -- value )
\ load a double from local variable 1
>[ jvm_not_implemented ]<

0x28 0 s" dload_2" \ ( -- value )
\ load a double from local variable 2
>[ jvm_not_implemented ]<

0x29 0 s" dload_3" \ ( -- value )
\ load a double from local variable 3
>[ jvm_not_implemented ]<

0x6B 0 s" dmul" \ ( value1, value2 -- result )
\ multiply two doubles
>[ jvm_not_implemented ]<

0x77 0 s" dneg" \ ( value -- result )
\ negate a double
>[ jvm_not_implemented ]<

0x73 0 s" drem" \ ( value1, value2 -- result )
\ get the remainder from a division between two doubles
>[ jvm_not_implemented ]<

0xAF 0 s" dreturn" \ ( value -- [empty] )
\ return a double from a method
>[ jvm_not_implemented ]<

0x39 1 s" dstore" \ 1[index] ( value -- )
\ store a double value into a local variable #index
>[ jvm_not_implemented ]<

0x47 0 s" dstore_0" \ ( value -- )
\ store a double into local variable 0
>[ jvm_not_implemented ]<

0x48 0 s" dstore_1" \ ( value -- )
\ store a double into local variable 1
>[ jvm_not_implemented ]<

0x49 0 s" dstore_2" \ ( value -- )
\ store a double into local variable 2
>[ jvm_not_implemented ]<

0x4A 0 s" dstore_3" \ ( value -- )
\ store a double into local variable 3
>[ jvm_not_implemented ]<

0x67 0 s" dsub" \ ( value1, value2 -- result )
\ subtract a double from another
>[ jvm_not_implemented ]<

\ FIXME workaround: s/dup/jdup.  seperate dict?
0x59 0 s" jdup" \ ( value -- value, value )
\ duplicate the value on top of the stack
>[ dup ]<

0x5A 0 s" dup_x1" \ ( value2, value1 -- value1, value2, value1 )
\ insert a copy of the top value into the stack two values from the top
>[ jvm_not_implemented ]<

0x5B 0 s" dup_x2" \ ( value3, value2, value1 -- value1, value3, value2, value1 )
\ insert a copy of the top value into the stack two (if value2 is double or
\ long it takes up the entry of value3, too) or three values (if value2 is
\ neither double nor long) from the top
>[ jvm_not_implemented ]<

0x5C 0 s" dup2" \ ( {value2, value1} -- {value2, value1}, {value2, value1} )
\ duplicate top two stack words (two values, if value1 is not double nor long;
\ a single value, if value1 is double or long)
>[ jvm_not_implemented ]<

0x5D 0 s" dup2_x1" \ ( value3, {value2, value1} -- {value2, value1}, value3, {value2, value1} )
\ duplicate two words and insert beneath third word (see explanation above)
>[ jvm_not_implemented ]<

0x5E 0 s" dup2_x2" \ ( {value4, value3}, {value2, value1} -- {value2, value1}, {value4, value3}, {value2, value1} )
\ duplicate two words and insert beneath fourth word
>[ jvm_not_implemented ]<

0x8D 0 s" f2d" \ ( value -- result )
\ convert a float to a double
>[ jvm_not_implemented ]<

0x8B 0 s" f2i" \ ( value -- result )
\ convert a float to an int
>[ jvm_not_implemented ]<

0x8C 0 s" f2l" \ ( value -- result )
\ convert a float to a long
>[ jvm_not_implemented ]<

0x62 0 s" fadd" \ ( value1, value2 -- result )
\ add two floats
>[ jvm_not_implemented ]<

0x30 0 s" faload" \ ( arrayref, index -- value )
\ load a float from an array
>[ jvm_not_implemented ]<

0x51 0 s" fastore" \ ( arrayref, index, value -- )
\ store a float in an array
>[ jvm_not_implemented ]<

0x96 0 s" fcmpg" \ ( value1, value2 -- result )
\ compare two floats
>[ jvm_not_implemented ]<

0x95 0 s" fcmpl" \ ( value1, value2 -- result )
\ compare two floats
>[ jvm_not_implemented ]<

0x0B 0 s" fconst_0" \ ( -- 0.0f )
\ push 0.0f on the stack
>[ jvm_not_implemented ]<

0x0C 0 s" fconst_1" \ ( -- 1.0f )
\ push 1.0f on the stack
>[ jvm_not_implemented ]<

0x0D 0 s" fconst_2" \ ( -- 2.0f )
\ push 2.0f on the stack
>[ jvm_not_implemented ]<

0x6E 0 s" fdiv" \ ( value1, value2 -- result )
\ divide two floats
>[ jvm_not_implemented ]<

0x17 1 s" fload" \ ( 1: index 	-- value )
\ load a float value from a local variable #index
>[ jvm_not_implemented ]<

0x22 0 s" fload_0" \ ( -- value )
\ load a float value from local variable 0
>[ jvm_not_implemented ]<

0x23 0 s" fload_1" \ ( -- value )
\ load a float value from local variable 1
>[ jvm_not_implemented ]<

0x24 0 s" fload_2" \ ( -- value )
\ load a float value from local variable 2
>[ jvm_not_implemented ]<

0x25 0 s" fload_3" \ ( -- value )
\ load a float value from local variable 3
>[ jvm_not_implemented ]<

0x6A 0 s" fmul" \ ( value1, value2 -- result )
\ multiply two floats
>[ jvm_not_implemented ]<

0x76 0 s" fneg" \ ( value -- result )
\ negate a float
>[ jvm_not_implemented ]<

0x72 0 s" frem" \ ( value1, value2 -- result )
\ get the remainder from a division between two floats
>[ jvm_not_implemented ]<

0xAE 0 s" freturn" \ ( value -- [empty] )
\ return a float
>[ jvm_not_implemented ]<

0x38 1 s" fstore" \ 1[index] ( value -- )
\ store a float value into a local variable #index
>[ jvm_not_implemented ]<

0x43 0 s" fstore_0" \ ( value -- )
\ store a float value into local variable 0
>[ jvm_not_implemented ]<

0x44 0 s" fstore_1" \ ( value -- )
\ store a float value into local variable 1
>[ jvm_not_implemented ]<

0x45 0 s" fstore_2" \ ( value -- )
\ store a float value into local variable 2
>[ jvm_not_implemented ]<

0x46 0 s" fstore_3" \ ( value -- )
\ store a float value into local variable 3
>[ jvm_not_implemented ]<

0x66 0 s" fsub" \ ( value1, value2 -- result )
\ subtract two floats
>[ jvm_not_implemented ]<

0xB4 2 s" getfield" \ 2[index1, index2] ( objectref -- value )
\ get a field value of an object objectref, where the field is identified by
\ field reference in the constant pool index (index1 ^> 8 + index2)
>[
  jvm_stack.fetchShort()
  jvm_stack.getCurrentFrame()
  jvm_frame.getClass()
  jvm_class.getRTCP()
  ( this addr_rtcp idx )
  jvm_exec.getClassAndNameForField()
  ( this addr_cl c-addr n)
  jvm_class.getField_offset() throw
  ( this off )
  + @
]<

0xB2 2 s" getstatic" \ 2[index1, index2] ( -- value )
\ get a static field value of a class, where the field is identified by field
\ reference in the constant pool index (index1 ^> 8 + index2)
>[
  jvm_stack.fetchShort()
  \ ." idx fetched " .s CR
  jvm_stack.getCurrentFrame() 
  jvm_frame.getClass()
  jvm_class.getRTCP()
  ( addr_rtcp idx )
  jvm_exec.getClassAndNameForField()
  ( addr_cl c-addr n)
  jvm_class.getStatic() throw
  \ ." got value " dup . CR
]<

0xA7 2 s" goto" \ 2[branchbyte1, branchbyte2] ( -- )
\ goes to another instruction at branchoffset (signed short constructed from
\ unsigned bytes branchbyte1 ^> 8 + branchbyte2)
>[
  jvm_stack.fetchSignedShort()
  jvm_stack.getPC() + jvm_stack.setPC()
]<

0xC8 4 s" goto_w" \ 4[branchbyte1, branchbyte2, branchbyte3, branchbyte4] ( -- )
\ goes to another instruction at branchoffset (signed int constructed from
\ unsigned bytes branchbyte1 ^> 24 + branchbyte2 ^> 16 + branchbyte3 ^> 8 + branchbyte4)
>[ jvm_not_implemented ]<

0x91 0 s" i2b" \ ( value -- result )
\ convert an int into a byte
>[ jvm_not_implemented ]<

0x92 0 s" i2c" \ ( value -- result )
\ convert an int into a character
>[ jvm_not_implemented ]<

0x87 0 s" i2d" \ ( value -- result )
\ convert an int into a double
>[ jvm_not_implemented ]<

0x86 0 s" i2f" \ ( value -- result )
\ convert an int into a float
>[ jvm_not_implemented ]<

0x85 0 s" i2l" \ ( value -- result )
\ convert an int into a long
>[ jvm_not_implemented ]<

0x93 0 s" i2s" \ ( value -- result )
\ convert an int into a short
>[ jvm_not_implemented ]<

0x60 0 s" iadd" \ ( value1, value2 -- result )
\ add two ints
>[ + ]<

0x2E 0 s" iaload" \ ( arrayref, index -- value )
\ load an int from an array
>[ jvm_not_implemented ]<

0x7E 0 s" iand" \ ( value1, value2 -- result )
\ perform a bitwise and on two integers
>[ jvm_not_implemented ]<

0x4F 0 s" iastore" \ ( arrayref, index, value -- )
\ store an int into an array
>[ jvm_not_implemented ]<

0x02 0 s" iconst_m1 " \ ( -- -1 )
\ load the int value -1 onto the stack
>[ jvm_not_implemented ]<

0x03 0 s" iconst_0" \ ( -- 0 )
\ load the int value 0 onto the stack
>[ 0 ]<

0x04 0 s" iconst_1" \ ( -- 1 )
\ load the int value 1 onto the stack
>[ 1 ]<

0x05 0 s" iconst_2" \ ( -- 2 )
\ load the int value 2 onto the stack
>[ 2 ]<

0x06 0 s" iconst_3" \ ( -- 3 )
\ load the int value 3 onto the stack
>[ 3 ]<

0x07 0 s" iconst_4" \ ( -- 4 )
\ load the int value 4 onto the stack
>[ 4 ]<

0x08 0 s" iconst_5" \ ( -- 5 )
\ load the int value 5 onto the stack
>[ 5 ]<

0x6C 0 s" idiv" \ ( value1, value2 -- result )
\ divide two integers
>[ jvm_not_implemented ]<

0xA5 2 s" if_acmpeq" \ 2[branchbyte1, branchbyte2] ( value1, value2 -- )
\ if references are equal, branch to instruction at branchoffset (signed short
\ constructed from unsigned bytes branchbyte1 ^> 8 + branchbyte2)
>[ jvm_not_implemented ]<

0xA6 2 s" if_acmpne" \ 2[branchbyte1, branchbyte2] ( value1, value2 -- )
\ if references are not equal, branch to instruction at branchoffset (signed
\ short constructed from unsigned bytes branchbyte1 ^> 8 + branchbyte2)
>[ jvm_not_implemented ]<

0x9F 2 s" if_icmpeq" \ 2[branchbyte1, branchbyte2] ( value1, value2 -- )
\ if ints are equal, branch to instruction at branchoffset (signed short
\ constructed from unsigned bytes branchbyte1 ^> 8 + branchbyte2)
>[
  = IF <[ goto ]> ELSE jvm_stack.fetchShort() drop ENDIF
]<

0xA0 2 s" if_icmpne" \ 2[branchbyte1, branchbyte2] ( value1, value2 -- )
\ if ints are not equal, branch to instruction at branchoffset (signed short
\ constructed from unsigned bytes branchbyte1 ^> 8 + branchbyte2)
>[ jvm_not_implemented ]<

0xA1 2 s" if_icmplt" \ 2[branchbyte1, branchbyte2] ( value1, value2 -- )
\ if value1 is less than value2, branch to instruction at branchoffset (signed
\ short constructed from unsigned bytes branchbyte1 ^> 8 + branchbyte2)
>[ jvm_not_implemented ]<

0xA2 2 s" if_icmpge" \ 2[branchbyte1, branchbyte2] ( value1, value2 -- )
\ if value1 is greater than or equal to value2, branch to instruction at
\ branchoffset (signed short constructed from unsigned bytes
\ branchbyte1 ^> 8 + branchbyte2)
>[
  >= IF <[ goto ]> ELSE jvm_stack.fetchShort() drop ENDIF
]<

0xA3 2 s" if_icmpgt" \ 2[branchbyte1, branchbyte2] ( value1, value2 -- )
\ if value1 is greater than value2, branch to instruction at branchoffset
\ (signed short constructed from unsigned bytes branchbyte1 ^> 8 + branchbyte2)
>[ jvm_not_implemented ]<

0xA4 2 s" if_icmple" \ 2[branchbyte1, branchbyte2] ( value1, value2 -- )
\ if value1 is less than or equal to value2, branch to instruction at
\ branchoffset (signed short constructed from unsigned bytes
\ branchbyte1 ^> 8 + branchbyte2)
>[ jvm_not_implemented ]<

0x99 2 s" ifeq" \ 2[branchbyte1, branchbyte2] ( value -- )
\ if value is 0, branch to instruction at branchoffset (signed short
\ constructed from unsigned bytes branchbyte1 ^> 8 + branchbyte2)
>[ jvm_not_implemented ]<

0x9A 2 s" ifne" \ 2[branchbyte1, branchbyte2] ( value -- )
\ if value is not 0, branch to instruction at branchoffset (signed short
\ constructed from unsigned bytes branchbyte1 ^> 8 + branchbyte2)
>[ jvm_not_implemented ]<

0x9B 2 s" iflt" \ 2[branchbyte1, branchbyte2] ( value -- )
\ if value is less than 0, branch to instruction at branchoffset (signed short
\ constructed from unsigned bytes branchbyte1 ^> 8 + branchbyte2)
>[ jvm_not_implemented ]<

0x9C 2 s" ifge" \ 2[branchbyte1, branchbyte2] ( value -- )
\ if value is greater than or equal to 0, branch to instruction at
\ branchoffset (signed short constructed from unsigned bytes
\ branchbyte1 ^> 8 + branchbyte2)
>[ jvm_not_implemented ]<

0x9D 2 s" ifgt" \ 2[branchbyte1, branchbyte2] ( value -- )
\ if value is greater than 0, branch to instruction at branchoffset (signed
\ short constructed from unsigned bytes branchbyte1 ^> 8 + branchbyte2)
>[ jvm_not_implemented ]<

0x9E 2 s" ifle" \ 2[branchbyte1, branchbyte2] ( value -- )
\ if value is less than or equal to 0, branch to instruction at branchoffset
\ (signed short constructed from unsigned bytes branchbyte1 ^> 8 + branchbyte2)
>[ jvm_not_implemented ]<

0xC7 2 s" ifnonnull" \ 2[branchbyte1, branchbyte2] ( value -- )
\ if value is not null, branch to instruction at branchoffset (signed short
\ constructed from unsigned bytes branchbyte1 ^> 8 + branchbyte2)
>[ jvm_not_implemented ]<

0xC6 2 s" ifnull" \ 2[branchbyte1, branchbyte2] ( value -- )
\ if value is null, branch to instruction at branchoffset (signed short
\ constructed from unsigned bytes branchbyte1 ^> 8 + branchbyte2)
>[ jvm_not_implemented ]<

0x84 2 s" iinc" \ 2[index, const] ( -- )
\ increment local variable #index by signed byte const
>[
  jvm_stack.getCurrentFrame() ( addr_fm )
  jvm_stack.fetchByte() 2dup ( addr_fm idx addr_fm idx )
  jvm_frame.getLocal() ( addr_fm idx value )
  jvm_stack.fetchByte() + ( addr_fm idx nvalue )
  -rot jvm_frame.setLocal()
]<

0x15 1 s" iload" \ 1[index] ( -- value )
\ load an int value from a local variable #index
>[ jvm_not_implemented ]<

0x1A 0 s" iload_0" \ ( -- value )
\ load an int value from local variable 0
>[ 
  jvm_stack.getCurrentFrame() 0 jvm_frame.getLocal()
]<

0x1B 0 s" iload_1" \ ( -- value )
\ load an int value from local variable 1
>[ 
  jvm_stack.getCurrentFrame() 1 jvm_frame.getLocal()
]<

0x1C 0 s" iload_2" \ ( -- value )
\ load an int value from local variable 2
>[ 
  jvm_stack.getCurrentFrame() 2 jvm_frame.getLocal()
]<

0x1D 0 s" iload_3" \ ( -- value )
\ load an int value from local variable 3
>[ 
  jvm_stack.getCurrentFrame() 3 jvm_frame.getLocal()
]<

0x68 0 s" imul" \ ( value1, value2 -- result )
\ multiply two integers
>[ * ]<

0x74 0 s" ineg" \ ( value -- result )
\ negate int
>[ jvm_not_implemented ]<

0xC1 2 s" instanceof" \ 2[indexbyte1, indexbyte2] ( objectref -- result )
\ determines if an object objectref is of a given type, identified by class
\ reference index in constant pool (indexbyte1 ^> 8 + indexbyte2)
>[ jvm_not_implemented ]<

0xBA 4 s" invokedynamic" \ 4[indexbyte1, indexbyte2, 0, 0] ( [arg1, [arg2 ...]] -- )
\ invokes a dynamic method identified by method reference index in constant
\ pool (indexbyte1 ^> 8 + indexbyte2)
>[ jvm_not_implemented ]<

0xB9 4 s" invokeinterface" \ 4[indexbyte1, indexbyte2, count, 0] ( objectref, [arg1, arg2, ...] -- )
\ invokes an interface method on object objectref, where the interface method
\ is identified by method reference index in constant pool (indexbyte1 ^> 8 + indexbyte2)
>[ jvm_not_implemented ]<

0xB8 2 s" invokestatic" \ 2[indexbyte1, indexbyte2] ( [arg1, arg2, ...] -- )
\ invoke a static method, where the method is identified by method reference
\ index in constant pool (indexbyte1 ^> 8 + indexbyte2)
>[ 
  [ ?debug_trace ] [IF] jvm_stack.incInvoke() [ENDIF]
  jvm_stack.fetchShort()
  \ ." idx fetched " .s CR
  jvm_stack.invokestatic() throw
]<

0xB7 2 s" invokespecial" \ 2[indexbyte1, indexbyte2] ( objectref, [arg1, arg2, ...] -- )
\ invoke instance method on object objectref, where the method is identified
\ by method reference index in constant pool (indexbyte1 ^> 8 + indexbyte2)
>[
  <[ invokestatic ]>
]<

0xB6 2 s" invokevirtual" \ 2[indexbyte1, indexbyte2] ( objectref, [arg1, arg2, ...] -- )
\ invoke virtual method on object objectref, where the method is identified by
\ method reference index in constant pool (indexbyte1 ^> 8 + indexbyte2)
>[
  \ TODO: dunno if this is really enough...
  <[ invokestatic ]>
]<

0x80 0 s" ior" \ ( value1, value2 -- result )
\ bitwise int or
>[ jvm_not_implemented ]<

0x70 0 s" irem" \ ( value1, value2 -- result )
\ logical int remainder
>[ jvm_not_implemented ]<

0xAC 0 s" ireturn" \ ( value -- [empty] )
\ return an integer from a method
>[
  cr ." ireturn: " .s cr
  >r
  [ ?debug_trace ] [IF] jvm_stack.decInvoke() [ENDIF]
  jvm_stack.getCurrentFrame()
  jvm_frame.getDynamicLink()
  jvm_stack.resetCurrentFrame()
  r>
]<

0x78 0 s" ishl" \ ( value1, value2 -- result )
\ int shift left
>[ jvm_not_implemented ]<

0x7A 0 s" ishr" \ ( value1, value2 -- result )
\ int arithmetic shift right
>[ jvm_not_implemented ]<

0x36 0 s" istore" \ 1[index] ( value -- )
\ store int value into variable #index
>[ jvm_not_implemented ]<

0x3B 0 s" istore_0" \ ( value -- )
\ store int value into variable 0
>[ jvm_not_implemented ]<

0x3C 0 s" istore_1" \ ( value -- )
\ store int value into variable 1
>[
  jvm_stack.getCurrentFrame()
  1 jvm_frame.setLocal()
]<

0x3D 0 s" istore_2" \ ( value -- )
\ store int value into variable 2
>[
  jvm_stack.getCurrentFrame()
  1 jvm_frame.setLocal()
]<

0x3E 0 s" istore_3" \ ( value -- )
\ store int value into variable 3
>[ jvm_not_implemented ]<

0x64 0 s" isub" \ ( value1, value2 -- result )
\ int subtract
>[ jvm_not_implemented ]<

0x7C 0 s" iushr" \ ( value1, value2 -- result )
\ int logical shift right
>[ jvm_not_implemented ]<

0x82 0 s" ixor" \ ( value1, value2 -- result )
\ int xor
>[ jvm_not_implemented ]<

0xA8 2 s" jsr" \ 2[branchbyte1, branchbyte2] ( -- address )
\ jump to subroutine at branchoffset (signed short constructed from unsigned
\ bytes branchbyte1 ^> 8 + branchbyte2) and place the return address on the stack
>[ jvm_not_implemented ]<

0xC9 4 s" jsr_w" \ 4[branchbyte1, branchbyte2, branchbyte3, branchbyte4] ( -- address )
\ jump to subroutine at branchoffset (signed int constructed from unsigned
\ bytes branchbyte1 ^> 24 + branchbyte2 ^> 16 + branchbyte3 ^> 8 + branchbyte4)
\ and place the return address on the stack
>[ jvm_not_implemented ]<

0x8A 0 s" l2d" \ ( value -- result )
\ convert a long to a double
>[ jvm_not_implemented ]<

0x89 0 s" l2f" \ ( value -- result )
\ convert a long to a float
>[ jvm_not_implemented ]<

0x88 0 s" l2i" \ ( value -- result )
\ convert a long to a int
>[ jvm_not_implemented ]<

0x61 0 s" ladd" \ ( value1, value2 -- result )
\ add two longs
>[ jvm_not_implemented ]<

0x2F 0 s" laload" \ ( arrayref, index -- value )
\ load a long from an array
>[ jvm_not_implemented ]<

0x7F 0 s" land" \ ( value1, value2 -- result )
\ bitwise and of two longs
>[ jvm_not_implemented ]<

0x50 0 s" lastore" \ ( arrayref, index, value -- )
\ store a long to an array
>[ jvm_not_implemented ]<

0x94 0 s" lcmp" \ ( value1, value2 -- result )
\ compare two longs values
>[ jvm_not_implemented ]<

0x09 0 s" lconst_0" \ ( -- 0L )
\ push the long 0 onto the stack
>[ jvm_not_implemented ]<

0x0A 0 s" lconst_1" \ ( -- 1L )
\ push the long 1 onto the stack
>[ jvm_not_implemented ]<

0x12 1 s" ldc" \ 1[index] ( -- value )
\ push a constant #index from a constant pool (String, int or float) onto the stack
>[
  depth >r
  jvm_stack.fetchByte()

  jvm_stack.getCurrentFrame()
  jvm_frame.getClass()
  jvm_class.getRTCP()
  dup rot ( addr_rtcp addr_rtcp idx )
  jvm_rtcp.getConstpoolByIdx() ( addr_rtcp addr_cp )
  \ TODO: should also work for int and float
  dup jvm_cp_tag 
  CASE
    CONSTANT_Integer OF
      jvm_cp_integer_bytes nip
    ENDOF
    CONSTANT_String OF
      jvm_cp_string_idx ( addr_rtcp idxu )
      swap jvm_rtcp.getClassfile() ( idxu addr_cl )
      jvm_cf_constpool_addr ( idxu addr_cf )
      swap jvm_constpool_idx ( addr )

      s" java/lang/String" 2dup
      jvm_stack.newClass() ( addr c-addr n )
      jvm_stack.findAndInitClass() throw dup ( addr addr_class addr_class )
      jvm_class.getRTCP() ( addr addr_class addr_rtcp )
      jvm_rtcp.getClassfile() ( addr addr_class addr_classf )

      jvm_cf_fields_count cells allocate throw ( addr addr_class addr_this )
      swap over ! dup >r ( addr addr_this )

      \ copy (constant) string to new String object
      cell+ swap jvm_cp_utf8_c-ref ( addr_this+cell c-addr n )
      rot ( c-addr n addr_this+cell )
      dup >r ! ( c-addr addr_this+cell )
      r> cell+ !
      r> ( addr_this )
    ENDOF
    \ default
    ." ldc does not yet support constant type: "
    over jvm_constpool_type_name type CR
    jvm_not_implemented
  ENDCASE
  depth r> 1+ assert( = ) \ check stack effect
]<

0x13 2 s" ldc_w" \ 2[indexbyte1, indexbyte2] ( -- value )
\ push a constant #index from a constant pool (String, int or float) onto the
\ stack (wide index is constructed as indexbyte1 ^> 8 + indexbyte2)
>[ jvm_not_implemented ]<

0x14 2 s" ldc2_w" \ 2[indexbyte1, indexbyte2] ( -- value )
\ push a constant #index from a constant pool (double or long) onto the stack
\ (wide index is constructed as indexbyte1 ^> 8 + indexbyte2)
>[ jvm_not_implemented ]<

0x6D 0 s" ldiv" \ ( value1, value2 -- result )
\ divide two longs
>[ jvm_not_implemented ]<

0x16 0 s" lload" \ 1[index] ( -- value )
\ load a long value from a local variable #index
>[ jvm_not_implemented ]<

0x1E 0 s" lload_0" \ ( -- value )
\ load a long value from a local variable 0
>[ jvm_not_implemented ]<

0x1F 0 s" lload_1" \ ( -- value )
\ load a long value from a local variable 1
>[ jvm_not_implemented ]<

0x20 0 s" lload_2" \ ( -- value )
\ load a long value from a local variable 2
>[ jvm_not_implemented ]<

0x21 0 s" lload_3" \ ( -- value )
\ load a long value from a local variable 3
>[ jvm_not_implemented ]<

0x69 0 s" lmul" \ ( value1, value2 -- result )
\ multiply two longs
>[ jvm_not_implemented ]<

0x75 0 s" lneg" \ ( value -- result )
\ negate a long
>[ jvm_not_implemented ]<

0xAB 4 s" lookupswitch" \ 4+[<0-3 bytes padding>, defaultbyte1, defaultbyte2, defaultbyte3, defaultbyte4, npairs1, npairs2, npairs3, npairs4, match-offset pairs...] ( key -- )
\ a target address is looked up from a table using a key and execution
\ continues from the instruction at that address
>[ jvm_not_implemented ]<

0x81 0 s" lor" \ ( value1, value2 -- result )
\ bitwise or of two longs
>[ jvm_not_implemented ]<

0x71 0 s" lrem" \ ( value1, value2 -- result )
\ remainder of division of two longs
>[ jvm_not_implemented ]<

0xAD 0 s" lreturn" \ ( value -- [empty] )
\ return a long value
>[ jvm_not_implemented ]<

0x79 0 s" lshl" \ ( value1, value2 -- result )
\ bitwise shift left of a long value1 by value2 positions
>[ jvm_not_implemented ]<

0x7B 0 s" lshr" \ ( value1, value2 -- result )
\ bitwise shift right of a long value1 by value2 positions
>[ jvm_not_implemented ]<

0x37 1 s" lstore" \ 1[index] ( value -- )
\ store a long value in a local variable #index
>[ jvm_not_implemented ]<

0x3F 0 s" lstore_0" \ ( value -- )
\ store a long value in a local variable 0
>[ jvm_not_implemented ]<

0x40 0 s" lstore_1" \ ( value -- )
\ store a long value in a local variable 1
>[ jvm_not_implemented ]<

0x41 0 s" lstore_2" \ ( value -- )
\ store a long value in a local variable 2
>[ jvm_not_implemented ]<

0x42 0 s" lstore_3" \ ( value -- )
\ store a long value in a local variable 3
>[ jvm_not_implemented ]<

0x65 0 s" lsub" \ ( value1, value2 -- result )
\ subtract two longs
>[ jvm_not_implemented ]<

0x7D 0 s" lushr" \ ( value1, value2 -- result )
\ bitwise shift right of a long value1 by value2 positions, unsigned
>[ jvm_not_implemented ]<

0x83 0 s" lxor" \ ( value1, value2 -- result )
\ bitwise exclusive or of two longs
>[ jvm_not_implemented ]<

0xC2 0 s" monitorenter" \ ( objectref -- )
\ enter monitor for object ("grab the lock" - start of synchronized() section)
>[ jvm_not_implemented ]<

0xC3 0 s" monitorexit" \ ( objectref -- )
\ exit monitor for object ("release the lock" - end of synchronized() section)
>[ jvm_not_implemented ]<

0xC5 3 s" multianewarray" \ 3[indexbyte1, indexbyte2, dimensions] ( count1, [count2,...] -- arrayref )
\ create a new array of dimensions dimensions with elements of type identified
\ by class reference in constant pool index (indexbyte1 ^> 8 + indexbyte2);
\ the sizes of each dimension is identified by count1, [count2, etc.]
>[ jvm_not_implemented ]<

0xBB 2 s" new" \ 2[indexbyte1, indexbyte2] ( -- objectref )
\ create new object of type identified by class reference in constant pool
\ index (indexbyte1 ^> 8 + indexbyte2)
>[
  jvm_stack.fetchShort()

  jvm_stack.getCurrentFrame() 
  jvm_frame.getClass()
  jvm_class.getRTCP()
  dup rot ( addr_rtcp addr_rtcp idx )
  jvm_rtcp.getConstpoolByIdx() ( addr_rtcp addr_fd )
  dup jvm_cp_tag assert( CONSTANT_Class = )
  ( addr_rtcp addr_fd )
  swap jvm_rtcp.getClassfile() jvm_cf_constpool_addr swap
  ( const-addr addr_fd )
  jvm_constpool.getClassname()
  jvm_cp_utf8_c-ref ( c-addr n )
  2dup jvm_stack.newClass() ( c-addr n )
  jvm_stack.findAndInitClass() throw dup ( addr_class addr_class )
  jvm_class.getRTCP() ( addr_class addr_rtcp )
  jvm_rtcp.getClassfile() ( addr_class addr_classf )

  \ TODO: (1) get correct size
  \       (2) recursive add sizes up to java.lang.Object
  jvm_cf_fields_count cells allocate throw ( addr_class addr_this )
  swap over ! ( addr_this )

  \ TODO: add stuff for GC here.
]<

0xBC 1 s" newarray" \ 1[atype] ( count -- arrayref )
\ create new array with count elements of primitive type identified by atype
>[
  jvm_stack.fetchByte()
  assert( 5 = ) \ TODO: implement more types
  allocate throw
]<

0x00 0 s" nop" \ ( -- )
\ perform no operation
>[ noop ]<

0x57 0 s" pop" \ ( value -- )
\ discard the top value on the stack
>[ drop ]<

0x58 0 s" pop2" \ ( {value2, value1} -- )
\ discard the top two values on the stack (or one value, if it is a double or long)
>[ jvm_not_implemented ]<

0xB5 2 s" putfield" \ 2[indexbyte1, indexbyte2] ( objectref, value -- )
\ set field to value in an object objectref, where the field is identified by
\ a field reference index in constant pool (indexbyte1 ^> 8 + indexbyte2)
>[
  jvm_stack.fetchShort()
  jvm_stack.getCurrentFrame()
  jvm_frame.getClass()
  jvm_class.getRTCP()
  ( this value addr_rtcp idx )
  jvm_exec.getClassAndNameForField()
  ( this value addr_cl c-addr n)
  jvm_class.getField_offset() throw
  ( this value off )
  rot ( value off this )
  + !
]<

0xB3 2 s" putstatic" \ 2[indexbyte1, indexbyte2] ( value -- )
\ set static field to value in a class, where the field is identified by a
\ field reference index in constant pool (indexbyte1 ^> 8 + indexbyte2)
>[
  jvm_stack.fetchShort()
  \ ." idx fetched " .s CR
  jvm_stack.getCurrentFrame() 
  jvm_frame.getClass()
  jvm_class.getRTCP()
  ( addr_rtcp idx )
  jvm_exec.getClassAndNameForField()
  ( addr_cl c-addr n)
  3 pick \ get value
  -rot
  ( addr_cl val c-addr n)
  jvm_class.setStatic() throw
  drop \ val
]<

0xA9 1 s" ret" \ 1[index] ( -- )
\ continue execution from address taken from a local variable #index
\ (the asymmetry with jsr is intentional)
>[ jvm_not_implemented ]<

0xB1 0 s" return" \ ( -- [empty] )
\ return void from method
>[
  [ ?debug_trace ] [IF] jvm_stack.decInvoke() [ENDIF]
  jvm_stack.getCurrentFrame()
  jvm_frame.getDynamicLink()
  0= IF
    [ ?debug_trace ] [IF] CR ." Data Stack: " CR .s CR [ENDIF]
    \ return from main
    JVM_RETURN_EXCEPTION throw
  ELSE
    jvm_stack.resetCurrentFrame()
  ENDIF
]<

0x35 0 s" saload" \ ( arrayref, index -- value )
\ load short from array
>[ jvm_not_implemented ]<

0x56 0 s" sastore" \ ( arrayref, index, value -- )
\ store short to array
>[ jvm_not_implemented ]<

0x11 2 s" sipush" \ 2[byte1, byte2] ( -- value )
\ push a short onto the stack
>[
  <[ bipush ]> \ fetch high byte and sign ext
  8 lshift
  jvm_stack.fetchByte() or \ fetch low byte
]<

\ FIXME workaround: s/swpa/jswap.  seperate dict?
0x5F 0 s" jswap" \ ( value2, value1 -- value1, value2 )
\ swaps two top words on the stack (note that value1 and value2 must not be
\ double or long)
>[ jvm_not_implemented ]<

\ TODO: 4+ arguments? :/
0xAA 4 s" tableswitch" \ 4+[[0-3 bytes padding], defaultbyte1, defaultbyte2, defaultbyte3, defaultbyte4, lowbyte1, lowbyte2, lowbyte3, lowbyte4, highbyte1, highbyte2, highbyte3, highbyte4, jump offsets... ( index -- )
\ continue execution from an address in the table at offset index
>[ jvm_not_implemented ]<

\ wide 	c4 	3/5: opcode, indexbyte1, indexbyte2

\ or

\ iinc, indexbyte1, indexbyte2, countbyte1, countbyte2 	[same as for corresponding instructions] 	execute opcode, where opcode is either iload, fload, aload, lload, dload, istore, fstore, astore, lstore, dstore, or ret, but assume the index is 16 bit; or execute iinc, where the index is 16 bits and the constant to increment by is a signed 16 bit short

0xCA 0 s" breakpoint" \ ( ??? )
\ reserved for breakpoints in Java debuggers;
\ should not appear in any class file
>[ JVM_BREAKPOINT_EXCEPTION throw ]<

0xFE 0 s" impdep1 " \ ( ??? )
\ reserved for implementation-dependent operations within debuggers;
\ should not appear in any class file
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xFF 0 s" impdep2" \ \ reserved for implementation-dependent operations within debuggers;
\ should not appear in any class file
>[ jvm_not_implemented ]<

\ FIXME what do?
\ (no name) 	cb-fd 			these values are currently unassigned for opcodes and are reserved for future use

\ ======
\ *> ###
\ ======

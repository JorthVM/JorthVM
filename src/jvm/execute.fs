require fetch.fs
require exception.fs

0x32 \ aaload ( arrayref, index -- value )
\ load onto the stack a reference from an array
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x53 \ aastore ( arrayref, index, value -- )
\ store into a reference in an array
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x01 \ aconst_null ( -- null )
\ push a null reference onto the stack
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x19 \ aload 1[index] ( -- objectref )
\ load a reference onto the stack from a local variable #index
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x2A \ aload_0 ( -- objectref )
\ load a reference onto the stack from local variable 0
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x2B \ aload_1 ( -- objectref )
\ load a reference onto the stack from local variable 1
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x2C \ aload_2 ( -- objectref )
\ load a reference onto the stack from local variable 2
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x2D \ aload_3 ( -- objectref )
\ load a reference onto the stack from local variable 3
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xBD \ anewarray  2[indexbyte1, indexbyte2] ( count -- arrayref )
\ create a new array of references of length count and component type
\ identified by the class reference index (indexbyte1 << 8 + indexbyte2)
\ in the constant pool
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xB0 \ areturn ( objectref -- [empty] )
\ return a reference from a method
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xBE \ arraylength ( arrayref -- length )
\ get the length of an array
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x3A \ astore 1[index] ( objectref -- )
\ store a reference into a local variable #index
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x4B \ astore_0 ( objectref -- )
\ store a reference into local variable 0
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x4C \ astore_1 ( objectref -- )
\ store a reference into local variable 1
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x4D \ astore_2 ( objectref -- )
\ store a reference into local variable 2
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x4E \ astore_3 ( objectref -- )
\ store a reference into local variable 3
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xBF \ athrow ( objectref -- [empty], objectref )
\ throws an error or exception (notice that the rest of the stack is cleared,
\ leaving only a reference to the Throwable)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x33 \ baload ( arrayref, index -- value )
\ load a byte or Boolean value from an array
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x54 \ bastore ( arrayref, index, value -- )
\ store a byte or Boolean value into an array
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x10 \ bipush 1[byte] ( -- value )
\ push a byte onto the stack as an integer value
>[ jvm_fetch_instruction \ load byte
   dup 0x80 and \ sign ext
   if
     [ -1 8 lshift ] literal or
   endif
]<

0x34 \ caload ( arrayref, index -- value )
\ load a char from an array
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x55 \ castore ( arrayref, index, value -- )
\ store a char into an array
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xC0 \ checkcast 2[indexbyte1, indexbyte2] ( objectref -- objectref )
\ checks whether an objectref is of a certain type, the class reference of
\ which is in the constant pool at index (indexbyte1 << 8 + indexbyte2)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x90 \ d2f ( value -- result )
\ convert a double to a float
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x8E \ d2i ( value -- result )
\ convert a double to an int
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x8F \ d2l ( value -- result )
\ convert a double to a long
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x63 \ dadd ( value1, value2 -- result )
\ add two doubles
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x31 \ daload ( arrayref, index -- value )
\ load a double from an array
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x52 \ dastore ( arrayref, index, value -- )
\ store a double into an array
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x98 \ dcmpg ( value1, value2 -- result )
\ compare two doubles
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x97 \ dcmpl ( value1, value2 -- result )
\ compare two doubles
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x0E \ dconst_0 ( -- 0.0 )
\ push the constant 0.0 onto the stack
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x0F \ dconst_1 ( -- 1.0 )
\ push the constant 1.0 onto the stack
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x6F \ ddiv ( value1, value2 -- result )
\ divide two doubles
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x18 \ dload 1[index] ( -- value )
\ load a double value from a local variable #index
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x26 \ dload_0 ( -- value )
\ load a double from local variable 0
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x27 \ dload_1 ( -- value )
\ load a double from local variable 1
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x28 \ dload_2 ( -- value )
\ load a double from local variable 2
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x29 \ dload_3 ( -- value )
\ load a double from local variable 3
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x6B \ dmul ( value1, value2 -- result )
\ multiply two doubles
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x77 \ dneg ( value -- result )
\ negate a double
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x73 \ drem ( value1, value2 -- result )
\ get the remainder from a division between two doubles
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xAF \ dreturn ( value -- [empty] )
\ return a double from a method
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x39 \ dstore 1[index] ( value -- )
\ store a double value into a local variable #index
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x47 \ dstore_0 ( value -- )
\ store a double into local variable 0
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x48 \ dstore_1 ( value -- )
\ store a double into local variable 1
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x49 \ dstore_2 ( value -- )
\ store a double into local variable 2
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x4A \ dstore_3 ( value -- )
\ store a double into local variable 3
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x67 \ dsub ( value1, value2 -- result )
\ subtract a double from another
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x59 \ dup ( value -- value, value )
\ duplicate the value on top of the stack
>[ dup ]<

0x5A \ dup_x1 ( value2, value1 -- value1, value2, value1 )
\ insert a copy of the top value into the stack two values from the top
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x5B \ dup_x2 ( value3, value2, value1 -- value1, value3, value2, value1 )
\ insert a copy of the top value into the stack two (if value2 is double or
\ long it takes up the entry of value3, too) or three values (if value2 is
\ neither double nor long) from the top
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x5C \ dup2 ( {value2, value1} -- {value2, value1}, {value2, value1} )
\ duplicate top two stack words (two values, if value1 is not double nor long;
\ a single value, if value1 is double or long)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x5D \ dup2_x1 ( value3, {value2, value1} -- {value2, value1}, value3, {value2, value1} )
\ duplicate two words and insert beneath third word (see explanation above)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x5E \ dup2_x2 ( {value4, value3}, {value2, value1} -- {value2, value1}, {value4, value3}, {value2, value1} )
\ duplicate two words and insert beneath fourth word
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x8D \ f2d ( value -- result )
\ convert a float to a double
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x8B \ f2i ( value -- result )
\ convert a float to an int
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x8C \ f2l ( value -- result )
\ convert a float to a long
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x62 \ fadd ( value1, value2 -- result )
\ add two floats
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x30 \ faload ( arrayref, index -- value )
\ load a float from an array
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x51 \ fastore ( arrayref, index, value -- )
\ store a float in an array
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x96 \ fcmpg ( value1, value2 -- result )
\ compare two floats
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x95 \ fcmpl ( value1, value2 -- result )
\ compare two floats
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x0B \ fconst_0 ( -- 0.0f )
\ push 0.0f on the stack
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x0C \ fconst_1 ( -- 1.0f )
\ push 1.0f on the stack
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x0D \ fconst_2 ( -- 2.0f )
\ push 2.0f on the stack
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x6E \ fdiv ( value1, value2 -- result )
\ divide two floats
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x17 \ fload ( 1: index 	-- value )
\ load a float value from a local variable #index
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x22 \ fload_0 ( -- value )
\ load a float value from local variable 0
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x23 \ fload_1 ( -- value )
\ load a float value from local variable 1
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x24 \ fload_2 ( -- value )
\ load a float value from local variable 2
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x25 \ fload_3 ( -- value )
\ load a float value from local variable 3
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x6A \ fmul ( value1, value2 -- result )
\ multiply two floats
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x76 \ fneg ( value -- result )
\ negate a float
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x72 \ frem ( value1, value2 -- result )
\ get the remainder from a division between two floats
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xAE \ freturn ( value -- [empty] )
\ return a float
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x38 \ fstore 1[index] ( value -- )
\ store a float value into a local variable #index
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x43 \ fstore_0 ( value -- )
\ store a float value into local variable 0
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x44 \ fstore_1 ( value -- )
\ store a float value into local variable 1
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x45 \ fstore_2 ( value -- )
\ store a float value into local variable 2
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x46 \ fstore_3 ( value -- )
\ store a float value into local variable 3
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x66 \ fsub ( value1, value2 -- result )
\ subtract two floats
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xB4 \ getfield 2[index1, index2] ( objectref -- value )
\ get a field value of an object objectref, where the field is identified by
\ field reference in the constant pool index (index1 << 8 + index2)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xB2 \ getstatic 2[index1, index2] ( -- value )
\ get a static field value of a class, where the field is identified by field
\ reference in the constant pool index (index1 << 8 + index2)
>[ jvm_fetch_instruction \ load byte
   8 lshift
   jvm_fetch_instruction \ load byte
   or
   cells
   jvm_p_static_fields @ + l@
   \ FIXME use @ instead?!
]<

0xA7 \ goto 2[branchbyte1, branchbyte2] ( -- )
\ goes to another instruction at branchoffset (signed short constructed from
\ unsigned bytes branchbyte1 << 8 + branchbyte2)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xC8 \ goto_w 4[branchbyte1, branchbyte2, branchbyte3, branchbyte4] ( -- )
\ goes to another instruction at branchoffset (signed int constructed from
\ unsigned bytes branchbyte1 << 24 + branchbyte2 << 16 + branchbyte3 << 8 + branchbyte4)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x91 \ i2b ( value -- result )
\ convert an int into a byte
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x92 \ i2c ( value -- result )
\ convert an int into a character
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x87 \ i2d ( value -- result )
\ convert an int into a double
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x86 \ i2f ( value -- result )
\ convert an int into a float
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x85 \ i2l ( value -- result )
\ convert an int into a long
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x93 \ i2s ( value -- result )
\ convert an int into a short
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x60 \ iadd ( value1, value2 -- result )
\ add two ints
>[ + ]<

0x2E \ iaload ( arrayref, index -- value )
\ load an int from an array
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x7E \ iand ( value1, value2 -- result )
\ perform a bitwise and on two integers
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x4F \ iastore ( arrayref, index, value -- )
\ store an int into an array
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x02 \ iconst_m1  ( -- -1 )
\ load the int value -1 onto the stack
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x03 \ iconst_3 ( -- 0 )
\ load the int value 0 onto the stack
>[ 0 ]<

0x04 \ iconst_1 ( -- 1 )
\ load the int value 1 onto the stack
>[ 1 ]<

0x05 \ iconst_2 ( -- 2 )
\ load the int value 2 onto the stack
>[ 2 ]<

0x06 \ iconst_3 ( -- 3 )
\ load the int value 3 onto the stack
>[ 3 ]<

0x07 \ iconst_4 ( -- 4 )
\ load the int value 4 onto the stack
>[ 4 ]<

0x08 \ iconst_5 ( -- 5 )
\ load the int value 5 onto the stack
>[ 5 ]<

0x6C \ idiv ( value1, value2 -- result )
\ divide two integers
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xA5 \ if_acmpeq 2[branchbyte1, branchbyte2] ( value1, value2 -- )
\ if references are equal, branch to instruction at branchoffset (signed short
\ constructed from unsigned bytes branchbyte1 << 8 + branchbyte2)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xA6 \ if_acmpne 2[branchbyte1, branchbyte2] ( value1, value2 -- )
\ if references are not equal, branch to instruction at branchoffset (signed
\ short constructed from unsigned bytes branchbyte1 << 8 + branchbyte2)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x9F \ if_icmpeq 2[branchbyte1, branchbyte2] ( value1, value2 -- )
\ if ints are equal, branch to instruction at branchoffset (signed short
\ constructed from unsigned bytes branchbyte1 << 8 + branchbyte2)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xA0 \ if_icmpne 2[branchbyte1, branchbyte2] ( value1, value2 -- )
\ if ints are not equal, branch to instruction at branchoffset (signed short
\ constructed from unsigned bytes branchbyte1 << 8 + branchbyte2)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xA1 \ if_icmplt 2[branchbyte1, branchbyte2] ( value1, value2 -- )
\ if value1 is less than value2, branch to instruction at branchoffset (signed
\ short constructed from unsigned bytes branchbyte1 << 8 + branchbyte2)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xA2 \ if_icmpge 2[branchbyte1, branchbyte2] ( value1, value2 -- )
\ if value1 is greater than or equal to value2, branch to instruction at
\ branchoffset (signed short constructed from unsigned bytes
\ branchbyte1 << 8 + branchbyte2)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xA3 \ if_icmpgt 2[branchbyte1, branchbyte2] ( value1, value2 -- )
\ if value1 is greater than value2, branch to instruction at branchoffset
\ (signed short constructed from unsigned bytes branchbyte1 << 8 + branchbyte2)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xA4 \ if_icmple 2[branchbyte1, branchbyte2] ( value1, value2 -- )
\ if value1 is less than or equal to value2, branch to instruction at
\ branchoffset (signed short constructed from unsigned bytes
\ branchbyte1 << 8 + branchbyte2)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x99 \ ifeq 2[branchbyte1, branchbyte2] ( value -- )
\ if value is 0, branch to instruction at branchoffset (signed short
\ constructed from unsigned bytes branchbyte1 << 8 + branchbyte2)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x9A \ ifne 2[branchbyte1, branchbyte2] ( value -- )
\ if value is not 0, branch to instruction at branchoffset (signed short
\ constructed from unsigned bytes branchbyte1 << 8 + branchbyte2)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x9B \ iflt 2[branchbyte1, branchbyte2] ( value -- )
\ if value is less than 0, branch to instruction at branchoffset (signed short
\ constructed from unsigned bytes branchbyte1 << 8 + branchbyte2)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x9C \ ifge 2[branchbyte1, branchbyte2] ( value -- )
\ if value is greater than or equal to 0, branch to instruction at
\ branchoffset (signed short constructed from unsigned bytes
\ branchbyte1 << 8 + branchbyte2)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x9D \ ifgt 2[branchbyte1, branchbyte2] ( value -- )
\ if value is greater than 0, branch to instruction at branchoffset (signed
\ short constructed from unsigned bytes branchbyte1 << 8 + branchbyte2)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x9E \ ifle 2[branchbyte1, branchbyte2] ( value -- )
\ if value is less than or equal to 0, branch to instruction at branchoffset
\ (signed short constructed from unsigned bytes branchbyte1 << 8 + branchbyte2)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xC7 \ ifnonnull 2[branchbyte1, branchbyte2] ( value -- )
\ if value is not null, branch to instruction at branchoffset (signed short
\ constructed from unsigned bytes branchbyte1 << 8 + branchbyte2)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xC6 \ ifnull 2[branchbyte1, branchbyte2] ( value -- )
\ if value is null, branch to instruction at branchoffset (signed short
\ constructed from unsigned bytes branchbyte1 << 8 + branchbyte2)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x84 \ iinc 2[index, const] ( -- )
\ increment local variable #index by signed byte const
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x15 \ iload 1[index] ( -- value )
\ load an int value from a local variable #index
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x1A \ iload_0 ( -- value )
\ load an int value from local variable 0
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x1B \ iload_1 ( -- value )
\ load an int value from local variable 1
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x1C \ iload_2 ( -- value )
\ load an int value from local variable 2
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x1D \ iload_3 ( -- value )
\ load an int value from local variable 3
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x68 \ imul ( value1, value2 -- result )
\ multiply two integers
>[ * ]<

0x74 \ ineg ( value -- result )
\ negate int
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xC1 \ instanceof 2[indexbyte1, indexbyte2] ( objectref -- result )
\ determines if an object objectref is of a given type, identified by class
\ reference index in constant pool (indexbyte1 << 8 + indexbyte2)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xBA \ invokedynamic 4[indexbyte1, indexbyte2, 0, 0] ( [arg1, [arg2 ...]] -- )
\ invokes a dynamic method identified by method reference index in constant
\ pool (indexbyte1 << 8 + indexbyte2)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xB9 \ invokeinterface 4[indexbyte1, indexbyte2, count, 0] ( objectref, [arg1, arg2, ...] -- )
\ invokes an interface method on object objectref, where the interface method
\ is identified by method reference index in constant pool (indexbyte1 << 8 + indexbyte2)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xB7 \ invokespecial 2[indexbyte1, indexbyte2] ( objectref, [arg1, arg2, ...] -- )
\ invoke instance method on object objectref, where the method is identified
\ by method reference index in constant pool (indexbyte1 << 8 + indexbyte2)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xB8 \ invokestatic 2[indexbyte1, indexbyte2] ( [arg1, arg2, ...] -- )
\ invoke a static method, where the method is identified by method reference
\ index in constant pool (indexbyte1 << 8 + indexbyte2)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xB6 \ invokevirtual 2[indexbyte1, indexbyte2] ( objectref, [arg1, arg2, ...] -- )
\ invoke virtual method on object objectref, where the method is identified by
\ method reference index in constant pool (indexbyte1 << 8 + indexbyte2)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x80 \ ior ( value1, value2 -- result )
\ bitwise int or
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x70 \ irem ( value1, value2 -- result )
\ logical int remainder
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xAC \ ireturn ( value -- [empty] )
\ return an integer from a method
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x78 \ ishl ( value1, value2 -- result )
\ int shift left
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x7A \ ishr ( value1, value2 -- result )
\ int arithmetic shift right
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x36 \ istore 1[index] ( value -- )
\ store int value into variable #index
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x3B \ istore_0 ( value -- )
\ store int value into variable 0
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x3C \ istore_1 ( value -- )
\ store int value into variable 1
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x3D \ istore_2 ( value -- )
\ store int value into variable 2
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x3E \ istore_3 ( value -- )
\ store int value into variable 3
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x64 \ isub ( value1, value2 -- result )
\ int subtract
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x7C \ iushr ( value1, value2 -- result )
\ int logical shift right
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x82 \ ixor ( value1, value2 -- result )
\ int xor
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xA8 \ jsr 2[branchbyte1, branchbyte2] ( -- address )
\ jump to subroutine at branchoffset (signed short constructed from unsigned
\ bytes branchbyte1 << 8 + branchbyte2) and place the return address on the stack
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xC9 \ jsr_w 4[branchbyte1, branchbyte2, branchbyte3, branchbyte4] ( -- address )
\ jump to subroutine at branchoffset (signed int constructed from unsigned
\ bytes branchbyte1 << 24 + branchbyte2 << 16 + branchbyte3 << 8 + branchbyte4)
\ and place the return address on the stack
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x8A \ l2d ( value -- result )
\ convert a long to a double
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x89 \ l2f ( value -- result )
\ convert a long to a float
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x88 \ l2i ( value -- result )
\ convert a long to a int
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x61 \ ladd ( value1, value2 -- result )
\ add two longs
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x2F \ laload ( arrayref, index -- value )
\ load a long from an array
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x7F \ land ( value1, value2 -- result )
\ bitwise and of two longs
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x50 \ lastore ( arrayref, index, value -- )
\ store a long to an array
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x94 \ lcmp ( value1, value2 -- result )
\ compare two longs values
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x09 \ lconst_0 ( -- 0L )
\ push the long 0 onto the stack
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x0A \ lconst_1 ( -- 1L )
\ push the long 1 onto the stack
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x12 \ ldc 1[index] ( -- value )
\ push a constant #index from a constant pool (String, int or float) onto the stack
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x13 \ ldc_w 2[indexbyte1, indexbyte2] ( -- value )
\ push a constant #index from a constant pool (String, int or float) onto the
\ stack (wide index is constructed as indexbyte1 << 8 + indexbyte2)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x14 \ ldc2_w 2[indexbyte1, indexbyte2] ( -- value )
\ push a constant #index from a constant pool (double or long) onto the stack
\ (wide index is constructed as indexbyte1 << 8 + indexbyte2)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x6D \ ldiv ( value1, value2 -- result )
\ divide two longs
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x16 \ lload 1[index] ( -- value )
\ load a long value from a local variable #index
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x1E \ lload_0 ( -- value )
\ load a long value from a local variable 0
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x1F \ lload_1 ( -- value )
\ load a long value from a local variable 1
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x20 \ lload_2 ( -- value )
\ load a long value from a local variable 2
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x21 \ lload_3 ( -- value )
\ load a long value from a local variable 3
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x69 \ lmul ( value1, value2 -- result )
\ multiply two longs
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x75 \ lneg ( value -- result )
\ negate a long
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xAB \ lookupswitch 4+[<0-3 bytes padding>, defaultbyte1, defaultbyte2, defaultbyte3, defaultbyte4, npairs1, npairs2, npairs3, npairs4, match-offset pairs...] ( key -- )
\ a target address is looked up from a table using a key and execution
\ continues from the instruction at that address
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x81 \ lor ( value1, value2 -- result )
\ bitwise or of two longs
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x71 \ lrem ( value1, value2 -- result )
\ remainder of division of two longs
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xAD \ lreturn ( value -- [empty] )
\ return a long value
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x79 \ lshl ( value1, value2 -- result )
\ bitwise shift left of a long value1 by value2 positions
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x7B \ lshr ( value1, value2 -- result )
\ bitwise shift right of a long value1 by value2 positions
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x37 \ lstore 1[index] ( value -- )
\ store a long value in a local variable #index
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x3F \ lstore_0 ( value -- )
\ store a long value in a local variable 0
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x40 \ lstore_1 ( value -- )
\ store a long value in a local variable 1
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x41 \ lstore_2 ( value -- )
\ store a long value in a local variable 2
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x42 \ lstore_3 ( value -- )
\ store a long value in a local variable 3
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x65 \ lsub ( value1, value2 -- result )
\ subtract two longs
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x7D \ lushr ( value1, value2 -- result )
\ bitwise shift right of a long value1 by value2 positions, unsigned
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x83 \ lxor ( value1, value2 -- result )
\ bitwise exclusive or of two longs
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xC2 \ monitorenter ( objectref -- )
\ enter monitor for object ("grab the lock" - start of synchronized() section)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xC3 \ monitorexit ( objectref -- )
\ exit monitor for object ("release the lock" - end of synchronized() section)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xC5 \ multianewarray 3[indexbyte1, indexbyte2, dimensions] ( count1, [count2,...] -- arrayref )
\ create a new array of dimensions dimensions with elements of type identified
\ by class reference in constant pool index (indexbyte1 << 8 + indexbyte2);
\ the sizes of each dimension is identified by count1, [count2, etc.]
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xBB \ new 2[indexbyte1, indexbyte2] ( -- objectref )
\ create new object of type identified by class reference in constant pool
\ index (indexbyte1 << 8 + indexbyte2)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xBC \ newarray 1[atype] ( count -- arrayref )
\ create new array with count elements of primitive type identified by atype
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x00 \ nop ( -- )
\ perform no operation
>[ ]<

0x57 \ pop ( value -- )
\ discard the top value on the stack
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x58 \ pop2 ( {value2, value1} -- )
\ discard the top two values on the stack (or one value, if it is a double or long)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xB5 \ putfield 2[indexbyte1, indexbyte2] ( objectref, value -- )
\ set field to value in an object objectref, where the field is identified by
\ a field reference index in constant pool (indexbyte1 << 8 + indexbyte2)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xB3 \ putstatic 2[indexbyte1, indexbyte2] ( value -- )
\ set static field to value in a class, where the field is identified by a
\ field reference index in constant pool (indexbyte1 << 8 + indexbyte2)
>[ jvm_fetch_instruction \ load byte
   8 lshift
   jvm_fetch_instruction \ load byte
   or
   cells
   jvm_p_static_fields @ + l!
   \ FIXME use ! instead?!
]<

0xA9 \ ret 1[index] ( -- )
\ continue execution from address taken from a local variable #index
\ (the asymmetry with jsr is intentional)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xB1 \ return ( -- [empty] )
\ return void from method
>[ CR ." Data Stack: " CR .s CR
   CR ." Static Fields: " CR jvm_p_static_fields @ 0x100 dump CR \ FIXME hardcoded
   JVM_RETURN_EXCEPTION throw
]<

0x35 \ saload ( arrayref, index -- value )
\ load short from array
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x56 \ sastore ( arrayref, index, value -- )
\ store short to array
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0x11 \ sipush 2[byte1, byte2] ( -- value )
\ push a short onto the stack
>[ <[ 0x10 ]> \ jvm_op_bipush \ fetch high byte and sign ext
   8 lshift
   jvm_fetch_instruction or \ fetch low byte
]<

0x5F \ swap ( value2, value1 -- value1, value2 )
\ swaps two top words on the stack (note that value1 and value2 must not be
\ double or long)
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xAA \ tableswitch 4+[[0-3 bytes padding], defaultbyte1, defaultbyte2, defaultbyte3, defaultbyte4, lowbyte1, lowbyte2, lowbyte3, lowbyte4, highbyte1, highbyte2, highbyte3, highbyte4, jump offsets... ( index -- )
\ continue execution from an address in the table at offset index
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

\ wide 	c4 	3/5: opcode, indexbyte1, indexbyte2

\ or

\ iinc, indexbyte1, indexbyte2, countbyte1, countbyte2 	[same as for corresponding instructions] 	execute opcode, where opcode is either iload, fload, aload, lload, dload, istore, fstore, astore, lstore, dstore, or ret, but assume the index is 16 bit; or execute iinc, where the index is 16 bits and the constant to increment by is a signed 16 bit short

0xCA \ breakpoint ( ??? )
\ reserved for breakpoints in Java debuggers;
\ should not appear in any class file
>[ JVM_BREAKPOINT_EXCEPTION throw ]<

0xFE \ impdep1
\ reserved for implementation-dependent operations within debuggers;
\ should not appear in any class file
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

0xFF \ impdep2
\ reserved for implementation-dependent operations within debuggers;
\ should not appear in any class file
>[ JVM_NOTIMPLEMENTED_EXCEPTION throw ]<

\ FIXME what do?
\ (no name) 	cb-fd 			these values are currently unassigned for opcodes and are reserved for future use


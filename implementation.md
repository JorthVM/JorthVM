JorthVM
=========

__NOTE:__ this file is outdated and needs reviewing. Look into the source code documentation for more recent
information.

Java Virtual Machine
--------------------

### Architecture

The Java Virtual Machine (JVM) is a stack machine.

### Instruction Set

The JVM instruction set consists of about 200 instructions. Their opcodes are encoded as 8 bit 
values.

### `Class` File

The JVM uses so-called `class` files to store machine code as well as constants, etc..

### Constant Pool

JVM heavily uses the constant pool.

### JVM stack

Each thread consists of a JVM stack. It is used to store frames for methods. The 
specification does not require the JVM stack to be implemented in a contiguous memory area.

### Frames

Frames are used to store local variables, operant stacks, for dynamic linking and other purposes. 
Frames may be heap allocated ([JVMStacks][]). The frame also contains a pointer to the previous 
frame.

### Operant Stack

Conceptually the operant stack is part of the current frame. In JorthVM the Forth stack is used
for operants were as other frame data (local variables, previous frame pointer, etc.) are 
stored on the heap.

Where is my Stuff? 
------------------

_aka where all the different things are stored_

### Classes

### Objects

### Class (`Static') Variables

### Constant Pool

### Instance Variables

### Local Variables

### Method Parameters


Creation and Destruction
------------------------

### JVM

### Thread

### Classes

A Class is initialized if
- an Instance of that class is created, or
- a static method is invoked, or
- a non-constant class variable (`static' but not `final') is used or assigned.
(See §2.17.4).

During the initialization of a class the static initializer (§2.11) is executed and the static fields 
(class variables, §2.9.2) are initialized.

During the initialization of an interface the static fields are initialized.

### Instance

An Instance is created when a new object of a certain class is created (e.g. with `new'). During this
instanciation all instance variables are initialazed.

### Local Variables

Core
----
### Implementation Details

- requires a cell width of at least 32 bit (e.g. `1 cells 4 >= .` yield true `-1`)

### Threading Technique `jvm_next`  

The main part of JorthVM is the so-called _NEXT routine_. It repeatedly performes the following 
tasks:

1. load the next instruction
2. increment the instruction pointer (`ip`)
3. execute the instruction

There are different ways how this can be done ([ThreadedCode][]). In the current implementation of 
the NEXT routine the so-called _call threading_ technique is used. The main disadvantage is that 
(indirect) calls are used instead of indirect jumps. If jumps are used one next cycle consists of 
the following steps:

1. load instruction and increment `ip`
2. jump to the address of the implementation of the instruction
3. perform instruction code
4. jump back to 1.

If call instead of jumps are used the following steps are performed:

1. load instruction and increment `ip`
2. call the subrouting that contains the implementation of the instruction
3. perform instruction code
4. return from the call
5. jump back to 1.

Even worse than the fact that there is one more step is, that call-return creates and destroys 
a new stack frame which decreases performance dramatically. 
*FIXME:* is this really the case in forth?


### Lookup Table

The lookup table is an array of 256 cells. Each cell stores an _execution token_. The offset 
corresponds to the opcode of the JVM e.g. at the offset 0x10 the execution token for the JVM 
instruction `bipush` is stored.


References
----------

- [JVMspec][]: The Java&trade; Virtual Machine Specification
- [ClassFile][]: The `class` File Format
- [ThreadedCode][]: Threaded Code

[JVMspec]: http://java.sun.com/docs/books/jvms/second_edition/html/VMSpecTOC.doc.html 
[JVMstacks]: http://java.sun.com/docs/books/jvms/second_edition/html/Overview.doc.html#30934
[ClassFile]: http://java.sun.com/docs/books/jvms/second_edition/html/ClassFile.doc.html
[ThreadedCode]: http://www.complang.tuwien.ac.at/forth/threaded-code.html


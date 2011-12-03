\ this file implements functionality that is needed to read class files



\   ClassFile {
\   	u4 magic;
\   	u2 minor_version;
\   	u2 major_version;
\   	u2 constant_pool_count;
\   	cp_info constant_pool[constant_pool_count-1];
\   	u2 access_flags;
\   	u2 this_class;
\   	u2 super_class;
\   	u2 interfaces_count;
\   	u2 interfaces[interfaces_count];
\   	u2 fields_count;
\   	field_info fields[fields_count];
\   	u2 methods_count;
\   	method_info methods[methods_count];
\   	u2 attributes_count;
\   	attribute_info attributes[attributes_count];
\   }

\ 0 Value fd-in
variable filebuffer
10 allocate throw
filebuffer !
variable classfile

: jvm_read_classfile ( c-addr u1 - u2 ) \ return the size of the file (in bytes)
  r/o open-file throw 
  dup classfile !            ( wfileid - wfileid ) \ store file id (wfileid)
  dup file-size throw throw  ( wfileid - wfileid u u ) \ uncatched exception ??? 
  dup
  allocate throw             ( wfileid u u - wfileid u a-addr )
  dup filebuffer !
  swap rot                   ( wfileid u a-addr - c-addr u wfileid )
  read-file throw            ( c-addr u wfileid - u2 )
;

: jvm_constpool_type_name ( a-addr - c-addr n ) \ get the name of an entry in the const table
  @ 0xff and
  CASE
     7 of s" CONSTANT_Class" ENDOF
     9 of s" CONSTANT_Fieldref" ENDOF
    10 of s" CONSTANT_Methodref" ENDOF
    11 of s" CONSTANT_InterfaceMethodref" ENDOF
     8 of s" CONSTANT_String" ENDOF
     3 of s" CONSTANT_Integer" ENDOF
     4 of s" CONSTANT_Float" ENDOF
     5 of s" CONSTANT_Long" ENDOF
     6 of s" CONSTANT_Double" ENDOF
    12 of s" CONSTANT_NameAndType" ENDOF
     1 of s" CONSTANT_Utf8" ENDOF
\ default
    drop 
    s" Unknown Constant Pool Type " exception throw
  ENDCASE
;

: jvm_swap_u2 ( u1 - u2 ) \ little endian to big endian (2 byte)
\ FIXME there must be something more efficient oO
  dup 0xff and 8 lshift swap  
  0xff00 and 8 rshift
  or
;

: jvm_swap_u4 ( u1 - u2 ) \ little endian to big endian (4 byte)
\ FIXME there must be something more efficient oO
  dup 0xff and 24 lshift swap  
  dup 0xff00 and 8 lshift swap
  dup 0xff0000 and 8 rshift swap
  0xff000000 and 24 rshift
  or or or
;

: jvm_constpool_type_size { addr } ( a-addr - n2 ) \ get the size of an entry in the const table
  addr @ 0xff and
  CASE
\ CONSTANT_Class 	7
     7 OF 3 ENDOF
\ CONSTANT_Fieldref 	9
     9 OF 5 ENDOF
\ CONSTANT_Methodref 	10
    10 OF 5 ENDOF
\ CONSTANT_InterfaceMethodref 	11
    11 OF 5 ENDOF
\ CONSTANT_String 	8
     8 OF 3 ENDOF
\ CONSTANT_Integer 	3
     3 OF 5 ENDOF
\ CONSTANT_Float 	4
     4 OF 5 ENDOF
\ CONSTANT_Long 	5
     5 OF 9 ENDOF
\ CONSTANT_Double 	6
     6 OF 9 ENDOF
\ CONSTANT_NameAndType 	12
    12 OF 5 ENDOF
\ CONSTANT_Utf8 	1
     1 OF  
      addr 1 + \ u2 length field
      @ jvm_swap_u2 \ read length
      3 + \ add u1 (tag) and u2 (length)
    ENDOF
\ default
    drop 
    s" Unknown Constant Pool Type " exception throw
  ENDCASE
;

: jvm_constpool_idx ( a-addr1 idx - a-addr2 ) 
\ returns address of entry with index idx from constant pool starting at address a-addr1
  1 ?DO
    dup jvm_constpool_type_size +
  LOOP
;

: jvm_print_classfile { addr -- }
  addr 
  .s CR
  \ print magic
  dup \ save a-addr  
  s" Magic:  " type 
  @ jvm_swap_u4 
  hex. CR
  s" Minor:  " type 
  
  4 + 
  dup \ save a-addr  
  @ jvm_swap_u2 
  hex. CR

  2 +
  dup \ save a-addr  
  s" Major:  " type 
  @ jvm_swap_u2 
  hex. CR
  
  2 + 
  dup \ save a-addr  
  s" Constant Pool count:  " type 
  @ jvm_swap_u2 
  dup . CR \ store count

\ print constant pool
  swap 2 + \ start address 
  swap
\  .s CR
  1 ?DO
    s" [" type
    i .
    s" ] " type
    dup jvm_constpool_type_name type  
    s" : " type
    dup jvm_constpool_type_size dup . +
    CR
  LOOP

  dup \ save a-addr  
  s" Access_Flags (Class):  " type 
  @ jvm_swap_u2 
  hex. CR
  
  2 +
  dup \ save a-addr  
  s" this class:  " type 
  @ jvm_swap_u2 
  dup hex. CR
  addr 10 + swap \ get start of the constpool
  jvm_constpool_idx ( a-addr1 idx - a-addr2 ) 
  jvm_constpool_type_name type CR
  
  2 +
  dup \ save a-addr  
  s" super class:  " type 
  @ jvm_swap_u2 
  dup hex. CR
  addr 10 + swap \ get start of the constpool
  jvm_constpool_idx ( a-addr1 idx - a-addr2 ) 
  jvm_constpool_type_name type CR
  
  


  drop \ drop last address
;

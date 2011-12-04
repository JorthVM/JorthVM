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

\ Constant Pool Entry access words

: jvm_cp_tag ( addr -- tag) \ get the tag of a given constant pool entry
  POSTPONE c@ 
; immediate

\ class
: jvm_cp_class_name_idx ( addr -- idx) \ get the name index of a class constant pool entry
  dup 1 + c@ 8 lshift swap 
  2 + c@ or 
; 

\ fieldref
: jvm_cp_fieldref_class_idx ( addr -- idx) \ get the class index of a fieldref constant pool entry
  dup 1 + c@ 8 lshift swap 
  2 + c@ or 
; 

: jvm_cp_fieldref_nametype_idx ( addr -- idx) \ get the class index of a fieldref constant pool entry
  dup 3 + c@ 8 lshift swap 
  5 + c@ or 
; 

\ methodref
: jvm_cp_methodref_class_idx ( addr -- idx) \ get the class index of a methodref constant pool entry
  POSTPONE jvm_cp_fieldref_class_idx 
; immediate

: jvm_cp_methodref_nametype_idx ( addr -- idx) \ get the class index of a methodref constant pool entry
  POSTPONE jvm_cp_fieldref_nametype_idx 
; immediate

\ interfacemethodref
: jvm_cp_interfacemethodref_class_idx ( addr -- idx) \ get the class index of a interfacemethodref constant pool entry
  POSTPONE jvm_cp_fieldref_class_idx 
; immediate

: jvm_cp_interfacemethodref_nametype_idx ( addr -- idx) \ get the class index of a methodref constant pool entry
  POSTPONE jvm_cp_fieldref_nametype_idx 
; immediate


: jvm_constpool_type_size { addr } ( a-addr - n2 ) \ get the size of an entry in the const table
  addr jvm_cp_tag
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

: jvm_constpool_print_utf8 { addr -- } \ prints a utf8 string
  addr 1 + \ u2 length field
  @ jvm_swap_u2 \ read length
  3 + \ add u1 (tag) and u2 (length)
  addr + \ end addr
  addr 3 + \ start addr
  
  begin
  2dup
  > while
    xc@+ xemit
  repeat
  2drop
;

: jvm_constpool_cmp_utf8 { addr xc-addr n -- } \ compare a counted xc string with a utf8 constant
\ FIXME not really efficient. may be we should ignore utf8 for the moment? anyway cell wide compare
\ would be more efficient
  addr 1 + \ u2 length field
  @ jvm_swap_u2 \ read length
  n = IF
    true
    n 0 ?DO
    ( b1 -- [b1 & *addr1=*addr2] )
    xc-addr i + addr 3 + i +
    @ 0xff and swap @ 0xff and = 
    and \ & b1
    LOOP
  ELSE
    false
  ENDIF
;

: jvm_constpool_print_classname { const-addr class-addr -- } \ print the class name of a constpool class entry
  class-addr 1 + \ u2 idx field
  @ jvm_swap_u2 \ read idx
  const-addr swap 
  jvm_constpool_idx
  jvm_constpool_print_utf8
;

: jvm_constpool_attr_size { attr-addr -- n } \ get the size of the attribute entry (in bytes)
  2 + \ length field
  @ jvm_swap_u4
  6 + \ add u2 (name_index) and u4 (length)
;

: jvm_constpool_print_attr { const-addr addr1 -- addr2 } 
  \ const-addr: address of the constpool, addr1: start address of the attribute, addr2: address after the attribute
  addr1
  s" attribute name:  " type 
  dup \ dup addr
  @ jvm_swap_u2 \ load name idx
  \ dup hex. space
  const-addr swap \ get start of the constpool
  jvm_constpool_idx ( a-addr1 idx - a-addr2 ) 
  jvm_constpool_print_utf8 CR
  2 +

  dup \ save a-addr  
  s" attribute length:  " type 
  @ jvm_swap_u4 
  dup . CR
  4 + + \ add length filed and info field
;

: jvm_constpool_print_code_attr { const-addr addr1 -- } 
  addr1 6 + 
  \ u2 max_stack
  dup \ dup addr
  ." max stack:  " 
  @ jvm_swap_u2 \ load name idx
  . CR
  2 +

  \ u2 max_locals
  dup \ dup addr
  ." max locals:  " 
  @ jvm_swap_u2 \ load name idx
  . CR
  2 +

  \ u4 code_length
  dup \ dup addr
  ." code length:  " 
  @ jvm_swap_u4 \ load name idx
  dup . CR
  swap 4 + swap
  
  2dup \ dup ( addr n )
  dump 
  + \ calc new address
  
  \ u4 exception_table_length
  dup \ dup addr
  ." exception table length:  " 
  @ jvm_swap_u2 \ load name idx
  dup . CR
  swap 2 + swap
  
  \ u2 attributes_count
  \ TODO examin exception table
  8 * \ u2 start_pc u2 end_pc u2 handler_pc u2 catch_type
  + 

  \ u2 attributes_count
  dup \ save a-addr  
  ." attributes count:  " 
  @ jvm_swap_u2 
  dup . CR
  swap 2 + swap 
  s" -----------" type CR
  ( addr count -- )
  0 ?DO 
    const-addr swap \ fixme
    ( const-addr addr1 -- addr2 )
    jvm_constpool_print_attr
  LOOP
  s" -----------" type CR

  drop
;

: jvm_print_classfile { addr -- }
\ addr stores the start address of the memory where the file is stored
\ first 4  bytes should be 0xCAFEBABE
\ after the info of an element has been printed, the top of the stack
\ contains the address of the next data field. the only exception to this is 
\ if the next filed is the array of a count/array pair. in this case after
\ printing the count the top of the stack contains the next address and the count 
\ e.g. ( addr1 n - addr2)

  addr 
\   	u4 magic;
  dup \ save a-addr  
s" Magic:  " type 
  @ jvm_swap_u4 
  hex. CR
  4 + 

\   	u2 minor_version;
  dup \ save a-addr  
  s" Minor:  " type 
  @ jvm_swap_u2 
  hex. CR
  2 +

\   	u2 major_version;
  dup \ save a-addr  
  s" Major:  " type 
  @ jvm_swap_u2 
  hex. CR
  2 + 
  
\   	u2 constant_pool_count;
  dup \ save a-addr  
  s" Constant Pool count:  " type 
  @ jvm_swap_u2 
  dup . CR \ store count
  swap 2 + swap

\   	cp_info constant_pool[constant_pool_count-1];
  1 ?DO
    s" [" type
    i .
    s" ] " type
    dup dup dup jvm_constpool_type_name type  
    @ 0xff and \ read tag
    CASE
    ( addr1 - ) \ addr1: address of the constpool entry
    1 OF  \ if utf8 string, print it!
      space 0x22 emit jvm_constpool_print_utf8 0x22 emit space \ 0x22 = "
    ENDOF
    12 OF \ NameAndType
            
    ENDOF
      drop 
    ENDCASE
    s" : " type
    dup jvm_constpool_type_size dup . +
    CR
  LOOP
  
  CR

\   	u2 access_flags;
  dup \ save a-addr  
  s" Access_Flags (Class):  " type 
  @ jvm_swap_u2 
  hex. CR
  2 +
  
\   	u2 this_class;
  dup \ save a-addr  
  ." this class:  " 
  @ jvm_swap_u2 
  \ dup hex. space
  addr 10 + swap \ get start of the constpool
  jvm_constpool_idx ( a-addr1 idx - a-addr2 ) 
  addr 10 + swap jvm_constpool_print_classname CR
  2 +
  
\   	u2 super_class;
  dup \ save a-addr  
  s" super class:  " type 
  @ jvm_swap_u2 
  \ dup hex. space
  addr 10 + swap \ get start of the constpool
  jvm_constpool_idx ( a-addr1 idx - a-addr2 ) 
  addr 10 + swap jvm_constpool_print_classname CR
  2 + 
  
\   	u2 interfaces_count;
  dup \ save a-addr  
  s" Interfaces count:  " type 
  @ jvm_swap_u2 
  dup . CR \ store count

\   	u2 interfaces[interfaces_count];
  0 ?DO
    2 + 
    dup \ save a-addr  
    @ jvm_swap_u2 
    ." Interface[ " i . ." ] "
    addr 10 + swap \ get start of the constpool
    jvm_constpool_idx ( a-addr1 idx - a-addr2 ) 
    addr 10 + swap jvm_constpool_print_classname CR
  LOOP
  2 + 

  CR

\   	u2 fields_count;
  dup \ save a-addr  
  s" Fields count:  " type 
  @ jvm_swap_u2 
  dup . CR \ store count
  swap 2 + swap

\   	field_info fields[fields_count];
  0 ?DO
    \ print field idx
    s" Field[ " type
    i .
    s" ] " type CR

    dup \ save a-addr  
    s" access flags:  " type 
    @ jvm_swap_u2 
    hex. CR
    2 +
    
    dup \ save a-addr  
    s" name_index:  " type 
    @ jvm_swap_u2 
    \ dup hex. space
    addr 10 + swap \ get start of the constpool
    jvm_constpool_idx ( a-addr1 idx - a-addr2 ) 
    jvm_constpool_print_utf8 CR
    2 +
    
    dup \ save a-addr  
    s" decriptor_index:  " type 
    @ jvm_swap_u2 
    \ dup hex. space
    addr 10 + swap \ get start of the constpool
    jvm_constpool_idx ( a-addr1 idx - a-addr2 ) 
    jvm_constpool_print_utf8 CR
    2 +
    
    dup \ save a-addr  
    s" attributes counts:  " type 
    @ jvm_swap_u2 
    dup . CR
    swap 2 + swap 
    s" -----------" type CR
    ( addr count -- )
    0 ?DO 
      addr 10 + swap \ fixme
      ( const-addr addr1 -- addr2 )
      jvm_constpool_print_attr
    LOOP
    s" -----------" type CR

  LOOP
 
  CR

\   	u2 methods_count;
  dup \ save a-addr  
  s" Methodes count:  " type 
  @ jvm_swap_u2 
  dup . CR \ store count
  swap 2 + swap

\   	method_info methods[methods_count];
  0 ?DO
    \ print field idx
    s" Method[ " type
    i .
    s" ] " type CR

    dup \ save a-addr  
    s" access flags:  " type 
    @ jvm_swap_u2 
    hex. CR
    2 +
    
    dup \ save a-addr  
    s" name_index:  " type 
    @ jvm_swap_u2 
    \ dup hex. space
    addr 10 + swap \ get start of the constpool
    jvm_constpool_idx ( a-addr1 idx - a-addr2 ) 
    jvm_constpool_print_utf8 CR
    2 +
    
    dup \ save a-addr  
    s" decriptor_index:  " type 
    @ jvm_swap_u2 
    \ dup hex. space
    addr 10 + swap \ get start of the constpool
    jvm_constpool_idx ( a-addr1 idx - a-addr2 ) 
    jvm_constpool_print_utf8 CR
    2 +
    
    dup \ save a-addr  
    s" attributes counts:  " type 
    @ jvm_swap_u2 
    dup . CR
    swap 2 + swap 
    s" -----------" type CR
    ( addr count -- )
    0 ?DO 
      dup @ jvm_swap_u2 \ const idx
      addr 10 + swap 
      jvm_constpool_idx
      s" Code" jvm_constpool_cmp_utf8 
      IF
        ." BEGIN CODE ATTRIBUTE" CR
        dup \ dup address 
        addr 10 + swap \ constpool address
        jvm_constpool_print_code_attr
        ." END CODE ATTRIBUTE" CR
      ENDIF
      addr 10 + swap \ fixme
      ( const-addr addr1 -- addr2 )
      jvm_constpool_print_attr
    LOOP
    s" -----------" type CR

  LOOP
  
  CR

\   	u2 attributes_count;
  dup \ save a-addr  
  s" class attributes counts:  " type 
  @ jvm_swap_u2 
  dup . CR
  swap 2 + swap 

\   	attribute_info attributes[attributes_count];
  ( addr count -- )
  0 ?DO 
    ( const-addr addr1 -- addr2 )
    addr 10 + swap \ fixme
    jvm_constpool_print_attr
  LOOP

  drop \ drop last address
;

: Usage ( -- )
   s" Main.class" jvm_read_classfile .
   filebuffer @ jvm_print_classfile  
;


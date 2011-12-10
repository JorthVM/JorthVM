\ this file implements functionality that is needed to read class files

include util.fs

\   ClassFile {
\       u4 magic;
\       u2 minor_version;
\       u2 major_version;
\       u2 constant_pool_count;
\       cp_info constant_pool[constant_pool_count-1];
\       u2 access_flags;
\       u2 this_class;
\       u2 super_class;
\       u2 interfaces_count;
\       u2 interfaces[interfaces_count];
\       u2 fields_count;
\       field_info fields[fields_count];
\       u2 methods_count;
\       method_info methods[methods_count];
\       u2 attributes_count;
\       attribute_info attributes[attributes_count];
\   }

\ 0 Value fd-in
variable filebuffer  \ stores the address of the filebuffer of the class file
variable classfile   \ stores the wfileid of the physical classfile (not really used)

\ NOTE these are not used yet
variable jvm_p_access_flags_addr \ stores the pointer to the first address after the const pool
variable jvm_p_fields_addr \ stores the pointer to the first field
variable jvm_p_methods_addr \ stores the pointer to the first field
variable jvm_p_attributes_addr \ stores the pointer to the first field

variable jvm_p_static_fields \ stores the pointer static fields

: jvm_read_classfile ( c-addr u1 - u2 ) \ return the size of the file (in bytes)
  r/o open-file throw 
  dup classfile !            ( wfileid - wfileid ) \ store file id (wfileid)
  dup file-size throw throw  ( wfileid - wfileid u u ) \ FIXME uncatched exception ??? 
  dup
  allocate throw             ( wfileid u u - wfileid u a-addr )
  dup filebuffer !
  swap rot                   ( wfileid u a-addr - c-addr u wfileid )
  read-file throw            ( c-addr u wfileid - u2 )
  classfile @ close-file throw
  \ FIXME fixme
  0x100 allocate throw 
  jvm_p_static_fields 
  !
  jvm_p_static_fields @ 0x100 erase
;


: xxx 
." xxxxxx" .s CR
; 

\ big endian load stuff

: ?bigendian ( -- true/false )
  [ 4 allocate throw
  dup 0xdeadbeef swap !
  dup c@ 0xde = swap
  free throw ] literal
;

: ?littleendian ( -- true/false )
  [ ?bigendian invert ] literal
;

: jvm_swap_u2 { u1 -- u2 } \ little endian to big endian (2 byte)
  [ ?littleendian ] [IF]
    u1 0x00ff and 8 lshift
    u1 0xff00 and 8 rshift
    or
  [ELSE] u1 [ENDIF]
;

: jvm_swap_u4 { u1 -- u2 } \ little endian to big endian (4 byte)
  [ ?littleendian ] [IF]
    u1 0x000000ff and 24 lshift
    u1 0x0000ff00 and 8 lshift
    u1 0x00ff0000 and 8 rshift
    u1 0xff000000 and 24 rshift
    or or or
  [ELSE] u1 [ENDIF]
;

\ big endian access
: jvm_uw@ ( addr - u2) \ read big endian from memory (2 bytes)
  w@ jvm_swap_u2
;

: jvm_ul@ ( addr - u4) \ read big endian from memory (4 bytes)
  l@ jvm_swap_u4
;


: w!-be
  [ ?littleendian ] [IF] swap jvm_swap_u2 swap [ENDIF]
  w!
;

: l!-be
  [ ?littleendian ] [IF] swap jvm_swap_u4 swap [ENDIF]
  l!
;

: w@-be jvm_uw@ ;
: l@-be jvm_ul@ ;


\ -----------------------------------------------------------------------------
\ Constant Pool Entry access words

: jvm_cp_tag ( addr -- tag) \ get the tag of a given constant pool entry
  POSTPONE c@ 
; immediate

\ class
: jvm_cp_class_name_idx ( addr -- idx) \ get the name index of a class constant pool entry
  1+ jvm_uw@
; 

\ fieldref
: jvm_cp_fieldref_class_idx ( addr -- idx) \ get the class index of a fieldref constant pool entry
  1+ jvm_uw@
; 

: jvm_cp_fieldref_nametype_idx ( addr -- idx) \ get the class index of a fieldref constant pool entry
  3 + jvm_uw@
; 

\ methodref
: jvm_cp_methodref_class_idx ( addr -- idx) \ get the class index of a methodref constant pool entry
  POSTPONE jvm_cp_fieldref_class_idx 
; immediate

: jvm_cp_methodref_nametype_idx ( addr -- idx) \ get the nametype index of a methodref constant pool entry
  POSTPONE jvm_cp_fieldref_nametype_idx 
; immediate

\ interfacemethodref
: jvm_cp_interfacemethodref_class_idx ( addr -- idx) \ get the class index of a interfacemethodref constant pool entry
  POSTPONE jvm_cp_fieldref_class_idx 
; immediate

: jvm_cp_interfacemethodref_nametype_idx ( addr -- idx) \ get the nametype index of a methodref constant pool entry
  POSTPONE jvm_cp_fieldref_nametype_idx 
; immediate

\ string
: jvm_cp_string_idx ( addr -- idx) \ get the string index of a string constant pool entry
  1+ jvm_uw@
; 

\ integer
: jvm_cp_integer_bytes ( addr -- n ) \ get the bytes of an integer constant pool entry
  1+ jvm_ul@
;

\ float
: jvm_cp_float_bytes ( addr -- n ) \ get the bytes of a float constant pool entry
\ FIXME should we use the float stack?
  1+ jvm_ul@
;

\ long
: jvm_cp_long_bytes ( addr -- n2 n1 ) \ get the bytes of a long constant pool entry 
\ (n2 high 32 bit, n1 low 32 bit)
  dup 1+ jvm_ul@ swap
  5 + jvm_ul@
;

\ double
: jvm_cp_double_bytes ( addr -- n2 n1 ) \ get the bytes of a double constant pool entry 
\ (n2 high 32 bit, n1 low 32 bit)
\ FIXME should we use the float stack?
  dup 1+ jvm_ul@ swap
  5 + jvm_ul@
;

\ name type
: jvm_cp_nametype_name_idx ( addr -- idx) \ get the name index of a nametype constant pool entry
  1+ jvm_uw@
; 

: jvm_cp_nametype_desc_idx ( addr -- idx) \ get the descriptor index of a nametype constant pool entry
  3 + jvm_uw@
; 

\ Utf8
: jvm_cp_utf8_length ( addr -- n) \ get the length of the data of a utf8 constant pool entry
  1+ jvm_uw@
; 

: jvm_cp_utf8_ref ( addr1 -- addr2) \ get the reference of the data of a utf8 constant pool entry
  3 POSTPONE literal POSTPONE + 
; immediate 

: jvm_cp_utf8_c-ref ( addr1 -- addr2 n) \ get the counted reference of the data of a utf8 constant pool entry
  dup jvm_cp_utf8_ref swap
  1+ jvm_uw@
;  




\ -----------------------------------------------------------------------------
\ Attribute Entry access words
\ NOTE addr is the start of the attribute!


: jvm_attr_name_idx ( addr - idx) \ returns the name index
  POSTPONE jvm_uw@
; immediate

: jvm_attr_length ( addr - n) \ returns the attribute length
  2 POSTPONE literal POSTPONE + POSTPONE jvm_ul@
; immediate

: jvm_attr_size ( addr - n) \ returns the attribute size (header + data) 
  POSTPONE jvm_attr_length 6 POSTPONE literal POSTPONE +
; immediate




\ -----------------------------------------------------------------------------
\ Field Entry access words
\ NOTE addr is the start of the field!

: jvm_fd_access_flags ( addr - flags) \ returns the access flags
  POSTPONE jvm_uw@
; immediate

: jvm_fd_name_idx ( addr - idx) \ returns the name index
  2 POSTPONE literal POSTPONE + POSTPONE jvm_uw@
; immediate

: jvm_fd_desc_idx ( addr - idx) \ returns the descriptor index
  4 POSTPONE literal POSTPONE + POSTPONE jvm_uw@
; immediate

: jvm_fd_attr_count ( addr - n) \ returns the attribute count
  6 POSTPONE literal POSTPONE + POSTPONE jvm_uw@
; immediate

: jvm_fd_attr ( addr - addr2) \ returns the address of the first attribute
  8 POSTPONE literal POSTPONE +
; immediate

: jvm_fd_size ( addr - n) \ returns the size of the field (in bytes)
  dup \ store addr
  jvm_fd_attr 
  over
  jvm_fd_attr_count 
  0 ?DO
    dup jvm_attr_size +
  LOOP
  swap -
;

: jvm_fd_print_access_flags { flags -- }
  flags hex. ." :"
  flags 0x0001 and IF ."  ACC_PUBLIC"    ENDIF \ Declared public; may be accessed from outside its package.
  flags 0x0002 and IF ."  ACC_PRIVATE"   ENDIF \ Declared private; usable only within the defining class.
  flags 0x0004 and IF ."  ACC_PROTECTED" ENDIF \ Declared protected; may be accessed within subclasses.
  flags 0x0008 and IF ."  ACC_STATIC"    ENDIF \ Declared static.
  flags 0x0010 and IF ."  ACC_FINAL"     ENDIF \ Declared final; no further assignment after initialization.
  flags 0x0040 and IF ."  ACC_VOLATILE"  ENDIF \ Declared volatile; cannot be cached.
  flags 0x0080 and IF ."  ACC_TRANSIENT" ENDIF \ Declared transient; not written or read by a persistent object manager.
  CR
;

\ -----------------------------------------------------------------------------
\ Method Entry access words
\ NOTE addr is the start of the method!

: jvm_md_access_flags ( addr - flags) \ returns the access flags
  POSTPONE jvm_uw@
; immediate

: jvm_md_name_idx ( addr - idx) \ returns the name index
  2 POSTPONE literal POSTPONE + POSTPONE jvm_uw@
; immediate

: jvm_md_desc_idx ( addr - idx) \ returns the descriptor index
  4 POSTPONE literal POSTPONE + POSTPONE jvm_uw@
; immediate

: jvm_md_attr_count ( addr - n) \ returns the attribute count
  6 POSTPONE literal POSTPONE + POSTPONE jvm_uw@
; immediate

: jvm_md_attr ( addr - addr2) \ returns the address of the first attribute
  8 POSTPONE literal POSTPONE +
; immediate

: jvm_md_size ( addr - n) \ returns the size of the method (in bytes)
  dup \ store addr
  jvm_md_attr 
  over
  jvm_md_attr_count 
  0 ?DO
    dup jvm_attr_size 
    +
  LOOP
  swap -
;

: jvm_md_print_access_flags { flags -- }
  flags hex. ." is"
  flags 0x0001 and IF ."  ACC_PUBLIC"       ENDIF \ Declared public; may be accessed from outside its package.
  flags 0x0002 and IF ."  ACC_PRIVATE"      ENDIF \ Declared private; accessible only within the defining class.
  flags 0x0004 and IF ."  ACC_PROTECTED"    ENDIF \ Declared protected; may be accessed within subclasses.
  flags 0x0008 and IF ."  ACC_STATIC"       ENDIF \ Declared static.
  flags 0x0010 and IF ."  ACC_FINAL"        ENDIF \ Declared final; may not be overridden.
  flags 0x0020 and IF ."  ACC_SYNCHRONIZED" ENDIF \ Declared synchronized; invocation is wrapped in a monitor lock.
  flags 0x0100 and IF ."  ACC_NATIVE"       ENDIF \ Declared native; implemented in a language other than Java.
  flags 0x0400 and IF ."  ACC_ABSTRACT"     ENDIF \ Declared abstract; no implementation is provided.
  flags 0x0800 and IF ."  ACC_STRICT"       ENDIF \ Declared strictfp; floating-point mode is FP-strict
  CR
;

\ -----------------------------------------------------------------------------
\ Class File Entry access words
\ NOTE addr is the start of the file buffer

: jvm_cf_magic ( addr - u) \ returns the magic word (hopefully 0xCAFEBABE)
  POSTPONE jvm_ul@
; immediate

: jvm_cf_minor_version ( addr - u) \ returns the minor version
  4 + jvm_uw@
;

: jvm_cf_major_version ( addr - u) \ returns the major version
  6 + jvm_uw@
;

: jvm_cf_constpool_count ( addr - u) \ returns number of entries+1 in the constant pool
\ NOTE The value of the constant_pool_count item is equal to the number of entries in
\ the constant_pool table plus one. A constant_pool index is considered valid if it i
\ greater than zero and less than constant_pool_count, with the exception for constants
\ of type long and double noted in §4.4.5.
  8 + jvm_uw@
;

: jvm_cf_constpool_addr ( addr - const-addr) \ returns start address of the constant pool
\ NOTE `jvm_cf_constpool_addr` might be longer than `10 +` but the semantics are clear and 
\ refactoring is much easier
  10 POSTPONE literal POSTPONE +
; immediate



: jvm_constpool_type_size { addr } ( a-addr - n2 ) \ get the size of an entry in the const table
  addr jvm_cp_tag
  CASE
     7 OF 3 ENDOF \ CONSTANT_Class               7
     9 OF 5 ENDOF \ CONSTANT_Fieldref            9
    10 OF 5 ENDOF \ CONSTANT_Methodref          10
    11 OF 5 ENDOF \ CONSTANT_InterfaceMethodref 11
     8 OF 3 ENDOF \ CONSTANT_String              8
     3 OF 5 ENDOF \ CONSTANT_Integer             3
     4 OF 5 ENDOF \ CONSTANT_Float               4
     5 OF 9 ENDOF \ CONSTANT_Long                5
     6 OF 9 ENDOF \ CONSTANT_Double              6
    12 OF 5 ENDOF \ CONSTANT_NameAndType        12
     1 OF         \ CONSTANT_Utf8                1
      addr jvm_cp_utf8_length 
      3 + \ add u1 (tag) and u2 (length)
    ENDOF
    \ default
    drop 
    s" Unknown Constant Pool Type " exception throw
  ENDCASE
;


: jvm_cf_access_flags_addr ( addr - addr2) \ returns address of the access flag
  dup jvm_cf_constpool_addr swap
  jvm_cf_constpool_count
  \ cp_info constant_pool[constant_pool_count-1];
  1 ?DO
    dup jvm_constpool_type_size +
  LOOP
  \ dup jvm_p_access_flags_addr !
;

\ : jvm_cf_access_flags_addr ( - addr2) \ returns address of the access flag
\ FIXME this a very strange approach :/ maybe we can think of something smarter
\ (word renaming maybe?)
\  POSTPONE jvm_p_access_flags_addr POSTPONE @
\ ; immediate


: jvm_cf_access_flags ( addr - flag) \ returns the access flag
  jvm_cf_access_flags_addr
  jvm_uw@
;

: jvm_cf_this_class ( addr - idx) \ returns the this class index
  jvm_cf_access_flags_addr 2 +
  jvm_uw@
;

: jvm_cf_super_class ( addr - idx) \ returns the super class index
  jvm_cf_access_flags_addr 4 +
  jvm_uw@
;

: jvm_cf_interfaces_count ( addr - n) \ returns number of interfaces
  jvm_cf_access_flags_addr 6 +
  jvm_uw@
;

: jvm_cf_interfaces_addr ( addr - addr2) \ returns address of the first interface field
  jvm_cf_access_flags_addr 8 +
; 

: jvm_cf_fields_count_addr ( addr - addr2) \ returns address of the fields_count field
  dup jvm_cf_interfaces_count 2 * \ idx size u2
  swap jvm_cf_interfaces_addr +
;

: jvm_cf_fields_count ( addr - n) \ returns fields_count
  POSTPONE jvm_cf_fields_count_addr 
  POSTPONE jvm_uw@
; immediate

: jvm_cf_fields_addr ( addr - addr2) \ returns address of the first field
  POSTPONE jvm_cf_fields_count_addr 2 POSTPONE literal POSTPONE +
; immediate

: jvm_cf_fields_size ( addr - n) \ returns the complete size of all fields in bytes
  dup jvm_cf_fields_addr dup 
  rot jvm_cf_fields_count
  0 ?DO
    \ ( addr1 - addr2 )
    dup jvm_fd_size +
  LOOP
  swap -
;

: jvm_cf_methods_count_addr ( addr - addr2) \ returns address of the methods_count field
  dup jvm_cf_fields_addr 
  swap jvm_cf_fields_size +
;

: jvm_cf_methods_count ( addr - n) \ returns methods_count 
  POSTPONE jvm_cf_methods_count_addr POSTPONE jvm_uw@
; immediate

: jvm_cf_methods_addr ( addr - addr2) \ returns address of the first field
  POSTPONE jvm_cf_methods_count_addr 2 POSTPONE literal POSTPONE +
; immediate

: jvm_cf_methods_size ( addr - n) \ returns the complete size of all fields in bytes
  dup jvm_cf_methods_addr dup 
  rot jvm_cf_methods_count
  0 ?DO
    \ ( addr1 - addr2 )
    dup jvm_md_size +
  LOOP
  swap -
;

: jvm_cf_attr_count_addr ( addr - addr2) \ returns address of the attributes_count field
  dup jvm_cf_methods_addr 
  swap jvm_cf_methods_size +
;

: jvm_cf_attr_count ( addr - n) \ returns attributes_count 
  POSTPONE jvm_cf_attr_count_addr POSTPONE jvm_uw@
; immediate

: jvm_cf_attr_addr ( addr - addr2) \ returns address of the first attribute
  POSTPONE jvm_cf_attr_count_addr 2 POSTPONE literal POSTPONE +
; immediate

: jvm_cf_attr_size ( addr - n) \ returns the complete size of all attributes in bytes
  dup jvm_cf_attr_addr dup 
  rot jvm_cf_attr_count
  0 ?DO
    \ ( addr1 - addr2 )
    dup jvm_attr_size +
  LOOP
  swap -
;




\ -----------------------------------------------------------------------------
\ Constant Pool Entry type words

: jvm_constpool_type_name ( a-addr - c-addr n ) \ get the name of an entry in the const table
  jvm_cp_tag
  CASE
     7 OF s" CONSTANT_Class" ENDOF
     9 OF s" CONSTANT_Fieldref" ENDOF
    10 OF s" CONSTANT_Methodref" ENDOF
    11 OF s" CONSTANT_InterfaceMethodref" ENDOF
     8 OF s" CONSTANT_String" ENDOF
     3 OF s" CONSTANT_Integer" ENDOF
     4 OF s" CONSTANT_Float" ENDOF
     5 OF s" CONSTANT_Long" ENDOF
     6 OF s" CONSTANT_Double" ENDOF
    12 OF s" CONSTANT_NameAndType" ENDOF
     1 OF s" CONSTANT_Utf8" ENDOF
\ default
    drop 
    s" Unknown Constant Pool Type " exception throw
  ENDCASE
;


\ -----------------------------------------------------------------------------
: jvm_constpool_idx ( a-addr1 idx - a-addr2 ) 
\ returns address of entry with index idx from constant pool starting at address a-addr1
  1 ?DO
    dup jvm_constpool_type_size +
  LOOP
;

: jvm_constpool_print_utf8 ( addr -- ) \ prints a utf8 string
  ( e-addr - c-addr length )
  jvm_cp_utf8_c-ref 
  ( c-addr length - end-addr c-addr )
  over + swap

  begin
  2dup
  > while
    xc@+ xemit
  repeat
  2drop
;
: jvm_constpool_print_utf8_idx ( const-addr idx -- ) \ prints a utf8 string
  POSTPONE jvm_constpool_idx POSTPONE jvm_constpool_print_utf8
; immediate

: jvm_constpool_cmp_utf8 { addr xc-addr n -- } \ compare a counted xc string with a utf8 constant
\ FIXME not really efficient. may be we should ignore utf8 for the moment? anyway cell wide compare
\ would be more efficient
  addr jvm_cp_utf8_length \ read length
  n = IF
    true
    n 0 ?DO
    ( b1 -- [b1 & *addr1=*addr2] )
    xc-addr i + addr 3 + i +
    c@ swap c@ = 
    and \ & b1
    LOOP
  ELSE
    false
  ENDIF
;

: jvm_constpool_print_classname { const-addr class-addr -- } \ print the class name of a constpool class entry
  class-addr 
  jvm_cp_class_name_idx \ read idx
  const-addr swap 
  jvm_constpool_idx
  jvm_constpool_print_utf8
;

: jvm_constpool_print_classname_idx ( const-addr idx -- ) \ print the class name of a class idx
  POSTPONE over POSTPONE -rot ( const-addr const-addr idx)
  POSTPONE jvm_constpool_idx ( const-addr idx - a-addr ) 
  POSTPONE jvm_constpool_print_classname 
; immediate

: jvm_constpool_attr_size { attr-addr -- n } \ get the size of the attribute entry (in bytes)
  2 + \ length field
  jvm_ul@
  6 + \ add u2 (name_index) and u4 (length)
;

: jvm_constpool_print_attr { const-addr addr1 -- addr2 } 
  \ const-addr: address of the constpool, addr1: start address of the attribute, addr2: address after the attribute
  addr1
  dup \ dup addr
  jvm_uw@ \ load name idx
  \ dup hex. space
  const-addr swap \ get start of the constpool
  jvm_constpool_idx ( a-addr1 idx - a-addr2 ) 
  s" attribute name:  " type 
  jvm_constpool_print_utf8 CR
  2 +

  dup \ save a-addr  
  jvm_ul@ 
  s" attribute length:  " type 
  dup . CR
  4 + + \ add length filed and info field
;

: jvm_constpool_print_code_attr { const-addr addr1 -- } 
  addr1 6 + 
  \ u2 max_stack
  dup \ dup addr
  ." max stack:  " 
  jvm_uw@ \ load name idx
  . CR
  2 +

  \ u2 max_locals
  dup \ dup addr
  ." max locals:  " 
  jvm_uw@ \ load name idx
  . CR
  2 +

  \ u4 code_length
  dup \ dup addr
  ." code length:  " 
  jvm_ul@ \ load name idx
  dup . CR
  swap 4 + swap
  
  2dup \ dup ( addr n )
  dump 
  + \ calc new address
  
  \ u4 exception_table_length
  dup \ dup addr
  ." exception table length:  " 
  jvm_uw@ \ load name idx
  dup . CR
  swap 2 + swap
  
  \ u2 attributes_count
  \ TODO examin exception table
  8 * \ u2 start_pc u2 end_pc u2 handler_pc u2 catch_type
  + 

  \ u2 attributes_count
  dup \ save a-addr  
  ." attributes count:  " 
  jvm_uw@ 
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

: jvm_constpool_print_nametype { const-addr nt-addr -- } 
  \ const-addr: address pool address 
  \ nt-addr: address of the name type constant pool entry
  nt-addr
  space ." Name: " [CHAR] " emit 
  jvm_cp_nametype_name_idx \ get name idx
  const-addr 
  swap jvm_constpool_print_utf8_idx
  [CHAR] " emit space
  
  nt-addr
  space ."  Desc: " [CHAR] " emit 
  jvm_cp_nametype_desc_idx \ get name idx
  const-addr
  swap jvm_constpool_print_utf8_idx
  [CHAR] " emit space
;

: jvm_cf_print_access_flags { flags -- }
  flags hex. ." is"
  flags 0x0001 and IF ."  ACC_PUBLIC"    ENDIF \ Declared public; may be accessed from outside its package.
  flags 0x0010 and IF ."  ACC_FINAL"     ENDIF \ Declared final; no subclasses allowed.
  flags 0x0020 and IF ."  ACC_SUPER"     ENDIF \ Treat superclass methods specially when invoked by the invokespecial instruction.
  flags 0x0200 and IF ."  ACC_INTERFACE" ENDIF \ Is an interface, not a class.
  flags 0x0400 and IF ."  ACC_ABSTRACT"  ENDIF \ Declared abstract; may not be instantiated.
  CR
;

\ -----------------------------------------------------------------------------
\ -----------------------------------------------------------------------------
: jvm_cf_magic_print ( addr -- )
\ first 4 bytes should be 0xCAFEBABE
  ." Magic: "
  jvm_cf_magic hex. CR
;

: jvm_cf_version_print ( addr -- )
  ." Version: "
  dup jvm_cf_minor_version 1 u.r [CHAR] . emit jvm_cf_major_version . CR
;

: jvm_cf_constpool_count_print ( addr -- )
  ." Constant Pool count: "
  jvm_cf_constpool_count . CR
;

: jvm_cf_constpool_print { addr -- }
  addr jvm_cf_constpool_addr
  addr jvm_cf_constpool_count
  1 ?DO
    ." [ " i addr jvm_cf_constpool_count 1- decimal_places .r ."  ] "
    dup jvm_constpool_type_name type  
    dup \ addr, used in the case statement
    dup jvm_cp_tag \ read tag
    CASE
    ( addr1 - ) \ addr1: address of the constpool entry
    1 OF  \ if utf8 string, print it!
      space [CHAR] " emit jvm_constpool_print_utf8 [CHAR] " emit space
    ENDOF
    10 OF \ MethodRef
      jvm_cp_methodref_nametype_idx  \ get nametype idx
      addr jvm_cf_constpool_addr \ get const pool address
      swap jvm_constpool_idx \ get nametype addr
      addr jvm_cf_constpool_addr \ get const pool address
      swap
      jvm_constpool_print_nametype 
    ENDOF
    9 OF \ FieldRef
      jvm_cp_fieldref_nametype_idx  \ get nametype idx
      addr jvm_cf_constpool_addr \ get const pool address
      swap jvm_constpool_idx \ get nametype addr
      addr jvm_cf_constpool_addr \ get const pool address
      swap
      jvm_constpool_print_nametype 
    ENDOF
    12 OF \ NameAndType
      addr jvm_cf_constpool_addr \ get const pool address
      swap
      jvm_constpool_print_nametype 
    ENDOF
      drop 
    ENDCASE
    ." : " 
    dup jvm_constpool_type_size dup . +
    CR
  LOOP
  drop \ what a waste
;

: jvm_cf_access_flags_print ( addr -- )
  ." Access Flags (Class): "
  jvm_cf_access_flags { flags -- }
  flags hex. ." is"
  flags 0x0001 and IF ."  ACC_PUBLIC"    ENDIF \ Declared public; may be accessed from outside its package.
  flags 0x0010 and IF ."  ACC_FINAL"     ENDIF \ Declared final; no subclasses allowed.
  flags 0x0020 and IF ."  ACC_SUPER"     ENDIF \ Treat superclass methods specially when invoked by the invokespecial instruction.
  flags 0x0200 and IF ."  ACC_INTERFACE" ENDIF \ Is an interface, not a class.
  flags 0x0400 and IF ."  ACC_ABSTRACT"  ENDIF \ Declared abstract; may not be instantiated.
  CR
;

: jvm_cf_this_class_print { addr -- }
  ." This Class: "
  addr jvm_cf_constpool_addr \ get start of the constpool
  addr jvm_cf_this_class
  jvm_constpool_print_classname_idx CR
;

: jvm_cf_super_class_print { addr -- }
  ." Super Class: " 
  addr jvm_cf_constpool_addr \ get start of the constpool
  addr jvm_cf_super_class 
  jvm_constpool_print_classname_idx CR 
;

: jvm_cf_interfaces_count_print ( addr -- )
  ." Interfaces Count: "
  jvm_cf_interfaces_count . CR
;

: jvm_cf_interfaces_print { addr -- }
  addr jvm_cf_interfaces_addr
  addr jvm_cf_interfaces_count
  0 ?DO
    addr jvm_cf_constpool_addr \ get start of the constpool
    over \ get addr  
    jvm_uw@ 
    ." Interface[ " i . ." ] "
    jvm_constpool_print_classname_idx CR
    2 + 
  LOOP
  drop \ don't need the address
;

: jvm_cf_fields_count_print ( addr -- )
  ." Fields Count: "
  jvm_cf_fields_count . CR
;

: jvm_cf_fields_print { addr -- }
  addr jvm_cf_fields_addr
  addr jvm_cf_fields_count
  0 ?DO
    ." Field[ " i . ." ] " CR \ print field idx

    dup 
    jvm_fd_access_flags
    ." access flags: " jvm_fd_print_access_flags

    addr jvm_cf_constpool_addr \ get start of the constpool
    over jvm_fd_name_idx
    ." name_index: "
    jvm_constpool_print_utf8_idx CR
    
    addr jvm_cf_constpool_addr \ get start of the constpool
    over jvm_fd_desc_idx
    ." decriptor_index: " 
    jvm_constpool_print_utf8_idx CR
    
    
    dup \ save a-addr  
    jvm_fd_attr_count 
    ." attributes counts: " 
    dup . CR
    swap jvm_fd_attr swap

    addr jvm_cf_constpool_addr -rot
    ." -----------" CR
    ( const-addr addr count -- )
    0 ?DO 
      dup
      ( const-addr const-addr addr1 -- const-addr addr2 )
      jvm_constpool_print_attr
    LOOP
    ." -----------" CR
    nip \ const addr

  LOOP
  drop \ addr2
;

: jvm_cf_methods_count_print ( addr -- )
  ." Methods Count: "
  jvm_cf_methods_count . CR
;

: jvm_cf_methods_print { addr -- }
  addr jvm_cf_methods_addr
  addr jvm_cf_methods_count
  0 ?DO
    ." Method[ " i . ." ] " CR \ print field idx

    dup \ save a-addr  
    jvm_md_access_flags
    ." access flags: " jvm_md_print_access_flags
    
    addr jvm_cf_constpool_addr \ get start of the constpool
    over jvm_md_name_idx
    ." name_index:  "
    jvm_constpool_print_utf8_idx CR
    
    addr jvm_cf_constpool_addr \ get start of the constpool
    over jvm_md_desc_idx
    ." decriptor_index:  " 
    jvm_constpool_print_utf8_idx CR
    
    dup \ save a-addr  
    jvm_md_attr_count 
    ." attributes counts:  " 
    dup . CR
    swap jvm_md_attr swap

    addr jvm_cf_constpool_addr -rot
    ." -----------" CR
    ( addr count -- )
    0 ?DO 
      dup jvm_uw@ \ const idx
      addr jvm_cf_constpool_addr swap 
      jvm_constpool_idx
      s" Code" jvm_constpool_cmp_utf8 
      IF
        ." BEGIN CODE ATTRIBUTE" CR
        dup \ dup address 
        addr jvm_cf_constpool_addr swap \ constpool address
        jvm_constpool_print_code_attr
        ." END CODE ATTRIBUTE" CR
      ENDIF
      addr jvm_cf_constpool_addr swap \ fixme
      ( const-addr addr1 -- addr2 )
      jvm_constpool_print_attr
    LOOP
    ." -----------" CR
    nip \ delete old address

  LOOP
  drop
;

: jvm_cf_attr_count_print ( addr -- )
  ." Attributes Count (Class): "
  jvm_cf_attr_count . CR
;

: jvm_cf_attr_print { addr -- }
  addr jvm_cf_attr_addr
  addr jvm_cf_attr_count
  ( addr count -- )
  0 ?DO 
    ( const-addr addr1 -- addr2 )
    addr jvm_cf_constpool_addr swap \ fixme
    jvm_constpool_print_attr
  LOOP
  drop \ drop last address
;

\ TODO : move somewhere else
\ : jvm_cp_utf8_c-ref_by_idx ( cp-addr idx -- addr2 n) \ get the counted reference of the data of a utf8 constant pool entry
\  POSTPONE jvm_constpool_idx
\  POSTPONE jvm_cp_utf8_c-ref
\ ; immediate  


\ TODO : move somewhere else
: jvm_get_method_by_nametype { cf-addr c-addr-name n-name c-addr-desc n-desc -- md-addr b } \ get the address of a method entry
\ by a name and type (desc) pair
\ returns the address and a flag that indicates if the method has been found
\ if the method hasn't been found md-addr is the address of the attr_count field
  true
  true
  cf-addr jvm_cf_attr_count_addr \ first address after methods area 
  cf-addr jvm_cf_methods_addr \ first method address
  BEGIN
    ( !found !found attr-addr next-addr -- )
    2dup >
    3 roll and
  WHILE
    ( !found attr-addr curr-addr) \ next-addr is not cur-addr
    2 roll drop \ drop found flag

    \ cf-addr jvm_cf_constpool_addr \ get start of the constpool
    \ over jvm_md_name_idx 
    \ jvm_constpool_print_utf8_idx
     
    ( attr-addr cur-addr)
    cf-addr jvm_cf_constpool_addr \ get start of the constpool
    over jvm_md_name_idx 
    jvm_constpool_idx \ get address of the utf8 string
    c-addr-name n-name \ input name
    jvm_constpool_cmp_utf8 \ compare
    IF
      ( attr-addr cur-addr)
      cf-addr jvm_cf_constpool_addr \ get start of the constpool
      over jvm_md_desc_idx 
      jvm_constpool_idx \ get address of the utf8 string
      c-addr-desc n-desc \ input desc
      jvm_constpool_cmp_utf8 \ compare
      invert \ if found we stop executing the loop
    ELSE
      true
    ENDIF
    ( !name attr-addr cur-addr !match)
    \ FIXME begin ugly 
    -rot     \ insert the flag 
    2 pick   \ get it back
    -rot     \ and insert it again
    2 pick   \ get it back
    \ FIXME end ugly
    IF 
      dup jvm_md_size +
    ENDIF
  REPEAT
  nip \ drop attr-addr
  swap \ exception flag top of stack
  invert \ and turn it into a found flag
;




: jvm_print_classfile { addr -- } \ addr stores the start address of the memory where the file is stored
  ." ===================="
  CR
  addr jvm_cf_magic_print            \ u4 magic
  addr jvm_cf_version_print          \ u2 minor_version, u2 major_version
  CR
  addr jvm_cf_constpool_count_print  \ u2 constant_pool_count
  addr jvm_cf_constpool_print        \ cp_info constant_pool[constant_pool_count-1]
  CR
  addr jvm_cf_access_flags_print     \ u2 access_flags
  addr jvm_cf_this_class_print       \ u2 this_class
  addr jvm_cf_super_class_print      \ u2 super_class
  CR
  addr jvm_cf_interfaces_count_print \ u2 interfaces_count
  addr jvm_cf_interfaces_print       \ u2 interfaces[interfaces_count]
  CR
  addr jvm_cf_fields_count_print     \ u2 fields_count
  addr jvm_cf_fields_print           \ field_info fields[fields_count]
  CR
  addr jvm_cf_methods_count_print    \ u2 methods_count
  addr jvm_cf_methods_print          \ method_info methods[methods_count]
  CR
  addr jvm_cf_attr_count_print       \ u2 attributes_count
  addr jvm_cf_attr_print             \ attribute_info attributes[attributes_count]
  ." ===================="
  CR
;

: Usage ( -- )
   s" ../testfiles/Main.class" jvm_read_classfile .
   filebuffer @ jvm_print_classfile  
;


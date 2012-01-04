\ vim: sw=2 ts=2 sta et

\ ========
\ *! rtconstpool
\ *T Runtime Constant Pool
\ 
\ *P This file contains the implentation of the Runtime Constant Pool 
\ ** as specified in the JVM specification §5.1 and §5.3.
\ ========

require exception.fs
require classfile.fs

\ ========
\ *S Runtime Constant Pool Struct
\ ========
\ 
\ *C runtime_constant_ool {
\ **    addr classfile;
\ **    addr constpool_table;
\ **    wid  class_info;
\ **    wid  fieldref_info;
\ **    wid  methodref_info;
\ **    wid  interfacemethodref_info;
\ **    addr integer_info_table;
\ **    addr float_info_table;
\ **    addr long_info_table;
\ **    addr double_info_table;
\ **  }
\ 

 0 cells constant jvm_rtcp.classfile
 1 cells constant jvm_rtcp.constpool_table
 2 cells constant jvm_rtcp.class_info
 3 cells constant jvm_rtcp.fieldref_info
 4 cells constant jvm_rtcp.methodref_info
 5 cells constant jvm_rtcp.interfacemethodref_info
 6 cells constant jvm_rtcp.integer_info_table
 7 cells constant jvm_rtcp.float_info_table
 8 cells constant jvm_rtcp.long_info_table
 9 cells constant jvm_rtcp.double_info_table

10 cells constant jvm_rtcp.size()

: jvm_rtcp.getClassfile() ( addr1 -- addr2 )
\ *G get the classfile addr from runtime classfile pool
  ( jvm_rtcp.classfile + ) @
;

: jvm_rtcp.getConstpool() { addr idx -- addr2 }
\ *G get the address of the constant pool entry denoted by idx
\ NOTE no sanity checks (e.g. 0 < idx < constant_pool_count)
  addr jvm_rtcp.constpool_table + @ idx 1- cells + @
;

\ FIXME other name?
: jvm_rtcp.getClassName() { addr idx -- c-addr n }
\ *G get the qualified classname from a class idx
  addr idx jvm_rtcp.getConstpool()
  ( class_addr)
  jvm_cp_class_name_idx
  ( name_idx)
  addr swap 
  ( addr_rtcp name_idx)
  jvm_rtcp.getConstpool()
  ( addr_utf8)
  jvm_cp_utf8_c-ref
;
\ FIXME other name?
: jvm_rtcp.getNameType() { addr idx -- c-addr n }
\ *G get the qualified NameType from a class idx
  addr idx jvm_rtcp.getConstpool()
  ( nametype_addr)
  dup jvm_cp_nametype_name_idx
  ( nametype_addr name_idx)
  addr swap 
  ( nametype addr_rtcp name_idx)
  jvm_rtcp.getConstpool()
  ( nametype addr_utf8)
  jvm_cp_utf8_c-ref
  ( nametype c-addr n)
  rot
\  drop

  ( c-addr n nametype)
  jvm_cp_nametype_desc_idx
  ( c-addr n desc_idx)
  addr swap 
  ( c-addr n addr_rtcp desc_idx)
  jvm_rtcp.getConstpool()
  ( c-addr n addr_utf8)
  jvm_cp_utf8_c-ref
  ( c-addr n c-addr2 n2)
  jvm_nametype_identifier
;


: jvm_rtcp.getClass_info() ( addr -- wid )
\ *G get the class_info wordlist id from runtime classfile pool
  jvm_rtcp.class_info + @
;

: jvm_rtcp.getFieldref_info() ( addr -- wid )
\ *G get the fieldref_info wordlist id from runtime classfile pool
  jvm_rtcp.fieldref_info + @
;

: jvm_rtcp.getMethodref_info() ( addr -- wid )
\ *G get the methodref_info wordlist id from runtime classfile pool
  jvm_rtcp.methodref_info + @
;

: jvm_rtcp.getInterfacemethodref_info() ( addr -- wid )
\ *G get the interfacemethodref_info wordlist id from runtime classfile pool
  jvm_rtcp.interfacemethodref_info + @
;

: jvm_rtcp.getInteger_info() { addr idx -- int }
\ *G get the integer info from runtime classfile pool
  addr jvm_rtcp.integer_info_table + @ idx + @
;

: jvm_rtcp.getFloat_info() { addr idx -- float }
\ *G get the float info from runtime classfile pool
  addr jvm_rtcp.float_info_table + @ idx + @
;

: jvm_rtcp.getLong_info() { addr idx -- long-lsb long-msb }
\ *G get the long info from runtime classfile pool
  \ FIXME make usage of 64 bit cells?
  \ FIXME little/big endian issues?
  addr jvm_rtcp.long_info_table + @ idx 2 * + 2@
;

: jvm_rtcp.getDouble_info() { addr idx -- double-lsb double-msb }
\ *G get the double info from runtime classfile pool
  \ FIXME make usage of 64 bit cells?
  \ FIXME little/big endian issues?
  addr jvm_rtcp.double_info_table + @ idx 2 * + 2@
;

: jvm_rtcp.createConstPoolTable() { addr1 -- addr2 }
\ *G create a constpool table from a classfile
  addr1 jvm_cf_constpool_count 1- cells allocate throw
  dup
  addr1 jvm_cf_constpool_addr
  addr1 jvm_cf_constpool_count
  \ TODO fix long/double stuff
  1 ?DO
    ( addr1 addr2 --)
    \ addr1 address of the current table entry
    \ addr2 address of the current constpool entry
    2dup swap !                   \ store table entry
    swap 1 cells + swap           \ imcrement table
    dup jvm_constpool_type_size + \ increment constpool entry
  LOOP
  2drop \ what a waste
;

: jvm_rtcp.new() { addr -- addr2 }
\ *G Create a new runtime constant pool from classfile at `addr1'
  jvm_rtcp.size() allocate throw
  dup ." new addr: " . dup ." - " hex. cr
  dup addr swap ( jvm_rtcp.classfile +) ! \ store classfile reference
  addr jvm_rtcp.createConstPoolTable() 
  over jvm_rtcp.constpool_table + ! 
  wordlist over jvm_rtcp.class_info + ! \ store class_info wordlist
  wordlist over jvm_rtcp.fieldref_info + ! \ store fieldref_info wordlist
  wordlist over jvm_rtcp.methodref_info + ! \ store methodref_info wordlist
  wordlist over jvm_rtcp.interfacemethodref_info + ! \ store interfacemethodref_info wordlist

  .s CR
  \ iterate over constpool
  addr jvm_cf_constpool_addr
  addr jvm_cf_constpool_count
  1 ?DO
    dup jvm_constpool_type_name type  
    dup jvm_cp_tag \ read tag
    CASE
    ( addr1 addr2 - ) 
    \ addr1: address of the runtime constant pool
    \ addr2: address of the constpool entry
    CONSTANT_Class OF
      dup \ next pointer
      dup \ value for wordlist
      jvm_cp_class_name_idx
      addr jvm_cf_constpool_addr 
      swap 
      jvm_constpool_idx
      jvm_cp_utf8_c-ref
      2dup space type
      4 pick \ get rt constpool addr
      jvm_rtcp.getClass_info()
      jvm_add_word
      \ FIXME for the time beeing the address of the Classinfo entry is 
      \ added to the class_info wordlist
    ENDOF
    \ default
    ENDCASE
    cr
    dup jvm_constpool_type_size +
  LOOP
  drop \ what a waste
  \ fill class_info

  \ fill fieldref_info

  \ fill methodref_info
  
  \ fill interfacemethodref_info
  ." Class_info" 
  dup jvm_rtcp.getClass_info() wordlist-words CR CR
  ." Fieldref_info" 
  dup jvm_rtcp.getFieldref_info() wordlist-words CR CR
  ." Methodref_info" 
  dup jvm_rtcp.getMethodref_info() wordlist-words CR CR
  ." Interfacemethodref_info" 
  dup jvm_rtcp.getInterfacemethodref_info() wordlist-words CR CR
  
  ." end new() " .s cr
;

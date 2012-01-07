\ vim: sw=2 ts=2 sta et

\ ========
\ *! class
\ *T JVM representation of a Java class
\ 
\ *P This file contains the class represenation in the JVM
\ ========

require exception.fs
require classfile.fs
require rtconstpool.fs

\ ========
\ *S Field Access Helpers
\ ========
\ 
: jvm_field_desc_size { c-addr n -- u }
\ *G Get the size of a field (in Bytes)

\ *C B              byte             signed byte
\ *C C              char             Unicode character
\ *C D              double           double-precision floating-point value
\ *C F              float            single-precision floating-point value
\ *C I              int              integer
\ *C J              long             long integer
\ *C L<classname>;  reference        an instance of class <classname>
\ *C S              short            signed short
\ *C Z              boolean          true or false
\ *C [              reference        one array dimension 

\ TODO be more efficient (e.g. on 64 bit systems)
  c-addr c@ \ get first char
  CASE
    [CHAR] B OF 1 cells ENDOF
    [CHAR] C OF 1 cells ENDOF
    [CHAR] D OF 2 cells ENDOF
    [CHAR] F OF 1 cells ENDOF
    [CHAR] I OF 1 cells ENDOF
    [CHAR] J OF 2 cells ENDOF
    [CHAR] L OF 1 cells ENDOF
    [CHAR] S OF 1 cells ENDOF
    [CHAR] Z OF 1 cells ENDOF
    [CHAR] [ OF 1 cells ENDOF
    \ default
    drop 1
  ENDCASE
;

: jvm_field_@ ( addr -- val )
\ *G Load a static field (32bit) from address
  POSTPONE @
; immediate

: jvm_field_2@ ( addr -- val_msb val_lsb )
\ *G Load a static field (64bit with 2 32bit fragments) from address
\ FIXME stack endianess ?
  POSTPONE 2@
; immediate

: jvm_field_! ( val addr -- )
\ *G store a static field (32bit) from address
  POSTPONE !
; immediate

: jvm_field_2! ( val_lsb val_msb addr -- )
\ *G store a static field (64bit with 2 32bit fragments) from address
\ FIXME stack endianess ?
  POSTPONE 2!
; immediate

\ ========
\ *S Class Struct
\ ========
\ 
\ *C jvm_class {
\ **    addr status;       \ status of the class
\ **    addr init_loader;  \ initial loader
\ **    addr rtcp;         \ runtime constant pool
\ **    addr super;        \ link to super class
\ **    wid  filed_offset; \ static field offset wordlist
\ **    addr field_table;  \ static field table
\ **  }
\ 

 0 cells constant jvm_class.status
 1 cells constant jvm_class.init_loader
 2 cells constant jvm_class.rtcp
 3 cells constant jvm_class.super
 4 cells constant jvm_class.field_offset
 5 cells constant jvm_class.field_table

 6 cells constant jvm_class.size()

 1 constant jvm_class.STATUS:UNINIT
 2 constant jvm_class.STATUS:PREPARED
 3 constant jvm_class.STATUS:INIT

: jvm_class.getStatus() ( addr1 -- status )
\ *G get the status of the class (prepared, loaded, etc.)
  ( jvm_class.status + ) @
;

: jvm_class.getInit_loader() ( addr1 -- addr2 )
\ *G get the address of the initial loader
  jvm_class.init_loader + @
;

: jvm_class.getRTCP() ( addr1 -- addr2 )
\ *G get the address of the runtime classfile pool
  jvm_class.rtcp + @
;

: jvm_class.getSuper() ( addr1 -- addr2 )
\ *G get the address of the super class
  jvm_class.super + @
;

: jvm_class.getField_offset_wid() ( addr -- wid )
\ *G get the wordlist id for the static field offset translation
  jvm_class.field_offset + @
;

: jvm_class.getField_offset() { addr c-addr n -- off woir }
\ *G get the offset of a static field
  addr jvm_class.getField_offset_wid()
  c-addr n rot
  jvm_find_word throw
  0
;

: jvm_class.getStaticOffset() { addr off -- value }
\ *G get the value of a static field (32bit) by offset
  addr jvm_class.field_table + off cells + jvm_field_@
;

: jvm_class.getStatic() { addr c-addr n -- value woir }
\ *G get the value of a static field (32bit) by name
  \ ." trying to get " c-addr n type  CR
  addr c-addr n jvm_class.getField_offset() throw
  \ ." offset: " dup . CR
  addr swap jvm_class.getStaticOffset() 0
;

: jvm_class.setStaticOffset() { addr val off -- }
\ *G set the value of a static field (32bit) by offset
  val addr jvm_class.field_table + off cells +
  jvm_field_!
;

: jvm_class.setStatic() { addr val c-addr n -- woir }
\ *G get the value of a static field (32bit) by name
  \ ." trying to set " c-addr n type  ."  to " val . CR
  addr val 
  over c-addr n jvm_class.getField_offset() throw \ get off
  \ ." offset: " dup . CR
  jvm_class.setStaticOffset() 
  0
;

: jvm_class.getStatic2Offset() { addr off -- lsb-value msb-value }
\ *G get the value of a static field (64bit) by offset
  addr jvm_class.field_table + off cells + jvm_field_2@
;

: jvm_class.getStatic2() { addr c-addr n -- lsb-value msb-value wior }
\ *G get the value of a static field (64bit) by name
  addr c-addr n jvm_class.getField_offset() throw
  addr swap jvm_class.getStatic2Offset() 0
;

: jvm_class.new() ( -- addr)
\ *G Create a new (uninitialized) class
  jvm_class.size() allocate throw
  dup jvm_class.size() erase
  jvm_class.STATUS:UNINIT over ( jvm_class.status + ) !
  \ 0 over jvm_class.init_loader + !
  \ 0 over jvm_class.rtcp + !
  wordlist over jvm_class.field_offset + !
  \ 0 over jvm_class.field_table + !
;

: jvm_class.prepare() { addr_cl loader addr_cf -- wior }
\ *G prepare a class using a classfile and a loader
\ NOTE loader is a value to identify loaders
  
  \ create runtime constant pool
  addr_cf jvm_rtcp.new() addr_cl jvm_class.rtcp + !
  \ TODO add super class

  \ create static fields
  0
  addr_cf jvm_cf_fields_addr
  addr_cf jvm_cf_fields_count
  0 ?DO
    ( off addr )
    \ off next free offset of the table
    \ addr address of the field entry
    dup ACC_STATIC jvm_fd_?flags IF 
      addr_cf over jvm_fd_identifier
      ( off addr c-addr n )
      addr_cf jvm_cf_constpool_addr 
      ( off addr c-addr n addr2 )
      3 pick jvm_fd_desc_idx 
      ( off addr c-addr n addr2 idx2 )
      jvm_constpool_idx
      ( off addr c-addr n addr3 )
      jvm_cp_utf8_c-ref
      ( off addr c-addr n c-addr2 n2 )
      jvm_field_desc_size -rot
      ( off addr size c-addr n )
      4 pick -rot
      ( off addr size off c-addr n )
      addr_cl jvm_class.getField_offset_wid() 
      ( off addr size off c-addr n wid )
      jvm_add_word
      ( off addr size )
      rot + swap 
    ENDIF

    dup jvm_fd_size + \ next entry
  LOOP
  drop \ drop last+1 field address
  
  allocate throw                                         \ allocate static table memory
  \ TODO Initial values §5.4.2, 1st paragraph

  addr_cl jvm_class.field_table + !                         \ store table

  jvm_class.STATUS:PREPARED addr_cl jvm_class.status + ! \ store status
  loader addr_cl jvm_class.init_loader + !               \ store loader
  0
;

: jvm_class.init() { addr_cl -- wior }
\ *G initialaze class
  \ TODO implement me
  jvm_class.STATUS:INIT addr_cl jvm_class.status + ! \ store status
  0
;

\ ======
\ *> ###
\ ======

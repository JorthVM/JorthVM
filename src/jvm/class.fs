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
\ **    addr rtcp;     \ runtime constant pool
\ **    wid  fd_idx;   \ static field index wordlist
\ **    addr fd_table; \ static field table
\ **  }
\ 

 0 cells constant jvm_class.status
 1 cells constant jvm_class.init_loader
 2 cells constant jvm_class.rtcp
 3 cells constant jvm_class.super
 4 cells constant jvm_class.fd_idx
 5 cells constant jvm_class.fd_table

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
\ FIXME change idx to offset!
: jvm_class.getFd_idx_wid() ( addr -- wid )
\ *G get the wordlist id for the static field index translation
  jvm_class.fd_idx + @
;

: jvm_class.getFd_idx() { addr c-addr n -- idx woir }
\ *G get the index of a static field
  addr jvm_class.getFd_idx_wid()
  c-addr n rot
  jvm_find_word throw
  0
;

: jvm_class.getStaticIdx() { addr idx -- value }
\ *G get the value of a static field (32bit) by index
  addr jvm_class.fd_table + idx cells + jvm_field_@
;

: jvm_class.getStatic() { addr c-addr n -- value woir }
\ *G get the value of a static field (32bit) by name
  ." trying to get " c-addr n type  CR
  addr c-addr n jvm_class.getFd_idx() throw
  ." index: " dup . CR
  addr swap jvm_class.getStaticIdx() 0
;

: jvm_class.setStaticIdx() { addr val idx -- }
\ *G set the value of a static field (32bit) by index
  val addr jvm_class.fd_table + idx cells +
  jvm_field_!
;

: jvm_class.setStatic() { addr val c-addr n -- woir }
\ *G get the value of a static field (32bit) by name
  ." trying to set " c-addr n type  ."  to " val . CR
  addr val 
  over c-addr n jvm_class.getFd_idx() throw \ get idx
  ." index: " dup . CR
  jvm_class.setStaticIdx() 
  0
;

: jvm_class.getStatic2Idx() { addr idx -- lsb-value msb-value }
\ *G get the value of a static field (64bit) by index
  addr jvm_class.fd_table + idx cells + jvm_field_2@
;

: jvm_class.getStatic2() { addr c-addr n -- lsb-value msb-value wior }
\ *G get the value of a static field (64bit) by name
  addr c-addr n jvm_class.getFd_idx() throw
  addr swap jvm_class.getStatic2Idx() 0
;

: jvm_class.new() ( -- addr)
\ *G Create a new (uninitialized) class
  jvm_class.size() allocate throw
  dup jvm_class.size() erase
  jvm_class.STATUS:UNINIT over ( jvm_class.status + ) !
  \ 0 over jvm_class.init_loader + !
  \ 0 over jvm_class.rtcp + !
  wordlist over jvm_class.fd_idx + !
  \ 0 over jvm_class.fd_table + !
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
    ( idx addr )
    \ idx next free index of the table
    \ addr address of the field entry
    dup ACC_STATIC jvm_fd_?flags IF 
      addr_cf over jvm_fd_identifier
      ( idx addr c-addr n )
      addr_cf jvm_cf_constpool_addr 
      ( idx addr c-addr n addr2 )
      3 pick jvm_fd_desc_idx 
      ( idx addr c-addr n addr2 idx2 )
      jvm_constpool_idx
      ( idx addr c-addr n addr3 )
      jvm_cp_utf8_c-ref
      ( idx addr c-addr n c-addr2 n2 )
      jvm_field_desc_size -rot
      ( idx addr size c-addr n )
      4 pick -rot
      ( idx addr size idx c-addr n )
      addr_cl jvm_class.getFd_idx_wid() 
      ( idx addr size idx c-addr n wid )
      jvm_add_word
      ( idx addr size )
      rot + swap 
    ENDIF

    dup jvm_fd_size + \ next entry
  LOOP
  drop \ drop last+1 field address
  
  allocate throw                                         \ allocate static table memory
  \ TODO Initial values §5.4.2, 1st paragraph

  addr_cl jvm_class.fd_table + !                         \ store table

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

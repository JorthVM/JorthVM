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
: jvm_field_desc_sizeC ( c -- u )
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
    1 swap
  ENDCASE
;

: jvm_field_desc_size { c-addr n -- u }
\ *G Get the size of a field (in Bytes)
  c-addr c@ jvm_field_desc_sizeC
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
\ **    wid  field_offset; \ static field offset wordlist
\ **    addr field_table;  \ static field table
\ **    wid  method_list;  \ method wordlist
\ **  }
\ 

 0 cells constant jvm_class.status
 1 cells constant jvm_class.init_loader
 2 cells constant jvm_class.rtcp
 3 cells constant jvm_class.super
 4 cells constant jvm_class.field_offset
 5 cells constant jvm_class.field_table
 6 cells constant jvm_class.method_list
 7 cells constant jvm_class.field_length

 8 cells constant jvm_class.size()

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

: jvm_class.getField_length() ( addr -- n )
\ *G get length of non-static field members
  jvm_class.field_length + @
;

: jvm_class.incField_length() ( inc addr -- )
\ *G increment length of non-static field members with `n'
  dup jvm_class.getField_length() ( inc addr n )
  rot ( addr n inc ) + swap ( incn addr )
  jvm_class.field_length + !
;

: jvm_class.getInheritanceStuff() { addr c-addr n wid-xt -- addr_cl off wior }
  addr BEGIN 
    dup wid-xt execute
    c-addr n rot
    TRY
      jvm_find_word
      IFERROR
        2drop \ weird stack effect o_O
      ENDIF
    ENDTRY
    WHILE \ if false, we're done (i.e. found an offset)
    drop \ we don't want this value then
    jvm_class.getSuper() ( addr_super )
    dup 0= IF
      \ we're are at java/lang/Object now, hence
      \ this field doesn't exist
      1 JVM_WORDNOTFOUND_EXCEPTION throw
    ENDIF
  REPEAT
  ( addr_cl off )
  0
;

: jvm_class.getField_offset() { addr c-addr n -- off wior }
  addr c-addr n ['] jvm_class.getField_offset_wid()
  jvm_class.getInheritanceStuff() rot drop
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

: jvm_class.getMethodList() ( addr -- wid )
  jvm_class.method_list + @
;

: jvm_class.getMethod() { addr c-addr n -- addr_cl addr_md woir }
\ *G get the address of a method entry and the according class (might be a super class)
  addr c-addr n ['] jvm_class.getMethodList()
  jvm_class.getInheritanceStuff()
;

: jvm_class.getMethodNoSuper() { addr c-addr n -- addr_md woir }
\ *G get the address of a method entry
  addr jvm_class.getMethodList()
  c-addr n rot
  jvm_find_word throw
  0
;

: jvm_class.getMethodCodeAttr() { addr addr_md -- addr_code_attr }
\ *G get the address of the code attribute of a method entry
\ FIXME implement me better 
  \ addr jvm_class.getRTCP() jvm_rtcp.getClassfile()
  addr_md jvm_md_attr
  addr_md jvm_md_attr_count
  0 ?DO
    ( addr_attr )
    addr jvm_class.getRTCP()  
    ( addr_attr rtcp )
    over jvm_attr_name_idx
    ( addr_attr rtcp name_idx )
    jvm_rtcp.getConstpoolByIdx()
    jvm_cp_utf8_c-ref
    s" Code"
    str= 
    IF
      dup \ dup address 
    ENDIF
    ( [addr_code_addr] addr_attr )
    dup jvm_attr_size +
  LOOP
  drop
;

: jvm_class.getMethodCodeAttrByName() { addr c-addr n -- addr_code_attr woir }
\ *G get the address of the code attribute of a method entry
  addr c-addr n jvm_class.getMethod() throw
  addr swap jvm_class.getMethodCodeAttr()
  0
;

: jvm_class.new() ( -- addr)
\ *G Create a new (uninitialized) class
  jvm_class.size() allocate throw
  dup jvm_class.size() erase
  jvm_class.STATUS:UNINIT over ( jvm_class.status + ) !
  \ 0 over jvm_class.init_loader + !
  \ 0 over jvm_class.rtcp + !
  wordlist over jvm_class.field_offset + !
  wordlist over jvm_class.method_list + !
  \ 0 over jvm_class.field_table + !
  cell over jvm_class.field_length + ! \ reserver one cell for class pointer
;

: jvm_class.createFields() { addr_cf addr_cl addr count -- ?? }
  0 addr count 0 ?DO
    ( off addr )
    \ off next free offset of the table
    \ addr address of the field entry
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
    3 pick ACC_STATIC jvm_fd_?flags IF
      4 pick -rot
    ELSE
      addr_cl jvm_class.getField_length() -rot
      ( off addr size foff c-addr n )
      \ FIXME: reserve space according to the actual field size
      2 cells addr_cl jvm_class.incField_length()
    ENDIF
    ( off addr size off c-addr n )
    addr_cl jvm_class.getField_offset_wid()
    ( off addr size off c-addr n wid )
    jvm_add_word
    ( off addr size )
    rot + swap

    dup jvm_fd_size + \ next entry
  LOOP
  drop \ drop last+1 field address

  allocate throw                                         \ allocate static table memory
  \ TODO Initial values §5.4.2, 1st paragraph

  addr_cl jvm_class.field_table + !                         \ store table
;

\ TODO move that into a deferred.fs file or so
defer jvm_stack.newClass()
defer jvm_stack.findAndInitClass()
defer jvm_stack.invokeStaticInitializer()

: jvm_class.prepare() { addr_cl loader addr_cf -- wior }
\ *G prepare a class using a classfile and a loader
\ NOTE loader is a value to identify loaders

  \ get this class
  addr_cf jvm_cf_constpool_addr dup ( addr_cp addr_cp )
  addr_cf jvm_cf_this_class ( addr_cp addr_cp idx1 )
  jvm_constpool.getClassname_idx() ( scl_addr )
  jvm_cp_utf8_c-ref ( scl_str n )
  s" java/lang/Object" str= invert IF
    \ get ref to superclass
    addr_cf jvm_cf_constpool_addr dup ( addr_cp addr_cp )
    addr_cf jvm_cf_super_class ( addr_cp idx1 )
    jvm_constpool.getClassname_idx() ( scl_addr )
    jvm_cp_utf8_c-ref ( scl_str n )
    2dup ( scl_str n scl_str n )
    jvm_stack.newClass() ( scl_str n )
    jvm_stack.findAndInitClass() throw ( addr_super_class )
    dup ( addr_super_class addr_super_class )
    addr_cl jvm_class.super + ! ( addr_super_class )

    \ set field length from superclass
    jvm_class.getField_length() ( length )
    addr_cl jvm_class.incField_length()
  ENDIF
  
  \ create runtime constant pool
  addr_cf jvm_rtcp.new() addr_cl jvm_class.rtcp + !
  \ TODO add super class

  \ create static fields
  addr_cf addr_cl
  addr_cf jvm_cf_fields_addr
  addr_cf jvm_cf_fields_count
  jvm_class.createFields()

  \ method
  addr_cf jvm_cf_methods_addr
  addr_cf jvm_cf_methods_count
  0 ?DO
    ( addr_md )
    dup \ for jvm_add_word
    dup jvm_md_name_idx
    over jvm_md_desc_idx
    ( addr_md addr_md name_idx desc_idx )
    addr_cf -rot jvm_cp_nametype_identifier
    \ 2dup ." Method: " type CR
    addr_cl jvm_class.getMethodList() 
    \ ." pre add word " .s CR
    jvm_add_word
    dup jvm_md_size +
  LOOP
  drop 

  jvm_class.STATUS:PREPARED addr_cl jvm_class.status + ! \ store status
  loader addr_cl jvm_class.init_loader + !               \ store loader
  0
;

: jvm_class.init() { addr_cl -- wior }
\ *G initialaze class
  \ TODO init superclass
  \ TODO call static initialazer
  dup jvm_class.STATUS:INIT addr_cl jvm_class.status + ! \ store status
  CR
  ." pre invokeStaticInitializer() " .s CR
  jvm_stack.invokeStaticInitializer() throw
  \ drop
  ." post invokeStaticInitializer() " .s CR
\  dup jvm_class.method_list + @ wordlist-words
\  ." pre clinit check" .s CR
\  try
\    s" <init>|()V" jvm_class.getMethod() throw
\    true
\  iferror 
\    ." iferror " .s CR
\    drop \ drop woir
\    false
\  endif
\  endtry
\  IF
\    ." clinit found " .s CR
\    2drop
\  ELSE
\    ." clinit not found " .s CR
\  ENDIF
  ( addr_cl addr_md )
  \ 2drop
  0
;

\ ======
\ *> ###
\ ======

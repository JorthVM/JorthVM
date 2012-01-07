\ vim: sw=2 ts=2 sta et
include ../jvm/rtconstpool.fs

: createConstPoolTable_test
  assert( depth 0= )
  s" bin/Test.class" jvm_read_classfile throw 
  dup jvm_rtcp.createConstPoolTable()
  swap dup
  jvm_cf_constpool_addr
  swap
  jvm_cf_constpool_count
  1 ?DO
    ( addr1 addr2 --)
    \ addr1 address of the current table entry
    \ addr2 address of the current constpool entry
    over @
    over assert( = )
    swap 1 cells + swap           \ imcrement table
    dup jvm_constpool_type_size + \ increment constpool entry
  LOOP
  2drop
  assert( depth 0= )
;

: rtcp_new_test
  assert( depth 0= )
  s" bin/Test.class" jvm_read_classfile throw 
  dup jvm_rtcp.new()
  tuck
  1 jvm_rtcp.getConstpoolByIdx() 
  swap jvm_cf_constpool_addr
  assert( = )
  \ TODO jvm_rtcp.getClass_info() >order
  drop \ drop rtcp addr
  
 \  assert( depth 0= )
;

: test
  createConstPoolTable_test
  rtcp_new_test
;

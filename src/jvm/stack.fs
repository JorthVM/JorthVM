\ vim: sw=2 ts=2 sta et
\ this file contains the implementaion of the JVM Stack 
\ of jvm stack frames

\ ========
\ *! stack
\ *T JVM Stack
\ ========

require class.fs
require classloader.fs
\ require decode.fs
require util.fs
require frame.fs

\ *S JVM Stack Structure
\ *E   Stack {
\ **      addr pc;
\ **      addr pc_next;
\ **      wid classes;
\ **      Frame currentFrame;
\ **  }

 0 cells constant jvm_stack.pc
 1 cells constant jvm_stack.pc_next
 2 cells constant jvm_stack.classes
 3 cells constant jvm_stack.currentFrame
 
 4 cells constant jvm_stack.size()

: jvm_stack.new() ( -- addr )
\ *G create a new JVM Stack
  jvm_stack.size() allocate throw
  dup jvm_stack.size() erase

  wordlist over jvm_stack.classes + !
;

jvm_stack.new() constant jvm_stack
\ *G this is the heart of the JVM

: jvm_stack.getPC() ( -- addr_pc )
  jvm_stack jvm_stack.pc + @
;

: jvm_stack.getPC_next() ( -- addr_pc_next )
  jvm_stack jvm_stack.pc_next + @
;

: jvm_stack.fetchByte() ( -- byte )
\ *G return program counter 
   jvm_stack.getPC_next() c@
   jvm_stack jvm_stack.pc_next + 1 swap 
   +!
;

: jvm_stack.currentInstruction() ( -- opcode )
\ *G return opcode of the current instruction 
   jvm_stack.getPC() c@
;

: jvm_stack.incPC() ( -- )
\ *G increment program counter
  jvm_stack.getPC_next() \ load pc next
  jvm_stack jvm_stack.pc + !      \ store to pc
;

: jvm_stack.setPC() ( addr -- )
  jvm_stack 2dup
  jvm_stack.pc_next + !
  jvm_stack.pc + !
;

: jvm_stack.findClass() { c-addr n -- addr2 woir }
\ *G get the address of a class
  c-addr n 
  jvm_stack jvm_stack.classes + @
  jvm_find_word 
  \ throw 0
;

: jvm_stack.addClass() { addr2 c-addr n -- woir }
\ *G add a class
\ addr2 class addr
  \ ." addclass: " c-addr n type CR 
  addr2 c-addr n 
  jvm_stack jvm_stack.classes + @
  jvm_add_word 
  \ throw 0
;

: jvm_stack.newClass() { c-addr n -- }
  \ ." newClass: " c-addr n type space .s CR 
  TRY
    c-addr n jvm_stack.findClass() 
    nip \ delete class address
  \ ." class already found: " c-addr n type space .s CR 
  IFERROR
    dup
    JVM_WORDNOTFOUND_EXCEPTION = IF
      drop
      \ add class to jvm stack
      \ ." add class to jvm stack" CR
      jvm_class.new() c-addr n 
      \ ." pre addclass: " 2dup type space .s CR
      jvm_stack.addClass()
      0
    ENDIF
  ENDIF
  ENDTRY
  throw
  \ ." END newClass" .s CR
;
: jvm_stack.loadClasses() ( addr -- ) 
\ add all class constpool entries
  jvm_class.getRTCP()
  \ ." (rtcp) " .s CR
  dup
  jvm_rtcp.getClassfile()
  jvm_cf_constpool_count
  1 ?DO
    \ ." LOOP (rtcp) " .s CR
    ( rtcp )
    dup i
    jvm_rtcp.getConstpoolByIdx() 
    jvm_cp_tag
    CASE
      \ ." CASE (rtcp) " .s CR
      CONSTANT_Class OF
        \ ." Class(rtcp) " .s CR
        dup
        i 
        jvm_rtcp.getClassName() 
        \ ." pre newClass: " 2dup type space .s cr
        jvm_stack.newClass()
        \ ." endClass(rtcp) " .s CR
      ENDOF
    ENDCASE
  LOOP
  drop \ drop rtcp
;

: jvm_stack.findAndInitClass() { c-addr n -- addr2 woir }
\ *G search for a class an initialize it if needed
  \ ." jvm_stack.findAndInitClass() " c-addr n type space .s CR
  c-addr n jvm_stack.findClass() throw
  \ ." jvm_stack.findAndInitClass() begin " .s CR
  dup jvm_class.getStatus()
  \ ." jvm_stack.findAndInitClass() pre case " .s CR
  CASE
    ( addr_cl )
    jvm_class.STATUS:UNINIT OF
      \ ." class " c-addr n type ."  not prepared: prepare and init" cr
      \ .s cr
      dup
      dup
      jvm_default_loader 
      c-addr n jvm_search_classpath throw
      \ ." search classpath" .s cr
      jvm_class.prepare() throw 
      jvm_stack.loadClasses()
      \ ." class prepare" .s cr
      dup
      jvm_class.init() 
      \ ." throw" .s cr
      throw
    ENDOF
    jvm_class.STATUS:PREPARED OF
      \ ." class " c-addr n type ."  not initialized: init" cr
      \ .s cr
      dup
      jvm_class.init() throw
    ENDOF
    jvm_class.STATUS:INIT OF
      \ ." class " c-addr n type ."  already initialized" cr
      \ .s cr
      \ do nothing
    ENDOF
    \ default
    ." Unknown class status: " dup . CR
    abort
  ENDCASE
  \ ." end jvm_stack.findAndInitClass(): " .s CR
  0 \ no exception
;

: jvm_stack.getCurrentFrame() ( -- addr )
\ *G get the current frame
  jvm_stack jvm_stack.currentFrame + @
;


: ?debug_trace true ;

\ -------------------------------------------------------- \
\ PROGRAM COUNTER                                          \
\ -------------------------------------------------------- \

\ variable jvm_pc

\ : jvm_set_pc ( ... addr -- ... )
\  dup
\  jvm_pc !
\ ;

\ : jvm_fetch_instruction ( ... -- opcode ... )
\  POSTPONE jvm_pc POSTPONE @ \ load pc 
\  POSTPONE c@ \ load instruction
\  1 POSTPONE literal POSTPONE jvm_pc POSTPONE +! \ increment pc
\ ; immediate

: show_insn ( opcode -- )
  dup jvm_mnemonic CR type
  jvm_mnemonic_imm 0 ?DO
    ." , " jvm_stack.getPC_next() i + c@ hex.
  LOOP
;

\ : jvm_next
\  POSTPONE jvm_fetch_instruction 
\  [ ?debug_trace ] [IF] POSTPONE dup POSTPONE show_insn [ENDIF]
\  POSTPONE jvm_execute
\ ; immediate

\ : jvm_run
\  begin 
\    jvm_next
\  again
\ ;

: jvm_stack.next()
  POSTPONE jvm_stack.fetchByte() 
  [ ?debug_trace ] [IF] POSTPONE dup POSTPONE show_insn [ENDIF]
  POSTPONE jvm_execute
  POSTPONE jvm_stack.incPC()
; immediate

: jvm_stack.run()
  ." run() start " .s CR
  try
    begin 
      jvm_stack.next()
    again
  restore 
  endtry
  dup CASE
    JVM_RETURN_EXCEPTION OF 
      drop 0 
    ENDOF
  ENDCASE
  ( woir )
  throw
  \ TODO handle exceptions
  ." run() terminating " .s CR
;

: jvm_stack.invokeInitial() { c-addr n -- wior }
\ *G Start the execution by invoking public static void main(String[] args)
  \ ." : jvm_stack.invokeInitial() { c-addr n -- wior } " .s CR
  \ FIXME we need to initialize the super class/interface as well (somewhere)
  assert( depth 0 = )
  c-addr n jvm_stack.findAndInitClass() throw
  \ ." class ready" .s cr
  ( addr_cl ) \ initialized class
  
  dup 

  \ search method
  jvm_class.getRTCP()
  jvm_rtcp.getClassfile()

  dup 
  s" main" s" ([Ljava/lang/String;)V" 
  jvm_get_method_by_nametype
  \ TODO check for public and static 
  invert IF
    JVM_MAINNOTFOUND_EXCEPTION throw
  ENDIF
  
  ( addr_cl addr_cf addr_md )
  rot over 0 0
  ( addr_cf addr_md addr_cl addr_md 0 0 )
  jvm_frame.new()

  \ store current frame
  jvm_stack jvm_stack.currentFrame + !
  
  ( addr_md )
  jvm_md_get_code_attr
  jvm_stack.setPC()
  jvm_stack.run()
;

\ ======
\ *> ###
\ ======

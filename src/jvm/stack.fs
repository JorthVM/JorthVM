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

: jvm_stack.incPC() ( -- )
  jvm_stack.getPC_next() \ load pc next
  jvm_stack jvm_stack.pc + !      \ store to pc
;

: jvm_stack.setPC() ( addr -- )
  jvm_stack 2dup
  jvm_stack.pc_next + !
  jvm_stack.pc + !
;

: jvm_stack.getClass() { c-addr n -- addr2 woir }
\ *G get the address of a class
  c-addr n 
  jvm_stack jvm_stack.classes + @
  jvm_find_word 
  \ throw 0
;

: jvm_stack.addClass() { addr2 c-addr n -- woir }
\ *G add a class
\ addr1 stack addr
\ addr2 class addr
  addr2 c-addr n 
  jvm_stack jvm_stack.classes + @
  jvm_add_word 
  \ throw 0
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
  begin 
    jvm_stack.next()
  again
;

: jvm_stack.invokeInitial() { c-addr n -- wior }
\ *G Start the execution by invoking public static void main(String[] args)
  jvm_stack c-addr n jvm_stack.getClass() throw
  dup jvm_class.getStatus()
  CASE
    ( addr_cl )
    jvm_class.STATUS:UNINIT OF
      ." class not prepared" cr
      dup
      jvm_default_loader 
      c-addr n jvm_search_classpath 
      ." search classpath" .s cr
      throw
      jvm_class.prepare() 
      ." class prepare" .s cr
      throw
      dup
      jvm_class.init() 
      ." throw" .s cr
      throw
    ENDOF
    jvm_class.STATUS:PREPARED OF
      ." class not initialazed" cr
      dup
      jvm_class.init() throw
    ENDOF
      ." class already initialazed" cr
      dup
    jvm_class.STATUS:INIT OF
      \ do nothing
    ENDOF
    \ default
    ." Unknown class status: " dup . CR
    abort
  ENDCASE
  ." class ready" .s cr
  ( addr_cl ) \ initialized class
  \ dup 
  jvm_class.getRTCP()
  jvm_rtcp.getClassfile()

  dup 
  s" main" s" ([Ljava/lang/String;)V" 
  jvm_get_method_by_nametype
  \ TODO check for public and static 
  invert IF
    JVM_MAINNOTFOUND_EXCEPTION throw
  ENDIF
  
  \ jvm_md_get_code_attr
  \ jvm_set_pc  \ TODO hardcoded
  \ jvm_run 
  
  jvm_md_get_code_attr
  jvm_stack.setPC()
  jvm_stack.run()
;

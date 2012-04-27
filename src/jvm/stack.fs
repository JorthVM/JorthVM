\ vim: sw=2 ts=2 sta et


\ Copyright (C) 2011-2012 Sebastian Rumpl, Josef Eisl, Bernhard Urban

\ This file is part of JorthVM

\ JorthVM is free software: you can redistribute it and/or modify
\ it under the terms of the GNU General Public License as published by
\ the Free Software Foundation, either version 3 of the License, or
\ (at your option) any later version.

\ This program is distributed in the hope that it will be useful,
\ but WITHOUT ANY WARRANTY; without even the implied warranty of
\ MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
\ GNU General Public License for more details.

\ You should have received a copy of the GNU General Public License
\ along with this program.  If not, see <http://www.gnu.org/licenses/>.

\ this file contains the implementaion of the JVM Stack

\ ========
\ *! stack
\ *T JVM Stack
\ ========

require class.fs
require classloader.fs
\ require decode.fs
require util.fs
require frame.fs
require native.fs

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
\ *G return byte pointed by the program counter
   jvm_stack.getPC_next() c@
   jvm_stack jvm_stack.pc_next + 1 swap
   +!
;

: jvm_stack.fetchShort() ( -- short )
\ *G return short pointed by the program counter
  jvm_stack.fetchByte() \ load byte
  8 lshift
  jvm_stack.fetchByte() \ load byte
  or
;

: jvm_stack.fetchSignedShort() ( -- short )
\ *G return signed short pointed by the program counter
  jvm_stack.fetchShort()
  dup 0x8000 and \ sign ext
  if
  [ -1 16 lshift ] literal or
  endif
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

\ : jvm_stack.newClass() { c-addr n -- }
:noname { c-addr n -- }
  TRY
    c-addr n jvm_stack.findClass()
    nip \ delete class address
  \ ." class already found: " c-addr n type space .s CR
  IFERROR
    ." newClass: " c-addr n type CR
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
; IS jvm_stack.newClass()

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

\ jvm_stack.findAndInitClass() { c-addr n -- addr2 woir }
:noname { c-addr n -- addr2 woir }
\ *G search for a class an initialize it if needed
  ." findAndInitClass: " c-addr n type CR
  c-addr n jvm_stack.findClass() throw
  \ ." jvm_stack.findAndInitClass() begin " .s CR
  dup jvm_class.getStatus()
  \ ." jvm_stack.findAndInitClass() pre case " .s CR
  CASE
    ( addr_cl )
    jvm_class.STATUS:UNINIT OF
      ." findAndInitClass " c-addr n type
      ."  not prepared: prepare and init" CR
      \ .s cr
      dup
      dup
      jvm_default_loader
      c-addr n jvm_search_classpath throw
      \ ." search classpath" .s cr
      jvm_class.prepare() throw
      \ ." class prepare" .s cr
      jvm_stack.loadClasses()
      \ ." class loadClasses" .s cr
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
; IS jvm_stack.findAndInitClass()

: jvm_stack.getCurrentFrame() ( -- addr )
\ *G get the current frame
  jvm_stack jvm_stack.currentFrame + @
;

variable debug_indent
0 debug_indent !
: jvm_stack.incInvoke() debug_indent @ 1+ debug_indent ! ;
: jvm_stack.decInvoke() debug_indent @ 1- debug_indent ! ;

\ FIXME outsource somewhere
[IFUNDEF] ?debug_trace
: ?debug_trace false ;
[ENDIF]
\ : ?debug_trace true ;

: show_insn ( opcode -- )
  dup jvm_decode.mnemonic() CR
  debug_indent @ 2* spaces
  type
  jvm_decode.mnemonic_imm() 0 ?DO
    ." , " jvm_stack.getPC_next() i + c@ hex.
  LOOP
;

: show_stack ( -- )
  base @ >r hex cr ." stack: " .s r> base !
;

: jvm_stack.next()
  POSTPONE jvm_stack.fetchByte()
  [ ?debug_trace ] [IF] POSTPONE dup POSTPONE show_insn [ENDIF]
  POSTPONE <[ POSTPONE swap POSTPONE ]>
  [ ?debug_trace ] [IF] POSTPONE show_stack [ENDIF]
  POSTPONE jvm_stack.incPC()
; immediate

: jvm_stack.run()
  [ ?debug_trace ] [IF] ." run() start " .s CR [ENDIF]
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
  [ ?debug_trace ] [IF] ." run() terminating " .s CR [ENDIF]
;

: jvm_stack.findMethod() { c-addr1 n1 c-addr2 n2 -- addr_c2 addr_md wior }
\ *G find a method return class address and method attribute address
  c-addr1 n1 jvm_stack.findAndInitClass() throw
  ( addr-cl )
  c-addr2 n2 jvm_class.getMethod() throw
  ( addr-cl2 addr_md )
  \ addr-cl2 can be different to addr-cl, e.g. when the
  \ method only exists in the superclass
  0
;

: jvm_stack.getNamesFromMethodRef() { idx -- c-addr1 n1 c-addr23 n23 ?static ?native }
\ *G get the class name and nametype from Methodref constant pool entry
  idx
  jvm_stack.getCurrentFrame()
  jvm_frame.getClass()
  jvm_class.getRTCP()

  dup rot jvm_rtcp.getConstpoolByIdx()
  ( addr_rtcp addr_md )
  dup jvm_cp_methodref_class_idx
  ( addr_rtcp addr_md class_idx )
  2 pick swap
  ( addr_rtcp addr_md addr_rtcp class_idx )
  jvm_rtcp.getClassName()
  ( addr_rtcp addr_md c-addr1 n1 )
  2swap
  ( c-addr1 n1 addr_rtcp addr_md )
  jvm_cp_methodref_nametype_idx
  ( c-addr1 n1 addr_rtcp md_idx )
  jvm_rtcp.getNameAndType()
  ( c-addr1 n1 c-addr2 n2 c-addr3 n3 )

  \ TODO: please cleanup (omg)
  5 pick 5 pick
  jvm_stack.findAndInitClass() throw
  jvm_class.getRTCP()
  jvm_rtcp.getClassfile()
  ( c-addr1 n1 c-addr2 n2 c-addr3 n3 cf-addr )
  4 pick 4 pick 4 pick 4 pick
  jvm_get_method_by_nametype drop
  ( c-addr1 n1 c-addr2 n2 c-addr3 n3 md-addr )
  >r jvm_nametype_identifier r> dup
  ( c-addr1 n1 c-addr23 n23 md-addr )
  \ check if native and static
  ACC_STATIC jvm_md_?flags swap
  ACC_NATIVE jvm_md_?flags
  ( c-addr1 n1 c-addr23 n23 ?static ?native)
;

: jvm_stack.setCurrentFrame() ( addr_fm -- )
\ *G set the current frame
  \ store current frame
  dup jvm_stack jvm_stack.currentFrame + !
  dup jvm_frame.getClass()
  swap jvm_frame.getMethod()
  ( addr_cl addr_md )
  jvm_class.getMethodCodeAttr()
  jvm_code_attr_code_addr
  jvm_stack.setPC()
;

: jvm_stack.resetCurrentFrame() ( -- )
\ *G reset the current frame
  \ restore frame
  jvm_stack.getCurrentFrame()
  dup jvm_frame.getDynamicLink()
  jvm_stack jvm_stack.currentFrame + ! \ restore frame
  dup jvm_frame.getReturnAddr()
  jvm_stack.setPC() \ restore frame
  free throw \ free frame
;

\ FIXME handle parameters! either in jvm_frame.new() or in the invoke words!

: jvm_stack.invokeInitial() { c-addr n -- wior }
\ *G Start the execution by invoking public static void main(String[] args)
  c-addr n s" main|([Ljava/lang/String;)V"
  jvm_stack.findMethod() throw
  ( addr_cl addr_md )
  0 0
  jvm_frame.new()
  ( frame )
  \ TODO dup >r jvm_frame.setParameters() r>

  jvm_stack.setCurrentFrame()
  jvm_stack.run()
;

\ : jvm_stack.invokeStaticInitializer() { addr_cl -- wior }
:noname ( addr_cl -- wior )
\ *G Execute the static initializer
  \ dup jvm_class.method_list + @ wordlist-words
  ( addr_cl )
  dup
  try
    s" <clinit>|()V" jvm_class.getMethodNoSuper() throw
    true
  iferror
    ( addr_cl addr_cl wior )
    drop \ drop woir
    false
  endif
  endtry
  ( addr_cl addr_md|addr_cl f )
  IF
    ( addr_cl addr_md )
    \ TODO bad habbit
    jvm_stack.getCurrentFrame() >r \ dynamic link
    jvm_stack.getPC() >r \ return address
    jvm_stack.getPC_next() >r \ return address
    0 0
    jvm_frame.new()
    ( frame )
    \ TODO dup >r jvm_frame.setParameters() r>

    jvm_stack.setCurrentFrame()
    jvm_stack.run()
    r> \ jvm_stack.getPC_next()             \ return address
    jvm_stack jvm_stack.pc_next + !       \ restore frame
    r> \ jvm_stack.getPC()                  \ return address
    jvm_stack jvm_stack.pc + !            \ restore frame
    r> \ jvm_stack.getCurrentFrame()        \ dynamic link
    jvm_stack jvm_stack.currentFrame + !  \ restore frame

  ELSE
    2drop \ addr_cl addr_cl
  ENDIF
  0
; IS jvm_stack.invokeStaticInitializer()

: jvm_stack.invokestatic() { idx -- wior }
\ *G invoke a static method
  idx jvm_stack.getNamesFromMethodRef()
  ( c-addr1 n1 c-addr2 n2 ?static ?native )
  IF \ it's native
    >r
    s" ." 2swap strcat strcat \ class.nametyp
    r> IF \ it's static
      s" .static" strcat
    ENDIF
    \ ." native call: " 2dup type cr
    TRY
      evaluate
      IFERROR JVM_NATIVENOTFOUND_EXCEPTION ELSE 0 ENDIF
    ENDTRY
    throw 0
  ELSE
    drop
    ( c-addr1 n1 c-addr2 n2 )
    \ 2swap ." Classname: " 2dup type CR
    \ 2swap ." NameType: " 2dup type CR
    jvm_stack.findMethod() throw
    ( addr_cl addr_md )
    jvm_stack.getCurrentFrame() \ dynamic link
    jvm_stack.getPC_next() \ return address
    jvm_frame.new()
    dup >r
    jvm_frame.setParameters()
    r>

    jvm_stack.setCurrentFrame()
    0
  ENDIF
;

\ ======
\ *> ###
\ ======


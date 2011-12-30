\ vim: sw=2 ts=2 sta et
\ this file contains the implementaion of the class loader 

require exception.fs
require util.fs
require classfile.fs

\ ========
\ *! classloader
\ *T Class Loader
\ ========


\ ========
\ *S Classpath
\ ========
\ 
\ *C classpath_entry {
\ **    addr string;
\ **    addr string_size;
\ **    addr next_entry;
\ **  }
\ 
\ *P The classpath list is implemented as linked list. The
\ ** next_entry pointer of the last entry is 0. 


variable jvm_classpath_list
0 jvm_classpath_list !

variable jvm_classentry_list
0 jvm_classentry_list !

0 cells constant jvm_classpath.string
1 cells constant jvm_classpath.string_size
2 cells constant jvm_classpath.next
3 cells constant jvm_classpath.size()


: jvm_classpath_entry.getNext() { addr1 -- addr2 }
\ *G get the next classpath entry
  addr1 jvm_classpath.next + @
;

: jvm_classpath_entry.getName() { addr -- c-addr n }
\ *G get the string of the classpath entry
  addr jvm_classpath.string + @
  addr jvm_classpath.string_size + @
;

: jvm_search_classpath { c-addr n -- addr wior }
\ *G Search for a class file and return the address to the memory location.
\ *P If the file is not found woir != 0
\ *P TODO do some more advanced stuff:
\ *(
\ *B search different class paths
\ *B c-addr n may contain packages so search for them in sub dirs
\ *)
\ FIXME iterate over all classpaths
  jvm_classpath_list @ dup 
  0= IF
    drop
    c-addr n 
    s" .class" 
    strcat
    jvm_read_classfile 
    \ TODO IO exception vs class not found exception
  ELSE
    0 swap \ dummy result
    BEGIN
      ( addr1 addr2 -- ) 
      \ addr1 result of read_classfile
      \ addr2 next classpath entry
      nip
      dup jvm_classpath_entry.getName()
      c-addr n
      \ TODO package 
      strcat
      s" .class" 
      strcat
      ( c-addr n -- )
      try
        jvm_read_classfile
      iferror
        drop
        2drop
        0
        true
      then
      endtry
      rot
      ( -- addr )
      
      jvm_classpath_entry.getNext() dup
      0<> rot 
      and
    WHILE
    REPEAT
    drop
    dup 
    0= IF
      JVM_CLASSNOTFOUND_EXCEPTION throw
    ELSE
      0
    ENDIF
  ENDIF
;

: jvm_classpath.new() ( -- addr )
\ *G return memory for a new classpath entry
\ *P TODO implement me
  jvm_classpath.size() allocate throw
  dup jvm_classpath.next +
  0 swap !
  jvm_classpath_list dup @
  0= IF
    over swap !
  ELSE
    @
    BEGIN
      \ ( addr -- )
      dup
      jvm_classpath_entry.getNext()
    0<> WHILE
      jvm_classpath_entry.getNext()
    REPEAT
    jvm_classpath.next + 
    over -rot
    !
  ENDIF
;

: jvm_classpath.add() { c-addr n -- }
\ *G Add a location to the classpath
\ *P TODO yeah, we should actually do something oO
  jvm_classpath.new() 
  dup jvm_classpath.string +
  c-addr swap !
  jvm_classpath.string_size +
  n swap !
;

\ *S Class

\ *C classentry {
\ **    addr string;
\ **    addr string_size;
\ **    addr classfile;
\ **    addr next_entry;
\ **  }

\ *P The class entry list is implemented as linked list. The
\ ** next_entry pointer of the last entry is 0. 

 0 cells constant jvm_classentry.string
 1 cells constant jvm_classentry.string_size
 2 cells constant jvm_classentry.classfile
 3 cells constant jvm_classentry.next
 4 cells constant jvm_classentry.size()

: jvm_classentry.getName() { addr -- c-addr n }
\ *G return the name of a class entry
  addr jvm_classentry.string + @
  addr jvm_classentry.string_size + @
;

: jvm_classentry.getClassfile() ( addr1 -- addr2 )
\ *G return the name of a class entry
  jvm_classentry.classfile + @
;

: jvm_classentry.getNext() ( addr1 -- addr2 )
\ *G return the next classfile entryy
  jvm_classentry.next + @
;

: jvm_classentry.new() ( -- addr )
\ *G return newly allocated memory for a classentry and append it to the end of the list
  jvm_classentry.size() allocate throw
  0 over jvm_classentry.next + !
  jvm_classentry_list @ dup
  0= IF
    drop dup 
    jvm_classentry_list !
  ELSE
    BEGIN  
    ( addr -- )
    dup jvm_classentry.getNext()
    0<> WHILE
      jvm_classentry.getNext()
    REPEAT
    jvm_classentry.next + 
    over -rot !
  ENDIF
;

: jvm_class_add { c-addr n addr -- addr2 }
\ *G Add a class
\ *D c-addr name of the class
\ *D n size of the name
\ *D addr address of the classfile buffer
  jvm_classentry.new() dup
  c-addr over jvm_classentry.string + !
  n over jvm_classentry.string_size + !
  addr over jvm_classentry.classfile + !
  drop
;

: jvm_class_link { c-addr n -- addr wior }
  c-addr n jvm_search_classpath throw
  c-addr n rot jvm_class_add
  0
;

: jvm_class_lookup { c-addr n -- addr wior }
\ *G Find the class denoted by c-addr n and return the addr of the corresponding
\ ** class entry. If the class is not already prepeared the jvm will search for it.
\ ** If the class is not found at all an exception will be raised.
  jvm_classentry_list @ dup
  0= IF
    drop
    \ JVM_CLASSNOTFOUND_EXCEPTION throw
    c-addr n jvm_class_link throw
    0
  ELSE
    BEGIN  
      ( addr -- )
      dup jvm_classentry.getName() 
      c-addr n compare 
      0<> IF 
        jvm_classentry.getNext() dup 0<>
      ELSE
        false
      ENDIF
    WHILE
    REPEAT
    dup 
    0= IF
      drop
      \ JVM_CLASSNOTFOUND_EXCEPTION throw
      c-addr n jvm_class_link throw
      0
    ELSE
      0
    ENDIF
  ENDIF
;


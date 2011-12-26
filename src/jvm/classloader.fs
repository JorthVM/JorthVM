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


: jvm_get_last_classpath_entry
\ *G return the last classpath entry (for appending)
\ *P TODO implement me
  jvm_classpath_list @
\ FIXME
;


: jvm_add_classpath { c-addr n -- }
\ *G Add a location to the classpath
\ *P TODO yeah, we should actually do something oO
  3 cells allocate throw
  dup
  c-addr swap !
  dup 1 cells +
  n swap !
  dup 2 cells +
  0 swap !
\ jvm_get_last_classpath_entry
  \ FIXME store to 
  jvm_classpath_list !
;

: jvm_classpath_entry_next { addr1 -- addr2 }
\ *G get the next classpath entry
  addr1 2 cells + @
;

: jvm_classpath_entry_get { addr -- c-addr n }
\ *G get the string of the classpath entry
  addr @
  addr 1 cells + @
;

: jvm_search_classpath ( c-addr n -- addr wior )
\ *G Search for a class file and return the address to the memory location.
\ *P If the file is not found woir != 0
\ *P TODO do some more advanced stuff:
\ *(
\ *B search different class paths
\ *B c-addr n may contain packages so search for them in sub dirs
\ *)
\ FIXME iterate over all classpaths
  jvm_classpath_list @ jvm_classpath_entry_get
  2swap
  strcat
  jvm_read_classfile
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

: jvm_classentry.getClassfile ( addr1 -- addr2 )
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

: jvm_class_add { c-addr n addr - }
\ *G Add a class
\ *D c-addr name of the class
\ *D n size of the name
\ *D addr address of the classfile buffer
  jvm_classentry.new()
  c-addr over !
  n over 1 cells + !
  addr over 2 cells + !

;

: jvm_class_lookup ( c-addr n -- addr wior )
\ *G Find the class denoted by c-addr n and return the addr of the corresponding
\ ** class entry. If the class is not already prepeared the jvm will search for it.
\ ** If the class is not found at all an exception will be raised.
  
;



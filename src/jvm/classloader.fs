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

: jvm_classentry_name { addr -- c-addr n }
\ *G return the name of a class entry
  addr @
  addr 1 cells @
;

: jvm_classentry_classfile { addr1 -- addr2 }
\ *G return the name of a class entry
  addr1 2 cells @
;

: jvm_classentry_next { addr1 -- addr2 }
\ *G return the next classfile entryy
  addr1 3 cells @
;

: jvm_classentry_new_entry ( -- addr )
\ *G return newly allocated memory for a classentry and append it to the end of the list
  4 cells allocate throw
  jvm_classentry_list @ dup
  0= IF
    drop dup 
    jvm_classentry_list !
  ELSE
    drop \ FIXME 
  ENDIF
;

: jvm_class_add { c-addr n addr - }
\ *G Add a class
\ *D c-addr name of the class
\ *D n size of the name
\ *D addr address of the classfile buffer
  jvm_classentry_new_entry
  c-addr over !
  n over 1 cells + !
  addr over 2 cells + !

;

: jvm_class_lookup ( c-addr n -- addr wior )
\ *G Find the class denoted by c-addr n and return the addr of the corresponding
\ ** class entry. If the class is not already prepeared the jvm will search for it.
\ ** If the class is not found at all an exception will be raised.
  
;



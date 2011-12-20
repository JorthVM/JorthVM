\ vim: sw=2 ts=2 sta et
\ this file contains the implementaion of the class loader 

require exception.fs

\ ========
\ *! classloader
\ *T Class Loader
\ ========

: jvm_search_classpath ( c-addr n -- addr wior )
\ *G Search for a class file and return the address to the memory location.
\ *P If the file is not found woir != 0
\ *P TODO do some more advanced stuff:
\ *(
\ *B search different class paths
\ *B c-addr n may contain packages so search for them in sub dirs
\ *)
  jvm_read_classfile 
;

\ *S Class

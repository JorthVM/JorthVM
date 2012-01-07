\ vim: sw=2 ts=2 sta et
\ auxiliary words

\ ========
\ *! util
\ *T Util
\ ========


require exception.fs

: decimal_places ( u1 -- u2 )
  1 swap begin
  10 / dup 0> while
    swap 1+ swap 
  repeat
  drop
;

: strcat { c-addr1 n1 c-addr2 n2 -- c-addr3 n3 }
\ *G Concatenate two string and return a new counted string
   n1 n2 + dup 
   allocate throw 
  ( n3 c-addr3 )
  dup
  n1 c-addr1 -rot
  ( n3 c-addr3 c-addr1 c-addr3 n1 )
  cmove
  ( n3 c-addr3 )
  dup n1 +
  n2 c-addr2 -rot
  ( n3 c-addr3 c-addr2 c-addr3+ n2 )
  cmove
  ( n3 c-addr3 )
  swap
;

: strreplacec { c-addr1 n s r -- c-addr2 n }
\ *G replace all character `s' in string `c-addr1 n' with character `r'
  c-addr1 n 
  BEGIN
    dup
  0> WHILE
    over c@
    s = IF
      over r swap c!
    ENDIF
    1 - swap
    1 + swap
  REPEAT
  2drop
  c-addr1 n
;

: jvm_add_word ( value c-addr n wid -- )
\ *G add a name/value pair to into a specific wordlist
  dup 2over
  nextname 
  \ switch compilition wordlist
  dup set-current >order
  variable
  -rot
  rot
  search-wordlist 
  assert( -1 = ) \ we just added it so this should hold
  execute ! \ store value
  \ restore compilation wordlist
  previous definitions
;

: jvm_find_word_addr ( c-addr n wid -- addr wior )
\ *G find a word and return the associated addr in a specific wordlist
  search-wordlist 
  case
    0 of
      ( -- ) \ word not found
      JVM_WORDNOTFOUND_EXCEPTION throw
    endof
    -1 of
      ( xt -- ) \ found, not immediate
      execute 0
    endof
    1 of
      ( xt -- )\ found, immediate
      abort
    endof
    ( default)
    abort
  endcase
;

: jvm_find_word ( c-addr n wid -- value wior )
\ *G find a word and return the associated value in a specific wordlist
  jvm_find_word_addr throw
  @ 0
;

: jvm_replace_word ( value c-addr n wid -- wior )
\ *G add a name/value pair to into a specific wordlist
  jvm_find_word_addr throw
  ! 0
;


\ ======
\ *> ###
\ ======

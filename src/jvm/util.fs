\ vim: sw=2 ts=2 sta et
\ auxiliary words

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


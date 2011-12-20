\ vim: sw=2 ts=2 sta et
\ auxiliary words

: decimal_places ( u1 -- u2 )
  1 swap begin
  10 / dup 0> while
    swap 1+ swap 
  repeat
  drop
;

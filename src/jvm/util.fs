\ auxiliary words

: decimal_places ( u1 -- u2 )
  1 swap begin
  10 / dup 0> while
    swap 1+ swap 
  repeat
  drop
;

: padding { u1 u2 -- u3 }
  u1 decimal_places
  u2 decimal_places
  -
;

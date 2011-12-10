\ auxiliary words

: decimal_places ( u1 -- u2 )
  1 swap begin
  10 / dup 0> while
    swap 1+ swap 
  repeat
  drop
;

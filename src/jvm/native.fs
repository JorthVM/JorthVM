: PrintMe.printHello|()V.static ( -- )
  ." ohai from gforth, whats up?" cr
;

: java/lang/Object.hashCode|()I ( this -- int1 )
  \ TODO dunno if this is okay
  0xffffffff and
;

: java/io/PrintStream.println|(Ljava/lang/String;)V ( this utf8_str -- )
  jvm_constpool_print_utf8 cr drop
;

\ vim: sw=2 ts=2 sta et
require ../jvm/frame.fs


: numberOfParameters_test
  \ single parameter
  s" (B)" jvm_frame.numberOfParamters() assert( 1 = )
  s" (C)" jvm_frame.numberOfParamters() assert( 1 = )
  s" (D)" jvm_frame.numberOfParamters() assert( 2 = )
  s" (F)" jvm_frame.numberOfParamters() assert( 1 = )
  s" (I)" jvm_frame.numberOfParamters() assert( 1 = )
  s" (J)" jvm_frame.numberOfParamters() assert( 2 = )
  s" (Ljava/lang/String;)" jvm_frame.numberOfParamters() assert( 1 = )
  s" (S)" jvm_frame.numberOfParamters() assert( 1 = )
  s" (Z)" jvm_frame.numberOfParamters() assert( 1 = )
  s" ([D)" jvm_frame.numberOfParamters() assert( 1 = )
  s" ([[[[[[[[[[D)" jvm_frame.numberOfParamters() assert( 1 = )
  s" ([[[[[[[[[[Ljava/lang/String;)" jvm_frame.numberOfParamters() assert( 1 = )

  \ multiparameter
  s" (IDLjava/lang/Thread;)Ljava/lang/Object;" jvm_frame.numberOfParamters() assert( 4 = )
  s" (I[[DLjava/lang/Thread;)Ljava/lang/Object;" jvm_frame.numberOfParamters() assert( 3 = )

  \ negativ test
  try
    s" B)" jvm_frame.numberOfParamters() assert( 1 = )
    -1
    iferror 
      dup -1 = IF drop 0 ENDIF
    endif
  endtry
  throw
;

: test
  numberOfParameters_test
;

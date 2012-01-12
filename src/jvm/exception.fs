\ vim: sw=2 ts=2 sta et

\ ========
\ *! exception
\ *T Exceptions
\ ========


\ *S JVM Exceptions
s" JVM Breakpoint Exception" exception constant JVM_BREAKPOINT_EXCEPTION
\ *G
s" JVM Not Yet Implemented Exception" exception constant JVM_NOTIMPLEMENTED_EXCEPTION
\ *G
s" JVM public static main(String[]) not found Exception" exception constant JVM_MAINNOTFOUND_EXCEPTION
\ *G
s" JVM class not found Exception" exception constant JVM_CLASSNOTFOUND_EXCEPTION
\ *G

s" JVM word not found Exception" exception constant JVM_WORDNOTFOUND_EXCEPTION
s" JVM native word not found Exception" exception constant JVM_NATIVENOTFOUND_EXCEPTION
\ *G
s" JVM Classfile: unknown Constant Pool Type Exception" exception constant JVM_UNKNOWNCONSTPOOLTYPE_EXCEPTION
\ *G

\ *S Specified exceptions
s" LinkerError" exception constant JVM_LINKERERROR
\ *G
s" ClassFormatError" exception constant JVM_CLASSFORMATERROR
\ *G
s" UnsupportedClassVersionError" exception constant JVM_UNSUPPORTEDCLASSVERSIONERROR
\ *G
s" NoClassDefError" exception constant JVM_NOCLASSDEFERROR
\ *G

\ *S Temporary Exceptions
s" JVM Return Exception" exception constant JVM_RETURN_EXCEPTION
\ *G

\ ======
\ *> ###
\ ======

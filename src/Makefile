JAVAC=javac
FORTH=gforth

.PHONY: all

all: testfiles/Main.class
	$(FORTH) jvm-test.fs

%.class: %.java
	$(JAVAC) $<

JAVAC=javac
FORTH=gforth
MAKE=make

UNITTEST_DIR=unittests


.PHONY: all unittests

all: testfiles/Main.class
	$(FORTH) jvm-test.fs

unittests:
	$(MAKE) -C $(UNITTEST_DIR)

%.class: %.java
	$(JAVAC) $<

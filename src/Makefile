JAVAC=javac
FORTH=gforth
BGFORTH=qemu-ppc -L ../ppcfs ../ppcfs/usr/bin/gforth
MAKE=make
IMGHOST=http://wien.tomnetworks.com/jvm

UNITTEST_DIR=unittests


.PHONY: all unittests bigendian

all: testfiles/Main.class
	$(FORTH) jvm-test.fs

unittests: bigendian
	FORTH="$(FORTH)" $(MAKE) -C $(UNITTEST_DIR)

%.class: %.java
	$(JAVAC) $<

ppcdeb.tar.gz:
	wget $(IMGHOST)/$@

ppcdeb.tar.gz.md5sum:
	wget $(IMGHOST)/$@

ppcfs: ppcdeb.tar.gz ppcdeb.tar.gz.md5sum
	md5sum --check $<.md5sum
	tar xf $<

bigendian: ppcfs/
	FORTH="$(BGFORTH)" $(MAKE) -C $(UNITTEST_DIR)

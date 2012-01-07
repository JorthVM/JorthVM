FORTH=gforth
BGFORTH=qemu-ppc -L ../ppcfs ../ppcfs/usr/bin/gforth
MAKE=make
IMGHOST=http://wien.tomnetworks.com/jvm

UNITTEST_DIR=unittests


.PHONY: all unittests bigendian clean

all: testfiles/StaticIntOther.class testfiles/StaticIntOtherStore.class
	$(FORTH) jvm-test.fs

%.class: %.java
	make -C $(dir $@) $(notdir $@)

unittests: bigendian
	FORTH="$(FORTH)" $(MAKE) -C $(UNITTEST_DIR)

ppcdeb.tar.gz:
	wget $(IMGHOST)/$@

ppcdeb.tar.gz.md5sum:
	wget $(IMGHOST)/$@

ppcfs: ppcdeb.tar.gz ppcdeb.tar.gz.md5sum
	md5sum --check $<.md5sum
	tar xf $<

bigendian: ppcfs/
	FORTH="$(BGFORTH)" $(MAKE) -C $(UNITTEST_DIR)

clean:
	make -C testfiles clean

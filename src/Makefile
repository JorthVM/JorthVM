FORTH=gforth
BGFORTH=qemu-ppc -L ../ppcfs ../ppcfs/usr/bin/gforth
MAKE=make
IMGHOST=http://wien.tomnetworks.com/jvm

UNITTEST_DIR=unittests

CLASSFILES := $(subst .java,.class,$(shell ls testfiles/*.java))

.PHONY: all ans-check unittests bigendian clean ohai

all: $(CLASSFILES)
	$(FORTH) jvm-test.fs

ohai: $(CLASSFILES)
	$(FORTH) jvm-test.fs -e RunDemoPrintln -e bye

ans-check:
	$(FORTH) ans-report.fs jvm/jvm.fs -e "print-ans-report bye"

%.class: %.java
	make -C $(dir $@) $(notdir $@)

unittests: bigendian
	FORTH="$(FORTH)" $(MAKE) -C $(UNITTEST_DIR)

ppcdeb.tar.gz:
	wget $(IMGHOST)/$@

ppcdeb.tar.gz.md5sum:
	wget $(IMGHOST)/$@

ppcfs/.extract: ppcdeb.tar.gz ppcdeb.tar.gz.md5sum
	md5sum --check $<.md5sum
	tar xf $<
	touch $@

bigendian: ppcfs/.extract
	FORTH="$(BGFORTH)" $(MAKE) -C $(UNITTEST_DIR)

clean:
	make -C testfiles clean

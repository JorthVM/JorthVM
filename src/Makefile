#
# Copyright (C) 2011-2012 Sebastian Rumpl, Josef Eisl, Bernhard Urban
#
# This file is part of JorthVM
#
# JorthVM is free software: you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation, either version 3 of the License, or
# (at your option) any later version.
#
# This program is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details.
#
# You should have received a copy of the GNU General Public License
# along with this program.  If not, see <http://www.gnu.org/licenses/>.
# this file implements functionality that is needed to read class files

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


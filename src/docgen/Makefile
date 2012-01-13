# `JorthVM', a Java VM implemented in Forth
# 
# Copyright (C) 2012 Sebastian Rumpl <e0828489@student.tuwien.ac.at>
# Copyright (C) 2012 Josef Eisl <zapster@zapster.cc>
# Copyright (C) 2012 Bernhard Urban <lewurm@gmail.com>
# 
# This program is free software: you can redistribute it and/or modify
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

VFX=vfxlin
DOCGEN_LIB=/usr/share/doc/VfxForth/Lib/DocGen.fth
DOCGEN_CONTROL=docgen.dgs
CLEAN_HTML=./contrib/cleanhtml.sh
INDICES_FIX_HTML=awk -f ./contrib/indices_add_header.awk
CONTENTS_FIX_HTML=awk -f ./contrib/contents_fix.awk
FINAL_DIR=html

FILES=classfile class classloader decode exception execute frame jvm rtconstpool stack util contents index indices 
HTML_FILES=$(addsuffix .html,$(FILES))
FINAL_HTML_FILES=$(addprefix $(FINAL_DIR)/,$(HTML_FILES))


RM=rm -f

.PHONY: all clean docgen

all: $(FINAL_HTML_FILES)

$(filter-out index.html,$(HTML_FILES)):
	$(VFX) include $(DOCGEN_LIB) include $(DOCGEN_CONTROL) bye > /dev/null

index.html:index.tmpl
	cp $< $@

$(FINAL_DIR):
	mkdir -p $@

$(FINAL_DIR)/%.html:%.html $(FINAL_DIR)
	$(CLEAN_HTML) $< > $@

$(FINAL_DIR)/contents.html:contents.html
	$(CLEAN_HTML) $<  | \
	sed -e 's/\(<meta [^>]*>\)/\1\n<\/head>\n<body>\n/g' | \
	sed -e 's/<a /<a target="main" /g' | \
	$(CONTENTS_FIX_HTML) \
	> $@

$(FINAL_DIR)/indices.html:indices.html
	$(CLEAN_HTML) $< |  \
	sed -e 's/\(<a.*<\/a>\)/<li>\1<\/li>/g' | \
	sed -e 's/<a /<a target="main" /g' | \
	$(INDICES_FIX_HTML) \
	> $@

$(FINAL_DIR)/index.html:index.html
	cp $< $@

clean:
	$(RM) $(FINAL_HTML_FILES)
	$(RM) $(HTML_FILES)
	$(RM) "###.html"


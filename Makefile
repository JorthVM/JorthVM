SM=markdown # standard markdown
PP=cpp -P

DOCS: HEADER.html README.html implementation.html

.PHONY: all test

all: $(DOCS)

test:
	make test -C src/unittests

%.html: %.md
	$(SM) $< > $@

%.html: %.txt
	$(SM) $< > $@

implementation.html: implementation.tmpl HEADER.html
	$(PP) $< $@ 

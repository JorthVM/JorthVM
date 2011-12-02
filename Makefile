SM=markdown # standard markdown

.PHONY: all test

all:

test:
	make test -C src/unittests

%.html: %.md
	$(SM) $< > $@

%.html: %.txt
	$(SM) $< > $@


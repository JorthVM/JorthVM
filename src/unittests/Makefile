UNITTESTS=$(addsuffix .test, $(shell ls *.fs))
FORTH_OPT=--clear-dictionary


test: $(UNITTESTS)

%.fs.test: %.fs
	$(FORTH) $(FORTH_OPT) $< -e bye
  

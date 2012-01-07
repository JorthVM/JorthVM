UNITTESTS=$(addsuffix .test, $(shell ls *.fs))
FORTH?=gforth
FORTH_OPT=--clear-dictionary
TESTFILE_DIR=../testfiles/


test: testfiles $(UNITTESTS)

testfiles:
	make -C $(TESTFILE_DIR)

%.fs.test: %.fs
	$(FORTH) $(FORTH_OPT) $< -e test -e bye
  

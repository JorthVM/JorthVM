JAVAC=javac
JAVA_FILES=$(wildcard *.java)
CLASS_FILES=$(JAVA_FILES:.java=.class)

.PHONY: all clean

all: $(CLASS_FILES)

%.class: %.java
	$(JAVAC) $<

clean:
	rm -f $(CLASS_FILES)

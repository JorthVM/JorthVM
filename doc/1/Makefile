SHELL := bash

all: praes.pdf

%.pdf: %.tex Makefile
	pdflatex $<
	pdflatex $<

.PHONY: clean
clean:
	rm -f praes.{aux,pdf,out,log}

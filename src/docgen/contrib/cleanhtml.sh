#! /bin/bash

file=`basename $1`
title=${file%.html}

sed '
# lowercase tags
s/\<\([A-Z0-9]*\)\>/\L\1/g
# remove br
s/<br>//g
# remove hr
s/<hr>//g
# remove font
s/<font[^>]*>//g
s/<\/font>//g
# remove code table
s/<table border="1" width="100%"><tr><td width="100%">/<div class="codeblock">/g
s/<\/td><\/tr><\/table>/<\/div>/g
# remove bgcolor
s/ bgcolor=#[0-9a-fA-F]*//g
# add style include
s|\(<body\)|\n<link href="css/style.css" rel="stylesheet" type="text/css" >\n<style type="text/css" media="all">code {display:block;}</style>\1|g
# add div to code
#s/<code>/<div><code>/g
#s/<\/code>/<\/code><\/div>/g
# add header
s|<html>|<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN"\n"http://www.w3.org/TR/html4/loose.dtd">\n<html>\n<head>\n<title>'$title'</title>\n<meta http-equiv="Content-Type" content="text/html;charset=utf-8" >\n|g
s/<body>/\n\<\/head>\n<body>\n/g ' <$1 



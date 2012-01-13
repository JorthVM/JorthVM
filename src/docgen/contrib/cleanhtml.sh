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



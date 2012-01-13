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

#!/bin/usr/awk -f
# 
BEGIN { 
  RS="\n";
  FS="";
  sep="&nbsp;&nbsp;";
  count=0
  regex="<code>(&nbsp;)*</code>"
} 
{
  if (match($0,regex)) 
  {
	n=split($0,array,sep)
	# print "n:", n, " count:", count
	if ( count < n) 
	{
	  print "<ul>";
	}
	if ( count == n) 
	{
	  print "</li>";
	}
	if ( count > n) 
	{
	  print "</ul>";
	  print "</li>";
	}
	print "<li>";
	sub(regex,"",$0);
	print $0;
	#print "</li>";
	count = n;
  } 
  else {
    for (;count>1;count--) {
	  print "</ul>";
	  print "</li>";
	}
    if (count>0) {
	  print "</ul>";
	}
	print $0;
  }
} 
END {
  for (;count>1;count--) {
	print "</ul>";
	print "</li>";
  }
  if (count>0) {
	print "</ul>";
  }
}

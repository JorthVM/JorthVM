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

#!/bin/usr/awk -f
# 
BEGIN { 
  print "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\"";
  print "\"http://www.w3.org/TR/html4/loose.dtd\">"
  print "<html>";
  print "<head>";
  print "<title>Indices</title>";
  print "<meta http-equiv=\"Content-Type\" content=\"text/html;charset=utf-8\" >";
  print "</head>";
  print "<body>";
  print "<ul>";
} 
{
  print $0
} 
END {
  print "</ul>";
  print "</body>";
  print "</html>";
}

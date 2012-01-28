#
# Copyright (C) 2011-2012 Sebastian Rumpl, Josef Eisl, Bernhard Urban
#
# This file is part of JorthVM
#
# JorthVM is free software: you can redistribute it and/or modify
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

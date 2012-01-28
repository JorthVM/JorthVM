//
// Copyright (C) 2011-2012 Sebastian Rumpl, Josef Eisl, Bernhard Urban
//
// This file is part of JorthVM
//
// JorthVM is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

public class IfIntegerCompare
{ 
  private static int res1;
  private static int res2;
  private static int res3;
  private static int res4;
  private static int cmp0 = 0;
  private static int cmp3 = 3;

	public static void main(String [] args)
	{
	  // if_icmpge
	  if ( cmp0 < cmp3 ) {
	    res1 = 0xDEAD;
	  } else {
	    res1 = 0;
	  }
	  // if_icmple
	  if ( cmp0 > cmp3 ) {
	    res2 = 0;
	  } else {
	    res2 = 0xBABE;
	  }
	  // if_icmplt
	  if ( cmp0 >= cmp3 ) {
	    res3 = 0;
	  } else {
	    res3 = 0xBEEF;
	  }
	  // if_icmpgt
	  if ( cmp0 <= cmp3 ) {
	    res4 = 0xAFFE;
	  } else {
	    res4 = 0;
	  }
	}

}


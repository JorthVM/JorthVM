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

public class TestArray
{ 
  private static int intarr[];

	public static void main(String [] args)
	{
		TestIntArr();
	}

	private static void TestIntArr() 
	{
		intarr = new int[5];
                intarr[0] = 2;
                intarr[1] = 3;
                intarr[2] = 5;
                intarr[3] = 7;
                intarr[4] = 11;
	}

}

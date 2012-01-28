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

public class StaticFibonacci
{ 
  private static int res;

	public static void main(String [] args)
	{
		res = fib(4);
	}

	private static int fib(int i) 
	{
		if ( i < 2 )
			return 1;
		return fib(i-1) + fib(i-2);
	}

}

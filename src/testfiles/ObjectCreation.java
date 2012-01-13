// `JorthVM', a Java VM implemented in Forth
// 
// Copyright (C) 2012 Sebastian Rumpl <e0828489@student.tuwien.ac.at>
// Copyright (C) 2012 Josef Eisl <zapster@zapster.cc>
// Copyright (C) 2012 Bernhard Urban <lewurm@gmail.com>
// 
// This program is free software: you can redistribute it and/or modify
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

public class ObjectCreation {
	public static int checkMe;
	public static ObjectCreation obj;

	public int stuff;

	public static void main(String []args) {
		CreateMe obj = new CreateMe();
		obj.executeMe();
		obj.objcOnly();
		CreateMe.woot();
	}

	public ObjectCreation() {
		this.stuff = 0x1300;
	}

	public void objcOnly() {
		ObjectCreation.checkMe += 0x3;
	}
	
	public void executeMe() {
		ObjectCreation.checkMe = 0xdead;
	}

	public static void woot() {
		checkMe++;
	}
}

class CreateMe extends ObjectCreation {
	public int var1;
	public int var2;

	public CreateMe() {
		this.var1 = 0x11;
		this.var2 = 0x22;
	}

	public void executeMe() {
		ObjectCreation.checkMe = this.var1 + this.var2 + this.stuff;
	}
}

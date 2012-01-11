public class ObjectCreation {
	public static int foo;
	public static ObjectCreation obj;

	public int stuff = 0;

	public static void main(String []args) {
		// ObjectCreation.obj = new ObjectCreation();
		// ObjectCreation.obj.whatEver();
		CreateMe obj = new CreateMe();
		obj.executeMe();
	}

	public ObjectCreation() {
		ObjectCreation.foo = 0x42;
	}
	
	public void whatEver() {
		stuff = 0x1337;
	}
}

class CreateMe extends ObjectCreation {
	public static int padding1;
	public static int padding2;
	public static int padding3;
	public static int padding4;
	// public long var1;
	public int var1;
	public int var2;

	public CreateMe() {
		stuff = 0x101;
		var1 = 0x11;
		var2 = 0x22;
	}

	public CreateMe(int a) {
		var1 = a;
	}

	public void executeMe() {
		stuff = 0x202;
		var1 = 0x33;
		var2 = 0x44;
	}
}

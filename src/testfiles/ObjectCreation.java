public class ObjectCreation {
	public static int foo;
	public static ObjectCreation lulz;

	public static void main(String []args) {
		ObjectCreation.lulz = new ObjectCreation();
	}

	public ObjectCreation() {
		ObjectCreation.foo = 0x42;
	}
}

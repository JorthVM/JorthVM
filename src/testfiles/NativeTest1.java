public class NativeTest1 {
	public static int a;
	static public void main(String []args) {
		NativeTest1.a = new NativeTest1().hashCode();
		// System.out.printf("hash: 0x%08x\n", NativeTest1.a);
	}
}

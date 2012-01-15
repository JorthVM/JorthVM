public class FrameTest1 {
	public static int res = 0;

	public static void main(String []args) {
		int i = 0x1337;
		int j = 0x101;

		FrameTest1.res = FrameTest1.foo(i, j); // 0x154a
		// System.out.printf("0x%08x\n", FrameTest1.res);
	}

	public static int foo(int i, int j) {
		int x = FrameTest1.bar(i, j, 0x11);
		return x + j;
	}
	public static int bar(int a, int b, int c) {
		return a + b + c;
	}
}

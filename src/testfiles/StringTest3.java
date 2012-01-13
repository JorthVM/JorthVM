public class StringTest3 {
	public char data[] = new char[0x100];

	public StringTest3() {
		for (int i = 0; i < 0x2; i++) {
			data[i] = (char) 0;
		}
	}

	public static void main(String []args) {
		new StringTest3();
	}
}

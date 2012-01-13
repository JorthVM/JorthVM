public class StringBuilder {
	char buf[];
	int length;

	public StringBuilder() {
		buf = new char[0x500];
		length = 0;
	}

	public StringBuilder append(int i) {
		for (int j = 0; j < 8; j++) {
			int t = (i & (0xf0000000 >> j));
			t >>= (8 - j);
			if (t < 0xa) {
				buf[this.length + j] = t + '0';
			} else {
				buf[this.length + j] = t - 0xa + 'a';
			}
		}
		this.length += 8;
	}

	public StringBuilder append(String str) {
		for (int i = 0; i < str.length ; i++) {
			buf[this.length + i] = str.charAt(i);
		}
		this.length += str.length;
	}

	public String toString() {
		return new String(this);
	}
}

package java.lang;

public class StringBuilder {
	char buf[];
	int length;

	public StringBuilder() {
		buf = new char[0x500];
		length = 0;
	}

	public StringBuilder(StringBuilder sb) {
		this();
		for (int i = 0; i < sb.length; i++) {
			this.buf[i] = sb.buf[i];
		}
		this.length = sb.length;
	}

	public StringBuilder append(int i) {
		StringBuilder n = new StringBuilder(this);

		for (int j = 0; j < 8; j++) {
			int t = (i & (0xf0000000 >> j));
			t >>= ((8 * 4) - j);
			char tc = (char) t;
			if (t < 0xa) {
				n.buf[n.length + j] = (char) (t + '0');
			} else {
				n.buf[n.length + j] = (char) (tc - 0xa + 'a');
			}
		}
		n.length += 8;

		return n;
	}

	public StringBuilder append(String str) {
		StringBuilder n = new StringBuilder(this);

		for (int i = 0; i < str.length ; i++) {
			n.buf[n.length + i] = str.charAt(i);
		}
		n.length += str.length;

		return n;
	}

	public String toString() {
		return new String(this);
	}
}

package java.lang;

public class String {
	int length;
	char buf[];

	public String(StringBuilder sb) {
 		this.buf = sb.buf;
 		this.length = sb.length;
 	}

	public String() {
		buf = new char[0x500];
		length = 0;
	}
		
	public char charAt(int i) {
		return buf[i];
	}
}

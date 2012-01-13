public class String {
	char buf[0x500];

	public String(StringBuilder sb) {
		this.buf = sb.buf;
	}
		
	public char charAt(int i) {
		return buf[i];
	}
}

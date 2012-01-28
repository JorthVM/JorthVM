public class StaticInitializer
{ 
  private static int foo;
  private static int bar;
  static {
    foo = 0x42;
  }
  static {
    bar = 0x84;
  }

	public static void main(String [] args)
	{
	}
}


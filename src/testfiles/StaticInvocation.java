public class StaticInvocation
{ 
  private static int foo;
  private static int bar;

	public static void main(String [] args)
	{
	  calc();
	}
	
	public static void calc()
	{
    foo = 0x42;
    bar = 2 * foo;
	}

}

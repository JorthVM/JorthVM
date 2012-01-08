public class StaticInvocation2
{ 
  private static int foo;
  private static int bar;

	public static void main(String [] args)
	{
	  calc();
    bar = 2 * foo;
	}
	
	public static void calc()
	{
    foo = 0x42;
	}

}

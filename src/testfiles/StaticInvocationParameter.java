public class StaticInvocationParameter
{ 
  private static int foo;
  private static int bar;

	public static void main(String [] args)
	{
	  calc(0x42, 2);
	  calc(0x11, 3);
	}
	
	public static void calc(int a, int b)
	{
    foo = a;
    bar = b * foo;
	}

}

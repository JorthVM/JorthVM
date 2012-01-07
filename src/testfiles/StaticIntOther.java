
public class StaticIntOther
{ 

	public static void main(String [] args)
	{
      StaticIntOtherStore.foo = 0x42;
      StaticIntOtherStore.bar = 2 * StaticIntOtherStore.foo;
	}

}

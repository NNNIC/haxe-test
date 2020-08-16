using System;

namespace TestDll
{
    public class Test
    {
        public void call_main()
        {
            Console.WriteLine("It's DLL now.");
            global::Test.Main();
        }
    }
}

using System;

namespace TestApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var p = new TestDll.Class1();
            p.call_main();
        }
    }
}

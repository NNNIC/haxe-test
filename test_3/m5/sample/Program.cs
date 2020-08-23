using System;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;

namespace sample
{
    class Program
    {
        public static void Main(string[] args)
        {
            var v = "from main";
            var r = new Refstr();// { s = v };
            r.s = v;
            test(r); ;
            v = r.s;
            Console.WriteLine("Hello World!\n");
            Console.WriteLine(v);
            var sm = new TestControl();
            sm.Run();
        }
        static void test(Refstr r)
        {
            r.s += "!test";
        }
    }
    public class Refstr
    {
        public String s;
    }
}

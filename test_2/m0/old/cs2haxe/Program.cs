using System;
using System.IO;

namespace cs2haxe
{
    class Program
    {
        static void Main(string[] args)
        {
            var testfile = @"G:\statego\samples\haxe-test\test_2\m0\sample\Program.cs";
            var src = File.ReadAllText(testfile/*args[0]*/);

            var engine = new yengine();

            // 終末記号に分類
            var lex_output = engine.Lex(src);
            /*
                lex_outputには、1行づつ、分解された値が格納されている。
                C#は、EOLは無意味なので、消して一つのバッファにする。
            */


        }
    }
}

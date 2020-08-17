using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace lextool
{
    class Program
    {
        static void Main(string[] args)
        {
            var src = File.ReadAllText(args[0]);

            var engine = new yengine();

            // 終末記号に分類
            var lex_output = engine.Lex(src);

            //スペース・コメント削除。"文字列"以外大文字化。
            engine.Normalize(ref lex_output);                            sys.logline("\n*lex_output");            engine.DumpList(lex_output, true);

            //第一解析
            var first_interpreted = engine.Interpret(lex_output);        sys.logline("\n*first_interpreted");     engine.DumpList(first_interpreted, true);

            //プリプロセス処理 #if等
            var preprocessed = engine.PreProcess(first_interpreted);     sys.logline("\n*preprocessed");          engine.DumpList(preprocessed, true);

            //実行用リスト作成・第二解析
            var executable_value_list = engine.Interpret(preprocessed);  sys.logline("\n*executable_value_list"); engine.DumpList(executable_value_list, true);

            //リストの整合性テスト
            int errorline;
            if (!engine.IsExecuable(executable_value_list,out errorline))
            {
                sys.error("Not executable. Check Line " + (errorline + 1));
            }

            //実行
            sys.logline("\n\n*Execute! \n");
            foreach (var l in executable_value_list)
            {
                runtime.MainProcessFunction.ExecuteSentence(l);
            }

            Console.WriteLine("end");
        }
    }
}

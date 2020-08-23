package sample;
using StringTools;
import system.*;
import anonymoustypes.*;

class Program
{
    public static function Main(args:Array<String>):Void
    {
        var v:String = "from main";
        var r:sample.Refstr = new sample.Refstr();
        r.s = v;
        test(r);
        v = r.s;
        system.Console.WriteLine("Hello World!\n");
        system.Console.WriteLine(v);
        var sm:TestControl = new TestControl();
        sm.Run();
    }
    static function test(r:sample.Refstr):Void
    {
        r.s += "!test";
    }
    public function new()
    {
    }
}

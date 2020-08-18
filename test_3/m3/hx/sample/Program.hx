package sample;
using StringTools;
import system.*;
import anonymoustypes.*;

class Program
{
    public static function Main(args:Array<String>):Void
    {
        system.Console.WriteLine("Hello World!");
        var sm:TestControl = new TestControl();
        sm.Run();
    }
    public function new()
    {
    }
}

import haxe.xml.Access;

typedef STATEFUNC = Bool->Void;
class Test {
    static public function main() {
        trace("Hello World");
        var c = new Sub();
        c.Set();
        c.Run();
    }
}
class Sub {
    var m_curfunc : STATEFUNC;
    
    var m_callstacks : Array<STATEFUNC> = [for(i in 0...10) null];

    public function new(){}

    function S_START(bFirst: Bool)  {
        trace("S_START");
    }
    public function Set() {
        m_callstacks[0] = S_START;
    }
    public function Run() {
        var func : Bool->Void = m_callstacks[0];
        func(true);
    }
}
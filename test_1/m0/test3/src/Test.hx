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

    public function new(){}

    function S_START(bFirst: Bool)  {
        trace("S_START");
    }

    public function Set() {
        m_curfunc = S_START;
    }
    public function Run() {
        m_curfunc(true);
    }

}
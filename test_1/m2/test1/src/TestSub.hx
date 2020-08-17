class TestSub {
    var name : String;
    var x : Float;
    var y : Float;

    public function new(name:String, x:Float, y:Float) {
        TestMacros.initLocals();
    }

    public function Say() {

        trace('Say $name $x $y ');

    }
}
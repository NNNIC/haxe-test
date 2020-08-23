/*
This file serves two purposes:  
    1)  It imports every type that CS2HX generated.  haXe will ignore 
        any types that aren't used by haXe code, so this ensures haXe 
        compiles all of your code.

    2)  It lists all the static constructors.  haXe doesn't have the 
        concept of static constructors, so CS2HX generated cctor()
        methods.  You must call these manually.  If you call
        Constructors.init(), all static constructors will be called 
        at once.
*/
package ;
import sample.Program;
import sample.Refstr;
import StateManager;
import TestControl;
import system.TimeSpan;
class Constructors
{
    public static function init()
    {
        TimeSpan.cctor();
    }
}

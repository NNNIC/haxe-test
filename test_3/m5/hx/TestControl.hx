package ;
using StringTools;
import system.*;
import anonymoustypes.*;

class TestControl extends StateManager
{
    function S_END(bFirst:Bool):Void
    {
        m_bEnd = true;
    }
    function S_EVEN_OR_ODD(bFirst:Bool):Void
    {
        if (m_i % 2 == 0)
        {
            Goto(S_PRINT_EVEN);
        }
        else
        {
            Goto(S_PRINT_ODD);
        }
    }
    function S_GOSUB(bFirst:Bool):Void
    {
        GoSubState(S_SUBSTART1, S_LOOP);
        NoWait();
    }
    var m_i:Int = 0;
    function S_LOOP(bFirst:Bool):Void
    {
        m_i = 0;
        Goto(S_LOOP_LoopCheckAndGosub____);
        NoWait();
    }
    function S_LOOP_LoopCheckAndGosub____(bFirst:Bool):Void
    {
        if (m_i < 10)
        {
            GoSubState(S_SUBSTART, S_LOOP_LoopNext____);
        }
        else
        {
            Goto(S_END);
        }
        NoWait();
    }
    function S_LOOP_LoopNext____(bFirst:Bool):Void
    {
        m_i++;
        Goto(S_LOOP_LoopCheckAndGosub____);
        NoWait();
    }
    function S_PRINT_EVEN(bFirst:Bool):Void
    {
        if (bFirst)
        {
            system.Console.WriteLine(Std.string(m_i) + ".. EVEN");
        }
        if (!HasNextState())
        {
            Goto(S_SUBRETURN);
        }
    }
    function S_PRINT_ODD(bFirst:Bool):Void
    {
        if (bFirst)
        {
            system.Console.WriteLine(Std.string(m_i) + ".. ODD");
        }
        if (!HasNextState())
        {
            Goto(S_SUBRETURN);
        }
    }
    function S_START(bFirst:Bool):Void
    {
        m_bEnd = false;
        Goto(S_GOSUB);
        NoWait();
    }
    function S_SUBRETURN(bFirst:Bool):Void
    {
        ReturnState();
        NoWait();
    }
    function S_SUBRETURN1(bFirst:Bool):Void
    {
        ReturnState();
        NoWait();
    }
    function S_SUBSTART(bFirst:Bool):Void
    {
        Goto(S_EVEN_OR_ODD);
        NoWait();
    }
    function S_SUBSTART1(bFirst:Bool):Void
    {
        Goto(S_WORK);
        NoWait();
    }
    function S_WORK(bFirst:Bool):Void
    {
        if (bFirst)
        {
            system.Console.WriteLine("!!!");
        }
        if (!HasNextState())
        {
            Goto(S_SUBRETURN1);
        }
    }
    var m_callstack:Array<(Bool -> Void)> = new Array<(Bool -> Void)>();
    function GoSubState(nextstate:(Bool -> Void), returnstate:(Bool -> Void)):Void
    {
        m_callstack.insert(0, returnstate);
        Goto(nextstate);
    }
    function ReturnState():Void
    {
        var nextstate:(Bool -> Void) = m_callstack[0];
        m_callstack.splice(0, 1);
        Goto(nextstate);
    }
    public function Start():Void
    {
        Goto(S_START);
    }
    public function IsEnd():Bool
    {
        return m_bEnd;
    }
    public function Run():Void
    {
        var LOOPMAX:Int = Std.int(1E+5);
        Start();
        { //for
            var loop:Int = 0;
            while (loop <= LOOPMAX)
            {
                if (loop >= LOOPMAX)
                {
                    throw new system.SystemException("Unexpected.{82BA5316-DD53-447F-8A67-8D76453B4A4C}");
                }
                Update();
                if (IsEnd())
                {
                    break;
                }
                loop++;
            }
        } //end for
    }
    var m_bYesNo:Bool = false;
    function br_YES(st:(Bool -> Void)):Void
    {
        if (!HasNextState())
        {
            if (m_bYesNo)
            {
                Goto(st);
            }
        }
    }
    function br_NO(st:(Bool -> Void)):Void
    {
        if (!HasNextState())
        {
            if (!m_bYesNo)
            {
                Goto(st);
            }
        }
    }
    public function new()
    {
        super();
    }
}

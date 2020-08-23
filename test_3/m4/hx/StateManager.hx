package ;
using StringTools;
import system.*;
import anonymoustypes.*;

class StateManager
{
    var m_curfunc:(Bool -> Void);
    var m_nextfunc:(Bool -> Void);
    public var m_noWait:Bool = false;
    public var m_bEnd:Bool = false;
    public function Update():Void
    {
        while (true)
        {
            var bFirst:Bool = false;
            if (m_nextfunc != null)
            {
                m_curfunc = m_nextfunc;
                m_nextfunc = null;
                bFirst = true;
            }
            m_noWait = false;
            if (m_curfunc != null)
            {
                m_curfunc(bFirst);
            }
            if (!m_noWait)
            {
                break;
            }
        }
    }
    public function Goto(func:(Bool -> Void)):Void
    {
        m_nextfunc = func;
    }
    public function CheckState(func:(Bool -> Void)):Bool
    {
        return m_curfunc == func;
    }
    public function HasNextState():Bool
    {
        return m_nextfunc != null;
    }
    public function NoWait():Void
    {
        m_noWait = true;
    }
    public function new()
    {
    }
}

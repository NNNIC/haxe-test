using System;
using System.Collections.Generic;
using System.Text;

public class StateManager
{
    Action<bool> m_curfunc;
    Action<bool> m_nextfunc;

    protected bool         m_noWait;
    protected bool         m_bEnd;
    
    public void Update()
    {
        while(true)
        {
            var bFirst = false;
            if (m_nextfunc!=null)
            {
                m_curfunc = m_nextfunc;
                m_nextfunc = null;
                bFirst = true;
            }
            m_noWait = false;
            if (m_curfunc!=null)
            {   
                m_curfunc(bFirst);
            }
            if (!m_noWait) break;
        }
    }
    protected void Goto(Action<bool> func)
    {
        m_nextfunc = func;
    }
    protected bool CheckState(Action<bool> func)
    {
        return m_curfunc == func;
    }
    protected bool HasNextState()
    {
        return m_nextfunc != null;
    }
    protected void NoWait()
    {
        m_noWait = true;
    }
}


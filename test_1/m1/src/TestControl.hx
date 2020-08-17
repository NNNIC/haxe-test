typedef STATEFUNC = Bool->Void;

class TestControl  {
//#region manager
    var m_curfunc : STATEFUNC;
    var m_nextfunc: STATEFUNC;

    var m_noWait : Bool;
    
    public function Update() {
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
    function Goto(func : STATEFUNC)
    {
        m_nextfunc = func;
    }
    function CheckState(func : STATEFUNC) : Bool
    {
        return m_curfunc == func;
    }
    function HasNextState() : Bool
    {
        return m_nextfunc != null;
    }
    function NoWait()
    {
        m_noWait = true;
    }
//#endregion
//#region gosub
    var MAX_CALLSTACK : Int = 10;
    var m_callstacks : Array<STATEFUNC>;
    var m_callstack_level = 0;
    function GoSubState(nextstate : STATEFUNC, returnstate : STATEFUNC)
    {
        if (m_callstack_level >= MAX_CALLSTACK -1) {
            trace("CALL STACK OVERFLOW");
            return;
        }
        m_callstacks[m_callstack_level] = returnstate;
        m_callstack_level += 1;
        Goto(nextstate);
    }
    function ReturnState()
    {
        if (m_callstack_level <= 0) {
            trace("CALL STACK UNDERFLOW");
            return;
        }
        m_callstack_level -= 1;
        var nextstate = m_callstacks[m_callstack_level];
        Goto(nextstate);
    }
//#endregion 

//#region CONSTRUCTOR
    public function new(){
        m_callstacks = [for(i in 0...MAX_CALLSTACK) null];
    }
//#endregion

    public function Start()
    {
        Goto(S_START);
    }
    public function IsEnd() : Bool    
    { 
        return CheckState(S_END); 
    }
    
    public function Run()
    {
        var LOOPMAX = 100000;
        var bEnd = false;
		Start();
		for(loop_1 in 0...LOOPMAX)
		{
            if (bEnd) break;
            if (loop_1 >= LOOPMAX-1){
                trace("OUT OF LOOP. INCREASE LOOPMAX OR MODIFY USING WHILE"); 
            }
            for(loop_2 in 0...LOOPMAX) {
                Update();
                bEnd = IsEnd();
                if (bEnd) break;
            }
        }
        
	}

	// [PSGG OUTPUT START] indent(4) $/./$
    //             psggConverterLib.dll converted from psgg-file:TestControl.psgg

    /*
        E_0001
    */
    var m_msg : String;
    /*
        S_0000
    */
    function S_0000(bFirst : Bool)
    {
        //
        if (bFirst)
        {
            trace("Hello World!");
        }
        //
        if (!HasNextState())
        {
            Goto(S_0008);
        }
    }
    /*
        S_0001
    */
    var m_val : Int;
    function S_0001(bFirst : Bool)
    {
        var sec = Date.now().getSeconds();
        var n = sec % 5;
        m_val = n;
        // branch
        if (n == 0) { Goto( S_0002 ); }
        else if (n == 1) { Goto( S_0003 ); }
        else if (n == 2) { Goto( S_0004 ); }
        else if (n == 3) { Goto( S_0005 ); }
        else { Goto( S_0006 ); }
    }
    /*
        S_0002
    */
    function S_0002(bFirst : Bool)
    {
        //
        if (bFirst)
        {
            trace("Zero");
        }
        //
        if (!HasNextState())
        {
            Goto(S_WAIT1SEC);
        }
    }
    /*
        S_0003
    */
    function S_0003(bFirst : Bool)
    {
        //
        if (bFirst)
        {
            trace("First");
        }
        //
        if (!HasNextState())
        {
            Goto(S_WAIT1SEC);
        }
    }
    /*
        S_0004
    */
    function S_0004(bFirst : Bool)
    {
        //
        if (bFirst)
        {
            trace("Two");
        }
        //
        if (!HasNextState())
        {
            Goto(S_WAIT1SEC);
        }
    }
    /*
        S_0005
    */
    function S_0005(bFirst : Bool)
    {
        //
        if (bFirst)
        {
            trace("Three");
        }
        //
        if (!HasNextState())
        {
            Goto(S_WAIT1SEC);
        }
    }
    /*
        S_0006
    */
    function S_0006(bFirst : Bool)
    {
        //
        if (bFirst)
        {
            trace('$m_val');
        }
        //
        if (!HasNextState())
        {
            Goto(S_WAIT1SEC);
        }
    }
    /*
        S_0007
    */
    function S_0007(bFirst : Bool)
    {
        //
        if (bFirst)
        {
            trace("In Subroutine!");
        }
        //
        if (!HasNextState())
        {
            Goto(S_RET000);
        }
    }
    /*
        S_0008
    */
    function S_0008(bFirst : Bool)
    {
        //
        if (bFirst)
        {
            m_msg = "Use m_msg.";
            trace(m_msg);
        }
        //
        if (!HasNextState())
        {
            Goto(S_GSB000);
        }
    }
    /*
        S_END
    */
    function S_END(bFirst : Bool)
    {
    }
    /*
        S_GSB000
    */
    function S_GSB000(bFirst : Bool)
    {
        GoSubState(S_SBS000,S_LOP000);
        NoWait();
    }
    /*
        S_LOP000
    */
    var m_index : Int;
    function S_LOP000(bFirst : Bool)
    {
        m_index = 0;
        Goto(S_LOP000_Check____);
        NoWait();
    }
    function S_LOP000_Check____(bFirst : Bool)
    {
        if (m_index < 10) GoSubState(S_SBS001,S_LOP000_Next____);
        else               Goto(S_END);
        NoWait();
    }
    function S_LOP000_Next____(bFirst : Bool)
    {
        m_index++;
        Goto(S_LOP000_Check____);
        NoWait();
    }
    /*
        S_RET000
    */
    function S_RET000(bFirst : Bool)
    {
        ReturnState();
        NoWait();
    }
    /*
        S_RET001
    */
    function S_RET001(bFirst : Bool)
    {
        ReturnState();
        NoWait();
    }
    /*
        S_SBS000
    */
    function S_SBS000(bFirst : Bool)
    {
        Goto(S_0007);
        NoWait();
    }
    /*
        S_SBS001
    */
    function S_SBS001(bFirst : Bool)
    {
        Goto(S_0001);
        NoWait();
    }
    /*
        S_START
    */
    function S_START(bFirst : Bool)
    {
        Goto(S_0000);
        NoWait();
    }
    /*
        S_WAIT1SEC
    */
    var m_S_WAIT1SEC : Int;
    function S_WAIT1SEC(bFirst : Bool)
    {
        //
        if (bFirst)
        {
            m_S_WAIT1SEC = Date.now().getSeconds();
        }
        if (m_S_WAIT1SEC == Date.now().getSeconds()) return;
        //
        if (!HasNextState())
        {
            Goto(S_RET001);
        }
    }


	// [PSGG OUTPUT END]

	// write your code below

}

/*  :::: PSGG MACRO ::::
:psgg-macro-start

commentline=// {%0}

@branch=@@@
<<<?"{%0}"/^brifc{0,1}$/
if ([[brcond:{%N}]]) { Goto( {%1} ); }
>>>
<<<?"{%0}"/^brelseifc{0,1}$/
else if ([[brcond:{%N}]]) { Goto( {%1} ); }
>>>
<<<?"{%0}"/^brelse$/
else { Goto( {%1} ); }
>>>
<<<?"{%0}"/^br_/
{%0}({%1});
>>>
@@@

:psgg-macro-end
*/


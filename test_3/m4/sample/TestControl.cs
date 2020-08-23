using System;
using System.Collections.Generic;
public partial class TestControl  {
   
	#region    // [PSGG OUTPUT START] indent(8) $/./$
        //             psggConverterLib.dll converted from psgg-file:TestControl.psgg

        /*
            S_END
        */
        void S_END(bool bFirst)
        {
            m_bEnd = true;
        }
        /*
            S_EVEN_OR_ODD
        */
        void S_EVEN_OR_ODD(bool bFirst)
        {
            // branch
            if (m_i % 2 == 0) { Goto( S_PRINT_EVEN ); }
            else { Goto( S_PRINT_ODD ); }
        }
        /*
            S_GOSUB
        */
        void S_GOSUB(bool bFirst)
        {
            GoSubState(S_SUBSTART1,S_LOOP);
            NoWait();
        }
        /*
            S_LOOP
        */
        int m_i;
        void S_LOOP(bool bFirst)
        {
            m_i=0;
            Goto(S_LOOP_LoopCheckAndGosub____);
            NoWait();
        }
        void S_LOOP_LoopCheckAndGosub____(bool bFirst)
        {
            if (m_i < 10) GoSubState(S_SUBSTART,S_LOOP_LoopNext____);
            else               Goto(S_END);
            NoWait();
        }
        void S_LOOP_LoopNext____(bool bFirst)
        {
            m_i++;
            Goto(S_LOOP_LoopCheckAndGosub____);
            NoWait();
        }
        /*
            S_PRINT_EVEN
        */
        void S_PRINT_EVEN(bool bFirst)
        {
            //
            if (bFirst)
            {
                Console.WriteLine(m_i.ToString() + ".. EVEN");
            }
            //
            if (!HasNextState())
            {
                Goto(S_SUBRETURN);
            }
        }
        /*
            S_PRINT_ODD
        */
        void S_PRINT_ODD(bool bFirst)
        {
            //
            if (bFirst)
            {
                Console.WriteLine(m_i.ToString() + ".. ODD");
            }
            //
            if (!HasNextState())
            {
                Goto(S_SUBRETURN);
            }
        }
        /*
            S_START
        */
        void S_START(bool bFirst)
        {
            m_bEnd = false;
            Goto(S_GOSUB);
            NoWait();
        }
        /*
            S_SUBRETURN
        */
        void S_SUBRETURN(bool bFirst)
        {
            ReturnState();
            NoWait();
        }
        /*
            S_SUBRETURN1
        */
        void S_SUBRETURN1(bool bFirst)
        {
            ReturnState();
            NoWait();
        }
        /*
            S_SUBSTART
        */
        void S_SUBSTART(bool bFirst)
        {
            Goto(S_EVEN_OR_ODD);
            NoWait();
        }
        /*
            S_SUBSTART1
        */
        void S_SUBSTART1(bool bFirst)
        {
            Goto(S_WORK);
            NoWait();
        }
        /*
            S_WORK
        */
        void S_WORK(bool bFirst)
        {
            //
            if (bFirst)
            {
                Console.WriteLine("!!!");
            }
            //
            if (!HasNextState())
            {
                Goto(S_SUBRETURN1);
            }
        }


	#endregion // [PSGG OUTPUT END]

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


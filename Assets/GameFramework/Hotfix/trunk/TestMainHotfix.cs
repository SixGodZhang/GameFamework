//-----------------------------------------------------------------------
// <filename>TestMainHotfix</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> ## </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/20 星期四 20:48:19# </time>
//-----------------------------------------------------------------------

using ILRuntime;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Taurus
{
    [AllowHotfix]
    public class TestMainHotfix
    {
        public void DoAction()
        {
            if (BugfixDelegateStatements.GameFramework_Taurus_TestMainHotfix_DoAction_IN__OUT_System_Void__Delegate != null)
            {
                BugfixDelegateStatements.GameFramework_Taurus_TestMainHotfix_DoAction_IN__OUT_System_Void__Delegate(this);
                return;
            }

            Debug.Log("do action in MainProject ......");
        }

        public static void DoAction(int arg)
        {
            Debug.Log("do action ......");
        }

        public static int DoAction(string arg)
        {
            return 0;
        }

        public static void DoPlay()
        {
            Debug.Log("do play ....");
        }

        //public static void DoPlay(ref string arg)
        //{

        //}

        //public static void DoPlay(out int arg)
        //{
        //    arg = 22;
        //}

    }
}

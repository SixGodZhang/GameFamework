//-----------------------------------------------------------------------
// <filename>HotFixMode</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #热更代码入口# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/3 星期一 16:58:52# </time>
//-----------------------------------------------------------------------

using System;
using GameFramework.Taurus;
using GameFramework.Test;
using Hotfix.Hotfix.Bugfix;
using Hotfix.Hotfix.Game;
using ILRuntime;

namespace HotFix.Taurus
{
    public class TTT { }

    public delegate TTT TTTTTTT(TTT register);

    public class HotFixMode: SubMonoBehavior
    {
       Action<int, string, bool> TestAction;
       TTTTTTT register = (TTT rer) =>
       {
           return rer;
       };

        public HotFixMode()
        {
            UnityEngine.Debug.Log("-------------------Start Hotfix Code-------------------");
        }

        public override void OnApplicationQuit()
        {
            UnityEngine.Debug.Log("-------------------Hotfix: call HotFixMode.OnApplicationQuit() -------------------");
        }

        public override void OnDestroy()
        {
            UnityEngine.Debug.Log("-------------------Hotfix: call HotFixMode.OnDestroy() -------------------");
        }

        public override void OnFixedUpdate()
        {
// UnityEngine.Debug.Log("-------------------Hotfix: call HotFixMode.OnFixedUpdate() -------------------");
        }

        public override void Start()
        {
            UnityEngine.Debug.Log("Hello world! 1111111111111 ");
            UnityEngine.Debug.Log("result: " + register.Equals(register));
            //hotfix
            //BugfixManager.isOpenHotfix = true;
            //if (BugfixManager.isOpenHotfix)
            //{
            //    UnityEngine.Debug.Log("开启热更修复功能!");
            //    BugfixManager.RegisterHotfixMethod();
            //}

            //TestMainHotfix mainHotfix = new TestMainHotfix();
            //mainHotfix.DoActionWithParams("11", 11);

            //new TestMainHotfix().DoAction();


            //TestMainHotfix.DoPlay();
            //TestClass.TESTNOPARAM += DoAction;
            //TestClass.TESTNOPARAM();
            //TestClass.TestFunc += DoFunc;
            //TestClass.TestFunc();
            //int a = 10;
            //UnityEngine.Debug.Log(a);
            //string b = "12";
            //UnityEngine.Debug.Log(b);
            //Type type = typeof(int);
            //UnityEngine.Debug.Log(type.ToString());
            //UnityEngine.Debug.Log("-------------------Hotfix: call HotFixMode.Start() -------------------");
            //TestAction += DoAction;
            //TestAction(1, "1", true);
            //TestDelegate.callBack += HotfixCallback;
            //TestClass tc = new TestClass();
            //tc.DoAction();
        }

        private int DoFunc()
        {
            UnityEngine.Debug.Log("this is hotfix func");
            return 0;
        }

        private void DoAction()
        {
            UnityEngine.Debug.Log("this is hotfix action");
        }

        private void DoAction(int arg1, string arg2, bool arg3)
        {
            UnityEngine.Debug.Log("test action");
        }

        private void HotfixCallback(int id, string name)
        {
            UnityEngine.Debug.Log("hello");
        }

        public override void Update()
        {
            UnityEngine.Debug.Log("-------------------Hotfix: call HotFixMode.Update() -------------------");
        }
    }
}

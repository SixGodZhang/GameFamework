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
using Hotfix.Hotfix.Game;

namespace HotFix.Taurus
{
    public class HotFixMode: SubMonoBehavior
    {
        Action<int, string, bool> TestAction;

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
            UnityEngine.Debug.Log("-------------------Hotfix: call HotFixMode.OnFixedUpdate() -------------------");
        }

        public override void Start()
        {
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

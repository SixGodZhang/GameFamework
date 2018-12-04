//-----------------------------------------------------------------------
// <filename>LoadHotfixState</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> ## </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/11/30 星期五 15:07:32# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Taurus
{
    [GameState(GameStateType.Generanal, Desc: "加载热更新")]
    public class LoadHotfixState : GameState
    {
        private const string _DLLPATH = "Assets/Game/HotFix/HotFix.dll.bytes";
        private const string _PDBPATH = "Assets/Game/HotFix/HotFix.pdb.bytes";

        #region 重写函数
        public override void OnEnter(params object[] parameters)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log("---------call LoadHotfixState------------");
#endif
            base.OnEnter(parameters);
            //加载热更代码
            LoadHotFixCode();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
        }

        public override void OnInit(GameStateContext context)
        {
            base.OnInit(context);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }
        #endregion

        #region 内部函数
        private void LoadHotFixCode()
        {
            //资源加载
            byte[] dll = GameMain.ResourceMG.LoadAsset<TextAsset>("hotfix", _DLLPATH).bytes;
            byte[] pdb = null;
#if UNITY_EDITOR
            pdb = GameMain.ResourceMG.LoadAsset<TextAsset>("hotfix", _PDBPATH).bytes;
            GameMain.HotfixMG.Appdomain.DebugService.StartDebugService(56000);
#endif
            GameMain.HotfixMG.LoadHotfixAssembly(dll, pdb);
        }
        #endregion
    }
}

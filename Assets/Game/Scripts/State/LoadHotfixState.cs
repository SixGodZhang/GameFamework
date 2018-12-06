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
        #region 重写函数
        public override void OnEnter(params object[] parameters)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log("---------call LoadHotfixState------------");
#endif
            base.OnEnter(parameters);

            //初始化热更
            GameMain.HotfixMG.Init(GameMain.ResourceMG);
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
    }
}

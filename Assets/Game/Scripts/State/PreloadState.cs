//-----------------------------------------------------------------------
// <filename>PreloadState</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #预加载状态# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/3 星期一 17:36:25# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.Taurus
{
    [GameState(GameStateType.Generanal, Desc: "预加载资源")]
    public class PreloadState : GameState
    {
        #region 重载函数
        public override void OnEnter(params object[] parameters)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log("---------call PreloadState------------");
#endif
            base.OnEnter(parameters);

            //测试 直接切换到热更新状态里面去
            ChangeState<LoadHotfixState>();
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

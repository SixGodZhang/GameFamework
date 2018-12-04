//-----------------------------------------------------------------------
// <filename>LaunchGameState</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> ## </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/11/30 星期五 14:58:23# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.Taurus
{
    [GameState(GameStateType.Start,Desc:"Game的第一个状态")]
    public class LaunchGameState:GameState
    {
        #region 重写函数
        public override void OnEnter(params object[] parameters)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log("---------call LaunchGameState------------");
#endif
            base.OnEnter(parameters);
            //加载更新&加载页面
            //TODO
            //....
            //System.Threading.Thread.Sleep(3000);
            //ChangeState<LoadHotfixState>();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            switch (GameMain.ResourceMG.ResUpdateType)
            {
                case ResourceUpdateType.Editor:
                    {
#if UNITY_EDITOR
                        GameMain.ResourceMG.SetResourceHelper(new EditorResourceHelper());
                        ChangeState<PreloadState>();
#else
                        GameMain.ResourceMG.ResUpdateType = ResourceUpdateType.Local;
                        ChangeState<LoadResourceState>();
#endif
                    }
                    break;
                case ResourceUpdateType.Local:
                    ChangeState<LoadResourceState>();
                    break;
                case ResourceUpdateType.Update:
                    ChangeState<CheckResourceState>();
                    break;
            }
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
        }

        public override void OnInit(GameStateContext context)
        {
            base.OnInit(context);
        }
#endregion
    }
}

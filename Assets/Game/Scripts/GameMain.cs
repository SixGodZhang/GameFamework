//-----------------------------------------------------------------------
// <filename>GameMain</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> ## </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/11/30 星期五 14:43:01# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Taurus
{
    public class GameMain:MonoBehaviour
    {
        #region 字段&属性
        public static EventManager EventMG;
        public static GameStateManager StateMG;
        public static ResourceManager ResourceMG;
        public static HotfixManager HotfixMG;
        public static WebRequestManager WebRequestMG;
        public static UIManager UIMG;

        public static System.Reflection.Assembly Assembly { get; private set; }
        #endregion

        /// <summary>
        /// 资源本地路径
        /// </summary>
        public PathType LocalPathType = PathType.ReadOnly;
        /// <summary>
        /// 资源更新的类型
        /// </summary>
        public ResourceUpdateType ResUpdateType = ResourceUpdateType.Editor;
        /// <summary>
        /// 资源更新的路径(比如从网络更新)
        /// </summary>
        public string ResUpdatePath = "";
        /// <summary>
        /// 是否开启调试器
        /// </summary>
        public bool DebugEnable = true;

        /// <summary>
        /// 是否使用热更代码
        /// </summary>
        public bool UseHotFix = true;

        IEnumerator Start()
        {
            DontDestroyOnLoad(gameObject);
            #region Modules
            StateMG = GameModuleProxy.GetModule<GameStateManager>();
            EventMG = GameModuleProxy.GetModule<EventManager>();
            ResourceMG = GameModuleProxy.GetModule<ResourceManager>();
            HotfixMG = GameModuleProxy.GetModule<HotfixManager>();
            WebRequestMG = GameModuleProxy.GetModule<WebRequestManager>();
            UIMG = GameModuleProxy.GetModule<UIManager>();
            #endregion

            #region Hotfix
            #endregion

            #region Resource
            ResourceMG.LocalPathType = LocalPathType;
            ResourceMG.ResUpdateType = ResUpdateType;
            ResourceMG.ResUpdatePath = ResUpdatePath;

            //添加对象池管理器
            GameObject GameObjectPoolHelper = new GameObject("IGameObjectPoolHelper");
            GameObjectPoolHelper.transform.SetParent(transform);
            ResourceMG.SetGameObjectPoolHelper(GameObjectPoolHelper.AddComponent<GameObjectPoolHelper>());
            #endregion

            #region WebRequest
            GameObject webRequestHelper = new GameObject("IWebRequestHelper");
            webRequestHelper.transform.SetParent(transform);
            GameObject webDownloadMonoHelper = new GameObject("IWebDownloadHelper");
            webDownloadMonoHelper.transform.SetParent(transform);

            WebRequestMG.SetWebRequestHelper(webRequestHelper.AddComponent<WebRequestMonoHelper>());
            WebRequestMG.SetWebDownloadHelper(webDownloadMonoHelper.AddComponent<WebDownloadMonoHelper>());
            #endregion

            #region GameState 初始化状态，开启整个流程
            Assembly = typeof(GameMain).Assembly;
            StateMG.CreateStateContext(Assembly);
            yield return new WaitForEndOfFrame();
            StateMG.StartFirstState();
            #endregion

        }

        private void Update()
        {
            GameModuleProxy.Update();
        }

        private void FixedUpdate()
        {
            GameModuleProxy.FixedUpdate();
        }

        private void OnDestroy()
        {
            GameModuleProxy.ShutDownAll();
        }

        private void OnApplicationQuit()
        {
            GameModuleProxy.OnAppilicationQuit();
        }
    }
}

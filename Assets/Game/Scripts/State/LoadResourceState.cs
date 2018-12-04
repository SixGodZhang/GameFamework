//-----------------------------------------------------------------------
// <filename>LoadResourceState</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> ## </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/1 星期六 14:41:27# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameFramework.Taurus
{
    [GameState(GameStateType.Generanal, Desc: "加载资源模块")]
    public class LoadResourceState : GameState
    {
        #region 重写函数
        public override void OnEnter(params object[] parameters)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log("---------call LoadResourceState------------");
#endif
            base.OnEnter(parameters);
            string localPath = Path.Combine(ResourceManager.GetDeafultPath(PathType.ReadOnly), "AssetVersion.txt");
            AssetBundleVersionInfo versionInfo = JsonUtility.FromJson<AssetBundleVersionInfo>(File.ReadAllText(localPath));

            //设置AB包的加载方式
            GameMain.ResourceMG.SetResourceHelper(new BundleResourceHelper());
            //设置资源依赖
            GameMain.ResourceMG.SetMainfestAssetBundle(versionInfo.ManifestAssetBundle, versionInfo.IsEncrypt);
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

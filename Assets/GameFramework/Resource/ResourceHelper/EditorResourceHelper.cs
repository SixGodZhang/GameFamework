//-----------------------------------------------------------------------
// <filename>EditorResourceHelper</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #在UnityEditor下记载资源# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/11/30 星期五 16:02:24# </time>
//-----------------------------------------------------------------------

#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameFramework.Taurus
{
    public class EditorResourceHelper : IResourceHelper
    {
        #region 重载函数
        public void Clear()
        {
            
        }

        public T LoadAsset<T>(string assetbundlename, string assetname) where T : UnityEngine.Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(assetname);
        }

        public void LoadAssetAsyn<T>(string assetbundlename, string assetname, Action<string, UnityEngine.Object> asyncallback) where T : UnityEngine.Object
        {
            UnityEngine.Object res = AssetDatabase.LoadAssetAtPath<T>(assetname);
            asyncallback.Invoke(assetname,res);
        }

        public AsyncOperation LoadSceneAsync(string assetbundlename, string scenename, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
        {
            if (loadSceneMode == LoadSceneMode.Additive)
                return EditorApplication.LoadLevelAdditiveAsyncInPlayMode(scenename);
            else
                return EditorApplication.LoadLevelAsyncInPlayMode(scenename);
        }

        public void SetResourcePath(PathType type, string rootAssetBundle = "", bool isEncrypt = false)
        {
            throw new NotImplementedException();
        }

        public void UnloadAsset(string assetbundlename, bool unload = false)
        {
            throw new NotImplementedException();
        }

        public AsyncOperation UnloadSceneAsync(string scenename)
        {
            return SceneManager.UnloadSceneAsync(scenename);
        }
        #endregion
    }
}
#endif

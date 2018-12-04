//-----------------------------------------------------------------------
// <filename>IResourceHelper</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> ## </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/11/30 星期五 15:36:15# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameFramework.Taurus
{
    public interface IResourceHelper
    {
        /// <summary>
        /// 设置资源加载的路径
        /// </summary>
        /// <param name="type"></param>
        /// <param name="rootAssetBundle">ab 的根路径,同目录下有一个Mainfest文件</param>
        /// <param name="isEncrypt"></param>
        void SetResourcePath(PathType type, string rootAssetBundle = "AssetBundles", bool isEncrypt = false);

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetbundlename"></param>
        /// <param name="assetname"></param>
        /// <returns></returns>
        T LoadAsset<T>(string assetbundlename, string assetname) where T : UnityEngine.Object;

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetbundlename"></param>
        /// <param name="assetname"></param>
        /// <param name="asyncallback">回调函数</param>
        void LoadAssetAsyn<T>(string assetbundlename, string assetname, Action<string,UnityEngine.Object> asyncallback) where T : UnityEngine.Object;

        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="assetbundlename"></param>
        /// <param name="unload"></param>
        void UnloadAsset(string assetbundlename, bool unload = false);

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="assetbundlename"></param>
        /// <param name="scenename"></param>
        /// <param name="loadSceneMode"></param>
        /// <returns></returns>
        AsyncOperation LoadSceneAsync(string assetbundlename, string scenename, LoadSceneMode loadSceneMode = LoadSceneMode.Additive);

        /// <summary>
        /// 异步卸载场景
        /// </summary>
        /// <param name="scenename"></param>
        /// <returns></returns>
        AsyncOperation UnloadSceneAsync(string scenename);

        /// <summary>
        /// 清理资源
        /// </summary>
        void Clear();
    }
}

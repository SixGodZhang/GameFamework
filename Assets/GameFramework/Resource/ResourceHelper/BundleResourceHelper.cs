//-----------------------------------------------------------------------
// <filename>BundleResourceHelper</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> ## </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/1 星期六 12:56:11# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameFramework.Taurus
{
    public class BundleResourceHelper : IResourceHelper
    {
        #region 字段&属性
        private string _readPath;
        private AssetBundleManifest _mainfest;
        /// <summary>
        /// 所有资源和它的AssetBundle，多对一关系
        /// </summary>
        private static Dictionary<string, AssetBundle> _allAssets = new Dictionary<string, AssetBundle>();
        /// <summary>
        /// AssetBundle和它的依赖项
        /// </summary>
        private static Dictionary<string, KeyValuePair<AssetBundle, string[]>> _allAssetBundles = new Dictionary<string, KeyValuePair<AssetBundle, string[]>>();
        #endregion

        #region 重载函数
        public void Clear()
        {
            foreach (var item in _allAssets.Values)
            {
                if(item!=null)
                    item.Unload(true);
            }

            _allAssets.Clear();
            _allAssetBundles.Clear();
            _mainfest = null;
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <!--
        /// desc:
        /// 资源加载流程:
        /// 1. 根据assetbundlename,先加载assetbundle
        /// 2. 找到assetbundle的所有依赖项
        /// 3. 将所有依赖项加载进来
        /// 4. 最后从当前的assetbundle中加载资源
        /// -->
        /// <typeparam name="T"></typeparam>
        /// <param name="assetbundlename"></param>
        /// <param name="assetname"></param>
        /// <returns></returns>
        public T LoadAsset<T>(string assetbundlename, string assetname) where T : UnityEngine.Object
        {
            assetname = assetname.ToLower();
            AssetBundle bundle;
            if (!_allAssets.TryGetValue(assetname, out bundle))
            {
                string bundlePath = Path.Combine(_readPath, assetbundlename);
                if (!File.Exists(bundlePath))
                    throw new Exception("AssetBundle not found!" + bundlePath);
                bundle = LoadAssetBundle(bundlePath);

                //获取bundle所有的依赖项
                string[] assetNames = bundle.isStreamedSceneAssetBundle ? bundle.GetAllScenePaths() : bundle.GetAllAssetNames();

                foreach (var name in assetNames)
                {
                    if (!_allAssets.ContainsKey(name))
                        _allAssets.Add(name, bundle);
                }

                _allAssetBundles[assetname] = new KeyValuePair<AssetBundle, string[]>(bundle, assetNames);
            }

            //加载依赖项
            LoadDependenciesAssetBundle(assetbundlename);
            //加载资源
            T asset = bundle.LoadAsset<T>(assetname);

            return asset;
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="assetbundlename">assetbundle名称</param>
        /// <param name="assetname">asset名称</param>
        /// <param name="asyncallback">异步回调函数</param>
        public void LoadAssetAsyn<T>(string assetbundlename, string assetname, Action<string, UnityEngine.Object> asyncallback) where T : UnityEngine.Object
        {
            assetname = assetname.ToLower();
            AssetBundle assetBundle;
            if (!_allAssets.TryGetValue(assetname,out assetBundle))
            {
                string assetBundlePath = Path.Combine(_readPath, assetbundlename);
                //异步加载AssetBundle
                AssetBundleCreateRequest createRequest = LoadAssetBundleAsyn(assetBundlePath);

                createRequest.completed += (operation) =>
                 {
                     //取得assetbundle
                     assetBundle = createRequest.assetBundle;
                     try
                     {
                         //获取assetbundle包含的所有资源
                         string[] assetNames = assetBundle.isStreamedSceneAssetBundle ? assetBundle.GetAllScenePaths() : assetBundle.GetAllAssetNames();
                         foreach (var item in assetNames)
                         {
                             if (!_allAssets.ContainsKey(item))
                                 _allAssets.Add(item, assetBundle);
                         }

                         //存放assetbundle
                         _allAssetBundles[assetname] = new KeyValuePair<AssetBundle, string[]>(assetBundle, assetNames);

                         //同步加载assetbundle的所有依赖项
                         LoadDependenciesAssetBundle(assetbundlename);

                         //异步加载资源
                         AssetBundleRequest requestAsset = assetBundle.LoadAssetAsync<T>(assetname);
                         requestAsset.completed += (asyncoperation) =>
                         {
                             //资源加载完成时的回调
                             asyncallback.Invoke(assetname, requestAsset.asset);
                         };
                     }
                     catch (GameException ex)
                     {
                         asyncallback.Invoke(assetname, null);
#if UNITY_EDITOR
                         Debug.LogError(ex.ToString() + "\n" + ex.StackTrace);
#endif
                     }
                 };
            }
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="assetbundlename"></param>
        /// <param name="scenename"></param>
        /// <param name="loadSceneMode"></param>
        /// <returns></returns>
        public AsyncOperation LoadSceneAsync(string assetbundlename, string scenename, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
        {
            AsyncOperation asyncOperation = null;
            try
            {
                string assetbundlePath = Path.Combine(_readPath, assetbundlename);
                AssetBundleCreateRequest createRequest = LoadAssetBundleAsyn(assetbundlePath);
                createRequest.completed += (operation) =>
                  {
                      AssetBundle assetBundle = createRequest.assetBundle;
                      LoadDependenciesAssetBundle(assetbundlename);

                      asyncOperation = SceneManager.LoadSceneAsync(scenename, loadSceneMode);
                      asyncOperation.completed += (operation1) =>
                        {
                            assetBundle.Unload(false);
                        };
                  };
            }
            catch (GameException ex)
            {
#if UNITY_EDITOR
                Debug.LogError(ex.ToString() + "\n" + ex.StackTrace);
#endif
            }

            return asyncOperation;
        }

        /// <summary>
        /// 设置资源的根路径(并加载Mainfest,包含所有资源的依赖关系)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="rootAssetBundle"></param>
        /// <param name="isEncrypt"></param>
        public void SetResourcePath(PathType type, string rootAssetBundle = "AssetBundles", bool isEncrypt = false)
        {
            _readPath = ResourceManager.GetDeafultPath(type);

            string rootABPath = Path.Combine(_readPath, rootAssetBundle);
            _readPath = Path.GetDirectoryName(rootABPath);

            LoadPlatformMainfest(rootABPath);
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="assetbundlename"></param>
        /// <param name="unload">是否卸载资源内存</param>
        public void UnloadAsset(string assetbundlename, bool unload = false)
        {
            KeyValuePair<AssetBundle, string[]> assetBundles;
            if (_allAssetBundles.TryGetValue(assetbundlename, out assetBundles))
            {
                if (!unload)
                    assetBundles.Key.Unload(false);
                else
                {
                    foreach (var item in assetBundles.Value)
                    {
                        if (_allAssets.ContainsKey(item))
                            _allAssets.Remove(item);
                    }
                    assetBundles.Key.Unload(true);
                }
            }
        }

        /// <summary>
        /// 异步卸载场景
        /// </summary>
        /// <param name="scenename"></param>
        /// <returns></returns>
        public AsyncOperation UnloadSceneAsync(string scenename)
        {
            return UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scenename);
        }


        #endregion

        #region 内部函数
        /// <summary>
        /// 异步加载AssetBundle
        /// </summary>
        /// <param name="assetbundlename"></param>
        /// <returns></returns>
        private AssetBundleCreateRequest LoadAssetBundleAsyn(string path)
        {
            if (!File.Exists(path))
                throw new Exception("assetbundle not found! \n" + path);

            //若assetbundle加密,此处应该有解密
            //TODO
            //...
            return AssetBundle.LoadFromFileAsync(path);
        }

        /// <summary>
        /// 加载assetbundle的所有依赖项
        /// </summary>
        /// <param name="assetbundlename"></param>
        private void LoadDependenciesAssetBundle(string assetbundlename)
        {
            string[] dependencies = _mainfest.GetAllDependencies(assetbundlename);

            foreach (var item in dependencies)
            {
                if (_allAssetBundles.ContainsKey(item))
                    continue;
                string depenciesBundlePath = Path.Combine(_readPath, item);
                AssetBundle bundle = LoadAssetBundle(depenciesBundlePath);

                //获取bundle所有的依赖项
                string[] assetNames = bundle.isStreamedSceneAssetBundle? bundle.GetAllScenePaths():bundle.GetAllAssetNames();

                foreach (var name in assetNames)
                {
                    if (!_allAssets.ContainsKey(name))
                        _allAssets.Add(name, bundle);
                }

                _allAssetBundles[item] = new KeyValuePair<AssetBundle, string[]>(bundle, assetNames);
            }
        }

        /// <summary>
        /// 加载Mainfest
        /// </summary>
        /// <param name="rootABPath"></param>
        private void LoadPlatformMainfest(string rootABPath)
        {
            AssetBundle bundle = LoadAssetBundle(rootABPath);
            _mainfest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            bundle.Unload(false);
        }

        /// <summary>
        /// 加载AssetBundle
        /// </summary>
        /// <param name="rootABPath"></param>
        /// <returns></returns>
        private AssetBundle LoadAssetBundle(string rootABPath)
        {
            if (!File.Exists(rootABPath))
                throw new Exception("asset bundle not found!+\n" + rootABPath);
            return AssetBundle.LoadFromFile(rootABPath);
        }
        #endregion
    }
}

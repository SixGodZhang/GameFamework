//-----------------------------------------------------------------------
// <filename>ResourceManager</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> ## </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/11/30 星期五 15:34:23# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameFramework.Taurus
{
    public class ResourceManager : IGameModule, IUpdate
    {
        #region 字段&属性
        /// <summary>
        /// 事件管理模块
        /// </summary>
        private EventManager _event;

        /// <summary>
        /// 操作资源的对象
        /// </summary>
        private IResourceHelper _resourceHelper;
        /// <summary>
        /// 操作对象池的对象
        /// </summary>
        private IGameObjectPoolHelper _gameObjectPoolHelper;

        /// <summary>
        /// 资源更新的类型(外部使用)
        /// </summary>
        public ResourceUpdateType ResUpdateType = ResourceUpdateType.Local;

        /// <summary>
        /// 资源更新的路径(外部使用)
        /// </summary>
        public string ResUpdatePath = "127.0.0.1:45678/Static";

        /// <summary>
        /// 资源本地路径类型
        /// </summary>
        public PathType LocalPathType = PathType.ReadOnly;

        /// <summary>
        /// 场景异步加载
        /// </summary>
        private Dictionary<string, AsyncOperation> _sceneAsyncOperations;

        /// <summary>
        /// 资源本地路径
        /// </summary>
        public string LocalPath
        {
            get
            {
                return GetDeafultPath(LocalPathType);
            }
        }
        #endregion

        #region 构造函数
        public ResourceManager()
        {
            _event = GameModuleProxy.GetModule<EventManager>();
            _sceneAsyncOperations = new Dictionary<string, AsyncOperation>();
        }
        #endregion

        #region 外部接口
        /// <summary>
        /// 获取Unity内置的一些路径
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetDeafultPath(PathType type)
        {
            string path = string.Empty;
            switch (type)
            {
                case PathType.ReadOnly:
                    path = Application.streamingAssetsPath;
                    break;
                case PathType.ReadWrite:
                    path = Application.persistentDataPath;
                    break;
                case PathType.Root:
                    path = Application.dataPath;
                    break;
                case PathType.TemporaryCache:
                    path = Application.temporaryCachePath;
                    break;
                default:
                    path = Application.streamingAssetsPath;
                    break;
            }

            return path;
        }

        /// <summary>
        /// 设置资源管理器
        /// </summary>
        /// <param name="helper"></param>
        public void SetResourceHelper(IResourceHelper helper)
        {
            _resourceHelper = helper;
        }

        /// <summary>
        /// 设置对象池管理器
        /// </summary>
        /// <param name="helper"></param>
        public void SetGameObjectPoolHelper(IGameObjectPoolHelper helper)
        {
            _gameObjectPoolHelper = helper;
        }


        #region 对象池的相关操作
        /// <summary>
        /// 加载预设信息
        /// </summary>
        /// <param name="assetbundlename"></param>
        /// <param name="assetname"></param>
        /// <param name="poolPrefabInfo"></param>
        public void LoadPrefabInfo(string assetbundlename, string assetname, PoolPrefabInfo poolPrefabInfo)
        {
            _gameObjectPoolHelper.PushPrefab(assetbundlename, assetname, poolPrefabInfo);
        }

        /// <summary>
        /// 在对象池中生成预设
        /// </summary>
        /// <param name="assetname"></param>
        /// <returns></returns>
        public GameObject SpawnInPool(string assetname)
        {
            return _gameObjectPoolHelper.Spwan(assetname);
        }

        /// <summary>
        /// 对象池中是否包含该预设
        /// </summary>
        /// <param name="assetname"></param>
        /// <returns></returns>
        public bool HasPrefabInfo(string assetname)
        {
            return _gameObjectPoolHelper.HasPrefab(assetname);
        }

        /// <summary>
        /// 移除场景中的物体
        /// </summary>
        /// <param name="go"></param>
        /// <param name="isDestroy">true:放入对象池中 false:直接销毁</param>
        public void DespawnInPool(GameObject go, bool isDestroy)
        {
            _gameObjectPoolHelper.Despwan(go,isDestroy);
        }

        /// <summary>
        /// 将场景中从对象池中创建的Gameobject全部放回对象池中
        /// </summary>
        public void DespawnAll()
        {
            _gameObjectPoolHelper.DespwanAll();
        }

        /// <summary>
        /// 将场景中从对对象池中创建的GAmeObject全部销毁
        /// </summary>
        public void DestroyAll()
        {
            _gameObjectPoolHelper.DestroyAll();
        }

        /// <summary>
        /// 将场景中的某一类型的预设全部放入对象池中
        /// </summary>
        /// <param name="assetname"></param>
        public void DespawnPrefab(string assetname)
        {
            _gameObjectPoolHelper.DespawnPrefab(assetname);
        }

        #endregion

        #endregion

        #region 资源加载相关接口

        /// <summary>
        /// 设置Mainfest
        /// </summary>
        /// <param name="mainfestname"></param>
        /// <param name="isEncrypt"></param>
        public void SetMainfestAssetBundle(string mainfestname, bool isEncrypt = false)
        {
            _resourceHelper?.SetResourcePath(LocalPathType, mainfestname, isEncrypt);
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetbundlename"></param>
        /// <param name="assetname"></param>
        /// <returns></returns>
        public T LoadAsset<T>(string assetbundlename, string assetname) where T : UnityEngine.Object
        {
            return _resourceHelper?.LoadAsset<T>(assetbundlename, assetname);
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetbundlename"></param>
        /// <param name="assetname"></param>
        public void LoadAssetAsyn<T>(string assetbundlename, string assetname) where T : UnityEngine.Object
        {
            _resourceHelper?.LoadAssetAsyn<T>(assetbundlename, assetname, LoadAssetAsyncCallback);
        }

        /// <summary>
        /// 卸载AssetBUndle
        /// </summary>
        /// <param name="assetbundlename"></param>
        /// <param name="unload"></param>
        public void UnloadAssetBundle(string assetbundlename, bool unload = false)
        {
            _resourceHelper?.UnloadAsset(assetbundlename,unload);
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
            AsyncOperation asyncOperation = _resourceHelper?.LoadSceneAsync(assetbundlename, scenename, loadSceneMode);
            _sceneAsyncOperations.Add(scenename, asyncOperation);
            return asyncOperation;
        }

        /// <summary>
        /// 异步卸载场景
        /// </summary>
        /// <param name="scenename"></param>
        public void UnloadSceneAsync(string scenename)
        {
            _resourceHelper?.UnloadSceneAsync(scenename);
        }

        #endregion

        #region 重载函数
        /// <summary>
        /// 资源模块被卸载时调用的函数
        /// </summary>
        public void OnClose()
        {
            if (_resourceHelper != null)
                _resourceHelper.Clear();
            if (_gameObjectPoolHelper != null)
                _gameObjectPoolHelper.DestroyAll();
        }

        public void OnUpdate()
        {
            if (_sceneAsyncOperations.Count <= 0)
                return;

            foreach (var item in _sceneAsyncOperations)
            {
                LoadSceneAsynEventArgs args = new LoadSceneAsynEventArgs();
                args.SceneName = item.Key;
                args.Progress = item.Value.progress;
                _event.CallEvent(this, EventType.LoadSceneAsync, args);
            }
        }
        #endregion

        #region 内部函数
        /// <summary>
        /// 异步函数回调
        /// </summary>
        /// <param name="assetname"></param>
        /// <param name="instance"></param>
        private void LoadAssetAsyncCallback(string assetname, UnityEngine.Object instance)
        {
            if (_event == null)
                return;
            LoadAssetAsynEventArgs args = new LoadAssetAsynEventArgs();
            args.AssetName = assetname;
            args.Asset = instance;
            _event.CallEvent(this, EventType.LoadAssetAsync, args);
        }
        #endregion
    }

    /// <summary>
    /// 路径类型
    /// </summary>
    public enum PathType
    {
        /// <summary>
        /// 只读路径
        /// </summary>
        ReadOnly,
        /// <summary>
        /// 持久化路径(可读可写)
        /// </summary>
        ReadWrite,
        /// <summary>
        /// 应用程序根目录
        /// </summary>
        Root,
        /// <summary>
        /// 缓存目录
        /// </summary>
        TemporaryCache

    }

    /// <summary>
    /// 资源更新的类型
    /// </summary>
    public enum ResourceUpdateType
    {
        /// <summary>
        /// 从远程更新资源下来,然后通过AB方式加载
        /// </summary>
        Update,
        /// <summary>
        /// 加载本地AB文件
        /// </summary>
        Local,
        /// <summary>
        /// 直接选择加载资源文件
        /// </summary>
        Editor
    }
}

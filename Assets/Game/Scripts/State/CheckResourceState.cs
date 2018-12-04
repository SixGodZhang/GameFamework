//-----------------------------------------------------------------------
// <filename>CheckResourceState</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> ## </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/3 星期一 17:42:15# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameFramework.Taurus
{
    [GameState(GameStateType.Generanal, Desc: "资源监测")]
    public class CheckResourceState : GameState
    {
        #region 字段&属性
        /// <summary>
        /// 平台的资源版本
        /// </summary>
        private string _assetPlatformVersionText = "AssetPlatformVersion.txt";

        /// <summary>
        /// 资源版本
        /// </summary>
        private string _assetVersionTxt = "AssetVersion.txt";

        /// <summary>
        /// 本地版本信息
        /// </summary>
        private AssetBundleVersionInfo _localVersion;

        /// <summary>
        /// 远程版本信息
        /// </summary>
        private AssetBundleVersionInfo _remoteVersion;

        /// <summary>
        /// 资源更新完成
        /// </summary>
        private bool _resourceUpdateDone;

        /// <summary>
        /// 下载的资源列表
        /// </summary>
        private Dictionary<string, string> _downloadResources;
        #endregion

        #region 重载函数
        public override void OnEnter(params object[] parameters)
        {
            base.OnEnter(parameters);
            //注册监听
            GameMain.EventMG.RegisterEvent(EventType.HttpReadTextSuccess, OnHttpReadTextSuccess);
            GameMain.EventMG.RegisterEvent(EventType.HttpReadTextFailure, OnHttpReadTextFailure);
            GameMain.EventMG.RegisterEvent(EventType.HttpDownLoadSuccess, OnHttpDownLoadSuccess);
            GameMain.EventMG.RegisterEvent(EventType.HttpDownLoadFailure, OnHttpDownLoadFailure);
            GameMain.EventMG.RegisterEvent(EventType.HttpDownLoadProgress, OnHttpDownLoadProgress);

            _localVersion = LoadLocalVersion();
            LoadRemoteVersion();
        }

        public override void OnExit()
        {
            GameMain.EventMG.RemoveEvent(EventType.HttpReadTextSuccess, OnHttpReadTextSuccess);
            GameMain.EventMG.RemoveEvent(EventType.HttpReadTextFailure, OnHttpReadTextFailure);
            GameMain.EventMG.RemoveEvent(EventType.HttpDownLoadSuccess, OnHttpDownLoadSuccess);
            GameMain.EventMG.RemoveEvent(EventType.HttpDownLoadFailure, OnHttpDownLoadFailure);
            GameMain.EventMG.RemoveEvent(EventType.HttpDownLoadProgress, OnHttpDownLoadProgress);
            base.OnExit();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
        }

        public override void OnInit(GameStateContext context)
        {
            base.OnInit(context);
            _downloadResources = new Dictionary<string, string>();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            //更新资源
            if (_resourceUpdateDone && _downloadResources.Count == 0)
            {
                UpdateAssetVersionTxt();
                ChangeState<LoadResourceState>();
            }
        }

        #endregion

        #region 内部函数
        /// <summary>
        /// 更新资源版本信息
        /// </summary>
        private void UpdateAssetVersionTxt()
        {
            if (!CompareVersion())
            {
                string localPath = Path.Combine(GameMain.ResourceMG.LocalPath, _assetVersionTxt);
                File.WriteAllText(localPath, JsonUtility.ToJson(_remoteVersion));
            }
        }

        /// <summary>
        /// 比较资源版本
        /// </summary>
        /// <returns></returns>
        private bool CompareVersion()
        {
            return _localVersion != null && _remoteVersion.Version == _localVersion.Version;
        }

        /// <summary>
        /// 加载本地版本
        /// </summary>
        /// <returns></returns>
        private AssetBundleVersionInfo LoadLocalVersion()
        {
            string localPath = Path.Combine(GameMain.ResourceMG.LocalPath, _assetVersionTxt);
            if (!File.Exists(localPath))
                return null;
            string content = File.ReadAllText(localPath);
            return JsonUtility.FromJson<AssetBundleVersionInfo>(content);
        }

        /// <summary>
        /// 获取远程资源版本
        /// </summary>
        private void LoadRemoteVersion()
        {
            string remotePath = Path.Combine(GameMain.ResourceMG.ResUpdatePath, _assetPlatformVersionText);
            GameMain.WebRequestMG.ReadHttpText(remotePath);
        }
        #endregion

        #region 监听函数
        /// <summary>
        /// 下载进度监听
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void OnHttpDownLoadProgress(object sender, IEventArgs e)
        {
            DownloadProgressEventArgs args = (DownloadProgressEventArgs)e;
#if UNITY_EDITOR
            Debug.Log(
    $"path:{args.LocalPath} progress:{args.DownloadProgress} bytes:{args.DownloadBytes} speed:{args.DownloadSpeed}");
#endif
        }

        /// <summary>
        /// 下载资源失败监听
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void OnHttpDownLoadFailure(object sender, IEventArgs e)
        {
#if UNITY_EDITOR
            HttpDownloadEventArgs args = (HttpDownloadEventArgs)e;
            if (args != null)
                UnityEngine.Debug.LogError("下载资源失败! \n Url: " + args.Url);
#endif
        }

        /// <summary>
        /// 下载资源成功监听
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void OnHttpDownLoadSuccess(object sender, IEventArgs e)
        {
            HttpDownloadEventArgs args = (HttpDownloadEventArgs)e;
            if (_downloadResources.ContainsKey(args.Url))
                _downloadResources.Remove(args.Url);
        }

        /// <summary>
        /// 下载版本信息文本失败监听
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void OnHttpReadTextFailure(object sender, IEventArgs e)
        {
#if UNITY_EDITOR
            HttpReadTextEventArgs args = (HttpReadTextEventArgs)e;
            if (args != null)
                UnityEngine.Debug.LogError("下载文本失败! \n Url: " + args.Url + "\n 详细信息: " + args.Additive);
#endif
        }

        /// <summary>
        /// 下载版本信息文本成功监听
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void OnHttpReadTextSuccess(object sender, IEventArgs e)
        {
            HttpReadTextEventArgs args = (HttpReadTextEventArgs)e;
            if (args != null)
            {
                if (args.Url == Path.Combine(GameMain.ResourceMG.ResUpdatePath, _assetPlatformVersionText))
                {
                    AssetPlatformVersionInfo assetPlatform = JsonUtility.FromJson<AssetPlatformVersionInfo>(args.Additive);
                    string platformName = GetPlatformName();
                    if (assetPlatform.Platforms.Contains(platformName))
                    {
                        //资源的更新路径
                        GameMain.ResourceMG.ResUpdatePath = Path.Combine(GameMain.ResourceMG.ResUpdatePath, platformName);

                        //读取远程的文本
                        string remotePath = Path.Combine(GameMain.ResourceMG.ResUpdatePath, _assetVersionTxt);
                        GameMain.WebRequestMG.ReadHttpText(remotePath);
                    }
                } else if (args.Url == Path.Combine(GameMain.ResourceMG.ResUpdatePath,_assetVersionTxt))
                {
                    _remoteVersion = JsonUtility.FromJson<AssetBundleVersionInfo>(args.Additive);
                    if (_remoteVersion == null)
                    {
#if UNITY_EDITOR
                        UnityEngine.Debug.LogError("Remote version is null.");
#endif
                        return;
                    }

                    if (!CompareVersion())
                    {
                        //更新资源
                        UpdateResource();
                        //下载资源
                        DownloadResource();
                    }

                    //资源更新完成
                    _resourceUpdateDone = true;
                }
            }
        }

        /// <summary>
        /// 下载资源
        /// </summary>
        private void DownloadResource()
        {
            foreach (var item in _downloadResources)
            {
                GameMain.WebRequestMG.StartDownload(item.Key, item.Value);
            }
        }

        /// <summary>
        /// 更新资源
        /// </summary>
        private void UpdateResource()
        {
            string remoteUrl = "";
            string localPath = "";
            string localDir = "";

            foreach (var item in _remoteVersion.Resources)
            {
                if (_localVersion != null && _localVersion.Resources != null && _localVersion.Resources.Contains(item))
                    continue;
                remoteUrl = Path.Combine(GameMain.ResourceMG.ResUpdatePath, item.Name);
                //获取本地文件路径
                localPath = Path.Combine(GameMain.ResourceMG.LocalPath, item.Name);

                localDir = Path.GetDirectoryName(localPath);
                if (!Directory.Exists(localDir))
                    Directory.CreateDirectory(localDir);

                _downloadResources.Add(remoteUrl, localPath);
            }
        }

        /// <summary>
        /// 获取当前平台名称
        /// </summary>
        /// <returns></returns>
        private string GetPlatformName()
        {
            string platformName = "StandaloneWindows";
#if UNITY_IOS
                platformName = "IOS";
#elif UNITY_ANDROID
                platformName = "Android";
#elif UNITY_STANDALONE_OSX
                platformName = "StandaloneOSX";
#endif
            return platformName.ToLower();
        }
        #endregion
    }
}

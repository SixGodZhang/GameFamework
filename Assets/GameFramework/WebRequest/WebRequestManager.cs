//-----------------------------------------------------------------------
// <filename>WebRequestManager</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> ## </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/3 星期一 18:32:42# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.Taurus
{
    public class WebRequestManager:IGameModule
    {
        #region 属性&字段
        /// <summary>
        /// 事件管理器
        /// </summary>
        private EventManager _event;

        /// <summary>
        /// 网页请求帮助类
        /// </summary>
        private IWebRequestHelper _webRequestHelper;

        /// <summary>
        /// Http下载帮助类
        /// </summary>
        private IWebDownloadHelper _webDownloadHelper;
        #endregion

        #region 构造函数
        public WebRequestManager()
        {
            _event = GameModuleProxy.GetModule<EventManager>();
        }
        #endregion

        #region 外部接口
        /// <summary>
        /// 设置Web请求帮助器的类型
        /// </summary>
        /// <param name="helper"></param>
        public void SetWebRequestHelper(IWebRequestHelper helper)
        {
            _webRequestHelper = helper;
        }

        /// <summary>
        /// 设置Http下载帮助器的类型
        /// </summary>
        /// <param name="helper"></param>
        public void SetWebDownloadHelper(IWebDownloadHelper helper)
        {
            _webDownloadHelper = helper;
        }

        /// <summary>
        /// 读取Http文本
        /// </summary>
        /// <param name="url"></param>
        public void ReadHttpText(string url)
        {
            _webRequestHelper?.ReadHttpText(url, ReadHttpTextCallback);
        }

        /// <summary>
        /// 下载资源
        /// </summary>
        /// <param name="remoteUrl"></param>
        /// <param name="localPath"></param>
        public void StartDownload(string remoteUrl, string localPath)
        {
            _webDownloadHelper?.StartDownLoad(remoteUrl, localPath, StartDownloadCallback, StartDownloadProgress);
        }
        #endregion

        #region 内部函数
        /// <summary>
        /// 回调函数,读取Http的文本
        /// </summary>
        /// <param name="path">Http读取的远程路径</param>
        /// <param name="result">读取结果</param>
        /// <param name="content">读取的文本内容</param>
        private void ReadHttpTextCallback(string path, bool result, string content)
        {
            HttpReadTextEventArgs httpReadTextEventArgs = new HttpReadTextEventArgs();
            httpReadTextEventArgs.Url = path;
            httpReadTextEventArgs.Additive = content;
            if (result)
                _event.CallEvent(this, EventType.HttpReadTextSuccess, httpReadTextEventArgs);
            else
                _event.CallEvent(this, EventType.HttpReadTextFailure, httpReadTextEventArgs);
        }

        /// <summary>
        /// 回调函数,下载资源进度
        /// </summary>
        /// <param name="remoteUrl">远程下载URL</param>
        /// <param name="localPath">本地路径</param>
        /// <param name="dataLength">数据长度</param>
        /// <param name="progess">下载进度</param>
        /// <param name="seconds">下载时间</param>
        private void StartDownloadProgress(string remoteUrl, string localPath, ulong dataLength, float progess, float seconds)
        {
            DownloadProgressEventArgs downloadProgressEventArgs = new DownloadProgressEventArgs();
            downloadProgressEventArgs.Url = remoteUrl;
            downloadProgressEventArgs.LocalPath = localPath;
            downloadProgressEventArgs.DownloadBytes = dataLength;
            downloadProgressEventArgs.DownloadProgress = progess;
            downloadProgressEventArgs.DownloadSeconds = seconds;
            downloadProgressEventArgs.DownloadSpeed = dataLength == 0.0f ? dataLength : dataLength / 1024.0f / seconds;
            _event.CallEvent(this, EventType.HttpDownLoadProgress, downloadProgressEventArgs);
        }

        /// <summary>
        /// 回调函数，下载资源
        /// </summary>
        /// <param name="remoteurl">下载资源地址</param>
        /// <param name="localpath">本地存储地址</param>
        /// <param name="result">下载结果</param>
        /// <param name="content">附加信息(包含错误信息等)</param>
        private void StartDownloadCallback(string remoteurl, string localpath, bool result, string content)
        {
            HttpDownloadEventArgs httpDownloadEventArgs = new HttpDownloadEventArgs();
            httpDownloadEventArgs.Url = remoteurl;
            httpDownloadEventArgs.LocalPath = localpath;
            httpDownloadEventArgs.Error = content;
            if (result)
                _event.CallEvent(this, EventType.HttpDownLoadSuccess, httpDownloadEventArgs);
            else
                _event.CallEvent(this, EventType.HttpDownLoadFailure, httpDownloadEventArgs);
        }

        #endregion

        #region 重载函数
        public void OnClose()
        {
            _event.OnClose();
            _event = null;
        }
        #endregion
    }
}

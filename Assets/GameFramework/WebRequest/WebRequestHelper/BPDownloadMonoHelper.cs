//-----------------------------------------------------------------------
// <filename>BPDownload</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #提供断点续传的接口# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/4 星期二 21:51:22# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace GameFramework.Taurus
{
    public class BPDownload:MonoBehaviour//,IWebDownloadHelper
    {
        #region 字段&属性
        private Dictionary<string, UnityWebRequest> _requests = new Dictionary<string, UnityWebRequest>();
        #endregion

        #region 重载函数
        public void StartDownLoad(string remoteUrl, string localPath ,DownloadFileType type, Action<string, string, bool, string> result, Action<string, string, ulong, float, float> progress)
        {
            StartCoroutine(BPDownloadResources(remoteUrl, localPath, result, progress));
        }
        #endregion

        #region 内部函数
        private IEnumerator BPDownloadResources(string remoteurl,string localpath, Action<string, string, bool, string> result, Action<string, string, ulong, float, float> progress)
        {
            yield return StartDownload(remoteurl, localpath, result, progress);
        }

        /// <summary>
        /// 开始下载
        /// </summary>
        /// <param name="url"></param>
        /// <param name="savepath"></param>
        /// <returns></returns>
        private UnityWebRequest StartDownload(string url ,string savepath, Action<string, string, bool, string> result, Action<string, string, ulong, float, float> progress)
        {
            BPDownloadHandler downloadHandler = new BPDownloadHandler(url,savepath);
            downloadHandler.CompleteDownloadEvent += result;
            downloadHandler.GetDownloadProgressEvent += progress;
            UnityWebRequest request = UnityWebRequest.Get(url);
            request.chunkedTransfer = true;
            request.disposeDownloadHandlerOnDispose = true;
            request.SetRequestHeader("Range", "bytes" + downloadHandler.DownedLength + "-");
            request.downloadHandler = downloadHandler;
            request.SendWebRequest();
            return request;
        }

        private void DownloadCompleteCallback(string msg)
        {
            Debug.Log("下载完成的回调");
        }

        /// <summary>
        /// 停止下载
        /// </summary>
        /// <param name="url"></param>
        private void StopDownload(UnityWebRequest request)
        {
            if (request.downloadHandler.GetType().IsAssignableFrom(typeof(BPDownloadHandler)))
            {
                (request.downloadHandler as BPDownloadHandler).Dispose();
                request.Abort();
                request.Dispose();
            }
        }
        #endregion
    }
}

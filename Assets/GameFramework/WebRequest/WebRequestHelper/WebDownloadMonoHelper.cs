//-----------------------------------------------------------------------
// <filename>WebDownloadMonoHelper</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> ## </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/3 星期一 19:56:18# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace GameFramework.Taurus
{
    public sealed class WebDownloadMonoHelper : MonoBehaviour, IWebDownloadHelper
    {
        #region 字段&属性
        /// <summary>
        /// 每次接收数据的大小
        /// </summary>
        private byte[] _bytes = new byte[2000];

        /// <summary>
        /// 异步请求操作
        /// </summary>
        private UnityWebRequestAsyncOperation asyncOperation = null;

        #endregion

        #region 重载函数
        public UnityWebRequestAsyncOperation StartDownLoad(string remoteUrl, string localPath , DownloadFileType type, Action<string, string, bool, string> result, Action<string, string, ulong, float, float> progress)
        {
            //StartCoroutine();//暂时考虑不使用协程
            StartCoroutine(UnityWebStartDownload(remoteUrl, localPath, type, result, progress));
            return asyncOperation;
        }
        #endregion

        #region 内部函数
        /// <summary>
        /// Unity下载资源
        /// </summary>
        /// <param name="remoteUrl"></param>
        /// <param name="localPath"></param>
        /// <param name="type"></param>
        /// <param name="result"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        IEnumerator UnityWebStartDownload(string remoteUrl, string localPath, DownloadFileType type, Action<string, string, bool, string> result, Action<string, string, ulong, float, float> progress)
        {
            UnityWebRequest request = UnityWebRequest.Get(remoteUrl);
            request.downloadHandler = new CustomDownloadHandlerFile(localPath, _bytes);
            asyncOperation =  request.SendWebRequest();

            long lastTicks = DateTime.Now.Ticks;

            while (!request.isDone)
            {
                float seconds = (DateTime.Now.Ticks - lastTicks) / 10000000.0f;
                progress.Invoke(remoteUrl, localPath, request.downloadedBytes, request.downloadProgress, seconds);
                yield return null;
            }

            if (request.responseCode == 200L)
            {
                result.Invoke(remoteUrl, localPath, true, "File Download success and save to " + localPath);
            }
            else
            {
                result.Invoke(remoteUrl, localPath, false, request.error);
            }

            //if (request.isNetworkError || request.isHttpError)
            //    result.Invoke(remoteUrl, localPath, false, "NetworkError: " + request.isNetworkError + " \t HttpError: " + request.isHttpError);
            //else
            //    result.Invoke(remoteUrl, localPath, true, "File Download success and save to " + localPath);
        } 
        #endregion
    }
}

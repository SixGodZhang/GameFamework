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
using UnityEngine;
using UnityEngine.Networking;

namespace GameFramework.Taurus
{
    public sealed class WebDownloadMonoHelper : MonoBehaviour, IWebDownloadHelper
    {
        #region 重载函数
        public void StartDownLoad(string remoteUrl, string localPath, Action<string, string, bool, string> result, Action<string, string, ulong, float, float> progress)
        {
            StartCoroutine(UnityWebStartDownload(remoteUrl, localPath, result, progress));
        }
        #endregion

        #region 内部函数
        IEnumerator UnityWebStartDownload(string remoteUrl, string localPath, Action<string, string, bool, string> result, Action<string, string, ulong, float, float> progress)
        {//断点续传 TODO
            UnityWebRequest request = UnityWebRequest.Get(remoteUrl);
            request.downloadHandler = new DownloadHandlerFile(localPath);
            request.SendWebRequest();

            long lastTicks = DateTime.Now.Ticks;

            while (request.isDone)
            {
                float seconds = (DateTime.Now.Ticks - lastTicks) / 10000000.0f;
                progress.Invoke(remoteUrl, localPath, request.downloadedBytes, request.downloadProgress, seconds);
                yield return null;
            }

            if (request.isNetworkError || request.isHttpError)
                result.Invoke(remoteUrl, localPath, false, "NetworkError: " + request.isNetworkError + " \t HttpError: " + request.isHttpError);
            else
                result.Invoke(remoteUrl, localPath, true, "File Download success and save to " + localPath);
        } 
        #endregion
    }
}

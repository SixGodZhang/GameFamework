//-----------------------------------------------------------------------
// <filename>WebRequestMonoHelper</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #Http请求文本# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/3 星期一 20:11:45# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections;
using UnityEngine;

namespace GameFramework.Taurus
{
    /// <summary>
    /// Unity Web 请求
    /// </summary>
    public class WebRequestMonoHelper : MonoBehaviour, IWebRequestHelper
    {
        #region 重载函数
        public void ReadHttpText(string url, Action<string, bool, string> result)
        {
            StartCoroutine(UnityWebRequest(url, result));
        }
        #endregion

        #region 内部函数
        /// <summary>
        /// Web请求下载文本
        /// </summary>
        /// <param name="url"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private IEnumerator UnityWebRequest(string url, Action<string, bool, string> result)
        {
            //url = url.Replace('/', '\\').Replace("\\\\", "\\");
#if UNITY_EDITOR
            Debug.Log("资源下载地址: " + url);
#endif
            yield return null;

            WWW request = new WWW(url);
            yield return request;
            while (!request.isDone)
            {
                yield return request;
            }

            if (request.error != null)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError("下载资源失败...");
#endif
                result.Invoke(url, false, request.error.ToString());
            }
            else
            {
                result.Invoke(url, true, request.text);
            }
                
        }
#endregion
    }
}

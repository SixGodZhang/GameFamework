//-----------------------------------------------------------------------
// <filename>IWebDownloadHelper</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #Http请求下载帮助类# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/3 星期一 18:37:02# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace GameFramework.Taurus
{
    public interface IWebDownloadHelper
    {
        /// <summary>
        /// 下载资源
        /// </summary>
        /// <param name="remoteUrl"></param>
        /// <param name="localPath"></param>
        /// <param name="result"></param>
        /// <param name="progress"></param>
        UnityWebRequestAsyncOperation StartDownLoad(string remoteUrl, string localPath, DownloadFileType type, Action<string, string, bool, string> result, Action<string, string, ulong, float, float> progress);
    }
}

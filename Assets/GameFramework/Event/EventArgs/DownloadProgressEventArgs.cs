//-----------------------------------------------------------------------
// <filename>DownloadProgressEventArgs</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #下载进度参数和信息# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/3 星期一 19:45:35# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.Taurus
{
    public class DownloadProgressEventArgs:GameEventArgs<DownloadProgressEventArgs>
    {
        /// <summary>
        /// 下载地址
        /// </summary>
        public string Url;

        /// <summary>
        /// 本地路径
        /// </summary>
        public string LocalPath;

        /// <summary>
        /// 下载的字节数目
        /// </summary>
        public ulong DownloadBytes;

        /// <summary>
        /// 下载进度
        /// </summary>
        public float DownloadProgress;

        /// <summary>
        /// 下载时间
        /// </summary>
        public float DownloadSeconds;

        /// <summary>
        /// 下载速度
        /// </summary>
        public float DownloadSpeed;
    }
}

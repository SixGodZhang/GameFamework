//-----------------------------------------------------------------------
// <filename>HttpDownloadEventArgs</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #Http下载资源时的参数# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/3 星期一 19:02:09# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.Taurus
{
    public class HttpDownloadEventArgs:GameEventArgs<HttpDownloadEventArgs>
    {
        /// <summary>
        /// Http下载远程地址
        /// </summary>
        public string Url;

        /// <summary>
        /// 下载完成之后本地存储的路径
        /// </summary>
        public string LocalPath;

        /// <summary>
        /// 错误信息(如果出现错误)
        /// </summary>
        public string Error;
    }
}

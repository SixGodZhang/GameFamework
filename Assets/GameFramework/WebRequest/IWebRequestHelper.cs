//-----------------------------------------------------------------------
// <filename>IWebRequestHelper</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #网络请求的接口# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/3 星期一 18:34:05# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.Taurus
{
    public interface IWebRequestHelper
    {
        /// <summary>
        /// 读取网页请求的图片
        /// </summary>
        /// <param name="url"></param>
        /// <param name="result"></param>
        void ReadHttpText(string url, Action<string, bool, string> result);
    }
}

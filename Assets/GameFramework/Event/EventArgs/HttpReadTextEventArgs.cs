//-----------------------------------------------------------------------
// <filename>HttpReadTextSuccessEventArgs</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #Http下载文本文件参数# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/3 星期一 18:50:52# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.Taurus
{
    public class HttpReadTextEventArgs:GameEventArgs<HttpReadTextEventArgs>
    {
        /// <summary>
        /// 下载链接
        /// </summary>
        public string Url;

        /// <summary>
        /// 附加信息
        /// </summary>
        public string Additive;
    }
}

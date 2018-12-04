//-----------------------------------------------------------------------
// <filename>LoadAssetAsynEventArgs</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> ## </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/1 星期六 12:08:52# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.Taurus
{
    /// <summary>
    /// 异步加载资源回调函数的参数
    /// </summary>
    public class LoadAssetAsynEventArgs:GameEventArgs<IEventArgs>
    {
        /// <summary>
        /// 异步加载的资源名
        /// </summary>
        public string AssetName;

        /// <summary>
        /// 若加载成功则为加载的实例，否则为null
        /// </summary>
        public UnityEngine.Object Asset;
    }
}

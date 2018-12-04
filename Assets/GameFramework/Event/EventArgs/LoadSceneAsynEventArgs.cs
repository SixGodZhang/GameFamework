//-----------------------------------------------------------------------
// <filename>LoadSceneAsynEventArgs</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> ## </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/3 星期一 14:33:13# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.Taurus
{
    public class LoadSceneAsynEventArgs:GameEventArgs<LoadAssetAsynEventArgs>
    {
        /// <summary>
        /// 场景名称
        /// </summary>
        public string SceneName;

        /// <summary>
        /// 加载的进度
        /// </summary>
        public float Progress;
    }
}

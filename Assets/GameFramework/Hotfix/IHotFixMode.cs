//-----------------------------------------------------------------------
// <filename>IHotFixMode</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #逻辑代码的加载方式# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/6 星期四 14:33:03# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.Taurus
{
    public interface IHotFixMode
    {
        /// <summary>
        /// 进入逻辑代码
        /// </summary>
        /// <param name="rm"></param>
        void EnterHotfix(ResourceManager rm);

        void Start();

        void Update();

        void OnFixedUpdate();

        void OnDestory();

        void OnApplicationQuit();
    }
}

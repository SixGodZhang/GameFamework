//-----------------------------------------------------------------------
// <filename>IGameModule</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #游戏模块的接口及标志(模块必须继承此接口)# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/11/30 星期五 11:56:55# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.Taurus
{
    public interface IGameModule
    {
        /// <summary>
        ///  关闭当前模块
        /// </summary>
        void OnClose();
    }
}

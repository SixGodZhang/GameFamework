//-----------------------------------------------------------------------
// <filename>IGameLoop</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> ## </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/6 星期四 15:18:32# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.Taurus
{
    /// <summary>
    /// 固定帧接口
    /// </summary>
    public interface IFixedUpdate
    {
        void OnFixedUpdate();
    }

    /// <summary>
    /// 模块接口,一般Manager类会实现该接口
    /// </summary>
    public interface IGameModule
    {
        /// <summary>
        ///  关闭当前模块
        /// </summary>
        void OnClose();
    }

    /// <summary>
    /// 固定帧接口
    /// </summary>
    public interface IUpdate
    {
        void OnUpdate();
    }

    /// <summary>
    /// 退出函数
    /// </summary>
    public interface IApplicationQuit
    {
        void OnApplicationQuit();
    }
}

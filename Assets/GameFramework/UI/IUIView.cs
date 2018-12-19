//-----------------------------------------------------------------------
// <filename>IUIView</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> ## </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/5 星期三 16:50:13# </time>
//-----------------------------------------------------------------------

using ILRuntime.Other;
using System;
using System.Collections.Generic;

namespace GameFramework.Taurus
{
    [SingleInterfaceExport]
    public interface IUIView
    {
        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="parameters"></param>
        void OnEnter(params object[] parameters);

        /// <summary>
        /// 退出界面
        /// </summary>
        void OnExit();

        /// <summary>
        /// 暂停界面
        /// </summary>
        void OnPause();

        /// <summary>
        /// 恢复
        /// </summary>
        void OnResume();
    }
}

//-----------------------------------------------------------------------
// <filename>UIView</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #UIView 的抽象类# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/5 星期三 16:52:33# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Taurus
{
    public abstract class UIView : MonoBehaviour, IUIView
    {
        /// <summary>
        /// 进入界面
        /// </summary>
        /// <param name="parameters"></param>
        public abstract void OnEnter(params object[] parameters);

        /// <summary>
        /// 退出界面
        /// </summary>
        public abstract void OnExit();

        /// <summary>
        /// 暂停界面
        /// </summary>
        public abstract void OnPause();

        /// <summary>
        /// 恢复界面
        /// </summary>
        public abstract void OnResume();
    }
}

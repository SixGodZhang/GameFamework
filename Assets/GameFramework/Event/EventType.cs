//-----------------------------------------------------------------------
// <filename>EventType</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #定义事件类型# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/5 星期三 17:35:16# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.Taurus
{
    public enum EventType
    {
        HttpDownLoadSuccess = 1,
        HttpDownLoadFailure,
        LoadAssetAsync,
        LoadSceneAsync,
        HttpReadTextSuccess,
        HttpReadTextFailure,
        HttpDownLoadProgress,
        UIEnter,
        UIExit,
        UIPause,
        UIResume,
    }
}

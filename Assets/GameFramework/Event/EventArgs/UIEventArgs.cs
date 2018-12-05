//-----------------------------------------------------------------------
// <filename>UIEventArgs</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> ## </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/5 星期三 17:33:50# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.Taurus
{
    public class UIEventArgs:GameEventArgs<UIEventArgs>
    {
        public IUIView UIView;
    }
}

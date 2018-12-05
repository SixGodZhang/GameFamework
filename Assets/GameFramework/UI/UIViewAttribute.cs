//-----------------------------------------------------------------------
// <filename>UIViewAttribute</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #UI特性# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/5 星期三 16:54:58# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.Taurus
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UIViewAttribute:Attribute
    {
        public string AssetBundleName { get; private set; }
        public string ViewPath { get; private set; }

        public UIViewAttribute(string assetbundlename, string viewpath)
        {
            AssetBundleName = assetbundlename;
            ViewPath = viewpath;
        }
    }
}

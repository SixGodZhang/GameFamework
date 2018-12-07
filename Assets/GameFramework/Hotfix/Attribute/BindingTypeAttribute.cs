//-----------------------------------------------------------------------
// <filename>BindingTypeAttribute</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #绑定特性# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/7 星期五 12:08:19# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.Taurus
{
    /// <summary>
    /// 绑定表示
    /// </summary>
    public enum BindingFlag
    {
        Export = 0,//导出
        NoExport,//导出绑定
        None//默认
    }

    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class BindingTypeAttribute:Attribute
    {
        private BindingFlag _bindingFlag = BindingFlag.None;

        public BindingFlag BindingFlag
        {
            get
            {
                return _bindingFlag;
            }

            set
            {
                _bindingFlag = value;
            }
        }
    }
}

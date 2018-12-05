//-----------------------------------------------------------------------
// <filename>IEventArgs</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> ## </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/11/30 星期五 16:17:10# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.Taurus
{
    public interface IEventArgs
    {
        int Id { get; }
    }

    /// <summary>
    /// 事件参数抽象类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class GameEventArgs<T> : IEventArgs where T : IEventArgs
    {
        public int Id
        {
            get
            {
                return typeof(T).GetHashCode();
            }
        }
    }
}

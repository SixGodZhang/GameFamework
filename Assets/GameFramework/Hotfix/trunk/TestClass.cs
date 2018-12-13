//-----------------------------------------------------------------------
// <filename>TestClass</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> ## </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/7 星期五 13:05:30# </time>
//-----------------------------------------------------------------------

using ILRuntime.Other;
using System;
using System.Collections.Generic;

namespace GameFramework.Test
{

    [NeedAdaptor]
    public class TestClass
    {
        public string id = "id";
        internal string name = "name";

        /// <summary>
        /// 避免这种写法
        /// </summary>
        //public virtual bool IsTestArray
        //{
        //    get { return false; }

        //}

        //internal virtual bool IsSzArray
        //{
        //    get { return false; }
        //}

        public void DoAction()
        {
            UnityEngine.Debug.Log("do action....");
        }

        internal void DoPlay()
        {

        }
    }
}

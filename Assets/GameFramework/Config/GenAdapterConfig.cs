//-----------------------------------------------------------------------
// <filename>GenAdapterConfig</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> ## </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/13 星期四 11:21:54# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Taurus
{
    /// <summary>
    /// 持久化保存生成适配器的一些配置
    /// </summary>
    public class GenAdapterConfig : ScriptableObject
    {
        public string out_path = "";
        public string main_assembly_path = "";
        public string il_assembly_path = "";
    }
}

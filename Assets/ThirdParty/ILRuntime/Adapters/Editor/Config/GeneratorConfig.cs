//-----------------------------------------------------------------------
// <filename>GeneratorConfig</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #代码自动生成的一些规则# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/6 星期四 21:57:28# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

public class GeneratorConfig
{

    /// <summary>
    /// 适配器存放的路径
    /// </summary>
    public const string GenAdapterPath = "Assets/ThirdParty/ILRuntime/ILRuntime/Adapter/";



    /// <summary>
    /// 白名单
    /// </summary>
    public static List<string> whiteUserAssemblyList = new List<string>()
        {
            "Assembly-CSharp",
        };

    /// <summary>
    /// 需要写适配器的类型(需要是全额限定名)
    /// </summary>
    public static List<string> GenAdaterCLRType = new List<string>()
        {
            "SubMonoBehavior",
        };

}

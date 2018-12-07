//-----------------------------------------------------------------------
// <filename>GameFrameworkCommon</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #公共类，提供一些常用接口# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/7 星期五 10:17:24# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.Taurus
{
    public class GameFrameworkCommon
    {
        /// <summary>
        /// 获取当前平台名称(有关资源下载时可用)
        /// </summary>
        /// <returns></returns>
        public static string GetPlatformName()
        {
            string platformName = "StandaloneWindows";
#if UNITY_IOS
                platformName = "IOS";
#elif UNITY_ANDROID
                platformName = "Android";
#elif UNITY_STANDALONE_OSX
                platformName = "StandaloneOSX";
#endif
            return platformName;
        }
    }
}

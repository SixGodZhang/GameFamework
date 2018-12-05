//-----------------------------------------------------------------------
// <filename>AssetBundleVersionInfo</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> ## </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/1 星期六 14:44:38# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.Taurus
{
    [Serializable]
    public class ResourcesInfo
    {
        public string Name;
        public string MD5;

        public override bool Equals(object obj)
        {
            var info = obj as ResourcesInfo;
            return info != null &&
                   Name == info.Name &&
                   MD5 == info.MD5;
        }

        public override int GetHashCode()
        {
            var hashCode = -1820180183;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(MD5);
            return hashCode;
        }
    }

    [Serializable]
    public class AssetBundleVersionInfo
    {
        public int Version;
        public bool IsEncrypt;
        public string ManifestAssetBundle;
        public List<ResourcesInfo> Resources;
    }

    [Serializable]
    public class AssetPlatformVersionInfo
    {
        public int Version;
        public List<string> Platforms;
    }
}

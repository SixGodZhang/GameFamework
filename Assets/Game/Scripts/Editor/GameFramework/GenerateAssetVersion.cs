//-----------------------------------------------------------------------
// <filename>GenerateAssetVersion</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #生成版本信息文件# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/4 星期二 12:40:00# </time>
//-----------------------------------------------------------------------

using ATFramework.FileOp;
using System;
using System.Collections.Generic;
using UnityEditor;

namespace GameFramework.Taurus
{
    public class GenerateAssetVersion
    {
        public class GenerateVersionInfoTest
        {
            /// <summary>
            /// 生成平台版本信息
            /// </summary>
            [MenuItem("GameFramework/GeneratePlatformVersion")]
            static void GenerateAssetPlatformVersionInfo()
            {
                AssetPlatformVersionInfo platforms = new AssetPlatformVersionInfo();
                platforms.Version = 100000;
                platforms.Platforms = new List<string>() { "StandaloneWindows,IOS,Android" };

                string platformVersion = EditorJsonUtility.ToJson(platforms);
                System.IO.File.WriteAllText(System.IO.Path.Combine(ResourceManager.GetDeafultPath(PathType.ReadOnly), "AssetPlatformVersion.txt"), platformVersion);
            }

            [MenuItem("GameFramework/GenerateAssetVersion")]
            static void GenerateAssetVersionInfo()
            {
                string dirpath = "";
                AssetBundleVersionInfo abversion = new AssetBundleVersionInfo();
                abversion.IsEncrypt = false;
                abversion.Version = 10000;
                abversion.ManifestAssetBundle = "";

                var infos = new List<ResourcesInfo>();
                var resources = CalculateMd5(dirpath);
                foreach (var item in resources)
                {
                    ResourcesInfo info = new ResourcesInfo();
                    info.Name = item.Key;
                    info.MD5 = item.Value;
                    infos.Add(info);
                }
                abversion.Resources = infos;

                string assetversion = EditorJsonUtility.ToJson(abversion);
                System.IO.File.WriteAllText(System.IO.Path.Combine(ResourceManager.GetDeafultPath(PathType.ReadOnly), "AssetVersion.txt"), assetversion);
            }

            /// <summary>
            /// 计算文件夹下所有文件的Md5值
            /// </summary>
            /// <param name="dirpath"></param>
            private static List<KeyValuePair<string,string>> CalculateMd5(string dirpath)
            {
                List<string> paths = ATFileOp.GetFilesInDirectory(dirpath);
                return Md5Manager.ComputeMd5(paths);
            }
        }
    }
}

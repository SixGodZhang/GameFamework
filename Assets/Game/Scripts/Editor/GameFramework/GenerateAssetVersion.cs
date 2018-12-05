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
using ATFramework.Taurus;
using System.Collections.Generic;
using System.IO;
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
                platforms.Platforms = new List<string>() { "StandaloneWindows", "IOS", "Android" };

                string platformVersion = EditorJsonUtility.ToJson(platforms);
                System.IO.File.WriteAllText(System.IO.Path.Combine(ResourceManager.GetDeafultPath(PathType.ReadOnly), CheckResourceState.AssetPlatformVersionText), platformVersion);

                ATFileOp.ShowExplorerWindow(System.IO.Path.Combine(ResourceManager.GetDeafultPath(PathType.ReadOnly), CheckResourceState.AssetPlatformVersionText));
            }

            [MenuItem("GameFramework/GenerateAssetVersion")]
            static void GenerateAssetVersionInfo()
            {
                string dirpath = System.IO.Path.Combine(UnityEngine.Application.dataPath.Replace("Assets", ""), "AssetBundles/StandaloneWindows");
                AssetBundleVersionInfo abversion = new AssetBundleVersionInfo();
                abversion.IsEncrypt = false;
                abversion.Version = 10000;
                abversion.ManifestAssetBundle = "StandaloneWindows";

                var infos = new List<ResourcesInfo>();
                var resources = CalculateMd5(dirpath);
                foreach (var item in resources)
                {
                    ResourcesInfo info = new ResourcesInfo();

                    info.Name = item.Key.Substring(item.Key.IndexOf("StandaloneWindows")+ "StandaloneWindows".Length + 1).Replace(@"\\","/");
                    info.MD5 = item.Value;
                    infos.Add(info);
                }
                abversion.Resources = infos;

                string assetversion = EditorJsonUtility.ToJson(abversion);
                System.IO.File.WriteAllText(System.IO.Path.Combine(ResourceManager.GetDeafultPath(PathType.ReadOnly), CheckResourceState.AssetVersionTxt), assetversion);

                ATFileOp.ShowExplorerWindow(System.IO.Path.Combine(ResourceManager.GetDeafultPath(PathType.ReadOnly), CheckResourceState.AssetVersionTxt));
            }

            /// <summary>
            /// 计算文件夹下所有文件的Md5值
            /// </summary>
            /// <param name="dirpath"></param>
            //[MenuItem("GameFramework/TestPaths")]
            static List<KeyValuePair<string,string>> CalculateMd5(string dirpath)
            {
                List<string> paths = new List<string>(Directory.GetFiles(dirpath));

                return Md5Manager.ComputeMd5(paths);
            }
        }
    }
}

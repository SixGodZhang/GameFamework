//-----------------------------------------------------------------------
// <filename>Md5Manager</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #Md5值计算# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/4 星期二 12:59:26# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ATFramework.Taurus
{
    /// <summary>
    /// Md5工具类
    /// </summary>
    public class Md5Manager
    {
        /// <summary>
        /// 计算字符串的Md5值
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetMD5FromString(string input)
        {
            string result = "";
            byte[] data = Encoding.GetEncoding("utf-8").GetBytes(input);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(data);
            for (int i = 0; i < bytes.Length; i++)
            {
                result += bytes[i].ToString("x2");
            }
            return result;
        }

        /// <summary>
        /// 批量计算文件的Md5值
        /// </summary>
        /// <param name="filePaths"></param>
        /// <returns></returns>
        public static List<KeyValuePair<string, string>> ComputeMd5(IEnumerable<string> filePaths)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            List<KeyValuePair<string, string>> md5Dict =new List<KeyValuePair<string, string>>();
            StringBuilder sb = new StringBuilder();
            int total = ((List<string>)filePaths).Count;
            int init = 0;
            foreach (var item in filePaths)
            {
                UnityEditorTool.ShowProgressBar(init / (total * 1.0f), total, "正在计算:" + item + " MD5值,请稍后...");
                using (FileStream fs = new FileStream(item, FileMode.Open))
                {
                    sb.Clear();
                    byte[] bytes = md5.ComputeHash(fs);

                    foreach (var it in bytes)
                    {
                        sb.Append(it.ToString("x2"));
                    }

                    md5Dict.Add(new KeyValuePair<string, string>(item, sb.ToString()));
                }
            }

            UnityEditorTool.ClearProgressBar();

            md5.Clear();

            return md5Dict;
        }

        /// <summary>
        /// 计算单个文件的Md5值
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetMd5FromFile(string path)
        {
            if (!File.Exists(path))
            {
#if UNITY_EDITOR
                UnityEngine.Debug.Log("资源不存在，无法计算其Md5值!\n " + path);
#endif
                return string.Empty;
            }

            byte[] data = File.ReadAllBytes(path);
            string result = string.Empty;
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = md5.ComputeHash(data);
            for (int i = 0; i < bytes.Length; i++)
            {
                result += bytes[i].ToString("x2");
            }
            return result;
        }
    }
}

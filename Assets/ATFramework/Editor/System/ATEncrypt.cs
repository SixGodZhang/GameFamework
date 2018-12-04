using ATFramework.FileOp;
using ATFramework.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ATFramework.Main
{
    public class EncryptTest
    {
        static void Main(string[] args)
        {
            List<string> fps = ATFileOp.GetFilesInDirectory(@"E:\WorkSpace\dalan\Assets\Resources");

            //DirectoryInfo di = new DirectoryInfo(@"E:\WorkSpace\dalan\Assets\Resources\UI");
            //FileInfo[] fis = di.GetFiles();
            //List<string> fps = new List<string>();
            //for (int i = 0; i < fis.Length; i++)
            //{
            //    fps[i] = fis[i].FullName;
            //}

            Dictionary<string, string> md5Dict = new Dictionary<string, string>();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //////多线程Md5计算处理
            md5Dict = new Md5EncryptMultiThread(fps).BeginComputeMd5();

            foreach (var item in md5Dict.Keys)
            {
                string ss = string.Format("[{0}:{1}]", item, md5Dict[item]);
                ATFileOp.WriteToFile(@"C:\Users\Administrator\Desktop\A.txt", ss, FileMode.Append);
            }
            stopwatch.Stop();
            //Console.WriteLine("多线程时间: " + stopwatch.ElapsedMilliseconds);

            ////单线程Md5计算处理
            //stopwatch.Restart();
            //md5Dict.Clear();
            //md5Dict = Md5Encrypt.ComputeMd5(fps);
            //foreach (var item in md5Dict.Keys)
            //{
            //    string ss = string.Format("[{0}:{1}]", item, md5Dict[item]);
            //    ATFileOp.WriteToFile(@"C:\Users\Administrator\Desktop\B.txt", ss, FileMode.Append);
            //}
            //stopwatch.Stop();
            //Console.WriteLine("单线程时间: " + stopwatch.ElapsedMilliseconds);
            Console.WriteLine("end...");
        }
    }

    public class Md5Encrypt
    {
        public static Dictionary<string, string> ComputeMd5(IEnumerable<string> filePaths)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            Dictionary<string, string> md5Dict = new Dictionary<string, string>();
            StringBuilder sb = new StringBuilder();
            foreach (var item in filePaths)
            {
                using (FileStream fs = new FileStream(item, FileMode.Open))
                {
                    sb.Clear();
                    byte[] bytes = md5.ComputeHash(fs);

                    foreach (var it in bytes)
                    {
                        sb.Append(it.ToString("x2"));
                    }

                    md5Dict.Add(item, sb.ToString());
                }
            }

            md5.Clear();

            return md5Dict;
        }
    }


    public class Md5EncryptMultiThread
    {
        private static readonly object Sync = new object();
        private const int THNNUMBER = 4;
        private static Stack<string> computeList;
        public Md5EncryptMultiThread(IEnumerable<string> filePaths)
        {
            computeList = new Stack<string>(filePaths);
        }

        public Dictionary<string, string> BeginComputeMd5()
        {
            if (computeList.Count == 0)
                return null;

            Task<Dictionary<string, string>> task = Task.Run(() =>
            {
                return Begin();
            });
            task.Wait();

           return task.Result;
        }

        private Dictionary<string, string> Begin()
        {
            Dictionary<string, string> md5List = new Dictionary<string, string>();
            List<Task> taskList = new List<Task>();
            for (int i = 0; i < THNNUMBER; i++)
            {
                if (computeList.Count == 0)
                    break;

                Task task = Task.Run(() =>
                {
                    while (computeList.Count != 0)
                    {
                        if (computeList.Count != 0)
                        {
                            string path = computeList.Pop();
                            md5List.Add(path, ComputeHash(path));
                        }
                    }
                });
                taskList.Add(task);
            }

            Task.WaitAll(taskList.ToArray());

            Console.WriteLine(md5List.Count ==0);
            return md5List;
        }

        private string ComputeHash(string path)
        {
            StringBuilder finalMd5 = new StringBuilder();
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                byte[] bytes = null;
                try
                {
                    bytes = md5.ComputeHash(fs);
                }
                catch (Exception ex)
                {
                    ATLog.Error(ex.Message + "\n" + ex.StackTrace);
                }
                finally
                {
                    md5.Clear();
                }

                foreach (var item in bytes)
                {
                    finalMd5.Append(item.ToString("x2"));
                }
            }

            return finalMd5.ToString();

        }



    }

    public class ATEncrypt
    {
        #region Md5加密 (不可逆)
        /// <summary>
        /// Md5加密字符串,采用UTF-8编码
        /// </summary>
        /// <param name="value">加密的字符串</param>
        /// <returns>加密后字符串的Md5值</returns>
        public static string MD5By32(string value)
        {
            return MD5By32(value, Encoding.UTF8);
        }

        /// <summary>
        /// Md5加密字符串,采用自定义编码
        /// </summary>
        /// <param name="value">加密的字符串</param>
        /// <param name="encoding">字符串采用的编码</param>
        /// <returns>加密后字符串的Md5值</returns>
        public static string MD5By32(string value, Encoding encoding)
        {
            return MD5(value, encoding, -1, -1);
        }

        /// <summary>
        /// Md5加密核心方法
        /// </summary>
        /// <param name="value">待加密的字符串</param>
        /// <param name="encoding">字符串编码</param>
        /// <param name="startIndex">保留字段，返回结果的起始序号</param>
        /// <param name="length">保留字段，返回结果的长度</param>
        /// <returns>Md5值</returns>
        /// 19-0D-2C-85-F6-E0-46-8C MD5("HelloWorld",Encoding.UTF8,4,8)
        /// 86-FB-26-9D-19-0D-2C-85-F6-E0-46-8C-EC-A4-2A-20  MD5("HelloWorld",Encoding.UTF8,-1,-1)
        private static string MD5(string value, Encoding encoding,Int32 startIndex,Int32 length)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            var md5 = new MD5CryptoServiceProvider();
            string result;
            try
            {
                var hash = md5.ComputeHash(encoding.GetBytes(value));
                result = startIndex < 0 ? BitConverter.ToString(hash) : BitConverter.ToString(hash, startIndex, length);
            }
            finally
            {
                md5.Clear();
            }

            return result;
        }
        #endregion

        #region DES加密 (可逆)
        
        #endregion

    }
}

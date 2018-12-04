using ATFramework.Log;
using ATFramework.Main;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace ATFramework.FileOp
{
    /// <summary>
    /// 和文件和文件夹操作相关的操作
    /// </summary>
    public class ATFileOp
    {
        private static readonly object Sync = new object();

        /// <summary>
        /// 使用Window系统文件浏览器打开文件
        /// </summary>
        /// <param name="filePath"> </param>
        public static void ShowExplorerWindow(string filePath)
        {
            global::System.Diagnostics.ProcessStartInfo info = new global::System.Diagnostics.ProcessStartInfo("Explorer.exe");
            //路径采用双斜杠，否则找不到目录
            string target = filePath.Replace("/", "\\");
            if (!global::System.IO.File.Exists(target))
            {
                return;
            }
            info.Arguments = "/select," + target;
            global::System.Diagnostics.Process.Start(info);
        }

        /// <summary>
        /// 向文件中写入内容,若文件不存在,则创建文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string WriteToFile(string filePath, string content, FileMode mode = FileMode.OpenOrCreate)
        {

            Console.WriteLine("Name: " + Thread.CurrentThread.Name + "  ,content:" + string.IsNullOrWhiteSpace(content));

            lock (Sync)
            {
                if (!File.Exists(filePath))
                    mode = FileMode.OpenOrCreate;

                string error = null;
                FileStream fs = null;
                StreamWriter sw = null;

                try
                {
                    fs = new FileStream(filePath, mode, FileAccess.Write);
                    sw = new StreamWriter(fs);
                    sw.WriteLine(content);
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    ATLog.Error(ex.Message + "\n" + ex.StackTrace);
                    return error;
                }
                finally
                {
                    if (sw != null)
                        sw.Close();
                }

                return error;
            }
        }

        /// <summary>
        /// 将文件夹下的所有内容全部复制给定目录中
        /// 注意:原根会被删除
        /// </summary>
        /// <param name="sPath"></param>
        /// <param name="tPath"></param>
        public static bool MoveFolder(string sPath, string tPath)
        {
            if (!Directory.Exists(sPath))
            {
                ATLog.Error(sPath + ": Not Found");
                return false;
            }

            try
            {
                if (Directory.Exists(tPath))
                    RemoveFullFolder(tPath);

                Directory.Move(sPath, tPath);  
            }
            catch (Exception ex)
            {
                ATLog.Error(ex.Message + "\n" + ex.StackTrace);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 移除文件
        /// </summary>
        /// <param name="path"></param>
        public static bool RemoveFile(string path)
        {
            try
            {
                if (global::System.IO.File.Exists(path))
                    global::System.IO.File.Delete(path);
            }
            catch(Exception ex)
            {
                ATLog.Error(ex.Message + "\n" + ex.StackTrace);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 通过递归获取目录及其子目录下的所有文件
        /// </summary>
        /// <param name="path">文件夹目录</param>
        /// <returns></returns>
        public static List<string> GetFilesInDirectory(string path)
        {
            List<string> list = new List<string>();

            path = path.Replace("\\", "/"); ;
            if (!Directory.Exists(path))
            {
                ATLog.Error(path + "Not Found!" + ATStackInfo.GetCurrentStackInfo());
                return null;
            }

            //获取下面的所有文件
            List<string> files = new List<string>(Directory.GetFiles(path));
            list.AddRange(files);

            //对文件夹进行递归
            List<string> folders = new List<string>(Directory.GetDirectories(path));
            folders.ForEach(c =>
            {
                string tempFolderName = Path.Combine(path, Path.GetFileName(c));
                list.AddRange(GetFilesInDirectory(tempFolderName));
            });

            return list;
        }

        /// <summary>
        /// 删除文件夹及其所有子文件
        /// </summary>
        /// <param name="path">文件夹路径</param>
        public static bool RemoveFullFolder(string path)
        {
            path = path.Replace("\\", "/");

            if (!Directory.Exists(path))
            {
                ATLog.Error(path + "Not Found");
                return false;
            }

            List<string> files = new List<string>(Directory.GetFiles(path));
            files.ForEach(c =>
            {
                string tempFileName = Path.Combine(path, Path.GetFileName(c));
                FileInfo fileInfo = new FileInfo(tempFileName);
                if (fileInfo.Attributes != FileAttributes.Normal)
                {
                    fileInfo.Attributes = FileAttributes.Normal;
                }
                fileInfo.Delete();
                ATLog.Info(fileInfo.Name + "已经删除!");
            });

            List<string> folders = new List<string>(Directory.GetDirectories(path));
            folders.ForEach(c =>
            {
                string tempFolderName = Path.Combine(path, Path.GetFileName(c));
                RemoveFullFolder(tempFolderName);
            });

            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (directoryInfo.Attributes != FileAttributes.Normal)
            {
                directoryInfo.Attributes = FileAttributes.Normal;
            }
            directoryInfo.Delete();
            ATLog.Info(directoryInfo.Name + "已经删除!");

            return true;
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="sourcePath">源文件名</param>
        /// <param name="destPath">目标文件名</param>
        public static void CopyFile(string sourcePath, string destPath, bool isrewrite = true)
        {
            sourcePath = sourcePath.Replace("\\", "/");
            destPath = destPath.Replace("\\", "/");

            try
            {
                global::System.IO.File.Copy(sourcePath, destPath, isrewrite);
            }
            catch (Exception ex)
            {
                ATLog.Error(ex);
            }
        }

        /// <summary>
        /// 复制文件夹及其子目录、文件到目标文件夹
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destPath"></param>
        public static void CopyFolder(string sourcePath, string destPath)
        {
            if (Directory.Exists(sourcePath))
            {
                if (!Directory.Exists(destPath))
                {
                    //目标目录不存在则创建
                    try
                    {
                        Directory.CreateDirectory(destPath);
                    }
                    catch (Exception ex)
                    {
                        ATLog.Error(ex);
                        throw new Exception("create target folder fail...：" + ex.Message);
                    }
                }

                List<string> files = new List<string>(Directory.GetFiles(sourcePath));
                files.ForEach(c =>
                {
                    string destFile = Path.Combine(destPath, Path.GetFileName(c));
                    global::System.IO.File.Copy(c, destFile, true);
                });

                List<string> folders = new List<string>(Directory.GetDirectories(sourcePath));
                folders.ForEach(c =>
                {
                    string destDir = Path.Combine(destPath, Path.GetFileName(c));
                    CopyFolder(c, destDir);
                });
            }
            else
            {
                throw new DirectoryNotFoundException("sourcePath: " + "source folder not found！");
            }
        }
    }
}
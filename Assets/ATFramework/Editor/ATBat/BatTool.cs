using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using System.Threading;

namespace ATFramework.Bat
{
    public class BatTool
    {
        private static string infoLogPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Logs/BatLog.log";
        //private static string errorLogPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Logs/BatErrorLog.log";
        private static bool batExcuteResult = true;

        /// <summary>
        /// 调用批处理
        /// </summary>
        /// <param name="batPath">Bat路径</param>
        /// CallBat(@"E:\WorkSpace\dalan\AutoTools\codeOP.bat", (str) => { Console.WriteLine(str); });
        public static bool CallBat(string batPath,Action<string> realTimeLog = null)
        {
            //Initialize
            batExcuteResult = true;

            Thread t1 = new Thread((data) => { ReadOutputStream(data, StreamType.Info, realTimeLog);});
            t1.Name = "ReadBatInfoThread";
            t1.IsBackground = true;

            Thread t2 = new Thread((data) => { ReadOutputStream(data, StreamType.Error, realTimeLog);});
            t2.Name = "ReadBatErrorThread";
            t2.IsBackground = true;

            using (Process pro = new Process())
            {
                FileInfo file = new FileInfo(batPath);
                pro.StartInfo.WorkingDirectory = file.Directory.FullName;
                pro.StartInfo.FileName = batPath;
                pro.StartInfo.CreateNoWindow = true;
                pro.StartInfo.RedirectStandardOutput = true;
                pro.StartInfo.RedirectStandardError = true;
                pro.StartInfo.StandardOutputEncoding = Encoding.GetEncoding("gb2312");
                pro.StartInfo.StandardErrorEncoding = Encoding.GetEncoding("gb2312");
                pro.StartInfo.UseShellExecute = false;

                pro.Start();
                t1.Start(pro);
                t2.Start(pro);
                t1.Join();
                t2.Join();

                pro.WaitForExit();
                if (pro.HasExited)
                {
                    pro.Close();
                }

                return batExcuteResult;
            }
        }

        enum StreamType
        {
            Info        = 0,
            Error       = 1
        }

        /// <summary>
        /// 读取命令行缓冲区中的流
        /// </summary>
        /// <param name="data">命令行实例</param>
        /// <param name="type">需要读取的Log类型</param>
        /// <param name="realTimeLog">处理实时Log的委托</param>
        private static void ReadOutputStream(object data, StreamType type, Action<string> realTimeLog = null)
        {
            Process process = data as Process;
            Contract.Assert(process != null, "process == null");
            if (process == null) return;

            StringBuilder sb = new StringBuilder();
            string outMsg = string.Empty;
            while (type == StreamType.Error && (outMsg = process.StandardError.ReadLine())!=null)
            {
                if (!string.IsNullOrEmpty(outMsg.Trim()))
                    batExcuteResult = false;

                sb.Append("e: " + outMsg + "\n");
                realTimeLog?.Invoke(outMsg); 
            }

            while (type == StreamType.Info && (outMsg = process.StandardOutput.ReadLine())!=null)
            {
                if (outMsg.Contains("errorlevel="))
                    batExcuteResult = "0".Equals(outMsg.Substring(outMsg.IndexOf("=") + 1, 1));

                sb.Append("d: " + outMsg + "\n");
                realTimeLog?.Invoke(outMsg);
            }

            FileOp.ATFileOp.WriteToFile(infoLogPath, sb.ToString(), FileMode.Append);
        }


        /// <summary>
        /// 读取错误输出流
        /// </summary>
        /// <param name="data"></param>
        private static void ReadError(object data)
        {
            Process temp = data as Process;
            if (temp == null)
                return;

            StringBuilder errorLog = new StringBuilder();
            string outputStr = string.Empty;
            while ((outputStr = temp.StandardError.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(outputStr.Trim()))
                    batExcuteResult = false;

                errorLog.Append("e: " + outputStr + "\n");
            }

           // FileOp.ATFileOp.WriteToFile(errorLogPath, errorLog.ToString(),FileMode.Append);
        }

        /// <summary>
        /// 读取标准输出流
        /// </summary>
        /// <param name="data"></param>
        private static void ReadOutput(object data)
        {
            Process temp = data as Process;
            if (temp == null)
                return;

            StringBuilder infoLog = new StringBuilder();
            string standardOutputStr = string.Empty;
            while ((standardOutputStr = temp.StandardOutput.ReadLine()) != null)
            {
                if (standardOutputStr.Contains("errorlevel="))
                    batExcuteResult = "0".Equals(standardOutputStr.Substring(standardOutputStr.IndexOf("=") + 1, 1));

                infoLog.Append("d: " + standardOutputStr + "\n");
            }

            FileOp.ATFileOp.WriteToFile(infoLogPath, infoLog.ToString(), FileMode.Append);
        }
    }

}

#define TRACE

using System.Diagnostics;
using System.IO;

namespace ATFramework.Log
{
    public class ATLog
    {
        static ATLog()
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new ATLogListener());
        }

        private static bool m_openLog = true;
        /// <summary>
        /// Log开关
        /// </summary>
        public static bool OpenLog
        {
            get { return m_openLog; }
            set { m_openLog = value; }
        }

        /// <summary>
        /// Debug 
        /// </summary>
        /// <param name="msg"></param>
        public static void Debug(object msg)
        {
            Trace.WriteLineIf(m_openLog, msg, "Debug");
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="msg"></param>
        public static void Error(object msg)
        {
            Trace.WriteLineIf(m_openLog, msg, "Error");
        }

        /// <summary>
        /// Info
        /// </summary>
        /// <param name="msg"></param>
        public static void Info(object msg)
        {
            Trace.WriteLineIf(m_openLog, msg, "Info");
        }

        /// <summary>
        /// Warn
        /// </summary>
        /// <param name="msg"></param>
        public static void Warn(object msg)
        {
            Trace.WriteLineIf(m_openLog, msg, "Warn");
        }

        /// <summary>
        /// 清除所有Log
        /// </summary>
        public static void ClearLog()
        {
            if (Directory.Exists(LogConstant.LogFolder))
            {
                string[] logsPath = Directory.GetFiles(LogConstant.LogFolder);
                for (int i = 0; i < logsPath.Length; i++)
                {
                    global::System.IO.File.Delete(logsPath[i]);
                }
            }
        }
    }
}

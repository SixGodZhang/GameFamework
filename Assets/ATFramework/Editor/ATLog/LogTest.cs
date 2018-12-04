using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATFramework.Log
{
    public class LogTest
    {
        /// <summary>
        /// 所有种类Log测试
        /// </summary>
        public static void AllLogTest()
        {
            ATLog.Debug("this is debug log");
            ATLog.Info("this is Info log");
            ATLog.Warn("this is warn log");
            ATLog.Error("this is error log");
        }

        /// <summary>
        /// 调试Log测试
        /// </summary>
        public static void DebugLogTest()
        {
            ATLog.Debug("this is debug log");
        }

        /// <summary>
        /// 重要Log测试
        /// </summary>
        public static void InfoLogTest()
        {
            ATLog.Info("this is Info log");
        }

        /// <summary>
        /// 警告Log测试
        /// </summary>
        public static void WarnLogTest()
        {
            ATLog.Warn("this is warn log");
        }

        /// <summary>
        /// 错误Log测试
        /// </summary>
        public static void ErrorLogTest()
        {
            ATLog.Error("this is error log");
        }

    }
}

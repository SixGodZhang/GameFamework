using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATFramework.Main
{
    public class ATStackInfo
    {
        /// <summary>
        /// 获取当前的堆栈信息
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentStackInfo()
        {
            StackTrace current = new StackTrace(true);
            string stackInfo = current.ToString();
            return stackInfo.Substring(stackInfo.IndexOf("\n") + 1);
        }
    }
}

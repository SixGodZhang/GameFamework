using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATFramework.Log
{
    internal class LogConstant
    {
        public static string LogFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Logs/";
        //
        static LogConstant()
        {
            string CurrentAppName = Process.GetCurrentProcess().MainModule.ModuleName;
            LogFolder += CurrentAppName;
        }
    }
}

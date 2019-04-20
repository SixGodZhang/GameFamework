using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotfix.Hotfix.Bugfix.Bugs
{
    class Bugs_GameMain_TestMainHotfix
    {
        public static void DoAction(object _this,string args,int arr)
        {
            UnityEngine.Debug.LogError("hotfix this doaction");
        }
    }
}

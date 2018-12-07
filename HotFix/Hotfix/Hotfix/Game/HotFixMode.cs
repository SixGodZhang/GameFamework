//-----------------------------------------------------------------------
// <filename>HotFixMode</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #热更代码入口# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/3 星期一 16:58:52# </time>
//-----------------------------------------------------------------------

namespace HotFix.Taurus
{
    public class HotFixMode: SubMonoBehavior
    {
        public HotFixMode()
        {
            UnityEngine.Debug.Log("-------------------Start Hotfix Code-------------------");
        }

        public override void OnApplicationQuit()
        {
            UnityEngine.Debug.Log("-------------------Hotfix: call HotFixMode.OnApplicationQuit() -------------------");
        }

        public override void OnDestroy()
        {
            UnityEngine.Debug.Log("-------------------Hotfix: call HotFixMode.OnDestroy() -------------------");
        }

        public override void OnFixedUpdate()
        {
            UnityEngine.Debug.Log("-------------------Hotfix: call HotFixMode.OnFixedUpdate() -------------------");
        }

        public override void Start()
        {
            UnityEngine.Debug.Log("-------------------Hotfix: call HotFixMode.Start() -------------------");
        }

        public override void Update()
        {
            UnityEngine.Debug.Log("-------------------Hotfix: call HotFixMode.Update() -------------------");
        }
    }
}

//-----------------------------------------------------------------------
// <filename>HotfixDevMode</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #热更代码开发者模式，直接从工程中加载# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/6 星期四 12:43:55# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace GameFramework.Taurus
{
    public class HotfixDevMode : IHotFixMode
    {
        /// <summary>
        /// 逻辑代码的Assembly
        /// </summary>
        private Assembly _hotfixAssembly = null;
        /// <summary>
        /// DLL的实例
        /// </summary>
        private SubMonoBehavior _assemblyEntity;

        #region 重载函数
        public bool EnterHotfix(ResourceManager rm)
        {
            bool result = false;
            HotFixPath? path = HotfixManager.HotFixPath;
            //加载热更代码,此处根据资源管理的加载资源的方式确定加载AB文件还是bytes文件
            result = LoadHotFixCode(rm, path?.DllPath, path?.MdbPath);
            _assemblyEntity = (SubMonoBehavior)_hotfixAssembly.CreateInstance("HotFix.Taurus.HotFixMode");
            //if (_assemblyEntity != null)
            //    _assemblyEntity.Start();
            return result;
        }

        #region 周期函数
        public void OnApplicationQuit()
        {
            _assemblyEntity.OnApplicationQuit();
        }

        public void OnDestory()
        {
            _assemblyEntity.OnDestroy();
        }

        public void OnFixedUpdate()
        {
            _assemblyEntity.OnFixedUpdate();
        }

        public void Start()
        {
            _assemblyEntity.Start();
        }

        public void Update()
        {
            _assemblyEntity.Update();
        }
        #endregion
        #endregion

        #region 内部函数
        /// <summary>
        /// 加载逻辑代码
        /// </summary>
        /// <param name="rm"></param>
        /// <param name="dllPath"></param>
        /// <param name="mdbPath"></param>
        private bool LoadHotFixCode(ResourceManager rm, string dllPath, string mdbPath)
        {
            //资源加载
            Debug.Log("dllpath: " + dllPath + " mdbpath: " + mdbPath);

            byte[] dll = rm.LoadAsset<TextAsset>("", dllPath).bytes;
            if (dll == null)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError("load hotfix code dll fail!");
                return false;
#endif
            }
            byte[] mdb = null;
#if UNITY_EDITOR
            //pdb and mdb 没有重命名bytes文件,因此不能被Unity识别,所以只能通过下面的文件流的方式加载
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                FileStream fileStream = File.OpenRead(mdbPath);
                if (fileStream != null && fileStream.Length > 0)
                {
                    byte[] byteData = new byte[fileStream.Length];
                    fileStream.Read(byteData, 0, (int)fileStream.Length);
                    fileStream.Close();
                    mdb = byteData;
                }
            }
#endif
            _hotfixAssembly = Assembly.Load(dll, mdb);

            return true;
        }
        #endregion


    }
}

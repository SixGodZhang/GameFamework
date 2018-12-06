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
        public void EnterHotfix(ResourceManager rm)
        {
            HotFixPath path = HotfixManager.GetDLLAndPdbPath(rm.ResUpdateType);
            //加载热更代码,此处根据资源管理的加载资源的方式确定加载AB文件还是bytes文件
            LoadHotFixCode(rm, path.DllPath, path.PdbPath);
            _assemblyEntity = (SubMonoBehavior)_hotfixAssembly.CreateInstance("HotFix.Taurus.HotFixMode");
            if (_assemblyEntity != null)
                _assemblyEntity.Start();
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
        private void LoadHotFixCode(ResourceManager rm, string dllPath, string pdbPath)
        {
            //资源加载
            byte[] dll = rm.LoadAsset<TextAsset>("hotfix", dllPath).bytes;
            byte[] pdb = null;
#if UNITY_EDITOR
            pdb = rm.LoadAsset<TextAsset>("hotfix", pdbPath).bytes;
#endif
            _hotfixAssembly = Assembly.Load(dll, pdb);
        }
        #endregion


    }
}

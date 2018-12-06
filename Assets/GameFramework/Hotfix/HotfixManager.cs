//-----------------------------------------------------------------------
// <filename>HotfixManager</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #HotFix管理模块# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/3 星期一 14:51:10# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace GameFramework.Taurus
{
    #region 数据结构
    public struct HotFixPath
    {
        public string DllPath { get; set; }
        public string PdbPath { get; set; }
    }
    #endregion

    public class HotfixManager : IGameModule, IUpdate, IFixedUpdate,IApplicationQuit
    {
        #region 字段&属性
        public AppDomain Appdomain { get; private set; }

        /// <summary>
        /// 是否使用热更代码
        /// </summary>
        private bool _usehotfix = true;
        public bool UseHotFix
        {
            get { return _usehotfix; }
            set { _usehotfix = value; }
        }

        //走本地代码模式 or 走资源热更模式
        private IHotFixMode mode;
        #endregion

        #region 构造函数
        public HotfixManager()
        {
            Appdomain = new AppDomain();
        }
        #endregion

        #region 公开接口

        /// <summary>
        /// 初始化热更模块
        /// </summary>
        public void Init(ResourceManager rm)
        {
            if (_usehotfix)
                mode = new HotfixReleaseMode();
            else
                mode = new HotfixDevMode();

            mode.EnterHotfix(rm);
            if (mode != null)
                mode.Start();
        }

        /// <summary>
        /// 获取本地DLL和ab的路径
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static HotFixPath GetDLLAndPdbPath(ResourceUpdateType type)
        {
            HotFixPath hotFixPath = new HotFixPath();
            if (type == ResourceUpdateType.Editor)
            {
                hotFixPath.DllPath = "Assets/Game/HotFix/HotFix.dll.bytes";
                hotFixPath.PdbPath = "Assets/Game/HotFix/HotFix.pdb.bytes";
            }
            else
            {
                hotFixPath.DllPath = "Assets/StreamingAssets/HotFix.dll.bytes.ab";
                hotFixPath.PdbPath = "Assets/StreamingAssets/HotFix.pdb.bytes.ab";
            }

            return hotFixPath;
        }
        #endregion

        #region 重载函数
        public void OnClose()
        {
            if (mode != null)
                mode.OnDestory();
            Appdomain = null;
        }

        public void OnUpdate()
        {
            if (mode != null)
                mode.Update();
        }

        public void OnFixedUpdate()
        {
            if (mode != null)
                mode.OnFixedUpdate();
        }

        public void Start()
        {
            if (mode != null)
                mode.Start();
        }

        public void OnApplicationQuit()
        {
            if (mode != null)
                mode.OnApplicationQuit();
        }
        #endregion
    }

}

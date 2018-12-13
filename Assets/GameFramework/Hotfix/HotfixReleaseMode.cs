//-----------------------------------------------------------------------
// <filename>HotfixReleaseMode</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #Hotfix的发布者模式,采用ILRuntime来解析逻辑代码# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/6 星期四 12:42:52# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace GameFramework.Taurus
{
    public class HotfixReleaseMode: IHotFixMode
    {
        #region 字段&属性
        public AppDomain Appdomain { get; private set; }

        //热更新的开头实例
        private SubMonoBehavior _hotFixVrCoreEntity;

        /// <summary>
        /// 所有的热更类型
        /// </summary>
        private List<Type> _hotfixTypes;

        /// <summary>
        /// Hotfix中的所有类型
        /// </summary>
        public List<Type> HotfixTypes
        {
            get
            {
                if (_hotfixTypes == null || _hotfixTypes.Count == 0)
                {
                    _hotfixTypes = new List<Type>();
                    if (Appdomain == null)
                        return _hotfixTypes;
                    foreach (var item in Appdomain.LoadedTypes.Values)
                    {
                        _hotfixTypes.Add(item.ReflectionType);
                    }
                }

                return _hotfixTypes;
            }
        }
        #endregion

        #region 构造函数
        public HotfixReleaseMode()
        {
            Appdomain = new AppDomain();
        }
        #endregion

        #region 公开接口
        /// <summary>
        /// 进入热更代码
        /// </summary>
        /// <param name="type"></param>
        public bool EnterHotfix(ResourceManager rm)
        {
            HotFixPath? path = HotfixManager.HotFixPath;
            //加载热更代码,此处根据资源管理的加载资源的方式确定加载AB文件还是bytes文件
            return LoadHotFixCode(rm, path?.DllPath, path?.PdbPath);
        }

        /// <summary>
        /// 加载热更新代码
        /// </summary>
        /// <param name="dll"></param>
        /// <param name="pdb"></param>
        private void LoadHotfixAssembly(byte[] dll, byte[] pdb = null)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(dll))
            {
                if (pdb == null)
                    Appdomain.LoadAssembly(ms, null, new Mono.Cecil.Pdb.PdbReaderProvider());
                else
                    using (System.IO.MemoryStream p = new System.IO.MemoryStream(pdb))
                    {
                        Appdomain.LoadAssembly(ms, p, new Mono.Cecil.Pdb.PdbReaderProvider());
                    }
            }

            //初始化ILRuntime
            InitializeILRuntime();
            //运行热更新的入口
            RunHotFixVrCoreEntity();
        }
        #endregion



        #region 内部函数

        /// <summary>
        /// 加载热更代码
        /// </summary>
        private bool LoadHotFixCode(ResourceManager rm, string dllpath,string pdbpath)
        {
            //资源加载
            byte[] dll = rm.LoadAsset<TextAsset>("hotfixdll.ab", dllpath).bytes;
            if (dll == null)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.Log("load hotfix code dll fail!");
                return false;
#endif
            }
            byte[] pdb = null;
#if UNITY_EDITOR
            //pdb不是AB文件,单独流程加载
            UnityEngine.Debug.Log(pdbpath);
            pdb = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(pdbpath).bytes;
            Appdomain.DebugService.StartDebugService(56000);
#endif
            LoadHotfixAssembly(dll, pdb);

            return true;
        } 

        /// <summary>
        /// 执行热更新
        /// </summary>
        private void RunHotFixVrCoreEntity()
        {
            //热更代码入口处
            _hotFixVrCoreEntity = Appdomain.Instantiate<SubMonoBehavior>("HotFix.Taurus.HotFixMode");
#if UNITY_EDITOR
            if (_hotFixVrCoreEntity == null)
            {
                UnityEngine.Debug.LogError("_hotFixVrCoreEntity == null \n 无法进入热更代码!");
            }
#endif
        }

        /// <summary>
        /// 初始化ILRuntime
        /// </summary>
        private void InitializeILRuntime()
        {
            //CLR绑定
            //ILRuntime.Runtime.Generated.CLRBindings.Initialize(Appdomain);

            //注册一些委托
            Appdomain.DelegateManager.RegisterMethodDelegate<System.Object>();
            Appdomain.DelegateManager.RegisterMethodDelegate<System.UInt16, System.Byte[]>();
            Appdomain.DelegateManager.RegisterMethodDelegate<System.Object, ILRuntime.Runtime.Intepreter.ILTypeInstance>();
            Appdomain.DelegateManager.RegisterDelegateConvertor<System.EventHandler<ILRuntime.Runtime.Intepreter.ILTypeInstance>>((act) =>
            {
                return new System.EventHandler<ILRuntime.Runtime.Intepreter.ILTypeInstance>((sender, e) =>
                {
                    ((Action<System.Object, ILRuntime.Runtime.Intepreter.ILTypeInstance>)act)(sender, e);
                });
            });

            //注册一些适配器
            //AdapterRegister.RegisterCrossBindingAdaptor(Appdomain);
        }

        #region 周期函数
        public void Start()
        {
            if (_hotFixVrCoreEntity != null)
                _hotFixVrCoreEntity.Start();
        }

        public void Update()
        {
            if (_hotFixVrCoreEntity != null)
                _hotFixVrCoreEntity.Update();
        }

        public void OnFixedUpdate()
        {
            if (_hotFixVrCoreEntity != null)
                _hotFixVrCoreEntity.OnFixedUpdate();
        }

        public void OnDestory()
        {
            if (_hotFixVrCoreEntity != null)
                _hotFixVrCoreEntity.OnDestroy();
        }

        public void OnApplicationQuit()
        {
            if (_hotFixVrCoreEntity != null)
                _hotFixVrCoreEntity.OnApplicationQuit();
        }
        #endregion
        #endregion
    }
}

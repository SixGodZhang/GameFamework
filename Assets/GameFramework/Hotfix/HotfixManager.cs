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
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace GameFramework.Taurus
{
    public class HotfixManager : IGameModule, IUpdate, IFixedUpdate
    {
        #region 字段&属性
        public AppDomain Appdomain { get; private set; }
        

        //热更新的开头函数
        private object _hotFixVrCoreEntity;

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

        #region 委托
        /// <summary>
        /// 渲染函数
        /// </summary>
        public Action Update;

        /// <summary>
        /// 固定渲染帧函数
        /// </summary>
        public Action FixedUpdate;

        /// <summary>
        /// 结束函数
        /// </summary>
        public Action Close;
        #endregion

        #region 构造函数
        public HotfixManager()
        {
            Appdomain = new AppDomain();
        }
        #endregion

        #region 公开接口
        /// <summary>
        /// 加载热更新代码
        /// </summary>
        /// <param name="dll"></param>
        /// <param name="pdb"></param>
        public void LoadHotfixAssembly(byte[] dll, byte[] pdb = null)
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
        /// 执行热更新
        /// </summary>
        private void RunHotFixVrCoreEntity()
        {
            //热更代码入口处
            _hotFixVrCoreEntity = Appdomain.Instantiate("HotFix.Taurus.HotFixMode");
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
            ILRuntime.Runtime.Generated.CLRBindings.Initialize(Appdomain);

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
        }
        #endregion

        #region 重载函数
        public void OnClose()
        {
            Close?.Invoke();
            Appdomain = null;
        }

        public void OnUpdate()
        {
            Update?.Invoke();
        }

        public void OnFixedUpdate()
        {
            FixedUpdate?.Invoke();
        }
        #endregion
    }

}

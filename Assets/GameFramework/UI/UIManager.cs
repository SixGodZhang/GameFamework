//-----------------------------------------------------------------------
// <filename>UIManager</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> ## </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/5 星期三 16:57:45# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Taurus
{
    public class UIManager : IGameModule
    {
        #region 字段&属性
        /// <summary>
        /// 事件管理器
        /// </summary>
        private EventManager _event;

        /// <summary>
        /// 资源管理器
        /// </summary>
        private ResourceManager _resource;

        /// <summary>
        /// UI堆栈
        /// </summary>
        private Stack<UIInstance> _stackUIAsset = new Stack<UIInstance>();

        /// <summary>
        /// 所有已经加载的UIInstance与其对应的UIView
        /// </summary>
        private static Dictionary<UIInstance, UIView> _allUIView = new Dictionary<UIInstance, UIView>();

        /// <summary>
        /// 已经加载的UIInstance
        /// </summary>
        private static Dictionary<int, UIInstance> _allUIInstances = new Dictionary<int, UIInstance>();
        
        //所有的UI资源
        private static Dictionary<int, UIInstance> _allUIAssets = new Dictionary<int, UIInstance>();
        #endregion

        #region 构造函数
        public UIManager()
        {
            _event = GameModuleProxy.GetModule<EventManager>();
            _resource = GameModuleProxy.GetModule<ResourceManager>();
        }
        #endregion

        #region 外部接口
        /// <summary>
        /// 推一个UI界面到栈顶，并打开
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="allowMulti"></param>
        /// <param name="parameters"></param>
        public void Push<T>(bool allowMulti = false, params object[] parameters) where T : UIView
        {
            UIInstance config = GetUIInstance(typeof(T));
            if (config == null)
                return;

            //先将栈顶的UI界面置为暂停状态
            if (_stackUIAsset.Count > 0)
            {
                UIInstance lastConfig = _stackUIAsset.Peek();
                //若该UI已经打开，且不允许加载多个,则返回
                if (Equals(lastConfig, config) && !allowMulti)
                    return;

                IUIView uIView = GetUIView(lastConfig);

                UIEventArgs pauseArgs = new UIEventArgs();
                pauseArgs.UIView = uIView;
                _event.CallEvent(this, EventType.UIPause, pauseArgs);

                uIView.OnPause();
            }

            //UIInstance newConfig = null;
            //newConfig = config;

            //if (allowMulti)
            //    newConfig = new UIInstance(config.AssetBundleName, config.AssetPath);
            //else
            //    newConfig = config;

            _stackUIAsset.Push(config);
            UIView newUIView = GetUIView(config);
            newUIView.OnEnter(parameters);

            //触发打开事件
            UIEventArgs newargs = new UIEventArgs();
            newargs.UIView = newUIView;
            _event.CallEvent(this, EventType.UIEnter, newargs);
        }

        /// <summary>
        /// 从栈顶关闭一个UI界面，并确定是否销毁
        /// </summary>
        /// <param name="isdestroy"></param>
        public void Pop(bool isdestroy)
        {
            if (_stackUIAsset.Count > 0)
            {
                UIInstance lastConfig = _stackUIAsset.Pop();
                UIView lastView;
                if (_allUIView.TryGetValue(lastConfig, out lastView))
                {
                    //触发关闭事件
                    UIEventArgs exitArgs = new UIEventArgs();
                    exitArgs.UIView = lastView;
                    _event.CallEvent(this, EventType.UIExit, exitArgs);

                    lastView.OnExit();

                    if (isdestroy)
                    {
                        _allUIView.Remove(lastConfig);
                        MonoBehaviour.Destroy(lastView);
                    }
                    else {
                        lastView.gameObject.SetActive(false);
                    }
                }
            }

            //如果还有面板
            if (_stackUIAsset.Count > 0)
            {
                UIInstance lastAssetConfig = _stackUIAsset.Peek();
                UIView lastUiView;
                if (_allUIView.TryGetValue(lastAssetConfig, out lastUiView))
                {
                    lastUiView.OnResume();
                    //触发恢复事件
                    UIEventArgs resumeArgs = new UIEventArgs();
                    resumeArgs.UIView = lastUiView;
                    _event.CallEvent(this, EventType.UIResume, resumeArgs);
                }
            }
        }
        #endregion

        #region 内部函数
        /// <summary>
        /// 获取UI面板实例
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        private UIView GetUIView(UIInstance config)
        {
            UIView uiView = null;
            if (!_allUIView.TryGetValue(config, out uiView))
            {
                GameObject uiTemplate = _resource.LoadAsset<GameObject>(config.AssetBundleName, config.AssetPath);
                if (uiTemplate == null)
                    throw new Exception("uiview not found: " + config.AssetBundleName + " : " + config.AssetPath);
                GameObject uiInstance = GameObject.Instantiate(uiTemplate);
                uiView = uiInstance.GetComponent<UIView>();
                if (uiView == null)
                    return null;
                _allUIView[config] = uiView;
                return uiView;
            }

            uiView.gameObject.SetActive(true);
            return uiView;
        }

        /// <summary>
        /// 根据类型获取UI的资源信息
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private UIInstance GetUIInstance(Type t)
        {
            int hash = t.GetHashCode();

            UIInstance config = null;
            if (!_allUIInstances.TryGetValue(hash, out config))
            {
                object[] attrs = t.GetCustomAttributes(typeof(UIViewAttribute), false);
                if (attrs.Length == 0)
                    return null;
                UIViewAttribute uIViewAttribute = attrs[0] as UIViewAttribute;

                if (string.IsNullOrEmpty(uIViewAttribute?.ViewPath) || string.IsNullOrEmpty(uIViewAttribute.AssetBundleName))
                    return null;

                config = new UIInstance(uIViewAttribute.AssetBundleName, uIViewAttribute.ViewPath);

                _allUIInstances.Add(hash, config);
            }

            return config;
        }
        #endregion

        #region 重载函数
        public void OnClose()
        {
            //遍历栈，弹出所有UI面板
            while (_stackUIAsset.Count > 0)
            {
                UIInstance instance = _stackUIAsset.Pop();
                UIView view = _allUIView[instance];
                
                UIEventArgs exitArgs = new UIEventArgs();
                exitArgs.UIView = view;
                _event.CallEvent(this, EventType.UIExit, exitArgs);
                view.OnExit();
            }

            foreach (var item in _allUIView.Values)
            {
                MonoBehaviour.Destroy(item);
            }
            _allUIView.Clear();
        }
        #endregion

        #region 数据结构

        /// <summary>
        /// 资源配置
        /// </summary>
        private class UIInstance
        {
            public string AssetBundleName;
            public string AssetPath;

            public UIInstance() { }

            public UIInstance(string assetbundlename, string assetpath)
            {
                AssetBundleName = assetbundlename;
                AssetPath = assetpath;
            }
        }



        #endregion
    }
}

//-----------------------------------------------------------------------
// <filename>EventManager</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> ## </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/11/30 星期五 16:15:46# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.Taurus
{
    /// <summary>
    /// 事件管理类
    /// </summary>
    public sealed class EventManager : IGameModule
    {
        #region 字段&属性

        private static Dictionary<int, Action<System.Object, IEventArgs>> _allActions = new Dictionary<int, Action<System.Object, IEventArgs>>();

        #endregion 字段&属性

        #region 公开接口

        /// <summary>
        /// 获取事件ID
        /// 此事件ID属于应用程序级别，最高级别。可以通过<b>事件类型</b>调用
        /// 可以跨类被同时触发
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        public static int GetEventID(EventType eventType)
        {
            return (int)eventType;
        }

        /// <summary>
        /// 获取事件ID
        /// 此事件ID属于类级别,可以通过<b>类型+事件类型</b>调用
        /// 同一类型上同种事件会被同时触发
        /// </summary>
        /// <param name="type">挂在事件的类型</param>
        /// <param name="eventType">事件类型(触发类型)</param>
        /// <param name="istoeventtype">在类型为null值，是否转成 通过事件类型触发</param>
        /// <returns></returns>
        public static int GetEventID(Type type, EventType eventType, bool istoeventtype = false)
        {
            if (type == null && istoeventtype)
                return GetEventID(eventType);
            else if (type == null)
                return -1;
            else
                return type.GetHashCode() * (int)eventType;
        }

        /// <summary>
        /// 获取事件ID
        /// 此事件ID数据实例级别,可以通过<b>实例+事件类型调用</b>
        /// </summary>
        /// <param name="instance">实例</param>
        /// <param name="eventType">事件类型(触发类型)</param>
        /// <param name="istoeventtype">在类型为null值，是否转成 通过事件类型触发</param>
        /// <returns></returns>
        public static int GetEventID(System.Object instance, EventType eventType, bool istoeventtype = false)
        {
            if (instance == null && istoeventtype)
                return GetEventID(eventType);
            else if (instance == null)
                return -1;
            else
                return instance.GetHashCode() * (int)eventType;
        }

        ///// <summary>
        ///// 适用于Unity对象
        ///// </summary>
        ///// <param name="instance"></param>
        ///// <param name="eventType"></param>
        ///// <param name="istoeventtype"></param>
        ///// <returns></returns>
        //public static int GetEventID(UnityEngine.Object instance, EventType eventType, bool istoeventtype = false)
        //{
        //    if (instance == null && istoeventtype)
        //        return GetEventID(eventType);
        //    else if (instance == null)
        //        return -1;
        //    else
        //        return instance.GetInstanceID() * (int)eventType;
        //}

        /// <summary>
        /// 添加监听事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="handler">事件</param>
        private void AddListener(int eventid, Action<System.Object, IEventArgs> handler)
        {
            Action<System.Object, IEventArgs> eventHandler = null;
            if (!_allActions.TryGetValue(eventid, out eventHandler))
                _allActions[eventid] = handler;
            else
                eventHandler += handler;
        }

        /// <summary>
        /// 移除监听事件
        /// </summary>
        /// <param name="instance">实例</param>
        /// <param name="eventType">事件类型</param>
        /// <param name="handler">事件</param>
        private void RemoveListener(int eventid, Action<System.Object, IEventArgs> handler)
        {
            Action<System.Object, IEventArgs> eventHandler = null;
            if (!_allActions.TryGetValue(eventid, out eventHandler))
                return;
            else
            {
                eventHandler -= handler;
                if (eventHandler == null)
                    _allActions.Remove(eventid);
            }
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="eventid"></param>
        private void Trigger(System.Object sender, int eventid, IEventArgs eventArgs = null)
        {
            HanleEvent(sender, eventid, eventArgs);
        }

        #region 实例方法

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="eventType"></param>
        /// <param name="callback"></param>
        public void RemoveEvent(System.Object instance, EventType eventType, Action<System.Object, IEventArgs> callback)
        {
            int eventid = -1;
            if ((eventid = EventManager.GetEventID(instance, eventType)) != -1)
                this.RemoveListener(eventid, callback);
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="type"></param>
        /// <param name="eventType"></param>
        /// <param name="callback"></param>
        public void RemoveEvent(Type type, EventType eventType, Action<System.Object, IEventArgs> callback)
        {
            int eventid = -1;
            if ((eventid = EventManager.GetEventID(type, eventType)) != -1)
                this.RemoveListener(eventid, callback);
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callback"></param>
        public void RemoveEvent(EventType eventType, Action<System.Object, IEventArgs> callback)
        {
            int eventid = -1;
            if ((eventid = EventManager.GetEventID(eventType)) != -1)
                this.RemoveListener(eventid, callback);
        }

        /// <summary>
        /// 调用事件
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="sender"></param>
        /// <param name="eventType"></param>
        /// <param name="eventArgs"></param>
        public void CallEvent(System.Object instance, System.Object sender, EventType eventType, IEventArgs eventArgs = null)
        {
            int eventid = -1;
            if ((eventid = EventManager.GetEventID(instance, eventType)) != -1)
                this.Trigger(sender, eventid, eventArgs);
        }

        /// <summary>
        /// 调用事件
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sender"></param>
        /// <param name="eventType"></param>
        /// <param name="eventArgs"></param>
        public void CallEvent(Type type, System.Object sender, EventType eventType, IEventArgs eventArgs = null)
        {
            int eventid = -1;
            if ((eventid = EventManager.GetEventID(type, eventType)) != -1)
                this.Trigger(sender, eventid, eventArgs);
        }

        /// <summary>
        /// 调用事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventType"></param>
        /// <param name="eventArgs"></param>
        public void CallEvent(System.Object sender, EventType eventType, IEventArgs eventArgs = null)
        {
            int eventid = -1;
            if ((eventid = EventManager.GetEventID(eventType)) != -1)
                this.Trigger(sender, eventid, eventArgs);
        }

        /// <summary>
        /// 注册监听事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="callback"></param>
        public void RegisterEvent(EventType eventType, Action<System.Object, IEventArgs> callback)
        {
            int eventid = -1;
            if ((eventid = EventManager.GetEventID(eventType)) != -1)
                this.AddListener(eventid, callback);
        }

        /// <summary>
        /// 注册监听事件
        /// </summary>
        /// <param name="type"></param>
        /// <param name="eventType"></param>
        /// <param name="callback"></param>
        public void RegisterEvent(Type type, EventType eventType, Action<System.Object, IEventArgs> callback)
        {
            int eventid = -1;
            if ((eventid = EventManager.GetEventID(type, eventType)) != -1)
                this.AddListener(eventid, callback);
        }

        /// <summary>
        /// 注册监听事件
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="eventType"></param>
        /// <param name="callback"></param>
        public void RegisterEvent(System.Object instance, EventType eventType, Action<System.Object, IEventArgs> callback)
        {
            int eventid = -1;
            if ((eventid = EventManager.GetEventID(instance, eventType)) != -1)
                this.AddListener(eventid, callback);
        }



        #endregion 实例方法

        #endregion 公开接口

        #region 内部函数

        private void HanleEvent(System.Object sender, int eventid, IEventArgs eventArgs = null)
        {
            Action<System.Object, IEventArgs> eventHandler = null;
            if (!_allActions.TryGetValue(eventid, out eventHandler))
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError("找不到事件! \n " + System.Environment.StackTrace);
#endif
                return;
            }
            eventHandler(sender, eventArgs);
        }

        #endregion 内部函数

        #region 重载函数

        /// <summary>
        /// 清空所有事件
        /// </summary>
        public void OnClose()
        {
            _allActions.Clear();
        }

        #endregion 重载函数
    }
}
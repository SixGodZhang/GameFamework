//-----------------------------------------------------------------------
// <filename>GameFrameworkProxy</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> ## </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/11/30 星期五 12:48:30# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.Taurus
{
    public static class GameModuleProxy
    {
        #region 属性&字段
        //所有的游戏内容模块
        private static Dictionary<int, IGameModule> _allModules = new Dictionary<int, IGameModule>();
        //所有渲染帧函数
        private static List<IUpdate> _allUpdates = new List<IUpdate>();
        //所有固定渲染帧函数
        private static List<IFixedUpdate> _allFixedUpdates = new List<IFixedUpdate>();
        #endregion

        #region 外部接口
        /// <summary>
        /// 获取MOdule
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetModule<T>() where T : IGameModule, new()
        {
            int hash = typeof(T).GetHashCode();

            if (_allModules.ContainsKey(hash))
                return (T)_allModules[hash];
            return (T)CreateModule(typeof(T));
        }

        /// <summary>
        /// 关闭所有模块
        /// </summary>
        public static void ShutDownAll()
        {
            foreach (var item in _allModules.Values)
                item.OnClose();
            _allUpdates.Clear();
            _allFixedUpdates.Clear();
            _allModules.Clear();
        }

        /// <summary>
        /// 渲染帧函数
        /// </summary>
        public static void Update()
        {
            foreach (var item in _allUpdates)
                item.OnUpdate();
        }

        /// <summary>
        /// 固定渲染帧函数
        /// </summary>
        public static void FixedUpdate()
        {
            foreach (var item in _allFixedUpdates)
                item.OnFixedUpdate();
        }
        #endregion

        #region 内部函数
        private static IGameModule CreateModule(Type type)
        {
            int hash = type.GetHashCode();
            IGameModule gameModule = (IGameModule)Activator.CreateInstance(type);
            _allModules[hash] = gameModule;

            //获取含IUpdate的模块
            var update = gameModule as IUpdate;
            if (update != null)
                _allUpdates.Add(update);

            //获取含IFixedUpdate的模块
            var fixedUpdate = gameModule as IFixedUpdate;
            if (fixedUpdate != null)
                _allFixedUpdates.Add(fixedUpdate);

            return gameModule;
        }
        #endregion
    }
}

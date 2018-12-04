//-----------------------------------------------------------------------
// <filename>GameStateManager</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #游戏状态管理类# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/11/30 星期五 11:53:08# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;

namespace GameFramework.Taurus
{
    [GameState(GameStateType.Ignore,Desc:"状态管理模块,不属于具体游戏内容模块")]
    public sealed class GameStateManager : IGameModule, IUpdate, IFixedUpdate
    {
        #region 字段&属性
        private GameStateContext _stateContext;
        private GameState _startState;
        public GameState CurrentState
        {
            get
            {
                if (_stateContext == null)
                    return null;
                return _stateContext.CurrentState;
            }
        }
        #endregion

        #region 外部接口
        /// <summary>
        /// 创建游戏状态上下文，并初始化所有游戏状态
        /// </summary>
        /// <param name="assembly"></param>
        public void CreateStateContext(Assembly assembly)
        {
            if (_stateContext != null)
                return;

            List<GameState> states = new List<GameState>();
            Type[] types = assembly.GetTypes();
            foreach (var item in types)
            {
                object[] attribute = item.GetCustomAttributes(typeof(GameStateAttribute), false);
                if (attribute.Length <= 0 || item.IsAbstract)
                    continue;
                GameStateAttribute stateAttribute = attribute[0] as GameStateAttribute;
                if (stateAttribute.StateType == GameStateType.Ignore)
                    continue;
                GameState gs = Activator.CreateInstance(item) as GameState;
                if (gs!=null)
                {
                    states.Add(gs);
                    if (stateAttribute.StateType == GameStateType.Start)
                        _startState = gs;
                }
            }

            _stateContext = new GameStateContext();
            _stateContext.SetAllState(states);

        }

        /// <summary>
        /// 启动第一个状态
        /// </summary>
        public void StartFirstState()
        {
            if (_stateContext != null && _startState != null)
                _stateContext.SetStartState(_startState);
        }

        #endregion

        #region 重写函数
        public void OnClose()
        {
            _stateContext.CloseAll();
            _stateContext = null;
        }

        public void OnFixedUpdate()
        {
            if (_stateContext != null)
                _stateContext.FixedUpdate();
        }

        public void OnUpdate()
        {
            if (_stateContext != null)
                _stateContext.Update();
        }
        #endregion
    }
}

//-----------------------------------------------------------------------
// <filename>GameStateContext</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #游戏状态上下文# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/11/30 星期五 11:09:31# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.Taurus
{
    /// <summary>
    /// 游戏状态上下文(1.用于控制游戏的当前状态 2. 游戏状态之间的切换)
    /// </summary>
    public class GameStateContext
    {

        #region 字段 + 属性
        //所有状态
        private static Dictionary<int, GameState> _allStates = new Dictionary<int, GameState>();
        //当前状态
        private GameState _currentState;

        public GameState CurrentState
        {
            get { return _currentState; }
        }
        #endregion

        #region 外部接口
        /// <summary>
        /// 设置所有的状态(用于Game初始化时期)
        /// </summary>
        /// <param name="states"></param>
        public void SetAllState<T>(IEnumerable<T> states) where T:GameState
        {
            foreach (var item in states)
            {
                int hash = item.GetType().GetHashCode();
                if (!_allStates.ContainsKey(hash))
                {
                    item.OnInit(this);
                    _allStates[hash] = item;
                }
            }
        }

        /// <summary>
        /// 设置第一个状态(泛型)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        public void SetStartState<T>(params object[] parameters) where T : GameState
        {
            if (_currentState == null)
            {
                int hash = typeof(T).GetHashCode();
                if (_allStates.ContainsKey(hash))
                {
                    _currentState = _allStates[hash];
                    _currentState.OnEnter(parameters);
                }
            }
        }

        /// <summary>
        /// 设置第一个状态(非泛型)
        /// </summary>
        /// <param name="parameters"></param>
        public void SetStartState(GameState state, params object[] parameters)
        {
            if (_currentState == null && _allStates.ContainsValue(state))
            {
                _currentState = state;
                state.OnEnter(parameters);
            }
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        public void ChangeState<T>(params object[] parameters) where T : GameState
        {
            int hash = typeof(T).GetHashCode();
            if (_allStates.ContainsKey(hash))
            {
                if (_currentState != null)
                    _currentState.OnExit();
                _currentState = _allStates[hash];
                _currentState.OnEnter(parameters);
            }
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="state">需要切换的目标状态</param>
        /// <param name="parameters"></param>
        public void ChangeState(GameState state, params object[] parameters)
        {
            if (_allStates.ContainsValue(state))
            {
                if (_currentState != null)
                    _currentState.OnExit();
                _currentState = state;
                _currentState.OnEnter(parameters);
            }
        }

        /// <summary>
        /// 渲染帧函数
        /// </summary>
        public void Update()
        {
            if (_currentState != null)
                _currentState.OnUpdate();
        }

        /// <summary>
        /// 渲染帧函数
        /// </summary>
        public void FixedUpdate()
        {
            if (_currentState != null)
                _currentState.OnFixedUpdate();
        }

        /// <summary>
        /// 清空所有的状态
        /// </summary>
        public void CloseAll()
        {
            foreach (var item in _allStates.Values)
                item.OnExit();
            _allStates.Clear();
        }
        #endregion


    }
}

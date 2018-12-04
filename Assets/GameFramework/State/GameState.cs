//-----------------------------------------------------------------------
// <filename>GameState</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #游戏状态# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/11/30 星期五 11:08:42# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.Taurus
{
    public abstract class GameState
    {
        //游戏状态上下文
        private GameStateContext _context;

        /// <summary>
        /// 初始化:只执行一次
        /// </summary>
        public virtual void OnInit(GameStateContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="parameters"></param>
        public virtual void OnEnter(params object[] parameters)
        {

        }

        public virtual void OnExit()
        {

        }

        /// <summary>
        /// 渲染帧函数
        /// </summary>
        public virtual void OnUpdate()
        {

        }

        /// <summary>
        /// 固定帧函数
        /// </summary>
        public virtual void OnFixedUpdate()
        {

        }

        /// <summary>
        /// 切换游戏状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        protected void ChangeState<T>(params object[] parameters) where T : GameState
        {
            if (_context != null)
                _context.ChangeState<T>(parameters);
        }

    }
}

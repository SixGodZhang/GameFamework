//-----------------------------------------------------------------------
// <filename>GameStateAttribute</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #游戏状态特性# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/11/30 星期五 11:45:36# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.Taurus
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = false,Inherited = true)]
    public class GameStateAttribute:Attribute
    {
        public GameStateType StateType
        {
            get;
            private set;
        }

        public string GameStateDesc
        {
            get;
            private set;
        }

        public GameStateAttribute()
        {
            StateType = GameStateType.Generanal;
            GameStateDesc = "游戏状态模块";
        }

        public GameStateAttribute(GameStateType gameStateType,string Desc ="游戏状态模块")
        {
            StateType = gameStateType;
            GameStateDesc = Desc;
        }
    }

    /// <summary>
    /// 游戏状态类型
    /// </summary>
    public enum GameStateType
    {
        /// <summary>
        /// 一般的
        /// </summary>
        Generanal = 0,
        /// <summary>
        /// 忽略
        /// </summary>
        Ignore,
        /// <summary>
        /// 起始状态
        /// </summary>
        Start
    }
}

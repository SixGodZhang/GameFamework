//-----------------------------------------------------------------------
// <filename>GameException</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #GameFamework异常类# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/3 星期一 11:04:31# </time>
//-----------------------------------------------------------------------

using System;
using System.Runtime.Serialization;

namespace GameFramework.Taurus
{
    [Serializable]
    public class GameException:Exception
    {
        public GameException() : base()
        {

        }

        /// <summary>
        /// 用指定错误消息初始化游戏框架异常类的新实例
        /// </summary>
        /// <param name="message"></param>
        public GameException(string message) : base(message)
        {

        }

        /// <summary>
        /// 使用指定错误消息和对作为此异常原因的内部异常的引用来初始化游戏框架异常类的新实例。
        /// </summary>
        /// <param name="message">解释异常原因的错误消息</param>
        /// <param name="exception">导致当前异常的异常。如果 exception 参数不为空引用，则在处理内部异常的 catch 块中引发当前异常。</param>
        public GameException(string message, Exception exception) : base(message, exception)
        {

        }

        /// <summary>
        /// 用序列化数据初始化游戏框架异常类的新实例
        /// </summary>
        /// <param name="info">存有有关所引发异常的序列化的对象数据</param>
        /// <param name="context">包含有关源或目标的上下文信息</param>
        protected GameException(SerializationInfo info, StreamingContext context):base(info,context)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATFramework.Main
{
    public static partial class ATExtension
    {
        /// <summary>
        /// 扩展方法,判断类型是否为Int类型
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsIntegerType(this Type t)
        {
            return (t == typeof(int) ||
                    t == typeof(short) ||
                    t == typeof(ushort) ||
                    t == typeof(byte) ||
                    t == typeof(sbyte) ||
                    t == typeof(uint) ||
                    t == typeof(long) ||
                    t == typeof(ulong) ||
                    t == typeof(char) ||
                    t == typeof(bool));
        }
    }
}

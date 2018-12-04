using ATFramework.Log;
using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Reflection;

namespace ATFramework.Main
{
    /// <summary>
    /// 枚举操作
    /// </summary>
    public static class ATEnum
    {
        /// <summary>
        /// 枚举中是否包含,相比于Enum.IsDefine,在字符串比较时区分大小写
        /// </summary>
        /// <typeparam name="T">指定的枚举类型</typeparam>
        /// <param name="enumValue">枚举实例</param>
        /// <returns></returns>
        /// enum Permission
        /// {
        ///     Normal,
        ///     Vip
        /// }
        /// Enum.IsDefined(typeof(Permission),"Normal")   true
        /// Enum.IsDefined(typeof(Permission), "normal") false
        /// ATEnum.Constains<Permission>("normal") true
        public static bool Constains<T>(object enumValue)
        {
            Type valueType = enumValue.GetType();
            if (valueType == typeof(string))
            {
                string[] names = ATEnum.GetNames<T>();
                string value = enumValue.ToString().ToLower();
                for (int i = 0; i < names.Length; i++)
                {
                    if (value.Equals(names[i].ToLower()))
                    {
                        return true;
                    }
                }
            }

            if (valueType.IsIntegerType())
            {
                return ATEnum.BinarySearch<T>(ATEnum.GetValues<T>(), enumValue) >= 0;
            }

            ATLog.Error("参数类型错误!" + ATStackInfo.GetCurrentStackInfo());
            throw new ArgumentException("参数类型错误!" + ATStackInfo.GetCurrentStackInfo());
        }

        /// <summary>
        /// 枚举的值转化为UInt64
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static ulong ToUInt64(Object value)
        {
            TypeCode typeCode = Convert.GetTypeCode(value);
            ulong result;

            switch (typeCode)
            {
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    result = (UInt64)Convert.ToInt64(value, CultureInfo.InvariantCulture);
                    break;

                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Boolean:
                case TypeCode.Char:
                    result = Convert.ToUInt64(value, CultureInfo.InvariantCulture);
                    break;

                default:
                    ATLog.Error("Invalid Object type in ToUInt64");
                    throw new InvalidOperationException("InvalidOperation_UnknownEnumType");
            }
            return result;
        }

        /// <summary>
        /// 二分法查找值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int BinarySearch<T>(Array array, object value)
        {
            Type type = typeof(T);
            if (!type.GetTypeInfo().IsEnum)
            {
                ATLog.Error("泛型参数不是枚举类型:\n" + ATStackInfo.GetCurrentStackInfo());
                throw new ArgumentException("泛型参数不是枚举类型:\n" + ATStackInfo.GetCurrentStackInfo());
            }

            ulong[] ulArray = new ulong[array.Length];
            for (int i = 0; i < array.Length; i++)
                ulArray[i] = ToUInt64(array.GetValue(i));

            ulong ulValue = ToUInt64(value);

            return Array.BinarySearch(ulArray, ulValue);
        }

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="obj">成员名称或值</param>
        ///
        /// //声明
        /// enum TestEnum
        /// {
        ///     A = 0
        /// }
        ///
        /// 示例调用:
        /// TestEnum type = ATEnum.Parse<TestEnum>("A");
        ///
        /// <returns>obj对应枚举实例中的成员</returns>
        public static T GetValue<T>(object obj)
        {
            Contract.Requires(obj != null);
            Contract.Requires(Contract.Result<T>() != null);
            Type type = typeof(T);
            if (!type.GetTypeInfo().IsEnum)
            {
                ATLog.Error("泛型参数不是枚举类型:\n" + ATStackInfo.GetCurrentStackInfo());
                throw new ArgumentException("泛型参数不是枚举类型:\n" + ATStackInfo.GetCurrentStackInfo());
            }

            string value = obj.ToString();
            return (T)Enum.Parse(type, value, true);
        }

        /// <summary>
        /// 获取枚举中所有成员的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] GetValues<T>()
        {
            Type type = typeof(T);
            if (!type.GetTypeInfo().IsEnum)
            {
                ATLog.Error("泛型参数不是枚举类型:\n" + ATStackInfo.GetCurrentStackInfo());
                throw new ArgumentException("泛型参数不是枚举类型:\n" + ATStackInfo.GetCurrentStackInfo());
            }

            return (T[])Enum.GetValues(type);
        }

        /// <summary>
        /// 获取枚举成员名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetName<T>(object obj)
        {
            Type type = typeof(T);
            if (!type.GetTypeInfo().IsEnum)
            {
                ATLog.Error("泛型参数不是枚举类型:\n" + ATStackInfo.GetCurrentStackInfo());
                throw new ArgumentException("泛型参数不是枚举类型:\n" + ATStackInfo.GetCurrentStackInfo());
            }

            return Enum.GetName(type, obj);
        }

        /// <summary>
        /// 获取枚举中所有成员名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string[] GetNames<T>()
        {
            Type type = typeof(T);
            if (!type.GetTypeInfo().IsEnum)
            {
                ATLog.Error("泛型参数不是枚举类型:\n" + ATStackInfo.GetCurrentStackInfo());
                throw new ArgumentException("泛型参数不是枚举类型:\n" + ATStackInfo.GetCurrentStackInfo());
            }

            return Enum.GetNames(type);
        }
    }
}
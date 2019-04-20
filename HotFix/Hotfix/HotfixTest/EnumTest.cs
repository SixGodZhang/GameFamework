using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// 目前测试只针对无参数无返回值，静态方法的情况
/// 有参数会导致IL类型在解析方法的时候增加，此后到时分析测试单元出错
/// </summary>
namespace Hotfix.Hotfix.HotfixTest
{
    /// <summary>
    /// 枚举测试
    /// </summary>
    class EnumTest
    {
        #region 枚举定义
        public enum TestEnum : long
        {
            Enum1,
            Enum2,
            Enum4 = 0x123456789
        }

        //enum TestEnum2 : ulong
        //{
        //    Enum1,
        //    Enum2,
        //    Enum3234 = 0x123456789,
        //}

        //enum TestEnum3 : byte
        //{
        //    Enum1bbb,
        //    Enum2bbb,
        //}

        //enum TestEnum4 : ushort
        //{
        //    零 = 0,
        //    One = 1,
        //    佰 = 100
        //}

        //enum TestEnumUint : uint
        //{
        //    Zero = 0,
        //    UOne = 1,
        //    Max = uint.MaxValue
        //}

        //enum TestEnumInt : int
        //{
        //    Min = int.MinValue,
        //    Max = int.MaxValue
        //}

        //enum TestEnumSByte : sbyte
        //{
        //    Min = sbyte.MinValue,
        //    Max = sbyte.MaxValue
        //}
        #endregion

        //static TestEnum b = TestEnum.Enum2;

        public static void Test01()
        {
            TestEnum a = TestEnum.Enum4;
            UnityEngine.Debug.Log("a= " + a);
        }

        public static void Test02()
        {
            TestEnum a = TestEnum.Enum4;
            UnityEngine.Debug.Log("a= " + a);
        }

        public static void Test03()
        {
            TestEnum a = TestEnum.Enum4;
            UnityEngine.Debug.Log("a= " + a);
        }

        public static void Test04()
        {
            TestEnum a = TestEnum.Enum4;
            UnityEngine.Debug.Log("a= " + a);
        }

        public static void Test05()
        {
            TestEnum a = TestEnum.Enum4;
            UnityEngine.Debug.Log("a= " + a);
        }
    }
}

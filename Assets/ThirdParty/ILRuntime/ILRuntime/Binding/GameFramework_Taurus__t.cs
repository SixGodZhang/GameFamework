using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;

namespace ILRuntime.Runtime.Generated
{
    unsafe class GameFramework_Taurus_TestDelegate_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(GameFramework.Taurus.TestDelegate);

            field = type.GetField("callBack", flag);
            app.RegisterCLRFieldGetter(field, get_callBack_0);
            app.RegisterCLRFieldSetter(field, set_callBack_0);


        }



        static object get_callBack_0(ref object o)
        {
            return GameFramework.Taurus.TestDelegate.callBack;
        }
        static void set_callBack_0(ref object o, object v)
        {
            GameFramework.Taurus.TestDelegate.callBack = (GameFramework.Taurus.TestCallBack)v;
        }


    }
}

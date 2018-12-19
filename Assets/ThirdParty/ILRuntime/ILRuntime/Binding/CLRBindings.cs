using System;
using System.Collections.Generic;
using System.Reflection;

namespace ILRuntime.Runtime.Generated
{
    public class CLRBindings
    {


        /// <summary>
        /// Initialize the CLR binding, please invoke this AFTER CLR Redirection registration
        /// </summary>
        public static void Initialize(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            System_Object_Binding.Register(app);
            System_String_Binding.Register(app);
            System_Activator_Binding.Register(app);
            UnityEngine_Debug_Binding.Register(app);
            System_Type_Binding.Register(app);
            System_Action_3_Int32_String_Boolean_Binding.Register(app);
            GameFramework_Taurus_TestDelegate_Binding.Register(app);
            GameFramework_Test_TestClass_Binding.Register(app);

            ILRuntime.CLR.TypeSystem.CLRType __clrType = null;
        }

        /// <summary>
        /// Release the CLR binding, please invoke this BEFORE ILRuntime Appdomain destroy
        /// </summary>
        public static void Shutdown(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
        }
    }
}

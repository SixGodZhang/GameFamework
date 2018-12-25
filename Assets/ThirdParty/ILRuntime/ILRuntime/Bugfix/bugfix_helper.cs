
using System;

namespace ILRuntime
{
    public partial class BugfixHelper
    {
        public static void Init(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            if (app == null)
            {
                // should log error
                return;
            }

			// delegate register 
						
			app.DelegateManager.RegisterMethodDelegate<System.Object>();
			
			app.DelegateManager.RegisterMethodDelegate<System.Object, System.Int32>();
			
			app.DelegateManager.RegisterFunctionDelegate<System.Object, System.String>();


			// delegate convertor
            
            app.DelegateManager.RegisterDelegateConvertor<method_delegate0>((action) =>
            {
                return new method_delegate0((arg0) =>
                {
                    ((Action<System.Object>)action)(arg0);
                });
            });
            app.DelegateManager.RegisterDelegateConvertor<method_delegate1>((action) =>
            {
                return new method_delegate1((arg0, arg1) =>
                {
                    ((Action<System.Object, System.Int32>)action)(arg0, arg1);
                });
            });            
            app.DelegateManager.RegisterDelegateConvertor<method_delegate2>((action) =>
            {
                return new method_delegate2((arg0, arg1) =>
                {
                    return ((Func<System.Object, System.String,System.Int32>)action)(arg0, arg1);
                });
            });

        }
    }
}
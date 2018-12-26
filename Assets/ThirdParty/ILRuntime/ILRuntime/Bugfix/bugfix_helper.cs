
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
						
			app.DelegateManager.RegisterMethodDelegate<System.Object, System.String, System.Int32>();


			// delegate convertor
            
            app.DelegateManager.RegisterDelegateConvertor<method_delegate0>((action) =>
            {
                return new method_delegate0((arg0, arg1, arg2) =>
                {
                    ((Action<System.Object, System.String, System.Int32>)action)(arg0, arg1, arg2);
                });
            });
        }
    }
}
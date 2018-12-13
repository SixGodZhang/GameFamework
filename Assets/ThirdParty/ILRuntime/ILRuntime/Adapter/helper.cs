
using System;

namespace ILRuntime
{
    class ILRuntimeHelper
    {
        public static void Init(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            if (app == null)
            {
                // should log error
                return;
            }

			// adaptor register 
               

			// interface adaptor register
			

			// delegate register 
						
			app.DelegateManager.RegisterMethodDelegate<System.Int32,System.String,System.Boolean>();


			// delegate convertor
            
        }
    }
}
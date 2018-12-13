
using GameFramework.Test;
using System;

namespace ILRuntime
{
    public class ILRuntimeHelper
    {
        public static void Init(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            if (app == null)
            {
                // should log error
                return;
            }

			// adaptor register 
                        
			app.RegisterCrossBindingAdaptor(new SubMonoBehaviorAdaptor());            
			app.RegisterCrossBindingAdaptor(new TestClassAdaptor());   

			// interface adaptor register
			            
			app.RegisterCrossBindingAdaptor(new IDisposableAdaptor());

			// delegate register 
			

			// delegate convertor
            
        }
    }
}
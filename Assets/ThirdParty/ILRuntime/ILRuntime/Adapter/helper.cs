
using GameFramework.Taurus;
using GameFramework.Test;
using MyTestLibrary;
using System;

namespace System
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
                                   
			app.RegisterCrossBindingAdaptor(new TestBaseAdaptor());            
			app.RegisterCrossBindingAdaptor(new SubMonoBehaviorAdaptor());            
			app.RegisterCrossBindingAdaptor(new TestClassAdaptor());   

			// interface adaptor register
			            
			app.RegisterCrossBindingAdaptor(new IDisposableAdaptor());            
			app.RegisterCrossBindingAdaptor(new ITestAdaptor());            
			app.RegisterCrossBindingAdaptor(new IUIViewAdaptor());

			// delegate register 
			

			// delegate convertor
            
        }
    }
}
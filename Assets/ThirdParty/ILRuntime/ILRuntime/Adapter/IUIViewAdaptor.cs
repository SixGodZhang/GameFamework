using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;

namespace GameFramework.Taurus
{
	public class IUIViewAdaptor : CrossBindingAdaptor
	{
        public override Type BaseCLRType
        {
            get
            {
                return typeof(IUIView);
            }
        }

        public override Type AdaptorType
        {
            get
            {
                return typeof(Adaptor);
            }
        }

        public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            return new Adaptor(appdomain, instance);
        }

		internal class Adaptor : IUIView, CrossBindingAdaptorType
        {
            ILTypeInstance instance;
            ILRuntime.Runtime.Enviorment.AppDomain appdomain;

            public Adaptor()
            {

            }

            public Adaptor(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
            {
                this.appdomain = appdomain;
                this.instance = instance;
            }

            public ILTypeInstance ILInstance { get { return instance; } }

            
            IMethod mOnEnter;
            public void OnEnter(Object[] parameters)
            {
                if(mOnEnter == null)
                {
                    mOnEnter = instance.Type.GetMethod("OnEnter", 1);
                }
                if (mOnEnter != null)
                    appdomain.Invoke(mOnEnter, instance ,parameters);
            }

            IMethod mOnExit;
            public void OnExit()
            {
                if(mOnExit == null)
                {
                    mOnExit = instance.Type.GetMethod("OnExit", 0);
                }
                if (mOnExit != null)
                    appdomain.Invoke(mOnExit, instance );
            }

            IMethod mOnPause;
            public void OnPause()
            {
                if(mOnPause == null)
                {
                    mOnPause = instance.Type.GetMethod("OnPause", 0);
                }
                if (mOnPause != null)
                    appdomain.Invoke(mOnPause, instance );
            }

            IMethod mOnResume;
            public void OnResume()
            {
                if(mOnResume == null)
                {
                    mOnResume = instance.Type.GetMethod("OnResume", 0);
                }
                if (mOnResume != null)
                    appdomain.Invoke(mOnResume, instance );
            }

            
            
            public override string ToString()
            {
                IMethod m = appdomain.ObjectType.GetMethod("ToString", 0);
                m = instance.Type.GetVirtualMethod(m);
                if (m == null || m is ILMethod)
                {
                    return instance.ToString();
                }
                else
                    return instance.Type.FullName;
            }
        }
    }
}
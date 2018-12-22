using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;

namespace MyTestLibrary
{
	public class ITestAdaptor : CrossBindingAdaptor
	{
        public override Type BaseCLRType
        {
            get
            {
                return typeof(ITest);
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

		internal class Adaptor : ITest, CrossBindingAdaptorType
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

            
            IMethod mDoAction;
            public void DoAction()
            {
                if(mDoAction == null)
                {
                    mDoAction = instance.Type.GetMethod("DoAction", 0);
                }
                if (mDoAction != null)
                    appdomain.Invoke(mDoAction, instance );
            }

            IMethod mSayHello;
            public void SayHello()
            {
                if(mSayHello == null)
                {
                    mSayHello = instance.Type.GetMethod("SayHello", 0);
                }
                if (mSayHello != null)
                    appdomain.Invoke(mSayHello, instance );
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
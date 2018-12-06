//-----------------------------------------------------------------------
// <filename>SubMonoBehaviourAdaper</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #SubMonoBehaviour适配器# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/6 星期四 18:09:24# </time>
//-----------------------------------------------------------------------

using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using System;

namespace ILRuntime.Runtime.Adaptors
{
    public class SubMonoBehaviourAdaper : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(SubMonoBehavior);
            }
        }

        public override Type AdaptorType
        {
            get
            {
                return typeof(Adaptor);
            }
        }

        public override object CreateCLRInstance(Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            return new Adaptor(appdomain, instance);
        }

        private class Adaptor : SubMonoBehavior, CrossBindingAdaptorType
        {
            ILTypeInstance instance;
            ILRuntime.Runtime.Enviorment.AppDomain appdomain;

            //缓存这个数组来避免调用时的GC Alloc
            object[] param1 = new object[1];
            public Adaptor()
            {

            }

            public ILTypeInstance ILInstance
            {
                get
                {
                    return instance;
                }
            }

            public Adaptor(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
            {
                this.appdomain = appdomain;
                this.instance = instance;
            }

            bool isStartGot;
            IMethod toStart;
            public override void Start()
            {
                if (!isStartGot)
                {
                    isStartGot = true;
                    toStart = instance.Type.GetMethod("Start", 0);
                }

                if (toStart != null)
                {
                    appdomain.Invoke(toStart, instance, null);
                }
            }

            bool isUpdateGot;
            IMethod toUpdate;
            public override void Update()
            {
                if (!isUpdateGot)
                {
                    isUpdateGot = true;
                    toUpdate = instance.Type.GetMethod("Update", 0);
                }

                if (toUpdate != null)
                {
                    appdomain.Invoke(toUpdate, instance, null);
                }
            }

            bool isOnFixedUpdateGot;
            IMethod toOnFixedUpdate;
            public override void OnFixedUpdate()
            {
                if (!isOnFixedUpdateGot)
                {
                    isOnFixedUpdateGot = true;
                    toOnFixedUpdate = instance.Type.GetMethod("OnFixedUpdate", 0);
                }

                if (toOnFixedUpdate != null)
                {
                    appdomain.Invoke(toOnFixedUpdate, instance, null);
                }
            }

            bool isOnDestroyGot;
            IMethod toOnDestroy;
            public override void OnDestroy()
            {
                if (!isOnDestroyGot)
                {
                    isOnDestroyGot = true;
                    toOnDestroy = instance.Type.GetMethod("OnDestroy", 0);
                }

                if (toOnDestroy != null)
                {
                    appdomain.Invoke(toOnDestroy, instance, null);
                }
            }

            bool isOnApplicationQuit;
            IMethod toOnApplicationQuit;
            public override void OnApplicationQuit()
            {
                if (!isOnApplicationQuit)
                {
                    isOnApplicationQuit = true;
                    toOnApplicationQuit = instance.Type.GetMethod("OnApplicationQuit", 0);
                }

                if (toOnApplicationQuit != null)
                {
                    appdomain.Invoke(toOnApplicationQuit, instance, null);
                }
            }
        }
    }

}

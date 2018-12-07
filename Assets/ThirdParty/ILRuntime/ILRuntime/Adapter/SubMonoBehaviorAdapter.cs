//=======================================================
//Copyright(C) None rights reserved
//<describe> #适配器文件# </describe>
//<email> whdhxyzh@gmail.com </email>
//<time> #2018/12/7 11:16:03# </time>
//=======================================================
using System;
using System.Collections;
using System.Collections.Generic;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
public class SubMonoBehaviorAdapter:CrossBindingAdaptor
{
public override Type BaseCLRType
{
    get
    {
        return typeof(SubMonoBehavior);//这是你想继承的那个类
    }
}
public override Type AdaptorType
{
    get
    {
        return typeof(Adaptor);//这是实际的适配器类
    }
}
public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
{
    return new Adaptor(appdomain, instance);//创建一个新的实例
}
//实际的适配器类需要继承你想继承的那个类，并且实现CrossBindingAdaptorType接口
public class Adaptor : SubMonoBehavior, CrossBindingAdaptorType
{
    ILTypeInstance instance;
    ILRuntime.Runtime.Enviorment.AppDomain appdomain;
    //缓存这个数组来避免调用时的GC Alloc
    object[] param1 = new object[1];
    public Adaptor()
    {

    }
    public Adaptor(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
    {
        this.appdomain = appdomain;
        this.instance = instance;
    }
    public ILTypeInstance ILInstance { get { return instance; } }
bool m_bStartGot = false;
IMethod m_Start = null;
public override void Start ()
{
   if(!m_bStartGot)
   {
       m_Start = instance.Type.GetMethod("Start",0);
       m_bStartGot = true;
   }
          if(m_Start != null)
       {
            appdomain.Invoke(m_Start, instance,null);
        }
       else
       {
           
       } 
}
bool m_bUpdateGot = false;
IMethod m_Update = null;
public override void Update ()
{
   if(!m_bUpdateGot)
   {
       m_Update = instance.Type.GetMethod("Update",0);
       m_bUpdateGot = true;
   }
          if(m_Update != null)
       {
            appdomain.Invoke(m_Update, instance,null);
        }
       else
       {
           
       } 
}
bool m_bOnFixedUpdateGot = false;
IMethod m_OnFixedUpdate = null;
public override void OnFixedUpdate ()
{
   if(!m_bOnFixedUpdateGot)
   {
       m_OnFixedUpdate = instance.Type.GetMethod("OnFixedUpdate",0);
       m_bOnFixedUpdateGot = true;
   }
          if(m_OnFixedUpdate != null)
       {
            appdomain.Invoke(m_OnFixedUpdate, instance,null);
        }
       else
       {
           
       } 
}
bool m_bOnDestroyGot = false;
IMethod m_OnDestroy = null;
public override void OnDestroy ()
{
   if(!m_bOnDestroyGot)
   {
       m_OnDestroy = instance.Type.GetMethod("OnDestroy",0);
       m_bOnDestroyGot = true;
   }
          if(m_OnDestroy != null)
       {
            appdomain.Invoke(m_OnDestroy, instance,null);
        }
       else
       {
           
       } 
}
bool m_bOnApplicationQuitGot = false;
IMethod m_OnApplicationQuit = null;
public override void OnApplicationQuit ()
{
   if(!m_bOnApplicationQuitGot)
   {
       m_OnApplicationQuit = instance.Type.GetMethod("OnApplicationQuit",0);
       m_bOnApplicationQuitGot = true;
   }
          if(m_OnApplicationQuit != null)
       {
            appdomain.Invoke(m_OnApplicationQuit, instance,null);
        }
       else
       {
           
       } 
}
}
}
//=======================================================
//Copyright(C) None rights reserved
//<describe> #适配器注册文件# </describe>
//<email> whdhxyzh@gmail.com </email>
//<time> #2018/12/7 11:16:03# </time>
//=======================================================
using System;
using System.Collections;
using System.Collections.Generic;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
public class AdapterRegister
    {
        public static void RegisterCrossBindingAdaptor(ILRuntime.Runtime.Enviorment.AppDomain domain)
        {
domain.RegisterCrossBindingAdaptor(new SubMonoBehaviorAdapter());
}
}
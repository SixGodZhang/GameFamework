//-----------------------------------------------------------------------
// <filename>ILRuntimeCLRBinding</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #ILRuntime自动生成适配器工具类# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/3 星期一 15:58:46# </time>
//-----------------------------------------------------------------------

#if UNITY_EDITOR
using GameFramework.Taurus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[System.Reflection.Obfuscation(Exclude = true)]
public class ILRuntimeCLRBinding
{
    /// <summary>
    /// 清理CLR类型绑定
    /// </summary>
    [MenuItem("ILRuntime/Clear CLR Binding Code")]
    static void ClearCLRTypeBinding()
    {
        if (!EditorUtility.DisplayDialog("提示", "是否清楚所有CLR类型绑定?", "是", "否"))
            return;

        if (Directory.Exists(GeneratorConfig.GenCLRBindingTrunkPath))
        {
            string[] files = Directory.GetFiles(GeneratorConfig.GenCLRBindingTrunkPath, "*.cs");
            foreach (var file in files)
            {
                File.Delete(file);
            }

            string copyFile = Path.GetFullPath(GeneratorConfig.GenCLRBindingTrunkPath + "/CLRBindings.cs_");
            string destFile = Path.GetFullPath(GeneratorConfig.GenCLRBindingTrunkPath + "/CLRBindings.cs");
            if (File.Exists(copyFile))
            {
                File.Copy(copyFile, destFile, true);
                Debug.Log("清理类型绑定完成,请等待编译通过");
                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogError("文件没有找到：" + copyFile);
            }

        }
    }

    /// <summary>
    /// 生成CLR类型绑定
    /// </summary>
    [MenuItem("ILRuntime/Generate CLR Binding Code")]
    static void GenerateCLRTypeBinding()
    {
        if (!EditorUtility.DisplayDialog("提示", "是否生成CLR类型的帮定", "是", "否"))
            return;

        List<Type> types = new List<Type>();
        types.Add(typeof(List<ILRuntime.Runtime.Intepreter.ILTypeInstance>));
        //导出主工程中的类型
        types.AddRange(ExportGameDllTyps());
        //导出Unity中的类型
        types.AddRange(ExportUnityTypes());
        ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.GenerateBindingCode(types, GeneratorConfig.GenCLRBindingTrunkPath);
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 导出Unity中的类型
    /// </summary>
    /// <returns></returns>
    private static List<Type> ExportUnityTypes()
    {
        List<Type> outTypes = new List<Type>();
        List<Type> tempTypes = new List<Type>();

        //增加白名单中所有的类型
        foreach (var item in GeneratorConfig.whiteAssemblyList)
        {
            tempTypes.AddRange(Assembly.Load(item).GetTypes());
        }

        foreach (var type in tempTypes)
        {
            //如果是特殊类型，不会被导出
            if (CheckSpecialTypes(type))
                continue;
            if (type.Namespace != null)
            {
                bool forbidExport = GeneratorConfig.blackNamespaceList.Exists((name) =>
                {
                    return name == type.Namespace;
                });

                if (forbidExport)
                    continue;

                if (GeneratorConfig.blackTypeList.Exists((_black)=>
                {
                    return _black == type;
                }))
                {
                    continue;
                }

                outTypes.Add(type);
            }
        }
        return outTypes;
    }

    /// <summary>
    /// 导出所有声明要绑定特性的类型&白名单中包含的命名空间中的一些公开类型
    /// </summary>
    private static List<Type> ExportGameDllTyps()
    {
        List<Type> outTypes = new List<Type>();
        List<Type> tempTypes = new List<Type>();

        //增加白名单中所有的类型
        foreach (var item in GeneratorConfig.whiteUserAssemblyList)
        {
            tempTypes.AddRange(Assembly.Load(item).GetTypes());
        }

        //过滤
        foreach (var type in tempTypes)
        {
            var attr = Array.Find(type.GetCustomAttributes(false), (attribute) =>
             {
                 return attribute is BindingTypeAttribute;
             }) as BindingTypeAttribute;

            if (attr != null && attr.BindingFlag == BindingFlag.Export)
            {
                outTypes.Add(type);
            }
            else
            {
                if (type.Namespace != null)
                {
                    bool allowExport = GeneratorConfig.whiteNameSpaceList.Exists((name) =>
                    {
                        return name == type.Namespace;
                    });
                    if (allowExport && !CheckSpecialTypes(type))
                    {//公开类型、委托类型将被导出
                        outTypes.Add(type);
                    }
                }
            }
        }

        return outTypes;
    }

    /// <summary>
    /// 检测type是否是特殊的类型(特殊类型将不会被导出)
    /// </summary>
    /// <returns></returns>
    private static bool CheckSpecialTypes(Type type)
    {
        return type.IsNotPublic || !type.IsPublic || type.IsGenericType ||
            type.BaseType == typeof(Delegate) || type.BaseType == typeof(MulticastDelegate) ||
            type.Name.Contains("<");
    }

    //[MenuItem("ILRuntime/Generate CLR Binding Code")]
    //static void GenerateCLRBinding()
    //{
    //    List<Type> types = new List<Type>();
    //    types.Add(typeof(int));
    //    types.Add(typeof(float));
    //    types.Add(typeof(long));
    //    types.Add(typeof(object));
    //    types.Add(typeof(string));
    //    types.Add(typeof(Array));
    //    types.Add(typeof(Vector2));
    //    types.Add(typeof(Vector3));
    //    types.Add(typeof(Quaternion));
    //    types.Add(typeof(GameObject));
    //    types.Add(typeof(UnityEngine.Object));
    //    types.Add(typeof(Transform));
    //    types.Add(typeof(RectTransform));
    //    // types.Add(typeof(CLRBindingTestClass));
    //    types.Add(typeof(Time));
    //    types.Add(typeof(Debug));
    //    //所有DLL内的类型的真实C#类型都是ILTypeInstance
    //    types.Add(typeof(List<ILRuntime.Runtime.Intepreter.ILTypeInstance>));

    //    //types.Add(typeof(SubMonoBehavior));

    //    ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.GenerateBindingCode(types, "Assets/ThirdParty/ILRuntime/ILRuntime/Generated");
    //}

    //[MenuItem("ILRuntime/Generate CLR Binding Code By Analysis")]
    //static void GenerateCLRBindingByAnalysis()
    //{
    //    ILRuntime.Runtime.Enviorment.AppDomain domain = new ILRuntime.Runtime.Enviorment.AppDomain();
    //    using (System.IO.FileStream fs = new System.IO.FileStream("Assets/Game/HotFix/HotFix.dll.bytes", System.IO.FileMode.Open, System.IO.FileAccess.Read))
    //    {
    //        domain.LoadAssembly(fs);
    //    }

    //    InitILRuntime(domain);
    //    ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.GenerateBindingCode(domain, "Assets/ThirdParty/ILRuntime/ILRuntime/Generated");
    //}

    //static void InitILRuntime(ILRuntime.Runtime.Enviorment.AppDomain domain)
    //{
    //    AdapterRegister.RegisterCrossBindingAdaptor(domain);
    //    //这里需要注册所有热更DLL中用到的跨域继承Adapter，否则无法正确抓取引用
    //    // domain.RegisterCrossBindingAdaptor(new MonoBehaviourAdapter());
    //    //  domain.RegisterCrossBindingAdaptor(new CoroutineAdapter());
    //    //  domain.RegisterCrossBindingAdaptor(new InheritanceAdapter());
    //}
}
#endif

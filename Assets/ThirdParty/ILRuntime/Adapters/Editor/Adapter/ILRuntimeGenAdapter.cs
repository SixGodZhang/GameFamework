//-----------------------------------------------------------------------
// <filename>ILRuntimeGenAdapter</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #跨域继承,生成ILRuntime的适配器# </describe>
// <desc> 被CSHotfix标记的代码，属于弃用代码</desc>
// <desc> 此工具类即将被重写 </desc>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/6 星期四 21:46:41# </time>
//-----------------------------------------------------------------------
using CodeGenerationTools.Generator;
using GameFramework.Taurus;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 创建配置文件
/// </summary>
public class GenAdapterConfigAsset : Editor
{
    [MenuItem("ILRuntime/CreateAdapterConfig")]
    static void CreateGenAdapterConfigAsset()
    {
        if (File.Exists(Application.dataPath + "/config.asset"))
        {
            if (EditorUtility.DisplayDialog("提示", "配置文件存在,确定是否重新创建", "OK", "Cancel"))
            {
                File.Delete(Application.dataPath + "/config.asset");
            }
            else
                return;
        }

        var config = ScriptableObject.CreateInstance<GenAdapterConfig>();

        AssetDatabase.CreateAsset(config, "Assets/config.asset");
        AssetDatabase.Refresh();
        UnityEngine.Debug.Log("op end!");
    }
}

#if UNITY_EDITOR
/// <summary>
/// 适配器生成类
/// </summary>
public class ILRuntimeGenAdapter
{
    #region 字段&属性
    private string _outputPath;

    private readonly Dictionary<string, TypeDefinition> _adaptorSingleInterfaceDic = new Dictionary<string, TypeDefinition>();
    private readonly Dictionary<string, TypeDefinition> _adaptorDic = new Dictionary<string, TypeDefinition>();

    private readonly Dictionary<string, TypeDefinition> _delegateCovDic = new Dictionary<string, TypeDefinition>();
    private readonly Dictionary<string, TypeReference> _delegateRegDic = new Dictionary<string, TypeReference>();

    private AdaptorGenerator _adGenerator;
    private HelperGenerator _helpGenerator;
    private InterfaceAdapterGenerator _interfaceAdapterGenerator;

    private string _adaptorAttrName = "ILRuntime.Other.NeedAdaptorAttribute";
    private string _delegateAttrName = "ILRuntime.Other.DelegateExportAttribute";
    private string _singleInterfaceAttrName = "ILRuntime.Other.SingleInterfaceExportAttribute";
    #endregion

    [MenuItem("ILRuntime/GenerateILRuntimeAdapter")]
    static void GeneratorILRuntimeAdapter()
    {

    }

    /// <summary>
    /// 工具入口
    /// </summary>
    void ToolMain()
    {
        _outputPath = Application.dataPath + "/ThirdParty/ILRuntime/ILRuntime/Runtime/Adaptors/";
        if (!Directory.Exists(_outputPath))
            Directory.CreateDirectory(_outputPath);

        string out_path = EditorPrefs.GetString("out_path");
        string main_assembly_path = EditorPrefs.GetString("main_assembly_path");
        string il_assembly_path = EditorPrefs.GetString("il_assembly_path");

        _adaptorSingleInterfaceDic.Clear();
        _adaptorDic.Clear();
        _delegateCovDic.Clear();
        _delegateRegDic.Clear();

        LoadTemplates();
    }

    /// <summary>
    /// 加载所有模板
    /// </summary>
    private void LoadTemplates()
    {
        var tmpdPath = Application.dataPath + "/ThirdParty/ILRuntime/Adapters/Editor/Adapter/Template";
        _interfaceAdapterGenerator = new InterfaceAdapterGenerator();
        _interfaceAdapterGenerator.LoadTemplateFromFile(tmpdPath + "adaptor_single_interface.tmpd");

        _adGenerator = new AdaptorGenerator();
        _adGenerator.LoadTemplateFromFile(tmpdPath + "adaptor.tmpd");

        _helpGenerator = new HelperGenerator();
        _helpGenerator.LoadTemplateFromFile(tmpdPath + "helper.tmpd");
    }
}

#endif


#if UNITY_EDITOR && CSHotfix

public class ILRuntimeGenAdapter
{
#region 字段&属性
    private static List<Type> _adapterList = new List<Type>();
#endregion

    [MenuItem("ILRuntime/GenerateILRuntimeAdapter")]
    static void GeneratorILRuntimeAdapter()
    {
        if (!EditorUtility.DisplayDialog("提示", "是否需要重新生成适配器", "是", "否"))
            return;
        _adapterList.Clear();

        List<Type> allwhitetypes = new List<Type>();
        //将白名单程序集中的所有公开类型加入进来
        foreach (var assembly in GeneratorConfig.whiteUserAssemblyList)
        {
            allwhitetypes.AddRange(Assembly.Load(assembly).GetTypes());
        }

        //筛选预先定义需要生成的适配器的类型
        _adapterList = allwhitetypes.FindAll((type) =>
        {
            string fullname = type.FullName;
            return GeneratorConfig.GenAdaterCLRType.Exists((_gentypename) => { return fullname == _gentypename; });
        });

        //确保适配器的生成路径存在
        string targetDir = GeneratorConfig.GenAdapterPath;
        if (!Directory.Exists(targetDir))
            Directory.CreateDirectory(targetDir);

        foreach (var clr in _adapterList)
        {
            //生成适配器文件
            GenAdapterFile(clr,targetDir);
        }

        //生成适配器注册文件
        GenAdaperRegisterFiles(targetDir);

        UnityEngine.Debug.Log("Create Adapter OK!");
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 生成适配器文件
    /// </summary>
    /// <param name="targetDir"></param>
    private static void GenAdapterFile(Type t, string targetDir)
    {
        string fileHeader = @"//=======================================================
//Copyright(C) None rights reserved
//<describe> #适配器文件# </describe>
//<email> whdhxyzh@gmail.com </email>
//<time> #" + DateTime.Now.ToString() + @"# </time>
//=======================================================
using System;
using System.Collections;
using System.Collections.Generic;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;";

        string className = t.Name;
        string fullName = t.FullName;
        string publicNameStr = "public class " + className + "Adapter:CrossBindingAdaptor\r\n" +
"{\r\n";
        string BaseCLRTypeStr =
    "public override Type BaseCLRType\r\n" +
    "{\r\n" +
    "    get\r\n" +
    "    {\r\n" +
    "        return typeof(" + fullName + ");//这是你想继承的那个类\r\n" +
    "    }\r\n" +
    "}\r\n" +

    "public override Type AdaptorType\r\n" +
    "{\r\n" +
    "    get\r\n" +
    "    {\r\n" +
    "        return typeof(Adaptor);//这是实际的适配器类\r\n" +
    "    }\r\n" +
    "}\r\n" +

    "public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)\r\n" +
    "{\r\n" +
    "    return new Adaptor(appdomain, instance);//创建一个新的实例\r\n" +
    "}\r\n" +

    "//实际的适配器类需要继承你想继承的那个类，并且实现CrossBindingAdaptorType接口\r\n" +
    "public class Adaptor : " + fullName + ", CrossBindingAdaptorType\r\n" +
    "{\r\n" +
    "    ILTypeInstance instance;\r\n" +
    "    ILRuntime.Runtime.Enviorment.AppDomain appdomain;\r\n" +
    "    //缓存这个数组来避免调用时的GC Alloc\r\n" +
    "    object[] param1 = new object[1];\r\n" +

    "    public Adaptor()\r\n" +
    "    {\r\n" +
    "\r\n" +
    "    }\r\n" +

    "    public Adaptor(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)\r\n" +
    "    {\r\n" +
    "        this.appdomain = appdomain;\r\n" +
    "        this.instance = instance;\r\n" +
    "    }\r\n" +

    "    public ILTypeInstance ILInstance { get { return instance; } }\r\n";
        //反射virtual的函数
        List<MethodInfo> methods = new List<MethodInfo>(
            Array.FindAll(t.GetMethods(), (_method) =>
            {
                return _method.IsPublic && _method.IsVirtual && _method.DeclaringType == t;
            })
            ); 
        string methodsStr = "";
        foreach (var md in methods)
        {
            methodsStr += CreateOverrideMethod(md) + "\r\n";
        }
        string outputString = fileHeader + "\r\n" + publicNameStr + BaseCLRTypeStr + methodsStr + "}\r\n}";
        FileStream file = null;
        StreamWriter sw = null;
        //有什么错误，就直接让系统去抛吧。
        file = new FileStream(targetDir + className + "Adapter.cs", FileMode.Create);
        sw = new StreamWriter(file);
        sw.Write(outputString);
        sw.Flush();
        sw.Close();
        file.Close();


    }

    private static string CreateOverrideMethod(MethodInfo info)
    {
        string gotFieldStr = "m_b" + info.Name + "Got";
        string fieldStr = "m_" + info.Name;
        string returnTypeStr = "void";
        bool hasReturn = false;
        if (info.ReturnType.Name != "Void")
        {
            hasReturn = true;
            returnTypeStr = info.ReturnType.FullName;
        }
        string paramsstr = "";
        int paramCount = 0;
        string paramarg = "null";
        if (info.GetParameters() != null)
        {

            paramCount = info.GetParameters().Length;
            if (paramCount > 0)
            {
                paramarg = "";
            }
            int idx = 0;
            foreach (var _param in info.GetParameters())
            {
                string arg = "arg" + idx;
                paramarg += arg;
                paramsstr += _param.ParameterType.FullName + " " + arg;
                if (idx++ < info.GetParameters().Length - 1)
                {
                    paramsstr += ",";
                    paramarg += ",";
                }
            }
        }

        string callmethod = "       if(" + fieldStr + " != null)\r\n" +
                            "       {\r\n" +
                            "           " + (hasReturn ? "return" : "") + " appdomain.Invoke(" + fieldStr + ", instance," + paramarg + ");\r\n " +
                            "       }\r\n" +
                            "       else\r\n" +
                            "       {\r\n" +
                            "           " + (hasReturn ? "return null;" : "") + "\r\n" +
                            "       }";
        string gotmethod = "bool " + gotFieldStr + " = false;\r\n" +
                    "IMethod " + fieldStr + " = null;\r\n" +
                    "public override " + returnTypeStr + " " + info.Name + " (" + paramsstr + ")\r\n" +
                    "{\r\n" +
                    "   if(!" + gotFieldStr + ")\r\n" +
                    "   {\r\n" +
                    "       " + fieldStr + " = instance.Type.GetMethod(\"" + info.Name + "\"," + paramCount + ");\r\n" +
                    "       " + gotFieldStr + " = true;\r\n" +
                    "   }\r\n" +
                    "   " + callmethod + " \r\n" +
                    "}";
        return gotmethod;
    }

    /// <summary>
    /// 生成适配器注册文件
    /// </summary>
    private static void GenAdaperRegisterFiles(string targetDir)
    {
        string fileHeader = @"//=======================================================
//Copyright(C) None rights reserved
//<describe> #适配器注册文件# </describe>
//<email> whdhxyzh@gmail.com </email>
//<time> #" + DateTime.Now.ToString() + @"# </time>
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
        {";
        string lines = "\r\n";
        foreach (var t in _adapterList)
        {
            string line = "domain.RegisterCrossBindingAdaptor(new " + t.Name + "Adapter());\r\n";
            lines += line;

        }
        string outputString = fileHeader + lines + "}\r\n}";

        FileStream file = null;
        StreamWriter sw = null;
        //有什么错误，就直接让系统去抛吧。
        file = new FileStream(targetDir + "AdapterRegister.cs", FileMode.Create);
        sw = new StreamWriter(file);
        sw.Write(outputString);
        sw.Flush();
        sw.Close();
        file.Close();
    }
}
#endif

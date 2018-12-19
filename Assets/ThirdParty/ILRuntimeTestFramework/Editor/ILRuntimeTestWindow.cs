//-----------------------------------------------------------------------
// <filename>ILRuntimeTestWindow</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #ILRuntime 测试# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/17 星期一 12:59:38# </time>
//-----------------------------------------------------------------------

using ILRuntime;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityTable;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace GameFramework.Taurus.UnityEditor
{
    public class ILRuntimeTestWindow:EditorWindow
    {
        #region 字段&属性
        private static ILRuntimeTestWindow _instance = null;
        public static ILRuntimeTestWindow Instance
        {

            get {
                if (_instance == null)
                    _instance = GetWindow<ILRuntimeTestWindow>();
                return _instance;
            }
        }

        public static AppDomain _app;

        private string _hotfix_dll_path;
        private Vector2 _scrollPositon;
        private List<TestResultInfo> _resultsList;
        private List<BaseTestUnit> _testUnitList = new List<BaseTestUnit>();//测试单元
        // private SerializedPropertyTable m_table;
        private ATTabelWindow tableWindow;
        private bool isLoadAssembly;
        #endregion

        #region 周期函数
        private void OnEnable()
        {
            _hotfix_dll_path = EditorPrefs.GetString("hotfix_dll_path");
            _resultsList = new List<TestResultInfo>();
            List<TestUnit> tests = new List<TestUnit>();

            //for (int i = 0; i < 20; i++)
            //{
            //    tests.Add(new TestUnit("name" + i, "true"));
            //}
            //for (int i = 20; i < 40; i++)
            //{
            //    tests.Add(new TestUnit("name" + i, "false"));
            //}

            tableWindow = new ATTabelWindow(tests, UnitTests);

            //for (int i = 0; i < 100; i++)
            //{
            //    tableWindow.Log(ToolLogType.Info, "1111");
            //}

            //m_table = new SerializedPropertyTable("Table", ShowAllResultInfo, CreateResultInfoColumn);

            isLoadAssembly = false;

            _app = new AppDomain();
            if (_app == null)
            {
                UnityEngine.Debug.Log("appdomain == null in initialization...");
                return;
            }

            //开启Debug调试
            _app.DebugService.StartDebugService(56000);
        }

        private void InitializeAppDomain()
        {

            if (string.IsNullOrWhiteSpace(_hotfix_dll_path))
            {
                UnityEngine.Debug.LogError("hotfix dll path == null");
                return;
            }

            try
            {
                using (FileStream fs = new FileStream(_hotfix_dll_path, FileMode.Open, FileAccess.Read))
                {
                    var path = Path.GetDirectoryName(_hotfix_dll_path);
                    //var name = Path.GetFileNameWithoutExtension(hotfix_dll_path) + ".dll";
                    var mdbPath = Path.Combine(path, "Hotfix.dll.mdb.bytes");
                    if (!File.Exists(mdbPath))
                    {
                        UnityEngine.Debug.LogError("hotfix.mdb.bytes 不存在!");
                    }


                    //mdb 调试有问题，故此处省去
                    _app.LoadAssembly(fs, null, null);
                    isLoadAssembly = true;

                    //委托
                    ILRuntimeHelper.Init(_app);
                    //绑定
                    //ILRuntime.Runtime.Generated.CLRBindings.Initialize(_app);


                    //更新按钮状态
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError("read hot fix dll error! \n" + ex.Message +"\n source: "+ex.Source + "\n Trace: \n" + ex.StackTrace);
                return;
            }
        }

        private BaseTestUnit[] ShowAllResultInfo()
        {
            return _testUnitList.ToArray<BaseTestUnit>();
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            _hotfix_dll_path = EditorGUILayout.TextField("热更代码DLL路径: ", _hotfix_dll_path,GUILayout.Width(800));

            if (GUILayout.Button("select", GUILayout.Width(50), GUILayout.Height(20)))
            {
                _hotfix_dll_path = EditorUtility.OpenFilePanelWithFilters("Hotfix Assembly", Application.dataPath + "/Game/Hotfix",
                    new string[] { "BYTES", "bytes" });
                if (!string.IsNullOrWhiteSpace(_hotfix_dll_path))
                {
                    EditorPrefs.SetString("hotfix_dll_path", _hotfix_dll_path);
                }
            }

            if (GUILayout.Button("load", GUILayout.Width(50), GUILayout.Height(20)))
            {
                //LoadHotfixDll(_hotfix_dll_path);
                if (!isLoadAssembly)
                {
                    InitializeAppDomain();
                }

                LoadAllUnitTest();
                tableWindow.m_SimpleTreeView.UpdateTableData(_testUnitList,true);
            }

            GUILayout.EndHorizontal();

            if (tableWindow != null)
            {
                tableWindow.OnGUI();
            }

            GUILayout.EndVertical();
        }


        /// <summary>
        /// 从Dll中加载所有单元测试
        /// </summary>
        private void LoadAllUnitTest()
        {
            _testUnitList.Clear();
            _resultsList.Clear();

            //???
            var types = _app.LoadedTypes.Values.ToList();

            //foreach (var type in types)
            //{
            //    Debug.Log("type: " + type.FullName);
            //}


            foreach (var type in types)
            {
                //Debug.Log("type: " + type.FullName);
                var ilType = type as ILType;
                if (ilType == null)
                    continue;

                List<IMethod> methods = new List<IMethod>();
                try
                {
                    methods = ilType.GetMethods();
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError("get hotfix dll's methods error! \n" + ex.Message + "\n source: " + ex.Source + "\n Trace: \n" + ex.StackTrace);
                }


                //Debug.Log("methods count: " + methods.Count);
                foreach (var methodInfo in methods)
                {
                    string fullName = ilType.FullName;
                    //UnityEngine.Debug.Log("2: " + fullName);
                    //Console.WriteLine("call the method:{0},return type {1},params count{2}", fullName + "." + methodInfo.Name, methodInfo.ReturnType, methodInfo.GetParameters().Length);
                    //目前只支持无参数，无返回值测试
                    if (methodInfo.ParameterCount == 0 && methodInfo.IsStatic && ((ILRuntime.CLR.Method.ILMethod)methodInfo).Definition.IsPublic)
                    {
                        UnityEngine.Debug.Log("Unit: " + fullName);

                        var testUnit = new StaticTestUnit();
                        testUnit.Init(_app, fullName, methodInfo.Name);
                        if (testUnit == null)
                            Debug.LogError("testUnit == null");
                        _testUnitList.Add(testUnit);
                    }
                }

                
            }

        }
        #endregion

        #region  内部函数

        public void UnitTests(List<TestUnit> units)
        {
            //foreach (var item in units)
            //{
            //    UnityEngine.Debug.Log("selected unit: " + item.TestName);
            //}
            foreach (var item in units)
            {
                for (int i = 0; i < _testUnitList.Count; i++)
                {
                    if (item.TestName.ToLower() == _testUnitList[i].TestName.ToLower())
                    {
                        _testUnitList[i].Run();
                        //
                        var res = _testUnitList[i].CheckResult();
                        StringBuilder sb = new StringBuilder();
                        sb.Append("Test:");
                        sb.AppendLine(res.TestName);
                        sb.Append("TestResult:");
                        sb.AppendLine(res.Result.ToString());
                        sb.AppendLine("Log:");
                        sb.AppendLine(res.Message);
                        sb.AppendLine("=======================");
                        //
                        tableWindow.Log(res.Result?ToolLogType.Info:ToolLogType.Error, sb.ToString());
                        break;
                    }
                        
                }
            }

            tableWindow.m_SimpleTreeView.UpdateTableData(_testUnitList);
        }

        #endregion

        #region 编辑器菜单
        [MenuItem("ILRuntime/ILRuntime Unit",false,200)]
        static void ShowILRuntimeUnitWindow()
        {
            ILRuntimeTestWindow window = Instance;
            window.titleContent = new GUIContent("热更代码单元测试");
            window.minSize = new Vector2(1000, 654);
            window.maxSize = new Vector2(1000, 654);
            window.Show();
        }
        #endregion

    }
}

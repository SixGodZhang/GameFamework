//-----------------------------------------------------------------------
// <filename>ILRToolConfig</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #ILRuntime 配置# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/19 星期三 20:23:50# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameFramework.Taurus
{
    public class ILRToolConfig:EditorWindow
    {
        #region 属性&字段
        private static ILRToolConfig _instance;

        public static ILRToolConfig Instance
        {
            get
            {
                if (_instance == null)
                    _instance = GetWindow<ILRToolConfig>();
                return _instance;
            }
        }

        private string _hotfixFolder;
        #endregion

        #region 周期函数
        private void OnEnable()
        {
            _hotfixFolder = EditorPrefs.GetString("hotfixFolder");
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            _hotfixFolder = EditorGUILayout.TextField("热更DLL文件夹", _hotfixFolder);
            if (GUILayout.Button("选择输出文件夹", GUILayout.Width(100), GUILayout.Height(20)))
            {
                _hotfixFolder = EditorUtility.OpenFolderPanel("选择文件夹", Application.dataPath, "");
                if (!string.IsNullOrWhiteSpace(_hotfixFolder))
                {
                    EditorPrefs.SetString("hotfixFolder", _hotfixFolder);
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }
        #endregion

        #region 内部函数

        #endregion

        #region Unity窗口工具
        [MenuItem("ILRuntime/ILRConfig",false,10)]
        static void ILRConfig()
        {
            ILRToolConfig window = Instance;
            window.titleContent = new GUIContent("ILR 工具配置");
            window.minSize = new Vector2(1000, 654);
            window.maxSize = new Vector2(1000, 654);
            window.Show();
        }
        #endregion
    }
}

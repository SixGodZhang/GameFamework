//-----------------------------------------------------------------------
// <filename>GenerateILRuntimeBindingWindow</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> ## </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/13 星期四 16:34:08# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameFramework.Taurus
{
    public class GenerateILRuntimeBindingWindow: EditorWindow
    {
        #region 属性&字段
        private static GenerateILRuntimeBindingWindow _instance;

        public static GenerateILRuntimeBindingWindow Instance
        {
            get {
                if (_instance == null)
                    _instance = GetWindow<GenerateILRuntimeBindingWindow>();
                return _instance;
            }
        }

        private void OnEnable()
        {
            
        }

        private void OnGUI()
        {
            
        }
        #endregion

        #region 编辑器菜单
        [MenuItem("ILRuntime/Adapter")]
        static void ShowGenerateILRuntimeAdapterWindow()
        {
            GenerateILRuntimeBindingWindow window = Instance;
            window.titleContent = new GUIContent("绑定自动分析工具");
            window.minSize = new Vector2(1000, 654);
            window.maxSize = new Vector2(1000, 654);
            window.Show();
        }
        #endregion
    }
}

//-----------------------------------------------------------------------
// <filename>GenAdapterConfigEditor</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> ## </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/13 星期四 11:23:33# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameFramework.Taurus
{
    [CustomEditor(typeof(GenAdapterConfig))]
    public class GenAdapterConfigEditor : Editor
    {
        private GenAdapterConfig _config;
        private string _out_path = "";
        private string _main_assembly_path = "";
        private string _il_assembly_path = "";

        private void OnEnable()
        {
            _config = target as GenAdapterConfig;
            _out_path = _config.out_path;
            _main_assembly_path = _config.main_assembly_path;
            _il_assembly_path = _config.il_assembly_path;
        }

        public override void OnInspectorGUI()
        {
            if (_config == null)
                return;

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            _out_path = EditorGUILayout.TextField("适配器输出目录: ", _out_path);
            
            if (GUILayout.Button( EditorGUIUtility.IconContent("GameManager Icon"),GUILayout.Width(50),GUILayout.Height(20)))
            {
                _out_path = EditorUtility.OpenFolderPanel("选择文件夹", Application.dataPath + "/ThirdParty/ILRuntime/ILRuntime/Runtime", "");
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            _main_assembly_path = EditorGUILayout.TextField("主工程Assembly", _main_assembly_path);
            if (GUILayout.Button(EditorGUIUtility.IconContent("GameManager Icon"), GUILayout.Width(50), GUILayout.Height(20)))
            {
                string path = Application.dataPath.Replace("Assets", "") + "Library\\ScriptAssemblies";
                Debug.Log(path);
                _main_assembly_path = EditorUtility.OpenFilePanelWithFilters("选择主工程Assembly", path,
                    new string[] { "All files", "dll" });
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            _il_assembly_path = EditorGUILayout.TextField("热更工程Assembly", _il_assembly_path);
            if (GUILayout.Button(EditorGUIUtility.IconContent("GameManager Icon"), GUILayout.Width(50), GUILayout.Height(20)))
            {
                _il_assembly_path = EditorUtility.OpenFilePanelWithFilters("热更工程Assembly", Application.dataPath + "/Game/Hotfix",
                    new string[] { "BYTES", "bytes" });
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            if (GUILayout.Button("Save"))
            {
                _config.out_path = _out_path;
                _config.main_assembly_path = _main_assembly_path;
                _config.il_assembly_path = _il_assembly_path;

                //保存
                EditorPrefs.SetString("out_path", _config.out_path);
                EditorPrefs.SetString("main_assembly_path", _config.main_assembly_path);
                EditorPrefs.SetString("il_assembly_path", _config.il_assembly_path);

            }
        }
    }
}

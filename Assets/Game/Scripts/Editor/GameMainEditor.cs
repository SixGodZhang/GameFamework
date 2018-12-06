using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace GameFramework.Taurus
{
    [CustomEditor(typeof(GameMain))]
    public class GameMainEditor:Editor
    {
        private GameMain _gameMain;

        private bool _resourceModule = true;
        private bool _operationModule = true;
        private bool _stateModule = true;
        private bool _dataTableModule = true;
        private bool _nodeDataModule = true;
        private bool _stepModule = true;
        private bool _settingModule = true;
        private bool _hotfixModule = true;

        ////Color.cyan;
        private Color _defaultColor;

        //资源加载模块的颜色
        private Color _resourceColor = new Color(0.851f, 0.227f, 0.286f, 1.0f);

        //可操作模块的颜色
        private Color _operationColor = new Color(0.953f, 0.424f, 0.129f, 1.0f);

        //状态模块的颜色
        private Color _stateColor = new Color(0.141f, 0.408f, 0.635f, 1.0f);

        //配置表模块的颜色
        private Color _dataTableColor = new Color(0.989f, 0.686f, 0.090f, 1.0f);

        //数据节点模块的颜色
        private Color _nodeDataColor = new Color(0.435f, 0.376f, 0.667f, 1.0f);

        //步骤模块的颜色
        private Color _stepColor = new Color(0.439f, 0.631f, 0.624f, 1.0f);

        //热更模块的颜色
        private Color _hotfixColor = new Color(0.989f, 0.631f, 0.624f, 1.0f);

        //调试模块的颜色
        private Color _debugColor = new Color(1f, 0.100f, 0.888f, 1.0f);

        private List<string> _listState;

        private void OnEnable()
        {
            _gameMain = target as GameMain;
            _defaultColor = GUI.color;

            _listState = new List<string>();
            Type[] types = typeof(GameMain).Assembly.GetTypes();
            foreach (var item in types)
            {
                object[] attribute = item.GetCustomAttributes(typeof(GameStateAttribute), false);
                if (attribute.Length <= 0 || item.IsAbstract)
                    continue;
                GameStateAttribute stateAttribute = (GameStateAttribute)attribute[0];
                object obj = Activator.CreateInstance(item);
                GameState gs = obj as GameState;
                if (gs != null)
                    _listState.Add("[" + stateAttribute.StateType.ToString() + "]\t" + item.FullName);
            }

            //Debug.Log("Count: " + _listState.Count);
        }

        public override void OnInspectorGUI()
        {
            if (_gameMain == null)
                return;
            #region 资源加载模块
            GUI.color = _resourceColor;
            GUILayout.BeginVertical("Box");
            GUI.color = _defaultColor;
            GUILayout.BeginHorizontal();
            GUILayout.Space(12);
            _resourceModule = EditorGUILayout.Foldout(_resourceModule, "Resource Module", true);
            GUILayout.EndHorizontal();
            if (_resourceModule)
                DrawEditorModule();
            GUILayout.EndVertical();
            #endregion

            #region 游戏状态模块
            GUI.color = _stateColor;
            GUILayout.BeginVertical("Box");
            GUI.color = _defaultColor;
            GUILayout.BeginHorizontal();
            GUILayout.Space(12);
            _stateModule = EditorGUILayout.Foldout(_stateModule, "GameState Module", true);
            GUILayout.EndHorizontal();
            if (_stateModule)
                DrawGameStateGUI();
            GUILayout.EndVertical();
            #endregion

            #region 热更模块
            GUI.color = _hotfixColor;
            GUILayout.BeginVertical("Box");
            GUI.color = _defaultColor;
            GUILayout.BeginHorizontal();
            GUILayout.Space(12);
            _hotfixModule = EditorGUILayout.Foldout(_hotfixModule, "Hotfix Module", true);
            GUILayout.EndHorizontal();
            if (_hotfixModule)
                DrawHotFixGUI();
            GUILayout.EndVertical();
            #endregion
        }

        /// <summary>
        /// 绘制热更模块的界面
        /// </summary>
        private void DrawHotFixGUI()
        {
            GUILayout.BeginVertical("Box");
            _gameMain.UseHotFix = EditorGUILayout.Toggle("Use Hotfix", _gameMain.UseHotFix);
            GUILayout.EndVertical();
        }

        /// <summary>
        /// 绘制 当前 资源加载的页面
        /// </summary>
        private void DrawEditorModule()
        {
            GUILayout.BeginVertical("Box");
            _gameMain.ResUpdateType = (ResourceUpdateType)EditorGUILayout.EnumPopup("Resource Update Type", _gameMain.ResUpdateType);

            switch (_gameMain.ResUpdateType)
            {
                case ResourceUpdateType.Editor:
                    break;
                case ResourceUpdateType.Local:
                    {
                        _gameMain.LocalPathType = (PathType)EditorGUILayout.EnumPopup("Local Path Type", _gameMain.LocalPathType);
                        EditorGUILayout.LabelField("Path", ResourceManager.GetDeafultPath(_gameMain.LocalPathType));
                    }
                    break;
                case ResourceUpdateType.Update:
                    {
                        _gameMain.LocalPathType = PathType.ReadOnly;
                        //_gameMain.LocalPathType = (PathType)EditorGUILayout.EnumPopup("Local Path Type", PathType.ReadWrite);
                        _gameMain.ResUpdatePath = EditorGUILayout.TextField("Resources Update Path", _gameMain.ResUpdatePath);
                        EditorGUILayout.LabelField("Path", ResourceManager.GetDeafultPath(_gameMain.LocalPathType));
                    }
                    break;
            }

            GUILayout.EndVertical();
        }

        /// <summary>
        /// 绘制游戏状态
        /// </summary>
        private void DrawGameStateGUI()
        {
            GUILayout.BeginVertical("HelpBox");
            foreach (var item in _listState)
            {
                if (EditorApplication.isPlaying)
                {
                    string runName = "";
                    if (GameMain.StateMG.CurrentState!=null)
                    {
                        runName = GameMain.StateMG.CurrentState.GetType().Name;
                    }
                    if (item.Contains(runName))
                    {
                        GUILayout.BeginHorizontal();
                        GUI.color = Color.green;
                        GUILayout.Label("", GUI.skin.GetStyle("Icon.ExtrapolationContinue"));
                        GUI.color = _defaultColor;
                        GUILayout.Label(item);
                        GUILayout.FlexibleSpace();
                        GUILayout.Label((Profiler.GetMonoUsedSizeLong() / 1000000.0f).ToString("f3"));
                        GUILayout.EndHorizontal();

                        continue;
                    }
                }

                //默认状态
                GUI.enabled = false;
                GUILayout.BeginHorizontal();
                GUILayout.Label("", GUI.skin.GetStyle("Icon.ExtrapolationContinue"));
                GUILayout.Label(item);
                GUILayout.EndHorizontal();
                GUI.enabled = true;
            }
            GUILayout.EndVertical();
        }
    }
}

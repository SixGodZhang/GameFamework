﻿//-----------------------------------------------------------------------
// <filename>ShowUnityIcon</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> ## </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/13 星期四 11:42:24# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class ListInternalIconWindow : EditorWindow
{
    private IEnumerator enumeratorLoadIcon, enumeratorFindTexture;

    private Rect headerRct = new Rect();

    private MethodInfo loadIconMethodInfo, findTextureMethodInfo;

    private List<GUIContent> lstWindowIcons, lstLoadIconParmContents, lstFindTextureParmContents;

    private Rect rectLoadIcon = new Rect(0, 0, 300, 35);

    private Rect rectScrollViewPos = new Rect(), rectScrollViewRect = new Rect();

    private Vector2 vct2LoadIconParmScroll;

    /// <summary>
    /// UnityEditor.dll反编译出来的项目文件夹
    /// </summary>
    private string editor_dll_proj_path = "E:/Unity2018217f1/ILSpyCode";

    [MenuItem("Window/Open Internal Icon Window")]
    static void Open()
    {
        GetWindow<ListInternalIconWindow>();
    }
    void Awake()
    {
        lstWindowIcons = new List<GUIContent>();
        lstLoadIconParmContents = new List<GUIContent>();
        lstFindTextureParmContents = new List<GUIContent>();

        loadIconMethodInfo = typeof(EditorGUIUtility).GetMethod("LoadIcon", BindingFlags.Static | BindingFlags.NonPublic);
        findTextureMethodInfo = typeof(EditorGUIUtility).GetMethod("FindTexture", BindingFlags.Static | BindingFlags.Public);

        InitWindowsIconList();
        enumeratorLoadIcon = MethodParamEnumerator("EditorGUIUtility.LoadIcon", loadIconMethodInfo);
        enumeratorFindTexture = MethodParamEnumerator("EditorGUIUtility.FindTexture", findTextureMethodInfo);

        // LoadIcon 的实参有的是字符串拼接的。。这种我就没有加载出来，可以到UnityEditor.dll源码中查看如何凭借
        // 这里我用一个源码中拼接的图标作为该窗口的图标
        titleContent = new GUIContent("InternalIcon", loadIconMethodInfo.Invoke(null, new object[] { "WaitSpin00" }) as Texture);
        minSize = new Vector2(512, 320);
    }

    /// <summary>
    /// 绘制 GUIContent list
    /// </summary>
    /// <param name="headerText">标头</param>
    /// <param name="offsetY">绘制区域的垂直偏移量</param>
    /// <param name="colCount">一行绘制几个</param>
    /// <param name="lstGUIContent">将要绘制的 GUIContent list</param>
    /// <returns>返回 结束后的偏移量</returns>
    private float DrawList(string headerText, float offsetY, int colCount, List<GUIContent> lstGUIContent, bool isRemoveReturn)
    {
        GUI.Label(headerRct, headerText);
        offsetY += headerRct.height;
        for (int i = 0; i < lstGUIContent.Count; ++i)
        {
            rectLoadIcon.x = (int)(rectLoadIcon.width * (i % colCount));
            rectLoadIcon.y = (int)(rectLoadIcon.height * (i / colCount)) + offsetY;

            if (GUI.Button(rectLoadIcon, lstGUIContent[i]))
            {
                string str = lstGUIContent[i].text;
                if (isRemoveReturn)
                {
                    str = str.Replace("\r", "");
                    str = str.Replace("\n", "");
                }
                Debug.Log(str);
            }
        }
        return offsetY + (lstGUIContent.Count / colCount + 1) * rectLoadIcon.height;
    }

    /// <summary>
    /// 反射得到属性值
    /// </summary>
    /// <typeparam name="T">属性类型</typeparam>
    /// <param name="type">属性所在的类型</param>
    /// <param name="obj">类型实例，若是静态属性，则obj传null即可</param>
    /// <param name="propertyName">属性名</param>
    /// <returns>属性值</returns>
    private T GetPropertyValue<T>(Type type, object obj, string propertyName)
    {
        T result = default(T);
        PropertyInfo propertyInfo = type.GetProperty(propertyName);
        if (null != propertyInfo)
        {
            result = (T)propertyInfo.GetValue(obj, null);
        }
        return result;
    }

    /// <summary>
    /// 通过反射得到 EditorWindowTitleAttribute 特性标记的 EditorWindow 子类
    /// 并通过这个特性中的属性得到 图标的名字，
    /// 然后继续通过反射调用内部方法 EditorGUIUtility.LoadIcon 来得到 图标的 Texture 实例
    /// </summary>
    private void InitWindowsIconList()
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        Type editorWindowTitleAttrType = typeof(EditorWindow).Assembly.GetType("UnityEditor.EditorWindowTitleAttribute");

        foreach (Assembly assembly in assemblies)
        {
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                if (!type.IsSubclassOf(typeof(EditorWindow)))
                    continue;

                object[] attrs = type.GetCustomAttributes(editorWindowTitleAttrType, true);
                for (int i = 0; i < attrs.Length; ++i)
                {
                    if (attrs[i].GetType() == editorWindowTitleAttrType)
                    {
                        string icon = GetPropertyValue<string>(editorWindowTitleAttrType, attrs[i], "icon");
                        if (string.IsNullOrEmpty(icon))
                        {
                            bool useTypeNameAsIconName = GetPropertyValue<bool>(editorWindowTitleAttrType, attrs[i], "useTypeNameAsIconName");
                            if (useTypeNameAsIconName)
                                icon = type.ToString();
                        }

                        if (!string.IsNullOrEmpty(icon) && null != loadIconMethodInfo)
                        {
                            var iconTexture = loadIconMethodInfo.Invoke(null, new object[] { icon }) as Texture2D;
                            if (null != iconTexture)
                                lstWindowIcons.Add(new GUIContent(type.Name + "\n" + icon, iconTexture));
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 对字符串插入 换行符
    /// </summary>
    /// <param name="str">待处理的字符串</param>
    /// <param name="interval">每几个字符插入一个 换行符</param>
    /// <returns></returns>
    private string InsertReturn(string str, int interval)
    {
        if (string.IsNullOrEmpty(str) || str.Length <= interval)
            return str;

        StringBuilder sb = new StringBuilder();
        int index = 0;
        while (index < str.Length)
        {
            if (0 != index)
                sb.Append("\r\n");

            int len = index + interval >= str.Length ? str.Length - index : interval;
            sb.Append(str.Substring(index, len));
            index += len;
        }
        return sb.ToString();
    }

    /// <summary>
    /// 通过将 Editor.dll 反编译出来，遍历反编译出来的所有文件，
    /// 通过正则找出所有 调用 EditorGUIUtility.LoadIcon 时传递 的参数
    /// </summary>
    /// <param name="methodName">加载贴图的函数名</param>
    /// <param name="loadTextureAction">加载贴图的函数</param>
    /// <returns></returns>
    private IEnumerator MethodParamEnumerator(string methodName, MethodInfo loadTextureMethodInfo)
    {
        Type editorResourcesUtility = typeof(EditorWindow).Assembly.GetType("UnityEditorInternal.EditorResourcesUtility");

        //Regex regex = new Regex(@"(?<=EditorGUIUtility.LoadIcon\("")[^""]+(?=""\))");
        Regex regex = new Regex(@"(?<=" + methodName + @"\()[^\(\)]*(((?'Open'\()[^\(\)]*)+((?'-Open'\))[^\(\)]*)+)*(?=\))(?(Open)(?!))");
        Regex quatRegex = new Regex(@"(?<=^"")[^""]+(?=""$)");

        // 这里是反编译 UnityEditor.dll 导出来的文件夹
        string[] files = Directory.GetFiles(editor_dll_proj_path, "*.cs", SearchOption.AllDirectories);

        var enumerable = from matchCollection in
                            (from content in
                                (from file in files select File.ReadAllText(file))
                             select regex.Matches(content))
                         select matchCollection;

        foreach (MatchCollection matchCollection in enumerable)
        {
            for (int i = 0; i < matchCollection.Count; ++i)
            {
                Match match = matchCollection[i];
                string iconName = ((Match)match).Groups[0].Value;

                if (string.IsNullOrEmpty(iconName) || null == loadTextureMethodInfo)
                    continue;

                bool isDispatchMethod = false;
                Texture iconTexture = null;
                if (quatRegex.IsMatch(iconName))
                {
                    isDispatchMethod = true;
                    iconName = iconName.Replace("\"", "");
                }
                else if (iconName.StartsWith("EditorResourcesUtility."))
                {
                    string resName = GetPropertyValue<string>(editorResourcesUtility, null, iconName.Replace("EditorResourcesUtility.", ""));
                    if (!string.IsNullOrEmpty(resName))
                    {
                        isDispatchMethod = true;
                        iconName = resName;
                    }
                }

                if (isDispatchMethod)
                {
                    try
                    {
                        iconTexture = loadTextureMethodInfo.Invoke(null, new object[] { iconName }) as Texture2D;
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(iconName + "\n" + e);
                    }
                }

                if (null != iconTexture)
                    yield return new GUIContent(InsertReturn(iconName, 20), iconTexture);
                else
                    yield return new GUIContent(InsertReturn(iconName, 30));
            }
        }
    }

    void OnGUI()
    {
        // Don't use yield in OnGUI() between GUILayout.BeginArea() and GUILayout.EndArea()
        if (null != enumeratorLoadIcon && enumeratorLoadIcon.MoveNext() && null != enumeratorLoadIcon.Current)
        {
            lstLoadIconParmContents.Add(enumeratorLoadIcon.Current as GUIContent);
            Repaint();
        }
        if (null != enumeratorFindTexture && enumeratorFindTexture.MoveNext() && null != enumeratorFindTexture.Current)
        {
            lstFindTextureParmContents.Add(enumeratorFindTexture.Current as GUIContent);
            Repaint();
        }

        headerRct.x = headerRct.y = 0;
        headerRct.width = position.width;
        headerRct.height = 30;

        int colCount = Mathf.Max(1, (int)(position.width / rectLoadIcon.width));
        int rowCount = (lstWindowIcons.Count + lstLoadIconParmContents.Count + lstFindTextureParmContents.Count) / colCount + 2;

        rectScrollViewRect.width = colCount * rectLoadIcon.width;
        rectScrollViewRect.height = rowCount * rectLoadIcon.height + 3 * headerRct.height;
        rectScrollViewPos.width = position.width;
        rectScrollViewPos.height = position.height;

        vct2LoadIconParmScroll = GUI.BeginScrollView(rectScrollViewPos, vct2LoadIconParmScroll, rectScrollViewRect);
        {
            float offsetY = 0;
            string headerText = "添加EditorWindowTitleAttribute 特性的窗口的图标：" + lstWindowIcons.Count + " 个";
            offsetY = DrawList(headerText, offsetY, colCount, lstWindowIcons, false);

            headerRct.y = offsetY;
            headerText = "传递给 EditorGUIUtility.LoadIcon 的参数：" + lstLoadIconParmContents.Count + " 个";
            offsetY = DrawList(headerText, offsetY, colCount, lstLoadIconParmContents, true);

            headerRct.y = offsetY;
            headerText = "传递给 EditorGUIUtility.FindTexture 的参数：" + lstFindTextureParmContents.Count + " 个";
            offsetY = DrawList(headerText, offsetY, colCount, lstFindTextureParmContents, true);
        }
        GUI.EndScrollView();
    }
}



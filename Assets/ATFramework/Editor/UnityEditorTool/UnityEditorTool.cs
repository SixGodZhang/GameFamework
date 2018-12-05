//-----------------------------------------------------------------------
// <filename>UnityEditorTool</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> #Unity编辑器工具# </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/12/4 星期二 15:52:34# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEditor;

namespace ATFramework.Taurus
{
    /// <summary>
    /// Unity编辑器工具
    /// </summary>
    public class UnityEditorTool
    {
        /// <summary>
        /// 显示进度条
        /// </summary>
        /// <param name="current">当前进度</param>
        /// <param name="total">总进度</param>
        /// <param name="taskName">任务名字</param>
        public static void ShowProgressBar(float current, float total = 100.0f, string taskName = "请稍后...")
        {
            float rate = current / total;
            if (rate >= 1)
            {
                EditorUtility.ClearProgressBar();
                return;
            }
            EditorUtility.DisplayProgressBar("任务进度", taskName, current / total);
        }

        /// <summary>
        /// 关闭进度条
        /// </summary>
        [MenuItem("ATFramework/ClearProgressBar")]
        public static void ClearProgressBar()
        {
            EditorUtility.ClearProgressBar();
        }
    }
}

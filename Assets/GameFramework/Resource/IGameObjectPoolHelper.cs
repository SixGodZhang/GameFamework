//-----------------------------------------------------------------------
// <filename>IGameObjectPoolHelper</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> ## </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/11/30 星期五 15:16:12# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Taurus
{
    //对象池接口
    public interface IGameObjectPoolHelper
    {
        /// <summary>
        /// 对象池名称
        /// </summary>
        string PoolName { get; set; }

        /// <summary>
        /// 向对象池中添加一种预设
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <param name="assetName"></param>
        /// <param name="prefabInfo"></param>
        void PushPrefab(string assetBundleName, string assetName, PoolPrefabInfo prefabInfo);

        /// <summary>
        /// 对象池中是否有该类型的预设
        /// </summary>
        /// <param name="assetName"></param>
        bool HasPrefab(string assetName);

        /// <summary>
        /// 在对象池中生成一种类型的实例
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        GameObject Spwan(string assetName);

        /// <summary>
        /// 在对象池中移除一种预设的实例
        /// </summary>
        /// <param name="go"></param>
        /// <param name="isDestroy">是否摧毁该预设</param>
        void Despwan(GameObject go, bool isDestroy = false);

        /// <summary>
        /// 将所有对象隐藏起来，放进对象池中
        /// </summary>
        void DespwanAll();

        /// <summary>
        /// 摧毁对象池中的所有物体
        /// </summary>
        void DestroyAll();

        /// <summary>
        /// 将一个对象放到对象池中
        /// </summary>
        /// <param name="assetName"></param>
        void DespawnPrefab(string assetName);
    }

    /// <summary>
    /// 对象池中的预设信息
    /// </summary>
    public struct PoolPrefabInfo
    {
        public GameObject Prefab;
        public int PreloadAmount;
        public int MaxAmount;
    }
}

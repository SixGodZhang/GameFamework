//-----------------------------------------------------------------------
// <filename>GameObjectPoolHelper</fileName>
// <copyright>
//     Copyright (c) 2018 Zhang Hui. All rights reserved.
// </copyright>
// <describe> ## </describe>
// <email> whdhxyzh@gmail.com </email>
// <time> #2018/11/30 星期五 15:27:42# </time>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Taurus
{
    public class GameObjectPoolHelper : MonoBehaviour, IGameObjectPoolHelper
    {
        #region 字段&属性
        public string PoolName { get; set; }

        /// <summary>
        /// 对象池中所有的预设
        /// </summary>
        private static Dictionary<string, PoolPrefabInfo> _prefabs = new Dictionary<string, PoolPrefabInfo>();

        /// <summary>
        /// 对象池已经生成并显示的物体
        /// </summary>
        private static Dictionary<string, List<GameObject>> _spawnPrefabs = new Dictionary<string, List<GameObject>>();

        /// <summary>
        /// 对象池中已经生成未显示的物体
        /// </summary>
        private static Dictionary<string, Queue<GameObject>> _despawnPrefabs = new Dictionary<string, Queue<GameObject>>();
        #endregion

        #region 公开方法
        public void DespawnPrefab(string assetName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将显示的物体销毁(根据isDestroy判断是放回对象池还是直接销毁)
        /// </summary>
        /// <param name="go"></param>
        /// <param name="isDestroy"></param>
        public void Despwan(GameObject go, bool isDestroy = false)
        {
            foreach (var item in _spawnPrefabs)
            {
                if (item.Value.Contains(go))
                {
                    if (isDestroy)
                    {
                        item.Value.Remove(go);
                        MonoBehaviour.Destroy(go);
                    }
                    else
                    {
                        Queue<GameObject> _queueObjs = _despawnPrefabs[item.Key];
                        if ((_prefabs[item.Key].MaxAmount >= 0) && (item.Value.Count + _queueObjs.Count) > _prefabs[item.Key].MaxAmount)
                        {
                            item.Value.Remove(go);
                            MonoBehaviour.Destroy(go);
                        }
                        else
                        {
                            item.Value.Remove(go);
                            go.SetActive(false);
                            _despawnPrefabs[item.Key].Enqueue(go);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 从对象池中创建的所有Go全部回收到对象池中
        /// </summary>
        public void DespwanAll()
        {
            foreach (var item in _spawnPrefabs)
            {
                foreach (var go in item.Value)
                {
                    item.Value.Remove(go);
                    go.SetActive(false);
                    _despawnPrefabs[item.Key].Enqueue(go);
                }
            }
        }

        /// <summary>
        /// 销毁对象池中所有预设
        /// </summary>
        public void DestroyAll()
        {
            foreach (var item in _spawnPrefabs)
            {
                foreach (var go in item.Value)
                {
                    item.Value.Remove(go);
                    MonoBehaviour.Destroy(go);
                }
            }

            foreach (var item in _despawnPrefabs.Values)
            {
                MonoBehaviour.Destroy(item.Dequeue());
            }
        }

        /// <summary>
        /// 对象池中是否含有预设信息
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public bool HasPrefab(string assetName)
        {
            return _prefabs.ContainsKey(assetName);
        }

        /// <summary>
        /// 向对象池中添加一种预设类型
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <param name="assetName"></param>
        /// <param name="prefabInfo"></param>
        public void PushPrefab(string assetBundleName, string assetName, PoolPrefabInfo prefabInfo = default(PoolPrefabInfo))
        {
            if (HasPrefab(assetName))
            {
#if UNITY_EDITOR
                Debug.Log(string.Format("{0} 已经存在", assetName));
#endif
                return;
            }

            if (prefabInfo.Prefab == null)
            {
                prefabInfo.Prefab = GameModuleProxy.GetModule<ResourceManager>().LoadAsset<GameObject>(assetBundleName, assetName);
                if (prefabInfo.Prefab == null)
                {
#if UNITY_EDITOR
                    Debug.LogError("预设资源为null: " + assetName);
#endif
                    return;
                }

                _prefabs[assetName] = prefabInfo;
                _spawnPrefabs[assetName] = new List<GameObject>();

                Initialization(assetName, prefabInfo);

            }

        }

        /// <summary>
        /// 从对象池中取出一个预设(前提:对象池中包含该预设的信息)
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public GameObject Spwan(string assetName)
        {
            GameObject go = null;
            Queue<GameObject> queueGos = _despawnPrefabs[assetName];
            if (queueGos.Count > 0)
            {
                go = queueGos.Dequeue();
                gameObject.SetActive(true);
            }
            else if (_prefabs.ContainsKey(assetName))
            {
                go = GameObject.Instantiate(_prefabs[assetName].Prefab);
                go.transform.SetParent(transform);
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning(string.Format("对象池不包含[{0}]的信息.", assetName));
#endif
                return null;
            }

            _spawnPrefabs[assetName].Add(gameObject);

            return go;
        }
        #endregion


        #region 内部函数
        /// <summary>
        /// 初始化对象池中的预设
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="prefabInfo"></param>
        private void Initialization(string assetName, PoolPrefabInfo prefabInfo)
        {
            var objects = new Queue<GameObject>();
            for (int i = 0; i < prefabInfo.PreloadAmount; i++)
            {
                var obj = GameObject.Instantiate(prefabInfo.Prefab);
                obj.transform.SetParent(transform);
                obj.SetActive(false);
                objects.Enqueue(obj);
            }

            _despawnPrefabs[assetName] = objects;
        }
        #endregion
    }
}

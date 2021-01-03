using System;
using UnityEngine;

namespace ToaruUnity.UI
{
    [DisallowMultipleComponent]
    public abstract class ViewLoader : MonoBehaviour
    {
        /// <summary>
        /// 加载界面的预制体。
        /// </summary>
        /// <param name="key">预制体的key</param>
        /// <param name="callback">加载完成预制体的回调方法</param>
        public abstract void LoadViewPrefab(object key, Action<AbstractView> callback);

        /// <summary>
        /// 释放界面的预制体。
        /// </summary>
        /// <param name="key">预制体的key</param>
        /// <param name="prefab">需要释放的预制体</param>
        public abstract void ReleaseViewPrefab(object key, AbstractView prefab);
    }
}
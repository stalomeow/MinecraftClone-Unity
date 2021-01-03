using System;
using ToaruUnity.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Minecraft
{
    public sealed class AddressableViewLoader : ViewLoader
    {
        public override void LoadViewPrefab(object key, Action<AbstractView> callback)
        {
            AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(key);
            asyncOperationHandle.Completed += handle =>
            {
                AbstractView prefab = handle.Result.GetComponent<AbstractView>();
                callback?.Invoke(prefab);
            };
        }

        public override void ReleaseViewPrefab(object key, AbstractView prefab)
        {
            Addressables.Release(prefab.gameObject);
        }
    }
}
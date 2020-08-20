using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Minecraft.AssetManagement
{
    internal sealed class RuntimeAssetBundle : IAssetBundle
    {
        private readonly string m_Name;
        private readonly AsyncHandler m_AsyncHandler;
        private AssetBundle m_AssetBundle;

        string IAssetBundle.Name => m_Name;

        AsyncHandler IAssetBundle.AsyncHandler => m_AsyncHandler;

        bool IAssetBundle.IsStreamedSceneAssetBundle
        {
            get
            {
                if (m_AssetBundle == null)
                {
                    throw new InvalidOperationException("AssetBundle未加载完成，无法访问属性");
                }

                return m_AssetBundle.isStreamedSceneAssetBundle;
            }
        }

        public RuntimeAssetBundle(string name, AsyncHandler asyncHandler)
        {
            m_Name = name;
            m_AsyncHandler = asyncHandler;
            m_AssetBundle = null;

            m_AsyncHandler.OnCompleted += OnCompletedCallback;
        }

        private void OnCompletedCallback(AsyncOperation obj)
        {
            AssetBundleCreateRequest request = obj as AssetBundleCreateRequest;
            m_AssetBundle = request.assetBundle;
        }

        AsyncAssets IAssetBundle.LoadAllAssets<T>()
        {
            if (m_AssetBundle == null)
            {
                throw new InvalidOperationException("AssetBundle未加载完成，无法加载资源");
            }

            return new AsyncAssets(m_AssetBundle.LoadAllAssetsAsync<T>());
        }

        AsyncAssets IAssetBundle.LoadAllAssets(Type type)
        {
            if (m_AssetBundle == null)
            {
                throw new InvalidOperationException("AssetBundle未加载完成，无法加载资源");
            }

            return new AsyncAssets(m_AssetBundle.LoadAllAssetsAsync(type));
        }

        AsyncAsset<T> IAssetBundle.LoadAsset<T>(string path)
        {
            if (m_AssetBundle == null)
            {
                throw new InvalidOperationException("AssetBundle未加载完成，无法加载资源");
            }

            return new AsyncAsset<T>(m_AssetBundle.LoadAssetAsync<T>(path));
        }

        AsyncAsset<Object> IAssetBundle.LoadAsset(string path, Type type)
        {
            if (m_AssetBundle == null)
            {
                throw new InvalidOperationException("AssetBundle未加载完成，无法加载资源");
            }

            return new AsyncAsset<Object>(m_AssetBundle.LoadAssetAsync(path, type));
        }

        void IAssetBundle.Unload(bool unloadAllLoadedObjects)
        {
            if (m_AssetBundle != null)
            {
                m_AssetBundle.Unload(unloadAllLoadedObjects);
            }
        }
    }
}
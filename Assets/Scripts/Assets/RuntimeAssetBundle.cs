using System;
using UnityEngine;

namespace Minecraft.Assets
{
    internal class RuntimeAssetBundle : IAssetBundle
    {
        private readonly string m_Name;
        private readonly IAssetBundle[] m_Dependencies;
        private AssetBundleCreateRequest m_Request;
        private AssetBundle m_AssetBundle;
        private int m_RefCount;
        private bool m_IsLoadingDone;


        string IAssetBundle.Name => m_Name;

        bool IAssetBundle.IsStreamedSceneAssetBundle => m_AssetBundle.isStreamedSceneAssetBundle;

        IAssetBundle[] IAssetBundle.Dependencies => m_Dependencies;

        int IAssetBundle.RefCount => m_RefCount;

        bool IAssetBundle.IsLoadingDone => m_IsLoadingDone;


        public RuntimeAssetBundle(string name, AssetBundleCreateRequest request, IAssetBundle[] dependencies)
        {
            m_Name = name;
            m_Dependencies = dependencies;
            m_Request = request;
            m_AssetBundle = null;
            m_RefCount = 0;
            m_IsLoadingDone = false;
        }


        void IAssetBundle.IncreaseRef()
        {
            m_RefCount++;
        }

        void IAssetBundle.DecreaseRef()
        {
            m_RefCount--;
        }

        bool IAssetBundle.UpdateLoadingState()
        {
            if (m_IsLoadingDone)
            {
                return true;
            }

            if (!m_Request.isDone)
            {
                return false;
            }

            for (int i = 0; i < m_Dependencies.Length; i++)
            {
                if (!m_Dependencies[i].IsLoadingDone)
                {
                    return false;
                }
            }

            m_AssetBundle = m_Request.assetBundle;
            m_Request = null;
            m_IsLoadingDone = true;
            return true;
        }

        AssetBundleRequest IAssetBundle.LoadAsset<T>(string name)
        {
            return m_AssetBundle.LoadAssetAsync<T>(name);
        }

        AssetBundleRequest IAssetBundle.LoadAsset(string name, Type type)
        {
            return m_AssetBundle.LoadAssetAsync(name, type);
        }

        void IAssetBundle.Unload(bool unloadAllLoadedObjects)
        {
            m_AssetBundle?.Unload(unloadAllLoadedObjects);
            m_AssetBundle = null;
        }
    }
}
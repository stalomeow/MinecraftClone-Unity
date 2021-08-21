using System;
using System.Collections;
using System.Collections.Generic;
using Minecraft.Lua;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Minecraft.Assets
{
    public class AsyncAsset : ILuaCallCSharp, IEnumerator // CustomYieldInstruction
    {
        private string m_AssetName;
        private Type m_AssetType;
        private IAssetBundle m_AssetBundle;
        private AssetBundleRequest m_Request;
        private Object m_Asset;
        private bool m_IsDone;


        public bool IsDone => m_IsDone;

        public float Progress
        {
            get
            {
                if (m_IsDone)
                {
                    return 1;
                }

                if (!m_AssetBundle.IsLoadingDone)
                {
                    return 0;
                }

                return (1 + m_Request.progress) * 0.5f;
            }
        }

        public string AssetName => m_AssetName;

        public IAssetBundle AssetBundle => m_AssetBundle;

        public Object Asset => m_IsDone ? m_Asset : throw new InvalidOperationException();

        public void Initialize(string name, Type type, IAssetBundle assetBundle)
        {
            m_AssetName = name;
            m_AssetType = type;
            m_AssetBundle = assetBundle;
            m_Request = null;
            m_Asset = null;
            m_IsDone = false;
        }

        public void Initialize(string name, Object asset, IAssetBundle assetBundle)
        {
            m_AssetName = name;
            m_AssetType = null;
            m_AssetBundle = assetBundle;
            m_Request = null;
            m_Asset = asset;
            m_IsDone = true;
        }

        public T GetAssetAs<T>() where T : Object
        {
            return Asset as T;
        }

        public bool UpdateLoadingState()
        {
            if (m_IsDone)
            {
                return true;
            }

            if (!m_AssetBundle.IsLoadingDone)
            {
                return false;
            }

            m_Request ??= m_AssetBundle.LoadAsset(m_AssetName, m_AssetType);

#if UNITY_EDITOR
            if (!(m_Request is EditorAssetBundle.EditorAssetBundleRequest) && !m_Request.isDone)
            {
                return false;
            }
#else
            if (!m_Request.isDone)
            {
                return false;
            }
#endif

            m_Asset = m_Request.asset;
            m_AssetType = null;
            m_Request = null;
            m_IsDone = true;
            return true;
        }

        public void Unload()
        {
            m_Asset = null;
        }


        object IEnumerator.Current => default;

        bool IEnumerator.MoveNext() => !m_IsDone;

        void IEnumerator.Reset() { }


        public static IEnumerator WaitAll(params AsyncAsset[] assets)
        {
            return WaitAll((IReadOnlyList<AsyncAsset>)assets);
        }

        public static IEnumerator WaitAll(IReadOnlyList<AsyncAsset> assets)
        {
            for (int i = 0; i < assets.Count; i++)
            {
                do
                {
                    yield return null;
                }
                while (!assets[i].IsDone);
            }
        }

        public static IEnumerator WaitAll<T>(T[] assetReferences, IReadOnlyList<AsyncAsset> assets) where T : Object
        {
            for (int i = 0; i < assets.Count; i++)
            {
                do
                {
                    yield return null;
                }
                while (!assets[i].IsDone);

                assetReferences[i] = assets[i].GetAssetAs<T>();
            }
        }
    }
}
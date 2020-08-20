using UnityEngine;
using Object = UnityEngine.Object;

namespace Minecraft.AssetManagement
{
#if UNITY_EDITOR

    public readonly struct AsyncAsset<T> where T : Object
    {
        private readonly bool m_IsLoadedObject;
        private readonly T m_Object;
        private readonly AssetBundleRequest m_Request;

        public bool IsDone => m_IsLoadedObject ? true : m_Request.isDone;

        public float Progress => m_IsLoadedObject ? 1 : m_Request.progress;

        public T Asset => m_IsLoadedObject ? m_Object: (m_Request.asset as T);

        public AsyncAsset(AssetBundleRequest request) : this()
        {
            m_IsLoadedObject = false;
            m_Request = request;
        }

        public AsyncAsset(T obj) : this()
        {
            m_IsLoadedObject = true;
            m_Object = obj;
        }
    }

    public readonly struct AsyncAssets
    {
        private readonly bool m_IsLoadedObjects;
        private readonly Object[] m_Objects;
        private readonly AssetBundleRequest m_Request;

        public bool IsDone => m_IsLoadedObjects ? true : m_Request.isDone;

        public float Progress => m_IsLoadedObjects ? 1 : m_Request.progress;

        public Object[] Assets => m_IsLoadedObjects ? m_Objects : m_Request.allAssets;

        public AsyncAssets(AssetBundleRequest request) : this()
        {
            m_IsLoadedObjects = false;
            m_Request = request;
        }

        public AsyncAssets(Object[] objs) : this()
        {
            m_IsLoadedObjects = true;
            m_Objects = objs;
        }
    }

#else

    public readonly struct AsyncAsset<T> where T : Object
    {
        private readonly AssetBundleRequest m_Request;

        public bool IsDone => m_Request.isDone;

        public float Progress => m_Request.progress;

        public T Asset => m_Request.asset as T;

        public AsyncAsset(AssetBundleRequest request)
        {
            m_Request = request;
        }
    }

    public readonly struct AsyncAssets
    {
        private readonly AssetBundleRequest m_Request;

        public bool IsDone => m_Request.isDone;

        public float Progress => m_Request.progress;

        public Object[] Assets => m_Request.allAssets;

        public AsyncAssets(AssetBundleRequest request)
        {
            m_Request = request;
        }
    }

#endif
}
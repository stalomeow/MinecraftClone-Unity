#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Minecraft.Assets
{
    internal class EditorAssetBundle : IAssetBundle
    {
        private readonly string m_Name;
        private readonly IAssetBundle[] m_Dependencies;
        private readonly bool m_IsStreamedSceneAssetBundle;
        private HashSet<string> m_Assets;
        private int m_RefCount;


        string IAssetBundle.Name => m_Name;

        bool IAssetBundle.IsStreamedSceneAssetBundle => m_IsStreamedSceneAssetBundle;

        IAssetBundle[] IAssetBundle.Dependencies => m_Dependencies;

        int IAssetBundle.RefCount => m_RefCount;

        bool IAssetBundle.IsLoadingDone => true;


        public EditorAssetBundle(string name, IAssetBundle[] dependencies)
        {
            m_Name = name;
            m_Dependencies = dependencies;

            string[] paths = AssetDatabase.GetAssetPathsFromAssetBundle(m_Name);
            m_IsStreamedSceneAssetBundle = paths.Length > 0 ? AssetDatabase.GetMainAssetTypeAtPath(paths[0]) == typeof(SceneAsset) : false;
            m_Assets = new HashSet<string>(paths);

            m_RefCount = 0;
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
            return true;
        }

        AssetBundleRequest IAssetBundle.LoadAsset<T>(string name)
        {
            name = m_Assets.Contains(name) ? name : AssetDatabase.GUIDToAssetPath(name);
            return new EditorAssetBundleRequest(AssetDatabase.LoadAssetAtPath<T>(name));
        }

        AssetBundleRequest IAssetBundle.LoadAsset(string name, Type type)
        {
            name = m_Assets.Contains(name) ? name : AssetDatabase.GUIDToAssetPath(name);
            return new EditorAssetBundleRequest(AssetDatabase.LoadAssetAtPath(name, type));
        }

        void IAssetBundle.Unload(bool unloadAllLoadedObjects)
        {
            m_Assets = null;
        }


        internal class EditorAssetBundleRequest : AssetBundleRequest
        {
            private Object m_Asset;

            public EditorAssetBundleRequest(Object asset)
            {
                m_Asset = asset;
            }

            protected override Object GetResult()
            {
                return m_Asset;
            }
        }
    }
}
#endif
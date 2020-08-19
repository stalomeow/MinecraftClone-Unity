//#define REAL_MODE // 取消注释则会真实加载ab包，否则从assetdatabase加载

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Minecraft.AssetManagement
{
    public sealed class AssetBundleLoader
    {
        private readonly Dictionary<string, IAssetBundle> m_LoadedAssetBundles;
        private readonly Dictionary<string, AsyncOperation> m_Operations;
        private readonly Dictionary<string, AssetBundleMeta> m_AssetBundleMeta;
        private readonly string m_AssetBundleDirectory;


        public AssetBundleLoader(string assetBundleDirectory)
        {
            IEqualityComparer<string> comparer = StringComparer.OrdinalIgnoreCase;

            m_LoadedAssetBundles = new Dictionary<string, IAssetBundle>(comparer);
            m_Operations = new Dictionary<string, AsyncOperation>(comparer);
            m_AssetBundleMeta = new Dictionary<string, AssetBundleMeta>(comparer);
            m_AssetBundleDirectory = assetBundleDirectory;

#if !UNITY_EDITOR || REAL_MODE
            string path = Path.Combine(m_AssetBundleDirectory, AssetBundleManifest.FileName);
            string json = File.ReadAllText(path);
            AssetBundleManifest manifest = JsonUtility.FromJson<AssetBundleManifest>(json);

            for (int i = 0; i < manifest.AssetBundles.Length; i++)
            {
                AssetBundleMeta meta = manifest.AssetBundles[i];
                m_AssetBundleMeta.Add(meta.Name, meta);
            }
#endif
        }

        public IAssetBundle LoadAssetBundle(string name)
        {
            if (m_LoadedAssetBundles.TryGetValue(name, out IAssetBundle bundle))
            {
                return bundle;
            }


#if UNITY_EDITOR && !REAL_MODE
            bundle = new EditorAssetBundle(name);
            m_LoadedAssetBundles.Add(name, bundle);
            return bundle;
#else
            return LoadFromFile(name);
#endif
        }

        public void UnloadAssetBundle(IAssetBundle assetBundle, bool unloadAllLoadedObjects)
        {
            assetBundle.Unload(unloadAllLoadedObjects);
            m_LoadedAssetBundles.Remove(assetBundle.Name);
            m_Operations.Remove(assetBundle.Name);
        }

        public void UnloadAllAssetBundles(bool unloadAllLoadedObjects)
        {
            foreach (IAssetBundle assetBundle in m_LoadedAssetBundles.Values)
            {
                assetBundle.Unload(unloadAllLoadedObjects);
            }

            m_LoadedAssetBundles.Clear();
            m_Operations.Clear();
        }

        private IAssetBundle LoadFromFile(string name)
        {
            if (!m_AssetBundleMeta.TryGetValue(name, out AssetBundleMeta meta))
            {
                return null;
            }

            AssetBundleCreateRequest main = LoadNewAssetBundleRequest(meta);

            if (main == null)
            {
                return null;
            }

            string[] deps = meta.Dependencies;
            List<AsyncOperation> operations = new List<AsyncOperation>(deps.Length + 1) { main };

            for (int i = 0; i < deps.Length; i++)
            {
                string dep = deps[i];

                if (m_Operations.TryGetValue(dep, out AsyncOperation op))
                {
                    if (!op.isDone)
                    {
                        operations.Add(op);
                    }
                }
                else if (m_AssetBundleMeta.TryGetValue(dep, out AssetBundleMeta depMeta))
                {
                    AssetBundleCreateRequest request = LoadNewAssetBundleRequest(depMeta);

                    if (request != null)
                    {
                        operations.Add(request);
                    }
                }
            }

            AsyncHandler asyncHandler = new AsyncHandler(operations.ToArray(), 0);
            return new RuntimeAssetBundle(name, asyncHandler);
        }

        private AssetBundleCreateRequest LoadNewAssetBundleRequest(AssetBundleMeta meta)
        {
            string filePath = Path.Combine(m_AssetBundleDirectory, meta.FileName);

            if (!File.Exists(filePath))
            {
                return null;
            }

            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(filePath);
            m_Operations.Add(meta.Name, request);
            return request;
        }
    }
}
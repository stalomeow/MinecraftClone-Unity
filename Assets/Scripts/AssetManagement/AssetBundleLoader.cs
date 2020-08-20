#define REAL_MODE // 取消注释则会真实加载ab包，否则从assetdatabase加载

using Minecraft.DebugUtils;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Minecraft.AssetManagement
{
    public sealed class AssetBundleLoader : IDebugMessageSender
    {
        string IDebugMessageSender.DisplayName =>
#if REAL_MODE || !UNITY_EDITOR
            $"AssetBundleLoader({LoadedAssetBundleCount})";
#else
            $"AssetBundleEditorLoader({LoadedAssetBundleCount})";
#endif


        private readonly Dictionary<string, IAssetBundle> m_LoadedAssetBundles;
        private readonly Dictionary<string, AssetBundleCreateRequest> m_Operations;
        private readonly Dictionary<string, AssetBundleMeta> m_AssetBundleMeta;
        private readonly string m_AssetBundleDirectory;


        public bool DisableLog { get; set; }

        public int LoadedAssetBundleCount => m_LoadedAssetBundles.Count;


        public AssetBundleLoader(string assetBundleDirectory)
        {
            IEqualityComparer<string> comparer = StringComparer.OrdinalIgnoreCase;

            m_LoadedAssetBundles = new Dictionary<string, IAssetBundle>(comparer);
            m_Operations = new Dictionary<string, AssetBundleCreateRequest>(comparer);
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

            this.Log("加载manifest，共", manifest.AssetBundles.Length, "个assetbundle");
#endif
        }

        public IAssetBundle LoadAssetBundle(string name)
        {
            if (m_LoadedAssetBundles.TryGetValue(name, out IAssetBundle ab))
            {
                this.Log("从缓存中加载：", name);
                return ab;
            }


#if UNITY_EDITOR && !REAL_MODE
            ab = new EditorAssetBundle(name);
            m_LoadedAssetBundles.Add(name, ab);
            this.Log(name, "加载成功");
            return ab;
#else
            return LoadFromFile(name);
#endif
        }

        public void UnloadAssetBundle(IAssetBundle assetBundle, bool unloadAllLoadedObjects)
        {
            assetBundle.Unload(unloadAllLoadedObjects);
            m_LoadedAssetBundles.Remove(assetBundle.Name);
            m_Operations.Remove(assetBundle.Name);

            this.Log("卸载：", assetBundle.Name);
        }

        public void UnloadAllAssetBundles(bool unloadAllLoadedObjects)
        {
            foreach (IAssetBundle assetBundle in m_LoadedAssetBundles.Values)
            {
                assetBundle.Unload(unloadAllLoadedObjects);

                this.Log("卸载：", assetBundle.Name);
            }

            m_LoadedAssetBundles.Clear();
            m_Operations.Clear();

            this.Log("已清空缓存");
        }

        private IAssetBundle LoadFromFile(string name)
        {
            if (!m_AssetBundleMeta.TryGetValue(name, out AssetBundleMeta meta))
            {
                this.Log("无法加载", name, "，因为对应的AssetBundleMeta对象不存在");
                return null;
            }

            AssetBundleCreateRequest main = GetAssetBundleRequest(meta);

            if (main == null)
            {
                this.Log("无法加载", name, "，因为AssetBundleCreateRequest创建失败");
                return null;
            }

            string[] deps = meta.Dependencies;
            List<AsyncOperation> operations = new List<AsyncOperation>(deps.Length + 1) { main };

            this.Log("尝试加载", name, "的", deps.Length, "个依赖");

            for (int i = 0; i < deps.Length; i++)
            {
                string dep = deps[i];

                if (m_AssetBundleMeta.TryGetValue(dep, out AssetBundleMeta depMeta))
                {
                    AssetBundleCreateRequest request = GetAssetBundleRequest(depMeta);

                    if (request != null && !request.isDone)
                    {
                        operations.Add(request);

                        this.Log("加载第", i + 1, "个依赖：", dep);
                    }
                    else
                    {
                        this.Log("取消添加第", i + 1, "个依赖：", dep, "，request：", request, "，isDone：", request?.isDone);
                    }
                }
                else
                {
                    this.Log("无法加载第", i + 1, "个依赖：", dep, "，因为对应的AssetBundleMeta对象不存在");
                }
            }

            AsyncHandler asyncHandler = new AsyncHandler(operations.ToArray(), 0);
            IAssetBundle ab = new RuntimeAssetBundle(name, asyncHandler);
            m_LoadedAssetBundles.Add(name, ab);
            this.Log(name, "加载成功");
            return ab;
        }

        private AssetBundleCreateRequest GetAssetBundleRequest(AssetBundleMeta meta)
        {
            if (m_Operations.TryGetValue(meta.Name, out AssetBundleCreateRequest request))
            {
                this.Log("从缓存中加载request：", meta.Name);
                return request;
            }

            string filePath = Path.Combine(m_AssetBundleDirectory, meta.FileName);

            if (!File.Exists(filePath))
            {
                this.Log("无法创建request：", meta.Name, "\n因为文件：", filePath, "不存在");
                return null;
            }

            request = AssetBundle.LoadFromFileAsync(filePath);
            m_Operations.Add(meta.Name, request);

            this.Log("从文件中加载request：", meta.Name);
            return request;
        }
    }
}
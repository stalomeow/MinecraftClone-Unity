using System;
using System.Collections;
using System.Collections.Generic;
using Minecraft.Assets;
using UnityEngine;
using XLua;

namespace Minecraft.Lua
{
    public class LuaManager : MonoBehaviour, ILuaCallCSharp
    {
        [SerializeField] private string m_EntryModule;
        [SerializeField] private string m_CleanUpModule;
        [SerializeField] private string[] m_LuaAssetBundles;
        [SerializeField] private string[] m_SearchPaths;
        [NonSerialized] private LuaEnv m_LuaEnv;
        [NonSerialized] private List<AsyncAsset> m_LuaFileAssets;
        [NonSerialized] private Dictionary<string, LuaAsset> m_LuaFileMap;


        public IEnumerator Initialize()
        {
            m_LuaEnv = new LuaEnv();
            m_LuaEnv.AddLoader(LoadLuaFile);
            m_LuaFileAssets = new List<AsyncAsset>();
            m_LuaFileMap = new Dictionary<string, LuaAsset>();

            for (int i = 0; i < m_LuaAssetBundles.Length; i++)
            {
                m_LuaFileAssets.AddRange(AssetManager.Instance.LoadAllAssets(m_LuaAssetBundles[i]));
            }

            yield return AsyncAsset.WaitAll(m_LuaFileAssets);

            for (int i = 0; i < m_LuaFileAssets.Count; i++)
            {
                AsyncAsset asset = m_LuaFileAssets[i];
                m_LuaFileMap.Add(asset.AssetName, asset.GetAssetAs<LuaAsset>());
            }
        }

        public LuaTable CreateTable()
        {
            return m_LuaEnv.NewTable();
        }

        public T GetLuaGlobal<T>(string key)
        {
            return m_LuaEnv.Global.Get<T>(key);
        }

        public void ExecuteLuaScripts()
        {
            m_LuaEnv.DoString($"require '{m_EntryModule}'");
        }

        private void Update()
        {
            if (m_LuaEnv != null)
            {
                m_LuaEnv.Tick();
            }
        }

        private void OnDestroy()
        {
            m_LuaEnv.DoString($"require '{m_CleanUpModule}'");
            m_LuaEnv.Dispose();
            m_LuaEnv = null;

            AssetManager.Instance.UnloadAssets(m_LuaFileAssets.ToArray());
            m_LuaFileAssets = null;
            m_LuaFileMap = null;
        }

        private byte[] LoadLuaFile(ref string filepath)
        {
            string fileName = $"{filepath.Replace('.', '/')}.lua";

            for (int i = 0; i < m_SearchPaths.Length; i++)
            {
                filepath = $"{m_SearchPaths[i]}/{fileName}";

                if (m_LuaFileMap.TryGetValue(filepath, out LuaAsset asset))
                {
                    return asset.GetDecodeBytes();
                }
            }

            return null;
        }
    }
}

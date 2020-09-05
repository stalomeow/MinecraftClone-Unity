using Minecraft.AssetManagement;
using Minecraft.BlocksData;
using Minecraft.DebugUtils;
using Minecraft.ItemsData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;
using Object = UnityEngine.Object;

namespace Minecraft
{
    [LuaCallCSharp]
    public sealed class DataManager : IDisposable, IDebugMessageSender
    {
        private sealed class BlockTypeComparer : IEqualityComparer<BlockType>
        {
            public bool Equals(BlockType x, BlockType y)
            {
                return x == y;
            }

            public int GetHashCode(BlockType obj)
            {
                return (int)obj;
            }
        }

        private sealed class ItemTypeComparer : IEqualityComparer<ItemType>
        {
            public bool Equals(ItemType x, ItemType y)
            {
                return x == y;
            }

            public int GetHashCode(ItemType obj)
            {
                return (int)obj;
            }
        }


        private readonly Dictionary<BlockType, Block> m_BlocksMap;
        private readonly Dictionary<ItemType, Item> m_ItemsMap;
        private readonly Dictionary<string, byte[]> m_LuaMap;
        private readonly AssetBundleLoader m_Loader;
        private readonly LuaEnv m_LuaEnv;

        public Material ChunkMaterial { get; private set; }

        public Material LiquidMaterial { get; private set; }

        public Material BlockEntityMaterial { get; private set; }

        public int BlockCount => m_BlocksMap.Count;

        public int ItemCount => m_ItemsMap.Count;

        string IDebugMessageSender.DisplayName => "DataManager";

        public bool DisableLog { get; set; }

        public DataManager(string resourcePackName)
        {
            m_BlocksMap = new Dictionary<BlockType, Block>(new BlockTypeComparer());
            m_ItemsMap = new Dictionary<ItemType, Item>(new ItemTypeComparer());
            m_LuaMap = new Dictionary<string, byte[]>(StringComparer.Ordinal);
            m_Loader = new AssetBundleLoader(Path.Combine(Application.streamingAssetsPath, WorldConsts.ResourcePackagesFolderName, resourcePackName));
            m_LuaEnv = new LuaEnv();
            
            m_LuaEnv.AddLoader((ref string s) =>
            {
                string name = s.EndsWith(".lua") ? s : s + ".lua";
                m_LuaMap.TryGetValue(name, out byte[] bytes);
                return bytes;
            });
        }


        public IEnumerator InitBlocks()
        {
            IAssetBundle ab = m_Loader.LoadAssetBundle("minecraft/blocks");
            yield return ab.AsyncHandler;

            AsyncAssets assets = ab.LoadAllAssets<Block>();

            while (!assets.IsDone)
            {
                yield return null;
            }

            Object[] blocks = assets.Assets;

            for (int i = 0; i < blocks.Length; i++)
            {
                Block block = blocks[i] as Block;
                m_BlocksMap.Add(block.Type, block);

                this.Log("加载方块：", block.BlockName);
            }
        }

        public IEnumerator InitItems()
        {
            IAssetBundle ab = m_Loader.LoadAssetBundle("minecraft/items");
            yield return ab.AsyncHandler;

            AsyncAssets assets = ab.LoadAllAssets<Item>();

            while (!assets.IsDone)
            {
                yield return null;
            }

            Object[] items = assets.Assets;

            for (int i = 0; i < items.Length; i++)
            {
                Item item = items[i] as Item;
                m_ItemsMap.Add(item.Type, item);

                this.Log("加载物品：", item.ItemName);
            }
        }

        public IEnumerator InitMaterials()
        {
            IAssetBundle ab = m_Loader.LoadAssetBundle("minecraft/materials");
            yield return ab.AsyncHandler;

            AsyncAsset<Material> asset;

            asset = ab.LoadAsset<Material>("chunkmaterial.mat");

            while (!asset.IsDone)
            {
                yield return null;
            }

            ChunkMaterial = asset.Asset;

            this.Log("加载Chunk材质球");

            asset = ab.LoadAsset<Material>("liquidmaterial.mat");

            while (!asset.IsDone)
            {
                yield return null;
            }

            LiquidMaterial = asset.Asset;

            this.Log("加载Liquid材质球");

            asset = ab.LoadAsset<Material>("blockentitymaterial.mat");

            while (!asset.IsDone)
            {
                yield return null;
            }

            BlockEntityMaterial = asset.Asset;

            this.Log("加载BlockEntity材质球");
        }

        public IEnumerator DoLua()
        {
            IAssetBundle ab = m_Loader.LoadAssetBundle("minecraft/lua");
            yield return ab.AsyncHandler;

            AsyncAssets assets = ab.LoadAllAssets<TextAsset>();

            while (!assets.IsDone)
            {
                yield return null;
            }

            Object[] luas = assets.Assets;
            byte[] mainLuaBytes = null;

            for (int i = 0; i < luas.Length; i++)
            {
                TextAsset text = luas[i] as TextAsset;
                byte[] bytes = text.bytes;

                m_LuaMap.Add(text.name, bytes);

                this.Log("加载lua：", text.name);

                if (text.name == "main.lua")
                {
                    mainLuaBytes = bytes;
                }
            }

            if (mainLuaBytes != null)
            {
                m_LuaEnv.DoString(mainLuaBytes);

                this.Log("调用main.lua");
            }

            m_Loader.UnloadAssetBundle(ab, true);
        }


        public void LuaFullGC()
        {
            m_LuaEnv.FullGc();
        }


        public void Dispose()
        {
            DisposeBlockEvents();

            m_BlocksMap.Clear();
            m_ItemsMap.Clear();
            m_LuaMap.Clear();

            m_Loader.UnloadAllAssetBundles(true);
            m_LuaEnv.Dispose();
        }

        private void DisposeBlockEvents()
        {
            foreach (Block block in m_BlocksMap.Values)
            {
                block.ClearEvents();
            }
        }


        public void ForeachAllBlocks(Action<Block> callback)
        {
            if (callback == null)
                return;

            foreach (Block block in m_BlocksMap.Values)
            {
                callback(block);
            }
        }

        public Block GetBlockByType(BlockType type)
        {
            return m_BlocksMap.TryGetValue(type, out Block block) ? block : null;
        }

        public Item GetItemByType(ItemType type)
        {
            return m_ItemsMap.TryGetValue(type, out Item item) ? item : null;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Minecraft.Assets;
using Minecraft.Lua;
using Newtonsoft.Json;
using UnityEngine;
using XLua;

namespace Minecraft.Configurations
{
    [CreateAssetMenu(menuName = "Minecraft/Configurations/BlockTable")]
    public class BlockTable : ScriptableObject, IDisposable, ILuaCallCSharp
    {
        [SerializeField] [EnsureAssetType(typeof(TextAsset))] private AssetPtr m_BlockTableJson;
        [SerializeField] [EnsureAssetType(typeof(TextAsset))] private AssetPtr m_BlockMeshTableJson;
        [SerializeField] [EnsureAssetType(typeof(TextAsset))] private AssetPtr m_BlockTextureTableJson;
        [SerializeField] [EnsureAssetType(typeof(TextAsset))] private AssetPtr m_BlockMaterialTableJson;

        [NonSerialized] private BlockData[] m_Blocks;
        [NonSerialized] private Dictionary<string, BlockData> m_BlockMap;
        [NonSerialized] private IBlockBehaviour[] m_BlockBehaviors;
        [NonSerialized] private AssetPtr[] m_BlockMeshPtrs;
        [NonSerialized] private BlockMesh[] m_BlockMeshes;
        [NonSerialized] private AssetPtr[] m_TexturePtrs;
        [NonSerialized] private Texture2DArray m_TextureArray;
        [NonSerialized] private AssetPtr[] m_MaterialPtrs;
        [NonSerialized] private Material[] m_Materials;

        public int BlockCount => m_Blocks.Length;

        public int MaterialCount => m_Materials.Length;

        public IEnumerator Initialize()
        {
            yield return InitBlocks();
            yield return InitBlockMeshes();
            yield return InitTextures();
            yield return InitMaterials();

            AssetManager.Instance.UnloadAsset(m_BlockTableJson);
            AssetManager.Instance.UnloadAsset(m_BlockMeshTableJson);
            AssetManager.Instance.UnloadAsset(m_BlockTextureTableJson);
            AssetManager.Instance.UnloadAsset(m_BlockMaterialTableJson);
        }

        public void LoadBlockBehavioursInLua(IWorld world)
        {
            m_BlockBehaviors = new IBlockBehaviour[m_Blocks.Length];

            for (int i = 0; i < m_Blocks.Length; i++)
            {
                ref IBlockBehaviour behaviour = ref m_BlockBehaviors[i];
                behaviour = world.LuaManager.GetLuaGlobal<IBlockBehaviour>(m_Blocks[i].InternalName);
                behaviour?.init(world, m_Blocks[i]);
            }
        }

        public void Dispose()
        {
            m_Blocks = null;
            m_BlockMap = null;
            m_BlockBehaviors = null;
            m_BlockMeshes = null;
            m_TextureArray = null;
            m_Materials = null;

            AssetManager.Instance.UnloadAssets(m_BlockMeshPtrs);
            AssetManager.Instance.UnloadAssets(m_MaterialPtrs);
        }


        private IEnumerator InitBlocks()
        {
            AsyncAsset json = AssetManager.Instance.LoadAsset<TextAsset>(m_BlockTableJson);
            yield return json;

            m_Blocks = JsonConvert.DeserializeObject<BlockData[]>(json.GetAssetAs<TextAsset>().text);
            m_BlockMap = new Dictionary<string, BlockData>(m_Blocks.Length);

            for (int i = 0; i < m_Blocks.Length; i++)
            {
                BlockData block = m_Blocks[i];
                m_BlockMap.Add(block.InternalName, block);
            }
        }

        private IEnumerator InitBlockMeshes()
        {
            AsyncAsset json = AssetManager.Instance.LoadAsset<TextAsset>(m_BlockMeshTableJson);
            yield return json;

            m_BlockMeshPtrs = JsonConvert.DeserializeObject<AssetPtr[]>(json.GetAssetAs<TextAsset>().text);
            m_BlockMeshes = new BlockMesh[m_BlockMeshPtrs.Length];

            AsyncAsset[] meshes = AssetManager.Instance.LoadAssets<BlockMesh>(m_BlockMeshPtrs);
            yield return AsyncAsset.WaitAll(m_BlockMeshes, meshes);
        }

        private IEnumerator InitTextures()
        {
            AsyncAsset json = AssetManager.Instance.LoadAsset<TextAsset>(m_BlockTextureTableJson);
            yield return json;

            m_TexturePtrs = JsonConvert.DeserializeObject<AssetPtr[]>(json.GetAssetAs<TextAsset>().text);

            AsyncAsset firstTexAsset = AssetManager.Instance.LoadAsset<Texture2D>(m_TexturePtrs[0]);
            yield return firstTexAsset;

            Texture2D firstTex = firstTexAsset.GetAssetAs<Texture2D>();

            m_TextureArray = new Texture2DArray(firstTex.width, firstTex.height, m_TexturePtrs.Length, firstTex.format, false)
            {
                anisoLevel = firstTex.anisoLevel,
                mipMapBias = firstTex.mipMapBias,
                wrapMode = firstTex.wrapMode,
                filterMode = firstTex.filterMode
            };

            for (int i = 0; i < m_TexturePtrs.Length; i++)
            {
                AsyncAsset texture = AssetManager.Instance.LoadAsset<Texture2D>(m_TexturePtrs[i]);
                yield return texture;

                Graphics.CopyTexture(texture.GetAssetAs<Texture2D>(), 0, 0, m_TextureArray, i, 0);
            }

            AssetManager.Instance.UnloadAssets(m_TexturePtrs);
            m_TexturePtrs = null;
        }

        private IEnumerator InitMaterials()
        {
            AsyncAsset json = AssetManager.Instance.LoadAsset<TextAsset>(m_BlockMaterialTableJson);
            yield return json;

            m_MaterialPtrs = JsonConvert.DeserializeObject<AssetPtr[]>(json.GetAssetAs<TextAsset>().text);
            m_Materials = new Material[m_MaterialPtrs.Length];

            AsyncAsset[] materials = AssetManager.Instance.LoadAssets<Material>(m_MaterialPtrs);
            yield return AsyncAsset.WaitAll(m_Materials, materials);
        }


        public BlockData GetBlock(int id)
        {
            return m_Blocks[id];
        }

        public BlockData GetBlock(string internalName)
        {
            return m_BlockMap[internalName];
        }

        public IBlockBehaviour GetBlockBehaviour(int id)
        {
            return m_BlockBehaviors[id];
        }

        public BlockMesh GetMesh(int index)
        {
            return m_BlockMeshes[index];
        }

        public Material GetMaterial(int index)
        {
            return m_Materials[index];
        }

        public Texture2DArray GetTextureArray()
        {
            return m_TextureArray;
        }
    }
}

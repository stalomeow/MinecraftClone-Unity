using System.Collections.Generic;
using UnityEngine;

namespace Minecraft.Rendering.Tests
{
    [DisallowMultipleComponent]
    public sealed class TestBlocksRenderer : MonoBehaviour
    {
        [SerializeField] [Range(1, 10)] private int m_WorldWidth = 5;
        [SerializeField] [Range(1, 10)] private int m_WorldHeight = 5;
        [SerializeField] private Material m_ChunkMaterial;
        [SerializeField] private Block m_SurfaceBlock;
        [SerializeField] private Block m_FillerBlock;
        [SerializeField] private Block m_BedrockBlock;

        private BlockTextureTable m_TextureTable;
        private MaterialPropertyBlock m_PropertyBlock;
        private List<AbstractMesh> m_Meshes;

        private void Start()
        {
            m_TextureTable = BlockTextureTable.BuildTable(m_SurfaceBlock, m_FillerBlock, m_BedrockBlock);
            m_PropertyBlock = new MaterialPropertyBlock();
            m_Meshes = new List<AbstractMesh>();

            m_PropertyBlock.SetTexture("_TexArr", m_TextureTable.TextureArray);

            Debug.Log($"load {m_TextureTable.TextureCount} textures.");

            GenerateBlocks();
        }

        private void Update()
        {
            foreach (AbstractMesh mesh in m_Meshes)
            {
                mesh.Render(Vector3.zero, Quaternion.identity, m_ChunkMaterial, m_PropertyBlock, Camera.main, WorldConsts.BlockLayer);
            }
        }

        private void GenerateBlocks()
        {
            for (int x = 0; x < m_WorldWidth; x++)
            {
                for (int z = 0; z < m_WorldWidth; z++)
                {
                    for (int y = 0; y < m_WorldHeight; y++)
                    {
                        if (y == 0)
                        {
                            AddBlock(x, y, z, m_BedrockBlock);
                        }
                        else if (y == m_WorldHeight - 1)
                        {
                            AddBlock(x, y, z, m_SurfaceBlock);
                        }
                        else
                        {
                            AddBlock(x, y, z, m_FillerBlock);
                        }
                    }
                }
            }
        }

        private void AddBlock(int x, int y, int z, Block block)
        {
            BlockMesh mesh = new BlockMesh();
            BlockMeshWriter meshWriter = block.MeshWriter;

            if (!meshWriter.IsEmpty)
            {
                meshWriter.Write(mesh, x, y, z, block, m_TextureTable, EvalBlockLight);
            }

            mesh.Apply();

            m_Meshes.Add(mesh);
        }

        private bool EvalBlockLight(int x, int y, int z, Block block, BlockDirection? dir, out VertexLightingData lighting)
        {
            lighting = new VertexLightingData(LightingUtility.MaxLight);
            return true;
        }
    }
}
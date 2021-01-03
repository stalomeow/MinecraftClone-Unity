using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Minecraft.Rendering;

namespace Minecraft
{
    public sealed class World : MonoBehaviour
    {
        public static World Active { get; private set; }

        [SerializeField] private Block[] m_Blocks;
        [SerializeField] private ChunkManager m_ChunkManager;
        [SerializeField] private RenderingManager m_RenderingManager;
        [SerializeField] private WorldType m_WorldType;
        [SerializeField] private int m_Seed;

        public int Seed => m_Seed;

        private Dictionary<int, Block> m_BlockMap;
        

        private void Start()
        {
            Active = this;
            m_BlockMap = new Dictionary<int, Block>(m_Blocks.Length);

            foreach (Block block in m_Blocks)
            {
                m_BlockMap.Add(block.Id, block);
            }


            m_ChunkManager.Initialize(this);
            m_RenderingManager.Initialize(m_Blocks);

            for (int x = -16 * 1; x <= 16 * 1; x += 16)
            {
                for (int z = -16 * 1; z <= 16 * 1; z += 16)
                {
                    Chunk chunk = m_ChunkManager.LoadChunk(x, z);
                    chunk.GeometryChanged += m_RenderingManager.UpdateChunkGeometry;
                    chunk.Initialize(x, z, this);
                }
            }

            m_RenderingManager.SetVisibleChunks(m_ChunkManager.Chunks.GetEnumerator());
        }

        private void Update()
        {
            m_RenderingManager.Update();
        }

        public void LightBlock(int x, int y, int z)
        {

        }

        public void TickBlock(int x, int y, int z)
        {
            
        }

        public byte GetLightValue(int x, int y, int z)
        {
            Vector2Int pos = Chunk.GetChunkPosition(x, z);
            Chunk chunk = GetChunk(pos.x, pos.y);

            if (chunk == null)
            {
                return LightingUtility.SkyLight;
            }

            chunk.WorldToLocalPosition(ref x, ref z);
            return chunk.GetLightValue(x, y, z);
        }

        public Chunk GetChunk(int x, int z)
        {
            return m_ChunkManager.GetChunk(x, z);
        }

        public Block GetBlock(byte blockId)
        {
            return m_BlockMap[blockId];
        }

        public void SetBlock(int x, int y, int z, byte id)
        {
            Vector2Int pos = Chunk.GetChunkPosition(x, z);
            Chunk chunk = GetChunk(pos.x, pos.y);

            if (chunk != null)
            {
                chunk.WorldToLocalPosition(ref x, ref z);
                chunk.SetBlockId(x, y, z, id);
            }
        }

        public Block GetBlock(int x, int y, int z, bool nullable = false)
        {
            Vector2Int pos = Chunk.GetChunkPosition(x, z);
            Chunk chunk = GetChunk(pos.x, pos.y);

            if (chunk == null)
            {
                return nullable ? null : m_BlockMap[Block.AirId];
            }

            chunk.WorldToLocalPosition(ref x, ref z);
            return GetBlock(chunk.GetBlockId(x, y, z));
        }
    }
}
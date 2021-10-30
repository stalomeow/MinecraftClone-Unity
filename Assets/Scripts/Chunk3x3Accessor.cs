using System;
using System.Collections;
using System.Collections.Generic;
using Minecraft.Configurations;
using UnityEngine;
using UnityEngine.Assertions;
using static Minecraft.WorldConsts;

namespace Minecraft
{
    public class Chunk3x3Accessor : IWorldRAccessor, IEnumerable<Chunk>, IDisposable
    {
        public const int XOffsetBegin = -1;
        public const int XOffsetEnd = 1;
        public const int ZOffsetBegin = -1;
        public const int ZOffsetEnd = 1;


        private readonly Chunk[,] m_Members;

        public bool Accessible { get; private set; }

        public Vector3Int WorldSpaceOrigin => this[0, 0].WorldSpaceOrigin;

        public IWorld World => this[0, 0].World;

        public Chunk this[int xOffset, int zOffset]
        {
            get
            {
                if (!Accessible)
                {
                    throw new InvalidOperationException("Chunk3x3Accessor is not accessible.");
                }

                return m_Members[xOffset + 1, zOffset + 1];
            }
        }


        public Chunk3x3Accessor()
        {
            m_Members = new Chunk[XOffsetEnd - XOffsetBegin + 1, ZOffsetEnd - ZOffsetBegin + 1];
            Accessible = false;
        }

        public void Initialize(ChunkPos pos, ChunkManager chunkManager)
        {
            for (int x = XOffsetBegin; x <= XOffsetEnd; x++)
            {
                for (int z = ZOffsetBegin; z <= ZOffsetEnd; z++)
                {
                    ChunkPos neighbor = pos.AddOffset(x, z);
                    bool result = chunkManager.GetChunk(neighbor, false, out Chunk chunk);
                    Assert.IsTrue(result);
                    m_Members[x + 1, z + 1] = chunk;
                }
            }

            Accessible = true;
        }

        public void Dispose()
        {
            Array.Clear(m_Members, 0, m_Members.Length);
            Accessible = false;
        }

        private Chunk GetChunk(ref int x, ref int z)
        {
            if (!Accessible)
            {
                throw new InvalidOperationException("Chunk3x3Accessor is not accessible.");
            }

            int xOffset = Mathf.FloorToInt((float)x / ChunkWidth);
            int zOffset = Mathf.FloorToInt((float)z / ChunkWidth);
            x -= xOffset * ChunkWidth;
            z -= zOffset * ChunkWidth;
            return m_Members[xOffset + 1, zOffset + 1];
        }

        public BlockData GetBlock(int x, int y, int z, BlockData defaultValue = null)
        {
            Chunk chunk = GetChunk(ref x, ref z);
            return chunk.GetBlock(x, y, z, defaultValue);
        }

        public Quaternion GetBlockRotation(int x, int y, int z, Quaternion defaultValue = default)
        {
            Chunk chunk = GetChunk(ref x, ref z);
            return chunk.GetBlockRotation(x, y, z, defaultValue);
        }

        public int GetMixedLightLevel(int x, int y, int z, int defaultValue = 0)
        {
            Chunk chunk = GetChunk(ref x, ref z);
            return chunk.GetMixedLightLevel(x, y, z, defaultValue);
        }

        public int GetSkyLight(int x, int y, int z, int defaultValue = 0)
        {
            Chunk chunk = GetChunk(ref x, ref z);
            return chunk.GetSkyLight(x, y, z, defaultValue);
        }

        public int GetAmbientLight(int x, int y, int z, int defaultValue = 0)
        {
            Chunk chunk = GetChunk(ref x, ref z);
            return chunk.GetAmbientLight(x, y, z, defaultValue);
        }

        public int GetTopVisibleBlockY(int x, int z, int defaultValue = 0)
        {
            Chunk chunk = GetChunk(ref x, ref z);
            return chunk.GetTopVisibleBlockY(x, z, defaultValue);
        }

        public IEnumerator<Chunk> GetEnumerator()
        {
            for (int x = 0; x < m_Members.GetLength(0); x++)
            {
                for (int z = 0; z < m_Members.GetLength(1); z++)
                {
                    yield return m_Members[x, z];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

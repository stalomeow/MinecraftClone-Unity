using System;
using Minecraft.Configurations;
using UnityEngine;
using static Minecraft.WorldConsts;
using Random = System.Random;

namespace Minecraft.ScriptableWorldGeneration
{
    [CreateAssetMenu(menuName = "Minecraft/WorldGeneration/MineGenerator")]
    public class MineGenerator : StatelessGenerator
    {
        [SerializeField] private string m_OreBlock;
        [SerializeField] private int m_Size = 17;
        [SerializeField] private int m_Count = 20;
        [SerializeField] private int m_MinHeight = 0;
        [SerializeField] private int m_MaxHeight = 128;
        [SerializeField] private string[] m_ReplaceableBlocks;

        public override void Generate(IWorld world, ChunkPos pos, BlockData[,,] blocks, Quaternion[,,] rotations, byte[,] heightMap, GenerationHelper helper, GenerationContext context)
        {
            int minHeight = m_MinHeight;
            int maxHeight = m_MaxHeight;
            int count = m_Count;
            BlockData ore = world.BlockDataTable.GetBlock(m_OreBlock);

            int chunkSeed = pos.X ^ pos.Z ^ helper.Seed ^ ore.ID;
            Random random = new Random(chunkSeed);

            if (minHeight > maxHeight)
            {
                int tmp = minHeight;
                minHeight = maxHeight;
                maxHeight = tmp;
            }
            else if (maxHeight == minHeight)
            {
                if (minHeight < ChunkHeight - 1)
                {
                    ++maxHeight;
                }
                else
                {
                    --minHeight;
                }
            }

            for (int i = 0; i < count; i++)
            {
                int x = pos.X + random.Next(ChunkWidth);
                int y = random.Next(minHeight, maxHeight);
                int z = pos.Z + random.Next(ChunkWidth);
                Generate(x, y, z, blocks, random, ore, count);
            }
        }

        public void Generate(int x, int y, int z, BlockData[,,] blocks, Random random, BlockData ore, int count)
        {
            // 在xz平面上的方向
            float angle = (float)random.NextDouble() * Mathf.PI;
            float xRange = Mathf.Sin(angle) * count / 8.0f;
            float zRange = Mathf.Cos(angle) * count / 8.0f;

            // 起始点和结束点
            float startX = (x + 8) + xRange;
            float endX = (x + 8) - xRange;
            float startZ = (z + 8) + zRange;
            float endZ = (z + 8) - zRange;
            float startY = (y - 2) + random.Next(3);
            float endY = (y - 2) + random.Next(3);

            for (int i = 0; i < count; i++)
            {
                // 插值参数
                float t = (float)i / count;

                // 椭球中心
                float centerX = Mathf.Lerp(startX, endX, t);
                float centerY = Mathf.Lerp(startY, endY, t);
                float centerZ = Mathf.Lerp(startZ, endZ, t);

                // 椭球尺寸（可以看出 XZ 和 Y 尺寸一样，应该是球）
                float scale = (float)random.NextDouble() * count / 16.0f;
                float diameterXZ = (Mathf.Sin(Mathf.PI * t) + 1) * scale + 1;
                float diameterY = (Mathf.Sin(Mathf.PI * t) + 1) * scale + 1;

                // 椭球包围盒
                int minX = Mathf.FloorToInt(centerX - diameterXZ * 0.5f);
                int minY = Mathf.FloorToInt(centerY - diameterY * 0.5f);
                int minZ = Mathf.FloorToInt(centerZ - diameterXZ * 0.5f);
                int maxX = Mathf.FloorToInt(centerX + diameterXZ * 0.5f);
                int maxY = Mathf.FloorToInt(centerY + diameterY * 0.5f);
                int maxZ = Mathf.FloorToInt(centerZ + diameterXZ * 0.5f);

                // 把这个椭球里的方块替换为矿石
                for (int dx = minX; dx <= maxX; dx++)
                {
                    float xDist = (dx + 0.5f - centerX) / (diameterXZ * 0.5f);

                    // 参考椭球方程
                    if (xDist * xDist < 1)
                    {
                        for (int dy = minY; dy <= maxY; dy++)
                        {
                            float yDist = (dy + 0.5f - centerY) / (diameterY * 0.5f);

                            if (dy < 0 || dy >= ChunkHeight)
                            {
                                continue;
                            }

                            // 参考椭球方程
                            if (xDist * xDist + yDist * yDist < 1)
                            {
                                for (int dz = minZ; dz <= maxZ; dz++)
                                {
                                    float zDist = (dz + 0.5f - centerZ) / (diameterXZ * 0.5f);

                                    // 参考椭球方程
                                    if (xDist * xDist + yDist * yDist + zDist * zDist < 1)
                                    {
                                        SetBlock(dx, dy, dz, blocks, ore);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void SetBlock(int x, int y, int z, BlockData[,,] blocks, BlockData ore)
        {
            x %= ChunkWidth;
            z %= ChunkWidth;

            if (x < 0)
            {
                x += ChunkWidth;
            }

            if (z < 0)
            {
                z += ChunkWidth;
            }

            ref BlockData block = ref blocks[x, y, z];

            if (Array.IndexOf(m_ReplaceableBlocks, block.InternalName) != -1)
            {
                block = ore;
            }
        }
    }
}

using System;
using Minecraft.Configurations;
using Minecraft.PhysicSystem;
using UnityEngine;
using static Minecraft.WorldConsts;
using Random = System.Random;

namespace Minecraft.ScriptableWorldGeneration
{
    [CreateAssetMenu(menuName = "Minecraft/WorldGeneration/PlantGenerator")]
    public class PlantGenerator : StatelessGenerator
    {
        protected enum PlantType
        {
            Grass,
            Flower,
            Cacti,
            Mushroom
        }

        [SerializeField] protected PlantType m_PlantType;
        [SerializeField] protected string m_PlantBlock;
        [SerializeField] [Range(1, 10)] protected int m_MinPlantHeight = 1;
        [SerializeField] [Range(0, 5)] protected int m_MaxPlantHeightDelta = 0;
        [SerializeField] protected string[] m_BlocksToGrowOn;
        [SerializeField] protected string[] m_ReplaceableBlocks;


        protected bool IsBlockReplaceable(BlockData block)
        {
            return Array.IndexOf(m_ReplaceableBlocks, block.InternalName) != -1;
        }

        protected bool CanGrowPlant(int x, int y, int z, BlockData[,,] blocks, int height)
        {
            if (y < 0 || (y + height) >= ChunkHeight)
            {
                return false;
            }

            if (Array.IndexOf(m_BlocksToGrowOn, blocks[x, y, z].InternalName) == -1)
            {
                return false;
            }

            for (int by = y + 1; by <= y + height; by++)
            {
                if (!IsBlockReplaceable(blocks[x, by, z]))
                {
                    return false;
                }
            }

            return true;
        }

        public override void Generate(IWorld world, ChunkPos pos, BlockData[,,] blocks, Quaternion[,,] rotations, byte[,] heightMap, GenerationHelper helper, GenerationContext context)
        {
            BiomeData biome = context.Biomes[8, 8];
            Random random = new Random(pos.X ^ pos.Z ^ helper.Seed);
            BlockData plantBlock = world.BlockDataTable.GetBlock(m_PlantBlock);
            int height = random.Next(m_MaxPlantHeightDelta) + m_MinPlantHeight;
            int count = random.Next(m_PlantType switch
            {
                PlantType.Grass => biome.GrassPerChunk,
                PlantType.Flower => biome.FlowersPerChunk,
                PlantType.Cacti => biome.CactiPerChunk,
                PlantType.Mushroom => biome.MushroomsPerChunk,
                _ => throw new NotImplementedException()
            });

            for (int i = 0; i < count; i++)
            {
                int x = random.Next(ChunkWidth);
                int z = random.Next(ChunkWidth);

                int h = 0;

                for (int y = ChunkHeight - 1; y >= 0; y--)
                {
                    if (blocks[x, y, z].PhysicState == PhysicState.Solid)
                    {
                        h = y;
                        break;
                    }
                }

                if (CanGrowPlant(x, h, z, blocks, height))
                {
                    for (int y = h + 1; y <= h + height; y++)
                    {
                        blocks[x, y, z] = plantBlock;
                    }
                }
            }
        }
    }
}

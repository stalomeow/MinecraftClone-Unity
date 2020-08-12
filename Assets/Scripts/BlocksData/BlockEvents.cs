using System.Collections;
using UnityEngine;

#pragma warning disable IDE0060 // 删除未使用的参数

namespace Minecraft.BlocksData
{
    [CreateAssetMenu(menuName = "Minecraft/BlockEvents", fileName = "_Events")]
    public class BlockEvents : ScriptableObject
    {
        //格式：BlockEvent_${event}_${block|block_types}

        public void BlockEvent_OnRandomTick_Dirt(int x, int y, int z, Block block)
        {
            WorldManager world = WorldManager.Active;
            Block up = world.GetBlock(x, y + 1, z);

            BlockType left = world.GetBlockType(x - 1, y, z);
            BlockType right = world.GetBlockType(x + 1, y, z);
            BlockType front = world.GetBlockType(x, y, z + 1);
            BlockType back = world.GetBlockType(x, y, z - 1);

            int grassCount = 0;

            if (left == BlockType.Grass)
                grassCount++;

            if (right == BlockType.Grass)
                grassCount++;

            if (front == BlockType.Grass)
                grassCount++;

            if (back == BlockType.Grass)
                grassCount++;


            if ((up.Type == BlockType.Air || up.HasAnyFlag(BlockFlags.FlowersAndPlants)) && grassCount > 2)
            {
                world.SetBlockType(x, y, z, BlockType.Grass); // 草的传播
            }
        }

        public void BlockEvent_OnTick_Grass(int x, int y, int z, Block block)
        {
            WorldManager world = WorldManager.Active;
            Block up = world.GetBlock(x, y + 1, z);

            // 不是植物或者空气就设置成泥土
            if (!up.HasAllFlags(BlockFlags.FlowersAndPlants) && up.Type != BlockType.Air)
            {
                world.SetBlockType(x, y, z, BlockType.Dirt);
            }
        }

        public void BlockEvent_OnTick_PlantsAndFlowers(int x, int y, int z, Block block)
        {
            WorldManager world = WorldManager.Active;
            BlockType down = world.GetBlockType(x, y - 1, z);

            if (down != BlockType.Dirt && down != BlockType.Grass)
            {
                world.SetBlockType(x, y, z, BlockType.Air);
            }
        }

        public void BlockEvent_OnTick_Water(int x, int y, int z, Block block)
        {
            WorldManager world = WorldManager.Active;

            Block down = world.GetBlock(x, y - 1, z);
            Block left = world.GetBlock(x - 1, y, z);
            Block right = world.GetBlock(x + 1, y, z);
            Block front = world.GetBlock(x, y, z + 1);
            Block back = world.GetBlock(x, y, z - 1);

            if (down.Type == BlockType.Air || down.HasAnyFlag(BlockFlags.FlowersAndPlants))
            {
                world.SetBlockType(x, y - 1, z, BlockType.Water);
            }

            if (left.Type == BlockType.Air || left.HasAnyFlag(BlockFlags.FlowersAndPlants))
            {
                world.SetBlockType(x - 1, y, z, BlockType.Water);
            }

            if (right.Type == BlockType.Air || right.HasAnyFlag(BlockFlags.FlowersAndPlants))
            {
                world.SetBlockType(x + 1, y, z, BlockType.Water);
            }

            if (front.Type == BlockType.Air || front.HasAnyFlag(BlockFlags.FlowersAndPlants))
            {
                world.SetBlockType(x, y, z + 1, BlockType.Water);
            }

            if (back.Type == BlockType.Air || back.HasAnyFlag(BlockFlags.FlowersAndPlants))
            {
                world.SetBlockType(x, y, z - 1, BlockType.Water);
            }
        }

        public void BlockEvent_OnCick_TNT(int x, int y, int z, Block block)
        {
            WorldManager world = WorldManager.Active;

            world.SetBlockType(x, y, z, BlockType.Air);
            TNTBlockEntity tnt = world.EntityManager.CreateEntity<TNTBlockEntity>();
            tnt.Initialize(x, y, z, block);
        }

        private void BlockEvent_OnClick_CraftingTable(int x, int y, int z, Block block)
        {

        }
    }
}
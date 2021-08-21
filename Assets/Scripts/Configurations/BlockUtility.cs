using Minecraft.Entities;
using UnityEngine;
using XLua;

namespace Minecraft.Configurations
{
    [LuaCallCSharp]
    public static class BlockUtility
    {
        public static bool HasFlag(this BlockData block, BlockFlags flag)
        {
            return (block.Flags & flag) == flag;
        }

        public static void Tick(this BlockData block, IWorld world, int x, int y, int z)
        {
            world.BlockDataTable.GetBlockBehaviour(block.ID)?.tick(x, y, z);
        }

        public static void Place(this BlockData block, IWorld world, int x, int y, int z)
        {
            world.BlockDataTable.GetBlockBehaviour(block.ID)?.place(x, y, z);
        }

        public static void Destroy(this BlockData block, IWorld world, int x, int y, int z)
        {
            world.BlockDataTable.GetBlockBehaviour(block.ID)?.destroy(x, y, z);
        }

        public static void Click(this BlockData block, IWorld world, int x, int y, int z)
        {
            world.BlockDataTable.GetBlockBehaviour(block.ID)?.click(x, y, z);
        }

        public static void EntityInit(this BlockData block, IWorld world, IAABBEntity entity, LuaTable context)
        {
            world.BlockDataTable.GetBlockBehaviour(block.ID)?.entity_init(entity, context);
        }

        public static void EntityDestroy(this BlockData block, IWorld world, IAABBEntity entity, LuaTable context)
        {
            world.BlockDataTable.GetBlockBehaviour(block.ID)?.entity_destroy(entity, context);
        }

        public static void EntityUpdate(this BlockData block, IWorld world, IAABBEntity entity, LuaTable context)
        {
            world.BlockDataTable.GetBlockBehaviour(block.ID)?.entity_update(entity, context);
        }

        public static void EntityFixedUpdate(this BlockData block, IWorld world, IAABBEntity entity, LuaTable context)
        {
            world.BlockDataTable.GetBlockBehaviour(block.ID)?.entity_fixed_update(entity, context);
        }

        public static void EntityOnCollisions(this BlockData block, IWorld world, IAABBEntity entity, CollisionFlags flags, LuaTable context)
        {
            world.BlockDataTable.GetBlockBehaviour(block.ID)?.entity_on_collisions(entity, flags, context);
        }
    }
}

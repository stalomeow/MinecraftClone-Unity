using Minecraft.Configurations;
using UnityEngine;

namespace Minecraft.PhysicSystem
{
    [XLua.LuaCallCSharp]
    public static class BlockPhysicsUtility
    {
        public static AABB? GetBoundingBox(this BlockData block, float x, float y, float z, IWorld world)
        {
            return GetBoundingBox(block, new Vector3(x, y, z), world);
        }

        public static AABB? GetBoundingBox(this BlockData block, Vector3 position, IWorld world)
        {
            if (block == null || block.HasFlag(BlockFlags.IgnoreCollisions))
            {
                return null;
            }

            BlockMesh mesh = world.BlockDataTable.GetMesh(block.Mesh.Value);
            return mesh.BoundingBox + position;
        }
    }
}
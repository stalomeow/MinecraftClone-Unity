using Minecraft.Configurations;
using UnityEngine;

namespace Minecraft.PhysicSystem
{
    [XLua.LuaCallCSharp]
    public static class BlockPhysicsUtility
    {
        public static AABB GetBoundingBox(this BlockData block, float x, float y, float z)
        {
            return GetBoundingBox(block, new Vector3(x, y, z));
        }

        public static AABB GetBoundingBox(this BlockData block, Vector3 position)
        {
            if (block == null || block.HasFlag(BlockFlags.IgnoreCollisions))
            {
                return new AABB(position, position);
            }

            return new AABB(position, position + Vector3.one);
        }
    }
}
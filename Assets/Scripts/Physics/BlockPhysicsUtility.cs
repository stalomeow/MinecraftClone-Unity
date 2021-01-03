using UnityEngine;

namespace Minecraft.XPhysics
{
    public static class BlockPhysicsUtility
    {
        public static AABB GetBoundingBox(this Block block, float x, float y, float z)
        {
            return GetBoundingBox(block, new Vector3(x, y, z));
        }

        public static AABB GetBoundingBox(this Block block, Vector3 position)
        {
            if (block.HasAnyFlag(BlockFlags.IgnoreCollisions))
            {
                return new AABB(position, position);
            }

            return block.MeshWriter.MeshBoundingBox + position;
        }
    }
}
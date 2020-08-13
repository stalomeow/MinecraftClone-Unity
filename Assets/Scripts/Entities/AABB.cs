using Minecraft.BlocksData;
using UnityEngine;

namespace Minecraft
{
    public struct AABB
    {
        public Vector3 Min { get; set; }

        public Vector3 Max { get; set; }

        public Vector3 Center => (Min + Max) * 0.5f;

        public AABB(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }

        public bool Intersects(AABB aabb)
        {
            return ((Min.x >= aabb.Min.x && Min.x <= aabb.Max.x) || (aabb.Min.x >= Min.x && aabb.Min.x <= Max.x))
                   && ((Min.y >= aabb.Min.y && Min.y <= aabb.Max.y) || (aabb.Min.y >= Min.y && aabb.Min.y <= Max.y))
                   && ((Min.z >= aabb.Min.z && Min.z <= aabb.Max.z) || (aabb.Min.z >= Min.z && aabb.Min.z <= Max.z));
        }

        public static AABB CreateBlockAABB(float x, float y, float z)
        {
            return new AABB(new Vector3(x, y, z), new Vector3(x + 1, y + 1, z + 1));
        }

        public static AABB CreateBlockAABB(float x, float y, float z, Block block)
        {
            Vector3 min = new Vector3(x, y, z);
            Vector3 max = block.HasAnyFlag(BlockFlags.IgnoreCollisions) ? min : new Vector3(x + 1, y + 1, z + 1);
            return new AABB(min, max);
        }

        public static AABB Merge(AABB left, AABB right)
        {
            Vector3 min = Vector3.Min(left.Min, right.Min);
            Vector3 max = Vector3.Max(left.Max, right.Max);
            return new AABB(min, max);
        }

        public static AABB operator +(AABB aabb, Vector3 vec) => new AABB(aabb.Min + vec, aabb.Max + vec);

        public static AABB operator -(AABB aabb, Vector3 vec) => new AABB(aabb.Min - vec, aabb.Max - vec);
    }
}
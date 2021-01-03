using UnityEngine;

namespace Minecraft.XPhysics
{
    public struct BlockRaycastHit
    {
        public Vector3Int Position { get; }

        public Vector3 Normal { get; }

        public World World { get; }

        public Block Block { get; }


        public BlockRaycastHit(Vector3Int pos, Vector3 normal, World world, Block block)
        {
            Position = pos;
            Normal = normal;
            World = world;
            Block = block;
        }
    }
}

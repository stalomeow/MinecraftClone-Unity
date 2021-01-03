using Minecraft.XPhysics;
using UnityEngine;

namespace Minecraft.Rendering
{
    public abstract class BlockMeshWriter : ScriptableObject
    {
        public delegate bool LightEvaluator(
            int x, int y, int z, Block block, BlockDirection? direction, out VertexLightingData lighting
        );

        public abstract bool IsEmpty { get; }

        public abstract AABB MeshBoundingBox { get; }

        public abstract string[] RequiredTextureNames { get; }

        public abstract void Write(
            AbstractMesh mesh, int x, int y, int z, Block block, BlockTextureTable texTable, LightEvaluator eval
        );

        protected BlockMeshWriter() { }
    }
}
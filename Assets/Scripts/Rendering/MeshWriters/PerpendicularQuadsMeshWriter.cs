using Minecraft.XPhysics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Minecraft.Rendering
{
    [CreateAssetMenu(menuName = "Minecraft/Block Mesh Writer/Perpendicular Quads", fileName = "Perpendicular Quads")]
    public class PerpendicularQuadsMeshWriter : BlockMeshWriter
    {
        public override string[] RequiredTextureNames => new string[1]
        {
            "Main"
        };

        public override AABB MeshBoundingBox => new AABB(Vector3.zero, Vector3.one);

        public override bool IsEmpty => false;

        public override void Write(
            AbstractMesh mesh, int x, int y, int z, Block block, BlockTextureTable texTable, LightEvaluator eval
        )
        {
            if (eval(x, y, z, block, null, out VertexLightingData lighting))
            {
                int texOffset = texTable[block, 0];

                AddTriangles(mesh);
                AddFirst(mesh, x, y, z, texOffset, in lighting);

                AddTriangles(mesh);
                AddSecond(mesh, x, y, z, texOffset, in lighting);
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void AddTriangles(AbstractMesh mesh)
        {
            mesh.AddTriangle(0, 3, 2);
            mesh.AddTriangle(2, 1, 0);

            mesh.AddTriangle(3, 0, 1);
            mesh.AddTriangle(1, 2, 3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void AddFirst(AbstractMesh buffer, int x, int y, int z, int texOffset, in VertexLightingData lighting)
        {
            Vector3 normal = new Vector3(-0.70711f, 0, -0.70711f);

            buffer.AddVertex(new Vector3(x, y, z + 1), normal, Vector2.zero, texOffset, lighting.LeftBottom);
            buffer.AddVertex(new Vector3(x + 1, y, z), normal, Vector2.right, texOffset, lighting.RightBottom);
            buffer.AddVertex(new Vector3(x + 1, y + 1, z), normal, Vector2.one, texOffset, lighting.RightTop);
            buffer.AddVertex(new Vector3(x, y + 1, z + 1), normal, Vector2.up, texOffset, lighting.LeftTop);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void AddSecond(AbstractMesh buffer, int x, int y, int z, int texOffset, in VertexLightingData lighting)
        {
            Vector3 normal = new Vector3(-0.70711f, 0, 0.70711f);

            buffer.AddVertex(new Vector3(x, y, z), normal, Vector2.zero, texOffset, lighting.LeftBottom);
            buffer.AddVertex(new Vector3(x + 1, y, z + 1), normal, Vector2.right, texOffset, lighting.RightBottom);
            buffer.AddVertex(new Vector3(x + 1, y + 1, z + 1), normal, Vector2.one, texOffset, lighting.RightTop);
            buffer.AddVertex(new Vector3(x, y + 1, z), normal, Vector2.up, texOffset, lighting.LeftTop);
        }
    }
}

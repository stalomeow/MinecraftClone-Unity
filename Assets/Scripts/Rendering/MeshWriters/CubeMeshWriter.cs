using Minecraft.XPhysics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Minecraft.Rendering
{
    [CreateAssetMenu(menuName = "Minecraft/Block Mesh Writer/Cube", fileName = "Cube")]
    public class CubeMeshWriter : BlockMeshWriter
    {
        public override string[] RequiredTextureNames => new string[6]
        {
            "Positive X",
            "Positive Y",
            "Positive Z",
            "Negative X",
            "Negative Y",
            "Negative Z"
        };

        public override AABB MeshBoundingBox => new AABB(Vector3.zero, Vector3.one);

        public override bool IsEmpty => false;

        public override void Write(
            AbstractMesh mesh, int x, int y, int z, Block block, BlockTextureTable texTable, LightEvaluator eval
        )
        {
            // 遍历6个面
            for (int i = 0; i < 6; i++)
            {
                BlockDirection direction = (BlockDirection)i;

                if (eval(x, y, z, block, direction, out VertexLightingData lighting))
                {
                    AddTriangles(mesh);

                    int texOffset = texTable[block, i];

                    switch (direction)
                    {
                        case BlockDirection.PositiveX: AddPositiveX(mesh, x, y, z, texOffset, in lighting); break;
                        case BlockDirection.PositiveY: AddPositiveY(mesh, x, y, z, texOffset, in lighting); break;
                        case BlockDirection.PositiveZ: AddPositiveZ(mesh, x, y, z, texOffset, in lighting); break;
                        case BlockDirection.NegativeX: AddNegativeX(mesh, x, y, z, texOffset, in lighting); break;
                        case BlockDirection.NegativeY: AddNegativeY(mesh, x, y, z, texOffset, in lighting); break;
                        case BlockDirection.NegativeZ: AddNegativeZ(mesh, x, y, z, texOffset, in lighting); break;
                    }
                }
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void AddTriangles(AbstractMesh mesh)
        {
            mesh.AddTriangle(0, 3, 2);
            mesh.AddTriangle(2, 1, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void AddPositiveX(AbstractMesh mesh, int x, int y, int z, int texOffset, in VertexLightingData lighting)
        {
            mesh.AddVertex(new Vector3(x + 1, y, z), Vector3.right, Vector2.zero, texOffset, lighting.LeftBottom);
            mesh.AddVertex(new Vector3(x + 1, y, z + 1), Vector3.right, Vector2.right, texOffset, lighting.RightBottom);
            mesh.AddVertex(new Vector3(x + 1, y + 1, z + 1), Vector3.right, Vector2.one, texOffset, lighting.RightTop);
            mesh.AddVertex(new Vector3(x + 1, y + 1, z), Vector3.right, Vector2.up, texOffset, lighting.LeftTop);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void AddPositiveY(AbstractMesh mesh, int x, int y, int z, int texOffset, in VertexLightingData lighting)
        {
            mesh.AddVertex(new Vector3(x, y + 1, z), Vector3.up, Vector2.zero, texOffset, lighting.LeftBottom);
            mesh.AddVertex(new Vector3(x + 1, y + 1, z), Vector3.up, Vector2.right, texOffset, lighting.RightBottom);
            mesh.AddVertex(new Vector3(x + 1, y + 1, z + 1), Vector3.up, Vector2.one, texOffset, lighting.RightTop);
            mesh.AddVertex(new Vector3(x, y + 1, z + 1), Vector3.up, Vector2.up, texOffset, lighting.LeftTop);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void AddPositiveZ(AbstractMesh mesh, int x, int y, int z, int texOffset, in VertexLightingData lighting)
        {
            mesh.AddVertex(new Vector3(x + 1, y, z + 1), Vector3.forward, Vector2.zero, texOffset, lighting.LeftBottom);
            mesh.AddVertex(new Vector3(x, y, z + 1), Vector3.forward, Vector2.right, texOffset, lighting.RightBottom);
            mesh.AddVertex(new Vector3(x, y + 1, z + 1), Vector3.forward, Vector2.one, texOffset, lighting.RightTop);
            mesh.AddVertex(new Vector3(x + 1, y + 1, z + 1), Vector3.forward, Vector2.up, texOffset, lighting.LeftTop);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void AddNegativeX(AbstractMesh mesh, int x, int y, int z, int texOffset, in VertexLightingData lighting)
        {
            mesh.AddVertex(new Vector3(x, y, z + 1), Vector3.left, Vector2.zero, texOffset, lighting.LeftBottom);
            mesh.AddVertex(new Vector3(x, y, z), Vector3.left, Vector2.right, texOffset, lighting.RightBottom);
            mesh.AddVertex(new Vector3(x, y + 1, z), Vector3.left, Vector2.one, texOffset, lighting.RightTop);
            mesh.AddVertex(new Vector3(x, y + 1, z + 1), Vector3.left, Vector2.up, texOffset, lighting.LeftTop);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void AddNegativeY(AbstractMesh mesh, int x, int y, int z, int texOffset, in VertexLightingData lighting)
        {
            mesh.AddVertex(new Vector3(x + 1, y, z), Vector3.down, Vector2.zero, texOffset, lighting.LeftBottom);
            mesh.AddVertex(new Vector3(x, y, z), Vector3.down, Vector2.right, texOffset, lighting.RightBottom);
            mesh.AddVertex(new Vector3(x, y, z + 1), Vector3.down, Vector2.one, texOffset, lighting.RightTop);
            mesh.AddVertex(new Vector3(x + 1, y, z + 1), Vector3.down, Vector2.up, texOffset, lighting.LeftTop);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void AddNegativeZ(AbstractMesh mesh, int x, int y, int z, int texOffset, in VertexLightingData lighting)
        {
            mesh.AddVertex(new Vector3(x, y, z), Vector3.back, Vector2.zero, texOffset, lighting.LeftBottom);
            mesh.AddVertex(new Vector3(x + 1, y, z), Vector3.back, Vector2.right, texOffset, lighting.RightBottom);
            mesh.AddVertex(new Vector3(x + 1, y + 1, z), Vector3.back, Vector2.one, texOffset, lighting.RightTop);
            mesh.AddVertex(new Vector3(x, y + 1, z), Vector3.back, Vector2.up, texOffset, lighting.LeftTop);
        }
    }
}

using Minecraft.XPhysics;
using System;
using UnityEngine;

namespace Minecraft.Rendering
{
    [CreateAssetMenu(menuName = "Minecraft/Block Mesh Writer/Empty", fileName = "Empty")]
    public class EmptyMeshWriter : BlockMeshWriter
    {
        public override string[] RequiredTextureNames => Array.Empty<string>();

        public override AABB MeshBoundingBox => new AABB(Vector3.zero, Vector3.zero);

        public override bool IsEmpty => true;

        public override void Write(
            AbstractMesh mesh, int x, int y, int z, Block block, BlockTextureTable texTable, LightEvaluator eval
        )
        {
            Debug.LogWarning("EmptyMeshWriter will write nothing into the mesh!");
        }
    }
}

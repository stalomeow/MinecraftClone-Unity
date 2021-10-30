using System;
using Minecraft.Lua;
using Minecraft.PhysicSystem;
using UnityEngine;

namespace Minecraft.Configurations
{
    [CreateAssetMenu(menuName = "Minecraft/Configurations/BlockMesh")]
    public class BlockMesh : ScriptableObject, ILuaCallCSharp
    {
        [Serializable]
        public struct FaceData
        {
            public BlockFace Face;
            public bool NeverClip;
            public BlockVertexData[] Vertices;
            public int[] Indices;
        }

        public Vector3 Pivot;
        public AABB BoundingBox;
        public FaceData[] Faces;
    }
}

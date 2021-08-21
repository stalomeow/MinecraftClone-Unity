using System;
using Minecraft.Lua;
using UnityEngine;

namespace Minecraft.Configurations
{
    [Serializable]
    [XLua.GCOptimize]
    public struct BlockVertexData : ILuaCallCSharp
    {
        public Vector3 Position;
        public Vector2 UV;
        public BlockFaceCorner CornerInFace;
    }
}

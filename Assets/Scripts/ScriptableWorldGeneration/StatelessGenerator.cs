using Minecraft.Configurations;
using Minecraft.Lua;
using UnityEngine;

namespace Minecraft.ScriptableWorldGeneration
{
    public abstract class StatelessGenerator : ScriptableObject, ILuaCallCSharp
    {
        public abstract void Generate(IWorld world, ChunkPos pos, BlockData[,,] blocks, byte[,] heightMap, GenerationHelper helper, GenerationContext context);
    }
}

using Minecraft.Configurations;
using Minecraft.Lua;
using UnityEngine;
using XLua;

namespace Minecraft
{
    public interface IWorldRAccessor : ILuaCallCSharp
    {
        bool Accessible { get; }

        Vector3Int WorldSpaceOrigin { get; }

        IWorld World { get; }

        BlockData GetBlock(int x, int y, int z, BlockData defaultValue = null);

        int GetMixedLightLevel(int x, int y, int z, int defaultValue = 0);

        int GetSkyLight(int x, int y, int z, int defaultValue = 0);

        int GetAmbientLight(int x, int y, int z, int defaultValue = 0);

        int GetTopVisibleBlockY(int x, int z, int defaultValue = 0);
    }
}

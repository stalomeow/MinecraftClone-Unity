using Minecraft.Configurations;
using Minecraft.Lua;
using UnityEngine;

namespace Minecraft
{
    public interface IWorldRAccessor : ILuaCallCSharp
    {
        bool Accessible { get; }

        Vector3Int WorldSpaceOrigin { get; }

        IWorld World { get; }

        BlockData GetBlock(int x, int y, int z, BlockData defaultValue = null);

        Quaternion GetBlockRotation(int x, int y, int z, Quaternion defaultValue = default);

        int GetMixedLightLevel(int x, int y, int z, int defaultValue = 0);

        int GetSkyLight(int x, int y, int z, int defaultValue = 0);

        int GetAmbientLight(int x, int y, int z, int defaultValue = 0);

        int GetTopVisibleBlockY(int x, int z, int defaultValue = 0);
    }
}

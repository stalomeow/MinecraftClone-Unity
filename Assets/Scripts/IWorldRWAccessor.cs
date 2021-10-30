using Minecraft.Configurations;
using UnityEngine;

namespace Minecraft
{
    public interface IWorldRWAccessor : IWorldRAccessor
    {
        bool SetBlock(int x, int y, int z, BlockData value, Quaternion rotation, ModificationSource source);

        bool SetAmbientLightLevel(int x, int y, int z, int value, ModificationSource source);
    }
}

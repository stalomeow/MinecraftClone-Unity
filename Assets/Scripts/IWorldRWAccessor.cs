using Minecraft.Configurations;

namespace Minecraft
{
    public interface IWorldRWAccessor : IWorldRAccessor
    {
        bool SetBlock(int x, int y, int z, BlockData value, ModificationSource source);

        bool SetAmbientLightLevel(int x, int y, int z, int value, ModificationSource source);
    }
}

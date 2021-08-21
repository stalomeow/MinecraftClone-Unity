using System;

namespace Minecraft.Configurations
{
    [Flags]
    [XLua.GCOptimize]
    [XLua.LuaCallCSharp]
    public enum BlockFlags
    {
        None = 0,
        IgnoreCollisions = 1 << 0,
        IgnorePlaceBlockRaycast = 1 << 1,
        IgnoreDestroyBlockRaycast = 1 << 2,
        Reserved = 1 << 3,
        IgnoreExplosions = 1 << 4,
        AlwaysInvisible = 1 << 5
    }
}

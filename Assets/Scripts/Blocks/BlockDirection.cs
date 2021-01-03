using XLua;

namespace Minecraft
{
    [LuaCallCSharp]
    public enum BlockDirection
    {
        /// <summary>
        /// +X
        /// </summary>
        PositiveX = 0,
        /// <summary>
        /// +Y
        /// </summary>
        PositiveY = 1,
        /// <summary>
        /// +Z
        /// </summary>
        PositiveZ = 2,
        /// <summary>
        /// -X
        /// </summary>
        NegativeX = 3,
        /// <summary>
        /// -Y
        /// </summary>
        NegativeY = 4,
        /// <summary>
        /// -Z
        /// </summary>
        NegativeZ = 5
    }
}
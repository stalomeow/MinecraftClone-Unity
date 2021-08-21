using XLua;

namespace Minecraft.Configurations
{
    /// <summary>
    /// 表示方块的一个面
    /// </summary>
    [GCOptimize]
    [LuaCallCSharp]
    public enum BlockFace
    {
        /// <summary>
        /// 法线方向为 (1, 0, 0) 的面
        /// </summary>
        PositiveX = 0,
        /// <summary>
        /// 法线方向为 (0, 1, 0) 的面
        /// </summary>
        PositiveY = 1,
        /// <summary>
        /// 法线方向为 (0, 0, 1) 的面
        /// </summary>
        PositiveZ = 2,
        /// <summary>
        /// 法线方向为 (-1, 0, 0) 的面
        /// </summary>
        NegativeX = 3,
        /// <summary>
        /// 法线方向为 (0, -1, 0) 的面
        /// </summary>
        NegativeY = 4,
        /// <summary>
        /// 法线方向为 (0, 0, -1) 的面
        /// </summary>
        NegativeZ = 5
    }
}
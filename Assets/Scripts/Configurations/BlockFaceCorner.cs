namespace Minecraft.Configurations
{
    /// <summary>
    /// 表示方块的一个面上的一个角落
    /// </summary>
    [XLua.GCOptimize]
    [XLua.LuaCallCSharp]
    public enum BlockFaceCorner
    {
        /// <summary>
        /// 左下角
        /// </summary>
        LeftBottom = 0,
        /// <summary>
        /// 右下角
        /// </summary>
        RightBottom = 1,
        /// <summary>
        /// 左上角
        /// </summary>
        LeftTop = 2,
        /// <summary>
        /// 右上角
        /// </summary>
        RightTop = 3
    }
}

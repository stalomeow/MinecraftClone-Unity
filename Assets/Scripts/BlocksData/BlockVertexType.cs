using XLua;

namespace Minecraft.BlocksData
{
    [LuaCallCSharp]
    public enum BlockVertexType
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,
        /// <summary>
        /// 方块
        /// </summary>
        Cube = 1,
        /// <summary>
        /// 两个互相垂直的面片
        /// </summary>
        PerpendicularQuads = 2
    }
}
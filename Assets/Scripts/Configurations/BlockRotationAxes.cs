using System;
using XLua;

namespace Minecraft.Configurations
{
    [Flags]
    [GCOptimize]
    [LuaCallCSharp]
    public enum BlockRotationAxes
    {
        /// <summary>
        /// 不旋转
        /// </summary>
        None = 0,
        /// <summary>
        /// 绕着 Y 轴旋转
        /// </summary>
        AroundYAxis = 1 << 0,
        /// <summary>
        /// 绕着 X 轴或 Z 轴旋转
        /// </summary>
        AroundXOrZAxis = 1 << 1
    }
}

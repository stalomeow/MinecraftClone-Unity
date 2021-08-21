namespace Minecraft.PhysicSystem
{
    [XLua.GCOptimize]
    [XLua.LuaCallCSharp]
    public enum PhysicState
    {
        /// <summary>
        /// 固体
        /// </summary>
        Solid,
        /// <summary>
        /// 流体
        /// </summary>
        Fluid
    }
}

namespace Minecraft.Configurations
{
    [XLua.GCOptimize]
    [XLua.LuaCallCSharp]
    public enum BlockEntityConversion
    {
        Never = 0,
        Initial = 1,
        Conditional = 2
    }
}

namespace Minecraft
{
    [XLua.GCOptimize]
    [XLua.LuaCallCSharp]
    public enum ModificationSource
    {
        InternalOrSystem = 0,

        PlayerAction = 1
    }
}

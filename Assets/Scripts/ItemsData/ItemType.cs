using XLua;

namespace Minecraft.ItemsData
{
    [LuaCallCSharp]
    public enum ItemType : byte
    {
        None = 0,
        Glass = 1,
        Log_Oak = 2,
        TNT = 3,
        GlowStone = 4,
        BookShelf = 5,
        CobbleStone = 6,
        Planks_Oak = 7,
        CraftingTable = 8,
        DiamondBlock = 9,
        Noteblock = 10
    }
}
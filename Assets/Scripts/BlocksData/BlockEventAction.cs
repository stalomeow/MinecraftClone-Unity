using XLua;

namespace Minecraft.BlocksData
{
    [CSharpCallLua]
    public delegate void BlockEventAction(int x, int y, int z, Block block);
}
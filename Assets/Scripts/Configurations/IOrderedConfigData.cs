using Minecraft.Lua;

namespace Minecraft.Configurations
{
    public interface IOrderedConfigData : ILuaCallCSharp
    {
        int ID { get; set; }

        string InternalName { get; set; }
    }
}

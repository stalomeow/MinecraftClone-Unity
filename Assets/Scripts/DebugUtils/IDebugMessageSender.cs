namespace Minecraft.DebugUtils
{
    public interface IDebugMessageSender
    {
        string DisplayName { get; }

        bool DisableLog { get; set; }
    }
}
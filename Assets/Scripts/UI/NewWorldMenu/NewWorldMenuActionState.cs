using ToaruUnity.UI;

namespace Minecraft.UI
{
    internal sealed class NewWorldMenuActionState : IActionState
    {
        public ValueObserved<string[]> ResPackNames = new ValueObserved<string[]>();
        public ValueObserved<string> ErrorText = new ValueObserved<string>(true);
    }
}
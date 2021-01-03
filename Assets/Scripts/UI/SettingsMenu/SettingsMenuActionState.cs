using ToaruUnity.UI;

namespace Minecraft.UI
{
    internal sealed class SettingsMenuActionState : IActionState
    {
        public ValueObserved<int> RenderRadius = new ValueObserved<int>(true);
        public ValueObserved<float> HorizontalFOV = new ValueObserved<float>(true);
        public ValueObserved<int> MaxChunkCountInMemory = new ValueObserved<int>(true);
        public ValueObserved<int> MaxTaskCountPerFrame = new ValueObserved<int>(true);
        public ValueObserved<bool> EnableDestroyEffect = new ValueObserved<bool>(changed: true);
    }
}
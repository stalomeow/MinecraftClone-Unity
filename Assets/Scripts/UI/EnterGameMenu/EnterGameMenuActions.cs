using ToaruUnity.UI;
using UnityEngine.AddressableAssets;

namespace Minecraft.UI
{
    internal sealed class EnterGameMenuActions : ActionCenter
    {
        [Action]
        public bool OpenWithSettings(AssetReference key)
        {
            Manager.SwitchView(key, SwitchViewMode.NavigateOrOpenNew, null, new SwitchViewParameters
            {
                OpenOrNavigateViewParam = GlobalSettings.Instance
            });
            return false;
        }

        [Action]
        public bool Open(AssetReference key)
        {
            Manager.SwitchView(key, SwitchViewMode.NavigateOrOpenNew, null, default);
            return false;
        }

        [Action]
        public bool Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
            return false;
        }
    }
}
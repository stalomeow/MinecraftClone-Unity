using ToaruUnity.UI;

namespace Minecraft.UI
{
    internal sealed class SettingsMenuActions : ActionCenter
    {
        protected override IActionState CreateState()
        {
            return new SettingsMenuActionState();
        }

        protected override void ResetState(ref IActionState state)
        {
            SettingsMenuActionState s = state as SettingsMenuActionState;

            s.RenderRadius.Value = default;
            s.HorizontalFOV.Value = default;
            s.MaxChunkCountInMemory.Value = default;
            s.MaxTaskCountPerFrame.Value = default;
            s.EnableDestroyEffect.Value = default;
        }


        [Action]
        public bool SetRenderRadius(int value)
        {
            GetState<SettingsMenuActionState>().RenderRadius.Value = value;
            return true;
        }

        [Action]
        public bool SetHorizontalFOV(float value)
        {
            GetState<SettingsMenuActionState>().HorizontalFOV.Value = value;
            return true;
        }

        [Action]
        public bool SetMaxChunkCountInMemory(int value)
        {
            GetState<SettingsMenuActionState>().MaxChunkCountInMemory.Value = value;
            return true;
        }

        [Action]
        public bool SetMaxTaskCountPerFrame(int value)
        {
            GetState<SettingsMenuActionState>().MaxTaskCountPerFrame.Value = value;
            return true;
        }

        [Action]
        public bool SetEnableDestroyEffect(bool value)
        {
            GetState<SettingsMenuActionState>().EnableDestroyEffect.Value = value;
            return true;
        }

        [Action]
        public bool Init()
        {
            SettingsMenuActionState state = GetState<SettingsMenuActionState>();

            state.RenderRadius.Value = GlobalSettings.Instance.RenderChunkRadius;
            state.HorizontalFOV.Value = GlobalSettings.Instance.HorizontalFOVInDEG;
            state.MaxChunkCountInMemory.Value = GlobalSettings.Instance.MaxChunkCountInMemory;
            state.MaxTaskCountPerFrame.Value = GlobalSettings.Instance.MaxTaskCountPerFrame;
            state.EnableDestroyEffect.Value = GlobalSettings.Instance.EnableDestroyEffect;

            return true;
        }

        [Action]
        public bool Close()
        {
            SettingsMenuActionState state = GetState<SettingsMenuActionState>();

            GlobalSettings.Instance.RenderChunkRadius = state.RenderRadius;
            GlobalSettings.Instance.HorizontalFOVInDEG = state.HorizontalFOV;
            GlobalSettings.Instance.MaxChunkCountInMemory = state.MaxChunkCountInMemory;
            GlobalSettings.Instance.MaxTaskCountPerFrame = state.MaxTaskCountPerFrame;
            GlobalSettings.Instance.EnableDestroyEffect = state.EnableDestroyEffect;

            GlobalSettings.SaveSettings();
            Manager.CloseActiveView();
            return false;
        }
    }
}
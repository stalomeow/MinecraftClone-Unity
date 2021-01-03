using ToaruUnity.UI;
using UnityEngine;

namespace Minecraft.UI
{
    internal sealed class LoadingMenuActions : ActionCenter
    {
        protected override IActionState CreateState()
        {
            return new LoadingMenuActionState();
        }

        protected override void ResetState(ref IActionState state)
        {
            LoadingMenuActionState s = state as LoadingMenuActionState;
            s.AccumulatedTime = 0;
            s.TipIndex.Value = 0;
            s.ProgressBarFillAmount.Value = 0;
        }

        [Action]
        public bool Update(float deltaTime, float interval, AsyncOperation operation)
        {
            LoadingMenuActionState state = GetState<LoadingMenuActionState>();

            if (state.ProgressBarFillAmount == 1)
            {
                Manager.CloseActiveView();
                return false;
            }

            state.AccumulatedTime += deltaTime;

            if (state.AccumulatedTime >= interval)
            {
                state.AccumulatedTime = 0;
                state.TipIndex.Value++;
            }

            if (operation != null)
            {
                float value = Mathf.Lerp(state.ProgressBarFillAmount, operation.progress + 0.1f, deltaTime);

                if (value >= 0.99f)
                {
                    value = 1;
                }

                state.ProgressBarFillAmount.Value = value;
            }

            return true;
        }
    }
}
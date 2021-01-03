namespace ToaruUnity.UI
{
    internal sealed class LoadingMenuActionState : IActionState
    {
        public float AccumulatedTime;
        public ValueObserved<int> TipIndex = new ValueObserved<int>(true);
        public ValueObserved<float> ProgressBarFillAmount = new ValueObserved<float>(true);
    }
}
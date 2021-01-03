namespace ToaruUnity.UI
{
    public ref struct SwitchViewParameters
    {
        public object OpenViewParam { get; set; }

        public object NavigateViewParam { get; set; }

        public object SuspendViewParam { get; set; }

        public object OpenOrNavigateViewParam
        {
            set
            {
                OpenViewParam = value;
                NavigateViewParam = value;
            }
        }
    }
}
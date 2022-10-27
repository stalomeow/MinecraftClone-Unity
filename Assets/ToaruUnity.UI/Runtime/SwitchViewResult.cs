namespace ToaruUnity.UI
{
    /// <summary>
    /// 切换页面的结果
    /// </summary>
    public enum SwitchViewResult
    {
        /// <summary>
        /// 成功导航到指定页面
        /// </summary>
        Navigated,
        /// <summary>
        /// 成功打开新页面
        /// </summary>
        NewViewOpened,
        /// <summary>
        /// 失败，因为Key是null
        /// </summary>
        Failed_BecauseKeyIsNull,
        /// <summary>
        /// 失败，因为Mode是<see cref="SwitchViewMode.None"/>
        /// </summary>
        Failed_BecauseModeIsNone,
        /// <summary>
        /// 失败，因为要导航的页面已经在顶部
        /// </summary>
        Failed_BecauseNavigationIsUnnecessary
    }
}

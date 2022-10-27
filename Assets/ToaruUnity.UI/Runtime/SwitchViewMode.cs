using System;

namespace ToaruUnity.UI
{
    /// <summary>
    /// 表示页面的切换模式
    /// </summary>
    [Flags]
    public enum SwitchViewMode
    {
        /// <summary>
        /// 不切换
        /// </summary>
        None = 0,
        /// <summary>
        /// 打开一个新的页面
        /// </summary>
        OpenNew = 1 << 0,
        /// <summary>
        /// 导航至已经打开的页面
        /// </summary>
        Navigate = 1 << 1,
        /// <summary>
        /// 先尝试导航至已经打开的页面，如果失败，就打开一个新的页面
        /// </summary>
        NavigateOrOpenNew = Navigate | OpenNew
    }
}
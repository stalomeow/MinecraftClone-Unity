namespace ToaruUnity.UI
{
    /// <summary>
    /// 切换页面的回调
    /// </summary>
    /// <param name="result">切换页面的结果</param>
    /// <param name="switchedViewKey">切换到的页面的Key</param>
    /// <param name="switchedView">切换到的页面对象</param>
    public delegate void SwitchViewCallbackHandler(SwitchViewResult result, object switchedViewKey, AbstractView switchedView);
}
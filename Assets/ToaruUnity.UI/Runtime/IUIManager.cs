using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ToaruUnity.UI
{
    /// <summary>
    /// UI管理接口
    /// </summary>
    public interface IUIManager
    {
        /// <summary>
        /// 获取打开的页面数量
        /// </summary>
        int ViewCount { get; }

        /// <summary>
        /// 获取页面的容器
        /// </summary>
        Transform ViewContainer { get; }

        /// <summary>
        /// 获取页面Key的比较器
        /// </summary>
        IEqualityComparer<object> ViewKeyComparer { get; }

        /// <summary>
        /// 打开页面事件
        /// </summary>
        event UnityAction<object, AbstractView> OnViewOpened;

        /// <summary>
        /// 导航到页面事件
        /// </summary>
        event UnityAction<object, AbstractView> OnViewNavigated;

        /// <summary>
        /// 关闭页面事件
        /// </summary>
        event UnityAction<object> OnViewClosed;

        /// <summary>
        /// 顶部页面变化事件
        /// </summary>
        event UnityAction OnActiveViewChanged;

        /// <summary>
        /// 获取顶部的页面
        /// </summary>
        AbstractView ActiveView { get; }


        /// <summary>
        /// 打开一个新的页面
        /// </summary>
        /// <param name="viewKey">页面的Key</param>
        void OpenNewView(object viewKey);

        /// <summary>
        /// 导航到指定页面
        /// </summary>
        /// <param name="viewKey">页面的Key</param>
        void NavigateToView(object viewKey);

        /// <summary>
        /// 切换页面
        /// </summary>
        /// <param name="viewKey">页面的Key</param>
        /// <param name="mode">切换模式</param>
        void SwitchView(object viewKey, SwitchViewMode mode);

        /// <summary>
        /// 切换页面
        /// </summary>
        /// <param name="viewKey">页面的Key</param>
        /// <param name="mode">切换模式</param>
        /// <param name="parameters">切换页面的参数</param>
        void SwitchView(object viewKey, SwitchViewMode mode, SwitchViewParameters parameters);

        /// <summary>
        /// 切换页面
        /// </summary>
        /// <param name="viewKey">页面的Key</param>
        /// <param name="mode">切换模式</param>
        /// <param name="callback">切换完成的回调</param>
        void SwitchView(object viewKey, SwitchViewMode mode, SwitchViewCallbackHandler callback);

        /// <summary>
        /// 切换页面
        /// </summary>
        /// <param name="viewKey">页面的Key</param>
        /// <param name="mode">切换模式</param>
        /// <param name="callback">切换完成的回调</param>
        /// <param name="parameters">切换页面的参数</param>
        void SwitchView(object viewKey, SwitchViewMode mode, SwitchViewCallbackHandler callback, SwitchViewParameters parameters);


        /// <summary>
        /// 关闭顶部页面
        /// </summary>
        /// <returns>如果顶部页面存在并且关闭成功，返回true；否则返回false</returns>
        bool CloseActiveView();

        /// <summary>
        /// 关闭顶部页面
        /// </summary>
        /// <param name="removedViewKey">被关闭页面的Key</param>
        /// <returns>如果顶部页面存在并且关闭成功，返回true；否则返回false</returns>
        bool CloseActiveView(out object removedViewKey);

        /// <summary>
        /// 关闭顶部页面
        /// </summary>
        /// <param name="parameters">关闭页面的参数</param>
        /// <returns>如果顶部页面存在并且关闭成功，返回true；否则返回false</returns>
        bool CloseActiveView(CloseViewParameters parameters);

        /// <summary>
        /// 关闭顶部页面
        /// </summary>
        /// <param name="removedViewKey">被关闭页面的Key</param>
        /// <param name="parameters">关闭页面的参数</param>
        /// <returns>如果顶部页面存在并且关闭成功，返回true；否则返回false</returns>
        bool CloseActiveView(out object removedViewKey, CloseViewParameters parameters);
    }
}
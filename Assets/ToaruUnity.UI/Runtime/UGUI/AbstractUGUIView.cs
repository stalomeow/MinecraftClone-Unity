using System.Collections;
using UnityEngine;

#pragma warning disable IDE0032

namespace ToaruUnity.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class AbstractUGUIView : AbstractView
    {
        private CanvasGroup m_CanvasGroup;

        /// <summary>
        /// 获取当前对象的<see cref="UnityEngine.CanvasGroup"/>组件
        /// </summary>
        protected CanvasGroup CanvasGroup => m_CanvasGroup ?? (m_CanvasGroup = GetComponent<CanvasGroup>());


        protected AbstractUGUIView() { }


        protected override IEnumerator OnOpen(object data)
        {
            CanvasGroup.alpha = 1;
            CanvasGroup.blocksRaycasts = true;

            return null;
        }

        protected override IEnumerator OnClose(object data)
        {
            CanvasGroup.alpha = 0;
            CanvasGroup.blocksRaycasts = false;

            return null;
        }

        protected override IEnumerator OnResume(object data)
        {
            CanvasGroup.blocksRaycasts = true;

            return null;
        }

        protected override IEnumerator OnSuspend(object data)
        {
            CanvasGroup.blocksRaycasts = false;

            return null;
        }
    }
}
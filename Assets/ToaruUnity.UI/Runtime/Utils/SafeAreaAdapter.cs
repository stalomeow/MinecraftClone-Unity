using System;
using UnityEngine;
using UnityEngine.Events;

namespace ToaruUnity.UI
{
    /// <summary>
    /// 安全区域适配器
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaAdapter : MonoBehaviour
    {
        private RectTransform m_Transform;
        private Rect m_CurrentArea;

        [SerializeField] private bool m_AdaptOnStart;
        [Space]
        [SerializeField] private SafeAreaChangedEvent m_OnSafeAreaChanged;


        /// <summary>
        /// 安全区域变化事件
        /// </summary>
        public event UnityAction<Rect> OnSafeAreaChanged
        {
            add => m_OnSafeAreaChanged.AddListener(value);
            remove => m_OnSafeAreaChanged.RemoveListener(value);
        }


        protected virtual void Start()
        {
            m_Transform = GetComponent<RectTransform>();
            m_CurrentArea = Rect.zero;

            if (m_AdaptOnStart)
            {
                SetSafeArea();
            }
        }


        public virtual void SetSafeArea()
        {
            SetSafeArea(Screen.safeArea);
        }

        public void SetSafeArea(Rect safeArea)
        {
            if (m_CurrentArea == safeArea)
                return;

            m_Transform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, safeArea.x, safeArea.width);
            m_Transform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, safeArea.y, safeArea.height);

            m_CurrentArea = safeArea;
            m_OnSafeAreaChanged.Invoke(safeArea);
        }


        [Serializable]
        private sealed class SafeAreaChangedEvent : UnityEvent<Rect> { }
    }
}
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

#pragma warning disable IDE0032
#pragma warning disable CS0649

namespace ToaruUnity.UI
{
    /// <summary>
    /// UI管理类
    /// </summary>
    [DisallowMultipleComponent]
    public class UIManager : MonoBehaviour, IUIManager
    {
        [Header("Settings")]

        [SerializeField, Range(1, 20)] private int m_StackMinGrow = 5;
        [SerializeField, Range(1, 40)] private int m_StackMaxGrow = 10;
        [SerializeField] private Transform m_ViewContainer;
        [SerializeField] private ViewLoader m_ViewLoader;

        [Header("Events")]

        [SerializeField] private ViewEvent m_OnViewOpened;
        [SerializeField] private ViewEvent m_OnViewNavigated;
        [SerializeField] private ViewKeyEvent m_OnViewClosed;
        [SerializeField] private UnityEvent m_OnActiveViewChanged;

        private IEqualityComparer<object> m_KeyComparer;
        private ViewStack m_Stack;
        private HybridDictionary<object, ViewPrefab> m_Prefabs;
        private HybridDictionary<object, AbstractView> m_ViewPool; // 每一种View只缓存一份


        /// <summary>
        /// 获取打开的页面数量
        /// </summary>
        public int ViewCount => m_Stack.Count;

        /// <summary>
        /// 获取页面的容器
        /// </summary>
        public Transform ViewContainer => m_ViewContainer;

        /// <summary>
        /// 获取页面Key的比较器
        /// </summary>
        public IEqualityComparer<object> ViewKeyComparer => m_KeyComparer;

        /// <summary>
        /// 打开页面事件
        /// </summary>
        public event UnityAction<object, AbstractView> OnViewOpened
        {
            add => m_OnViewOpened.AddListener(value);
            remove => m_OnViewOpened.RemoveListener(value);
        }

        /// <summary>
        /// 导航到页面事件
        /// </summary>
        public event UnityAction<object, AbstractView> OnViewNavigated
        {
            add => m_OnViewNavigated.AddListener(value);
            remove => m_OnViewNavigated.RemoveListener(value);
        }

        /// <summary>
        /// 关闭页面事件
        /// </summary>
        public event UnityAction<object> OnViewClosed
        {
            add => m_OnViewClosed.AddListener(value);
            remove => m_OnViewClosed.RemoveListener(value);
        }

        /// <summary>
        /// 顶部页面变化事件
        /// </summary>
        public event UnityAction OnActiveViewChanged
        {
            add => m_OnActiveViewChanged.AddListener(value);
            remove => m_OnActiveViewChanged.RemoveListener(value);
        }

        /// <summary>
        /// 获取顶部的页面
        /// </summary>
        public AbstractView ActiveView => m_Stack[0];


        public UIManager() { }


        private void Awake()
        {
            m_KeyComparer = CreateViewKeyComparer() ?? throw new NullReferenceException("ViewKeyComparer");
            m_Stack = new ViewStack(m_StackMinGrow, m_StackMaxGrow, m_KeyComparer);
            m_Prefabs = new HybridDictionary<object, ViewPrefab>(m_KeyComparer);
            m_ViewPool = new HybridDictionary<object, AbstractView>(m_KeyComparer);

            if (!m_ViewContainer)
            {
                Debug.LogError("ViewContainer is missing!");
            }

            if (!m_ViewLoader)
            {
                Debug.LogError("ViewLoader is missing!");
            }
        }

        private void OnDestroy()
        {
            foreach (KeyValuePair<object, ViewPrefab> pair in m_Prefabs)
            {
                m_ViewLoader.ReleaseViewPrefab(pair.Key, pair.Value.RawPrefab);
            }
        }

        protected virtual IEqualityComparer<object> CreateViewKeyComparer()
        {
            return EqualityComparer<object>.Default;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OpenNewView(object viewKey)
        {
            SwitchView(viewKey, SwitchViewMode.OpenNew, null, default);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void NavigateToView(object viewKey)
        {
            SwitchView(viewKey, SwitchViewMode.Navigate, null, default);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SwitchView(object viewKey, SwitchViewMode mode)
        {
            SwitchView(viewKey, mode, null, default);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SwitchView(object viewKey, SwitchViewMode mode, SwitchViewParameters parameters)
        {
            SwitchView(viewKey, mode, null, parameters);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SwitchView(object viewKey, SwitchViewMode mode, SwitchViewCallbackHandler callback)
        {
            SwitchView(viewKey, mode, callback, default);
        }

        public void SwitchView(object viewKey, SwitchViewMode mode, SwitchViewCallbackHandler callback, SwitchViewParameters parameters)
        {
            if (viewKey == null)
            {
                callback?.Invoke(SwitchViewResult.Failed_BecauseKeyIsNull, null, null);
                return;
            }

            if (mode == SwitchViewMode.None)
            {
                callback?.Invoke(SwitchViewResult.Failed_BecauseModeIsNone, viewKey, null);
                return;
            }

            //先导航
            if ((mode & SwitchViewMode.Navigate) == SwitchViewMode.Navigate)
            {
                if (m_Stack.MatchTopKey(viewKey))
                {
                    // 要导航的页面已经在最顶层
                    callback?.Invoke(SwitchViewResult.Failed_BecauseNavigationIsUnnecessary, viewKey, ActiveView);
                    return;
                }

                if (m_Stack.TryMoveToTop(viewKey, out AbstractView navigatedView, parameters.NavigateViewParam, parameters.SuspendViewParam))
                {
                    m_OnViewNavigated.Invoke(viewKey, navigatedView);
                    m_OnActiveViewChanged.Invoke();
                    callback?.Invoke(SwitchViewResult.Navigated, viewKey, navigatedView);
                    return;
                }
            }
            
            if ((mode & SwitchViewMode.OpenNew) == SwitchViewMode.OpenNew)
            {
                if (m_ViewPool.TryGetAndRemoveValue(viewKey, out AbstractView view))
                {
                    PushViewInstance(viewKey, view, callback, parameters.OpenViewParam, parameters.SuspendViewParam);
                }
                else if (m_Prefabs.TryGetValue(viewKey, out ViewPrefab prefab))
                {
                    PushViewInstance(viewKey, prefab, callback, parameters.OpenViewParam, parameters.SuspendViewParam);
                }
                else
                {
                    PushViewInstance(viewKey, callback, parameters.OpenViewParam, parameters.SuspendViewParam);
                }
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CloseActiveView()
        {
            return CloseActiveView(out _, default);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CloseActiveView(out object removedViewKey)
        {
            return CloseActiveView(out removedViewKey, default);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CloseActiveView(CloseViewParameters parameters)
        {
            return CloseActiveView(out _, parameters);
        }

        public bool CloseActiveView(out object removedViewKey, CloseViewParameters parameters)
        {
            if (m_Stack.TryPop(parameters.CloseViewParam, parameters.ResumeViewParam, out removedViewKey, out _))
            {
                m_OnViewClosed.Invoke(removedViewKey);
                m_OnActiveViewChanged.Invoke();
                return true;
            }

            return false;
        }

        private void PushViewInstance(object viewKey, AbstractView view, SwitchViewCallbackHandler callback, object openViewParam, object suspendViewParam)
        {
            view.Transform.SetParent(ViewContainer);
            m_Stack.Push(viewKey, view, openViewParam, suspendViewParam);

            m_OnViewOpened.Invoke(viewKey, view);
            m_OnActiveViewChanged.Invoke();
            callback?.Invoke(SwitchViewResult.NewViewOpened, viewKey, view);
        }

        private void PushViewInstance(object viewKey, ViewPrefab prefab, SwitchViewCallbackHandler callback, object openViewParam, object suspendViewParam)
        {
            AbstractView view = prefab.Instantiate(ViewContainer);
            view.OnAfterTransition += v => 
            {
                if (v && v.State == ViewState.Closed)
                {
                    if ((!this) || m_ViewPool.ContainsKey(viewKey))
                    {
                        Destroy(v.gameObject);
                    }
                    else
                    {
                        m_ViewPool.Add(viewKey, v);
                    }
                }
            };
            PushViewInstance(viewKey, view, callback, openViewParam, suspendViewParam);
        }

        private void PushViewInstance(object viewKey, SwitchViewCallbackHandler callback, object openViewParam, object suspendViewParam)
        {
            m_ViewLoader.LoadViewPrefab(viewKey, prefab =>
            {
                AddViewPrefab(viewKey, prefab, out ViewPrefab viewPrefab);
                PushViewInstance(viewKey, viewPrefab, callback, openViewParam, suspendViewParam);
            });
        }

        private void AddViewPrefab(object viewKey, AbstractView prefab, out ViewPrefab viewPrefab)
        {
            Type viewType = prefab.GetType();
            ActionCenter center = ActionCenter.New(viewType, this);
            viewPrefab = new ViewPrefab(prefab, center);

            m_Prefabs.Add(viewKey, viewPrefab);
        }

        
        private class ViewPrefab
        {
            private readonly AbstractView m_ViewObj;
            private readonly ActionCenter m_CenterObj;
            private bool m_IsCenterUsed;

            public AbstractView RawPrefab => m_ViewObj;

            public ViewPrefab(AbstractView prefab, ActionCenter center)
            {
                m_ViewObj = prefab;
                m_CenterObj = center;
                m_IsCenterUsed = false;
            }

            public AbstractView Instantiate(Transform container)
            {
                ActionCenter center = AllocateCenter();
                AbstractView view = Object.Instantiate(m_ViewObj, container, false);

                view.Create(center);
                return view;
            }

            private ActionCenter AllocateCenter()
            {
                if (m_IsCenterUsed)
                {
                    return ActionCenter.Clone(m_CenterObj);
                }

                m_IsCenterUsed = true;
                return m_CenterObj;
            }
        }

        // (object viewKey, AbstractView view)
        [Serializable]
        private sealed class ViewEvent : UnityEvent<object, AbstractView> { }

        // (object viewKey)
        [Serializable]
        private sealed class ViewKeyEvent : UnityEvent<object> { }
    }
}
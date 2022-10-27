using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#pragma warning disable CS0649
#pragma warning disable IDE0032
#pragma warning disable IDE1006
#pragma warning disable IDE0044

namespace ToaruUnity.UI
{
    /// <summary>
    /// 所有页面的抽象基类
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class AbstractView : MonoBehaviour
    {
        private ViewState m_State;
        private bool m_IsTransiting;
        private TransitionQueue m_TransitionQueue;
        private Transform m_Transform; // may be null

        private ActionCenter m_ActionCenter; // may be null

        [SerializeField]
        [Tooltip("当界面进行状态过渡前触发")]
        private BeforeTransitionEvent m_OnBeforeTransition;

        [SerializeField]
        [Tooltip("当界面进行状态过渡后触发")]
        private AfterTransitionEvent m_OnAfterTransition;


        /// <summary>
        /// 获取当前对象的状态
        /// </summary>
        public ViewState State => m_State;

        /// <summary>
        /// 获取是否正在进行状态的过渡
        /// </summary>
        public bool IsTransiting => m_IsTransiting;

        /// <summary>
        /// 当界面进行状态过渡前触发
        /// </summary>
        public event UnityAction<AbstractView, ViewState> OnBeforeTransition
        {
            add => m_OnBeforeTransition.AddListener(value);
            remove => m_OnBeforeTransition.RemoveListener(value);
        }

        /// <summary>
        /// 当界面进行状态过渡后触发
        /// </summary>
        public event UnityAction<AbstractView> OnAfterTransition
        {
            add => m_OnAfterTransition.AddListener(value);
            remove => m_OnAfterTransition.RemoveListener(value);
        }


        /// <summary>
        /// 获取剩余的状态过渡任务数量
        /// </summary>
        public int RemainingTransitionCount => m_TransitionQueue.Count;


        /// <summary>
        /// 获取当前对象的<see cref="UnityEngine.Transform"/>组件
        /// </summary>
        public Transform Transform => m_Transform ?? (m_Transform = GetComponent<Transform>());



        /// <summary>
        /// 获取注入的<see cref="ActionCenter"/>对象。
        /// 如果没有指定注入类型或者注入失败，将会返回null。
        /// </summary>
        protected internal ActionCenter Actions => m_ActionCenter;


        protected AbstractView() { }


        internal void Create(ActionCenter actionCenter)
        {
            m_State = ViewState.Closed;
            m_IsTransiting = false;
            m_TransitionQueue = new TransitionQueue();
            m_Transform = null;

            m_ActionCenter = actionCenter;
            m_ActionCenter?.RegisterStateChangeHandler(OnRefreshView);

            OnCreate();
        }

        internal void SetState(ViewState nextState, object param)
        {
            Transition transition = new Transition(nextState, param);

            if (IsTransiting)
            {
                m_TransitionQueue.Enqueue(in transition);
            }
            else
            {
                IEnumerator routine = CreateTransitionRoutine(in transition);

                if (routine == null)
                {
                    // 非协程
                    SetStateAfterTransition(nextState);
                }
                else
                {
                    StartCoroutine(DoTransitions(nextState, routine));
                }
            }
        }

        private IEnumerator DoTransitions(ViewState nextState, IEnumerator routine)
        {
            m_IsTransiting = true;

            do
            {
                yield return routine; // 这里是null的话，就等一帧

                SetStateAfterTransition(nextState);
            }
            while (TryGetNextTransition(out nextState, out routine));

            m_IsTransiting = false;
        }

        private bool TryGetNextTransition(out ViewState nextState, out IEnumerator routine)
        {
            if (m_TransitionQueue.TryDequeue(out Transition transition))
            {
                nextState = transition.NextState;
                routine = CreateTransitionRoutine(in transition);
                return true;
            }

            nextState = default;
            routine = default;
            return false;
        }

        private IEnumerator CreateTransitionRoutine(in Transition transition)
        {
            m_OnBeforeTransition.Invoke(this, transition.NextState);

            switch (transition.NextState)
            {
                case ViewState.Closed when State == ViewState.Active:
                    return OnClose(transition.Param);

                case ViewState.Suspended when State == ViewState.Active:
                    return OnSuspend(transition.Param);

                case ViewState.Active when State == ViewState.Closed:
                    return OnOpen(transition.Param);

                case ViewState.Active when State == ViewState.Suspended:
                    return OnResume(transition.Param);

                default:
                    throw new InvalidOperationException($"无法从状态{State}切换到{transition.NextState}");
            }
        }

        private void SetStateAfterTransition(ViewState value)
        {
            m_State = value;
            m_OnAfterTransition.Invoke(this);
        }



        protected virtual void OnCreate() { }

        protected virtual void OnDestroy() { }

        protected virtual void OnRefreshView(IActionState state) { }

        protected virtual void OnUpdate(float deltaTime) { }

        protected virtual IEnumerator OnOpen(object param) { return null; }

        protected virtual IEnumerator OnClose(object param) { return null; }

        protected virtual IEnumerator OnResume(object param) { return null; }

        protected virtual IEnumerator OnSuspend(object param) { return null; }



        private void Update()
        {
            Actions?.UpdateCoroutines();
            OnUpdate(Time.deltaTime);
        }


        // (AbstractView view, ViewState nextState)
        [Serializable]
        private sealed class BeforeTransitionEvent : UnityEvent<AbstractView, ViewState> { }

        // (AbstractView view)
        [Serializable]
        private sealed class AfterTransitionEvent : UnityEvent<AbstractView> { }

        private readonly struct Transition
        {
            public readonly ViewState NextState;
            public readonly object Param;

            public Transition(ViewState nextState, object param)
            {
                NextState = nextState;
                Param = param;
            }
        }

        private struct TransitionQueue
        {
            private Queue<Transition> m_Queue;

            public int Count => m_Queue == null ? 0 : m_Queue.Count;

            public void Enqueue(in Transition transition)
            {
                if (m_Queue == null)
                {
                    m_Queue = new Queue<Transition>();
                }

                m_Queue.Enqueue(transition);
            }

            public bool TryDequeue(out Transition transition)
            {
                if (Count == 0)
                {
                    transition = default;
                    return false;
                }

                transition = m_Queue.Dequeue();
                return true;
            }
        }
    }
}
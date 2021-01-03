using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS0414
#pragma warning disable CS0649

namespace ToaruUnity.UI
{
    public abstract class TweenUGUIView : AbstractUGUIView
    {
        [Flags]
        internal enum TransitionType
        {
            ActiveToClosed = 1 << 0,
            ActiveToSuspended = 1 << 1,
            ClosedToActive = 1 << 2,
            SuspendedToActive = 1 << 3,
        }

        internal enum TransitionMode
        {
            Position,
            Rotation,
            Scale,
            Color,
            Alpha,
            Animation
        }

        [Serializable]
        internal sealed class Transition
        {
            public bool Enabled;
            public TransitionType Type;
            public TransitionMode Mode;


            public Color FromColor;
            public Color ToColor;
            public Graphic TargetGraphic;


            public Vector2 FromPosition;
            public Vector2 ToPosition;

            public Vector3 FromRotation;
            public Vector3 ToRotation;

            public Vector3 FromScale;
            public Vector3 ToScale;

            public RectTransform TargetTransform;


            [Range(0, 1)] public float FromAlpha;
            [Range(0, 1)] public float ToAlpha;


            public AnimationCurve Curve;
            [Range(0.1f, 120f)] public float Duration = 0.2f;


            public string AnimationName;
            public Animator TargetAnimator;


            public IEnumerator GetTransition(CanvasGroup cg, bool blockRaycasts)
            {
                if (!Enabled)
                    return null;

                switch (Mode)
                {
                    case TransitionMode.Position:
                        return DoPosition(FromPosition, ToPosition, Curve, Duration, TargetTransform, cg, blockRaycasts);

                    case TransitionMode.Rotation:
                        return DoRotation(FromRotation, ToRotation, Curve, Duration, TargetTransform, cg, blockRaycasts);

                    case TransitionMode.Scale:
                        return DoScale(FromScale, ToScale, Curve, Duration, TargetTransform, cg, blockRaycasts);

                    case TransitionMode.Color:
                        return DoColor(FromColor, ToColor, Curve, Duration, TargetGraphic, cg, blockRaycasts);

                    case TransitionMode.Alpha:
                        return DoAlpha(FromAlpha, ToAlpha, Curve, Duration, cg, blockRaycasts);

                    case TransitionMode.Animation:
                        return DoAnimation(AnimationName, TargetAnimator, cg, blockRaycasts);

                    default:
                        return null;
                }
            }

            private IEnumerator DoPosition(Vector2 from, Vector2 to, AnimationCurve curve, float duration, RectTransform transform, CanvasGroup cg, bool blockRaycasts)
            {
                float time = 0;
                cg.blocksRaycasts = false;

                while (time < duration)
                {
                    time += Time.deltaTime;
                    transform.anchoredPosition = Vector2.Lerp(from, to, curve.Evaluate(time / duration));
                    yield return null;
                }

                cg.blocksRaycasts = blockRaycasts;
            }

            private IEnumerator DoRotation(Vector3 from, Vector3 to, AnimationCurve curve, float duration, RectTransform transform, CanvasGroup cg, bool blockRaycasts)
            {
                float time = 0;
                cg.blocksRaycasts = false;

                while (time < duration)
                {
                    time += Time.deltaTime;
                    transform.localRotation = Quaternion.Euler(Vector3.Lerp(from, to, curve.Evaluate(time / duration)));
                    yield return null;
                }

                cg.blocksRaycasts = blockRaycasts;
            }

            private IEnumerator DoScale(Vector3 from, Vector3 to, AnimationCurve curve, float duration, RectTransform transform, CanvasGroup cg, bool blockRaycasts)
            {
                float time = 0;
                cg.blocksRaycasts = false;

                while (time < duration)
                {
                    time += Time.deltaTime;
                    transform.localScale = Vector3.Lerp(from, to, curve.Evaluate(time / duration));
                    yield return null;
                }

                cg.blocksRaycasts = blockRaycasts;
            }

            private IEnumerator DoColor(Color from, Color to, AnimationCurve curve, float duration, Graphic graphic, CanvasGroup cg, bool blockRaycasts)
            {
                float time = 0;
                cg.blocksRaycasts = false;

                while (time < duration)
                {
                    time += Time.deltaTime;
                    graphic.color = Color.Lerp(from, to, curve.Evaluate(time / duration));
                    yield return null;
                }

                cg.blocksRaycasts = blockRaycasts;
            }

            private IEnumerator DoAlpha(float from, float to, AnimationCurve curve, float duration, CanvasGroup cg, bool blockRaycasts)
            {
                float time = 0;
                cg.blocksRaycasts = false;

                while (time < duration)
                {
                    time += Time.deltaTime;
                    cg.alpha = Mathf.Lerp(from, to, curve.Evaluate(time / duration));
                    yield return null;
                }

                cg.blocksRaycasts = blockRaycasts;
            }

            private IEnumerator DoAnimation(string name, Animator animator, CanvasGroup cg, bool blockRaycasts)
            {
                cg.blocksRaycasts = false;

                animator.Play(name);

                while (true)
                {
                    AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
                    yield return null;

                    if (info.normalizedTime >= 1 && info.IsName(name))
                        break;                    
                }

                cg.blocksRaycasts = blockRaycasts;
            }
        }


        [SerializeField] private Transition[] m_Transitions;
        private List<IEnumerator> m_ExecutingTransitionList;


        protected TweenUGUIView() { }


        protected override IEnumerator OnOpen(object data)
        {
            return GetTransition(TransitionType.ClosedToActive, true);
        }

        protected override IEnumerator OnClose(object data)
        {
            return GetTransition(TransitionType.ActiveToClosed, false);
        }

        protected override IEnumerator OnResume(object data)
        {
            return GetTransition(TransitionType.SuspendedToActive, true);
        }

        protected override IEnumerator OnSuspend(object data)
        {
            return GetTransition(TransitionType.ActiveToSuspended, false);
        }

        private IEnumerator GetTransition(TransitionType type, bool blockRaycasts)
        {
            for (int i = 0; i < m_Transitions.Length; i++)
            {
                Transition transition = m_Transitions[i];

                if ((transition.Type & type) == 0)
                    continue;

                IEnumerator transitionCoroutine = transition.GetTransition(CanvasGroup, blockRaycasts);

                if (transitionCoroutine != null)
                {
                    if (m_ExecutingTransitionList == null)
                    {
                        m_ExecutingTransitionList = new List<IEnumerator>();
                    }

                    m_ExecutingTransitionList.Add(transitionCoroutine);
                }
            }

            // 必须立刻开始，将状态设置为初始值

            if (m_ExecutingTransitionList == null)
                yield break;

            bool shouldContinue;

            do
            {
                shouldContinue = false;

                for (int i = m_ExecutingTransitionList.Count - 1; i > -1; i--)
                {
                    IEnumerator transition = m_ExecutingTransitionList[i];

                    if (transition.MoveNext())
                    {
                        shouldContinue = true;
                        //yield return transition.Current;
                    }
                    else
                    {
                        m_ExecutingTransitionList.RemoveAt(i);
                    }
                }

                yield return null;
            }
            while (shouldContinue);

            m_ExecutingTransitionList.Clear();
        }
    }
}
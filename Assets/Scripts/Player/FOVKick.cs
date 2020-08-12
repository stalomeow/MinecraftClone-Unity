using System;
using System.Collections;
using UnityEngine;

#pragma warning disable CS0649

namespace Minecraft
{
    [Serializable]
    public sealed class FOVKick
    {
        [SerializeField]
        [Tooltip("the amount the field of view increases when going into a run")]
        private float m_FOVIncrease = 3f;

        [SerializeField]
        [Tooltip("the amount of time the field of view will increase over")]
        private float m_TimeToIncrease = 1f;

        [SerializeField]
        [Tooltip("the amount of time the field of view will take to return to its original size")]
        private float m_TimeToDecrease = 1f;

        [SerializeField]
        private AnimationCurve m_IncreaseCurve;

        private Camera m_Camera;
        private float m_OriginalFov;
        private readonly WaitForEndOfFrame m_WaitForEndOfFrame = new WaitForEndOfFrame();

        public void Setup(Camera camera)
        {
            m_Camera = camera;
            m_OriginalFov = camera.fieldOfView;
        }

        public IEnumerator FOVKickUp()
        {
            float t = Mathf.Abs((m_Camera.fieldOfView - m_OriginalFov) / m_FOVIncrease);

            while (t < m_TimeToIncrease)
            {
                m_Camera.fieldOfView = m_OriginalFov + (m_IncreaseCurve.Evaluate(t / m_TimeToIncrease) * m_FOVIncrease);
                t += Time.deltaTime;

                yield return m_WaitForEndOfFrame;
            }
        }

        public IEnumerator FOVKickDown()
        {
            float t = Mathf.Abs((m_Camera.fieldOfView - m_OriginalFov) / m_FOVIncrease);

            while (t > 0)
            {
                m_Camera.fieldOfView = m_OriginalFov + (m_IncreaseCurve.Evaluate(t / m_TimeToDecrease) * m_FOVIncrease);
                t -= Time.deltaTime;

                yield return m_WaitForEndOfFrame;
            }

            //make sure that fov returns to the original size
            m_Camera.fieldOfView = m_OriginalFov;
        }
    }
}
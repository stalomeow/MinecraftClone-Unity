using System;
using UnityEngine;

namespace Minecraft.PlayerControls
{
    [Serializable]
    public class CurveControlledBob
    {
        public bool Enabled = true;
        public float VerticalToHorizontalRatio = 2f;
        [SerializeField] private Vector2 m_BobAmplitude = new Vector2(0.1f, 0.1f);
        [SerializeField] private AnimationCurve m_BobCurve = new AnimationCurve(
            new Keyframe(0f, 0f),
            new Keyframe(0.5f, 1f),
            new Keyframe(1f, 0f),
            new Keyframe(1.5f, -1f),
            new Keyframe(2f, 0f)
        ); // sin curve for head bob

        [NonSerialized] private Vector2 m_CyclePosition;
        [NonSerialized] private float m_Time;
        [NonSerialized] private Vector3 m_OriginalCameraPosition;


        public void Initialize(Transform camera)
        {
            m_CyclePosition = Vector2.zero;
            m_Time = m_BobCurve[m_BobCurve.length - 1].time; // get the length of the curve in time
            m_OriginalCameraPosition = camera.localPosition;
        }

        public Vector3 DoHeadBob(float speed, float bobBaseInterval, float deltaTime)
        {
            float xPos = m_OriginalCameraPosition.x + (m_BobCurve.Evaluate(m_CyclePosition.x) * m_BobAmplitude.x);
            float yPos = m_OriginalCameraPosition.y + (m_BobCurve.Evaluate(m_CyclePosition.y) * m_BobAmplitude.y);

            m_CyclePosition.x += speed * deltaTime / bobBaseInterval;
            m_CyclePosition.y += speed * deltaTime / bobBaseInterval * VerticalToHorizontalRatio;

            if (m_CyclePosition.x > m_Time)
            {
                m_CyclePosition.x -= m_Time;
            }

            if (m_CyclePosition.y > m_Time)
            {
                m_CyclePosition.y -= m_Time;
            }

            return new Vector3(xPos, yPos, 0f);
        }
    }
}
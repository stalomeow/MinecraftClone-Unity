using System;
using UnityEngine;

namespace Minecraft
{
    [Serializable]
    public sealed class CurveControlledBob
    {
        [SerializeField]
        private float m_HorizontalBobRange = 0.33f;

        [SerializeField]
        private float m_VerticalBobRange = 0.33f;

        [SerializeField] 
        private AnimationCurve m_Bobcurve = new AnimationCurve(
            new Keyframe(0f, 0f), 
            new Keyframe(0.5f, 1f), 
            new Keyframe(1f, 0f),
            new Keyframe(1.5f, -1f),
            new Keyframe(2f, 0f)
        );// sin curve for head bob

        [SerializeField]
        private float m_VerticaltoHorizontalRatio = 1f;

        private float m_CyclePositionX;
        private float m_CyclePositionY;
        private float m_BobBaseInterval;
        private Vector3 m_OriginalCameraPosition;
        private float m_Time;


        public void Setup(Transform cameraTransform, float bobBaseInterval)
        {
            m_BobBaseInterval = bobBaseInterval;
            m_OriginalCameraPosition = cameraTransform.localPosition;

            // get the length of the curve in time
            m_Time = m_Bobcurve[m_Bobcurve.length - 1].time;
        }

        public Vector3 DoHeadBob(float speed)
        {
            float xPos = m_OriginalCameraPosition.x + (m_Bobcurve.Evaluate(m_CyclePositionX) * m_HorizontalBobRange);
            float yPos = m_OriginalCameraPosition.y + (m_Bobcurve.Evaluate(m_CyclePositionY) * m_VerticalBobRange);

            m_CyclePositionX += speed * Time.deltaTime / m_BobBaseInterval;
            m_CyclePositionY += speed * Time.deltaTime / m_BobBaseInterval * m_VerticaltoHorizontalRatio;

            if (m_CyclePositionX > m_Time)
            {
                m_CyclePositionX -= m_Time;
            }

            if (m_CyclePositionY > m_Time)
            {
                m_CyclePositionY -= m_Time;
            }

            return new Vector3(xPos, yPos, 0f);
        }
    }
}
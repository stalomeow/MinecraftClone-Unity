using System;
using System.Collections;
using UnityEngine;

#pragma warning disable CS0649

namespace Minecraft.PlayerControls
{
    [Serializable]
    public class LerpControlledBob
    {
        [SerializeField] private float m_BobDuration = 0.2f;
        [SerializeField] private float m_BobAmount = 0.1f;

        [NonSerialized] private float m_Offset;
        [NonSerialized] private WaitForFixedUpdate m_WaitForFixedUpdate;

        public void Initialize()
        {
            m_Offset = 0;
            m_WaitForFixedUpdate = new WaitForFixedUpdate();
        }

        public float GetOffset()
        {
            return m_Offset;
        }

        public IEnumerator DoBobCycle()
        {
            // make the camera move down slightly
            float t = 0f;

            while (t < m_BobDuration)
            {
                m_Offset = Mathf.Lerp(0f, m_BobAmount, t / m_BobDuration);
                t += Time.fixedDeltaTime;

                yield return m_WaitForFixedUpdate;
            }

            // make it move back to neutral
            t = 0f;

            while (t < m_BobDuration)
            {
                m_Offset = Mathf.Lerp(m_BobAmount, 0f, t / m_BobDuration);
                t += Time.fixedDeltaTime;

                yield return m_WaitForFixedUpdate;
            }

            m_Offset = 0f;
        }
    }
}
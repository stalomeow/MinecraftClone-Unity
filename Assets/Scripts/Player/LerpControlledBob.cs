using System;
using System.Collections;
using UnityEngine;

#pragma warning disable CS0649

namespace Minecraft
{
    [Serializable]
    public sealed class LerpControlledBob
    {
        [SerializeField] private float m_BobDuration;
        [SerializeField] private float m_BobAmount;

        private readonly WaitForFixedUpdate m_WaitForFixedUpdate = new WaitForFixedUpdate();

        public float Offset { get; private set; }


        public IEnumerator DoBobCycle()
        {
            // make the camera move down slightly
            float t = 0f;

            while (t < m_BobDuration)
            {
                Offset = Mathf.Lerp(0f, m_BobAmount, t / m_BobDuration);
                t += Time.deltaTime;

                yield return m_WaitForFixedUpdate;
            }

            // make it move back to neutral
            t = 0f;

            while (t < m_BobDuration)
            {
                Offset = Mathf.Lerp(m_BobAmount, 0f, t / m_BobDuration);
                t += Time.deltaTime;

                yield return m_WaitForFixedUpdate;
            }

            Offset = 0f;
        }
    }
}
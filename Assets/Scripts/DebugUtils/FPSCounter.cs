using System.Text;
using TMPro;
using UnityEngine;

namespace Minecraft.DebugUtils
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class FPSCounter : MonoBehaviour
    {
        [SerializeField] private float m_FpsMeasurePeriod = 0.5f;

        private int m_FpsAccumulator = 0;
        private float m_FpsNextPeriod = 0;
        private int m_CurrentFps;
        private TextMeshProUGUI m_Text;
        private StringBuilder m_StrBuilder;

        private void Start()
        {
            m_FpsNextPeriod = Time.realtimeSinceStartup + m_FpsMeasurePeriod;
            m_Text = GetComponent<TextMeshProUGUI>();
            m_StrBuilder = new StringBuilder();
        }

        private void Update()
        {
            m_FpsAccumulator++;

            if (Time.realtimeSinceStartup > m_FpsNextPeriod)
            {
                m_CurrentFps = (int)(m_FpsAccumulator / m_FpsMeasurePeriod);
                m_FpsAccumulator = 0;
                m_FpsNextPeriod += m_FpsMeasurePeriod;

                m_StrBuilder.Clear();
                m_StrBuilder.Append(m_CurrentFps).Append(" fps");
                m_Text.text = m_StrBuilder.ToString();
            }
        }
    }
}
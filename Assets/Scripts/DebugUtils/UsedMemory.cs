using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;

namespace Minecraft.DebugUtils
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UsedMemory : MonoBehaviour
    {
        private TextMeshProUGUI m_Text;
        private StringBuilder m_StrBuilder;

        private void Start()
        {
            m_Text = GetComponent<TextMeshProUGUI>();
            m_StrBuilder = new StringBuilder();
        }

        private void LateUpdate()
        {
            m_StrBuilder.Clear();

            long used = Profiler.GetMonoUsedSizeLong() / (1024 * 1024);
            m_StrBuilder.AppendFormat("allocated managed-memory {0}MB", used.ToString());

            m_Text.text = m_StrBuilder.ToString();
        }
    }
}
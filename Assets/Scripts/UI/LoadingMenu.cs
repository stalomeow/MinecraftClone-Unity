using TMPro;
using UnityEngine;

#pragma warning disable CS0649

namespace Minecraft
{
    public sealed class LoadingMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_Text;
        [SerializeField] private string[] m_LoadingTexts;
        private float m_Time = 100;
        private int m_Count = 0;

        private void Update()
        {
            m_Time += Time.deltaTime;

            if (m_Time < 0.5f)
                return;

            m_Time = 0;
            m_Text.text = m_LoadingTexts[m_Count % m_LoadingTexts.Length];
            m_Count++;
        }
    }
}
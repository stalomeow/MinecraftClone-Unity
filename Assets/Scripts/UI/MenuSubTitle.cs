using TMPro;
using UnityEngine;

#pragma warning disable CS0649

namespace Minecraft
{
    public sealed class MenuSubTitle : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_SubTitle;
        [SerializeField] private string[] m_Titles;
        private void Start()
        {
            m_SubTitle.text = m_Titles[Random.Range(0, m_Titles.Length)];
        }

        void Update()
        {
            float t = Time.time;
            float scale = (Mathf.Sin(t * 12f) + 1f) * 0.05f + 1;
            transform.localScale = new Vector3(scale, scale, 1);
        }
    }
}
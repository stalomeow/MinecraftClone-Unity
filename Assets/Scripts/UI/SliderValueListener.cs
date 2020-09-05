using System;
using TMPro;
using UnityEngine;

#pragma warning disable CS0649

namespace Minecraft
{
    [DisallowMultipleComponent]
    public sealed class SliderValueListener : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_ValueText;

        public void OnValueChanged(float value)
        {
            m_ValueText.text = Math.Round(value, 1).ToString();
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS0649

namespace Minecraft
{
    public sealed class OptionsMenu : MonoBehaviour
    {
        [SerializeField] private GameObject m_MainMenu;

        [SerializeField] private Slider m_RenderRadius;
        [SerializeField] private Slider m_HorizontalFOV;
        [SerializeField] private Slider m_MaxChunkCountInMemory;
        [SerializeField] private Slider m_MaxTaskCountPerFrame;
        [SerializeField] private Toggle m_EnableDestroyEffects;

        private void OnEnable()
        {
            m_RenderRadius.value = GlobalSettings.Instance.RenderChunkRadius;
            m_HorizontalFOV.value = GlobalSettings.Instance.HorizontalFOVInDEG;
            m_MaxChunkCountInMemory.value = GlobalSettings.Instance.MaxChunkCountInMemory;
            m_MaxTaskCountPerFrame.value = GlobalSettings.Instance.MaxTaskCountPerFrame;
            m_EnableDestroyEffects.isOn = GlobalSettings.Instance.EnableDestroyEffect;   
        }

        public void OnBackButtonClick()
        {
            GlobalSettings.Instance.RenderChunkRadius = (int)m_RenderRadius.value;
            GlobalSettings.Instance.HorizontalFOVInDEG = m_HorizontalFOV.value;
            GlobalSettings.Instance.MaxChunkCountInMemory = (int)m_MaxChunkCountInMemory.value;
            GlobalSettings.Instance.MaxTaskCountPerFrame = (int)m_MaxTaskCountPerFrame.value;
            GlobalSettings.Instance.EnableDestroyEffect = m_EnableDestroyEffects.isOn;
            GlobalSettings.SaveSettings();

            m_MainMenu.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}

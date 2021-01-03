using System;
using System.Collections;
using TMPro;
using ToaruUnity.UI;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS0649

namespace Minecraft.UI
{
    [InjectActions(typeof(SettingsMenuActions))]
    public sealed class SettingsMenu : TweenUGUIView
    {
        [SerializeField] private Button m_BackButton;

        [SerializeField] private Slider m_RenderRadius;
        [SerializeField] private TextMeshProUGUI m_RenderRadiusText;

        [SerializeField] private Slider m_HorizontalFOV;
        [SerializeField] private TextMeshProUGUI m_HorizontalFOVText;

        [SerializeField] private Slider m_MaxChunkCountInMemory;
        [SerializeField] private TextMeshProUGUI m_MaxChunkCountInMemoryText;

        [SerializeField] private Slider m_MaxTaskCountPerFrame;
        [SerializeField] private TextMeshProUGUI m_MaxTaskCountPerFrameText;

        [SerializeField] private Toggle m_EnableDestroyEffects;
        

        protected override void OnCreate()
        {
            base.OnCreate();

            m_BackButton.onClick.AddListener(() => Actions.Execute("Close"));
            m_RenderRadius.onValueChanged.AddListener(value => Actions.Execute("SetRenderRadius", (int)value));
            m_HorizontalFOV.onValueChanged.AddListener(value => Actions.Execute("SetHorizontalFOV", value));
            m_MaxChunkCountInMemory.onValueChanged.AddListener(value => Actions.Execute("SetMaxChunkCountInMemory", (int)value));
            m_MaxTaskCountPerFrame.onValueChanged.AddListener(value => Actions.Execute("SetMaxTaskCountPerFrame", (int)value));
            m_EnableDestroyEffects.onValueChanged.AddListener(value => Actions.Execute("SetEnableDestroyEffect", value));
        }

        protected override IEnumerator OnOpen(object param)
        {
            Actions.Execute("Init");
            return base.OnOpen(param);
        }

        protected override void OnRefreshView(IActionState s)
        {
            SettingsMenuActionState state = s as SettingsMenuActionState;

            if (state.RenderRadius.ApplyChanges())
            {
                m_RenderRadius.value = state.RenderRadius;
                m_RenderRadiusText.text = state.RenderRadius.ToString();
            }

            if (state.HorizontalFOV.ApplyChanges())
            {
                m_HorizontalFOV.value = state.HorizontalFOV;
                m_HorizontalFOVText.text = Math.Round(state.HorizontalFOV, 1).ToString();
            }

            if (state.MaxChunkCountInMemory.ApplyChanges())
            {
                m_MaxChunkCountInMemory.value = state.MaxChunkCountInMemory;
                m_MaxChunkCountInMemoryText.text = state.MaxChunkCountInMemory.ToString();
            }

            if (state.MaxTaskCountPerFrame.ApplyChanges())
            {
                m_MaxTaskCountPerFrame.value = state.MaxTaskCountPerFrame;
                m_MaxTaskCountPerFrameText.text = state.MaxTaskCountPerFrame.ToString();
            }

            if (state.EnableDestroyEffect.ApplyChanges())
            {
                m_EnableDestroyEffects.isOn = state.EnableDestroyEffect;
            }
        }
    }
}

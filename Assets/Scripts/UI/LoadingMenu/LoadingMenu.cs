using System.Collections;
using TMPro;
using ToaruUnity.UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

#pragma warning disable CS0649

namespace Minecraft.UI
{
    [InjectActions(typeof(LoadingMenuActions))]
    public sealed class LoadingMenu : TweenUGUIView
    {
        [SerializeField]
        private TextMeshProUGUI m_Text;

        [SerializeField]
        private Image m_ProgressBar;

        [SerializeField]
        [FormerlySerializedAs("m_LoadingTexts")]
        private string[] m_Tips;

        [SerializeField]
        [Range(0.5f, 2f)]
        private float m_TipsUpdateInterval = 0.5f;

        private AsyncOperation m_Operation;


        protected override IEnumerator OnOpen(object data)
        {
            m_Operation = data as AsyncOperation;
            return base.OnOpen(data);
        }

        protected override IEnumerator OnResume(object data)
        {
            m_Operation = data as AsyncOperation;
            return base.OnResume(data);
        }

        protected override void OnUpdate(float deltaTime)
        {
            Actions.Execute("Update", deltaTime, m_TipsUpdateInterval, m_Operation);
        }

        protected override void OnRefreshView(IActionState state)
        {
            LoadingMenuActionState s = state as LoadingMenuActionState;

            if (s.TipIndex.ApplyChanges())
            {
                m_Text.text = m_Tips[s.TipIndex % m_Tips.Length];
            }

            if (s.ProgressBarFillAmount.ApplyChanges())
            {
                m_ProgressBar.fillAmount = s.ProgressBarFillAmount;
            }
        }
    }
}
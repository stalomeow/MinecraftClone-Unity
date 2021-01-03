using System.Collections;
using System.Collections.Generic;
using TMPro;
using ToaruUnity.UI;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS0649

namespace Minecraft.UI
{
    [InjectActions(typeof(NewWorldMenuActions))]
    public sealed class NewWorldMenu : TweenUGUIView
    {
        [SerializeField] private TextMeshProUGUI m_ErrorText;
        [SerializeField] private Button m_BackButton;
        [SerializeField] private Button m_GoButton;
        [SerializeField] private TMP_InputField m_NameInput;
        [SerializeField] private TMP_InputField m_SeedInput;
        [SerializeField] private TMP_Dropdown m_WorldTypeDropdown;
        [SerializeField] private TMP_Dropdown m_ResPackDropdown;


        protected override void OnCreate()
        {
            base.OnCreate();

            m_BackButton.onClick.AddListener(() => Actions.Execute("Close"));
            m_GoButton.onClick.AddListener(() => Actions.Execute("Create World",
                m_NameInput.text,
                m_SeedInput.text,
                m_WorldTypeDropdown.captionText.text,
                m_ResPackDropdown.captionText.text
            ));
        }
        
        protected override IEnumerator OnOpen(object param)
        {
            Actions.Execute("Load ResPack");
            return base.OnOpen(param);
        }

        protected override void OnRefreshView(IActionState state)
        {
            NewWorldMenuActionState s = state as NewWorldMenuActionState;

            if (s.ErrorText.ApplyChanges())
            {
                m_ErrorText.text = s.ErrorText;
            }

            if (s.ResPackNames.ApplyChanges())
            {
                string[] names = s.ResPackNames;
                List<TMP_Dropdown.OptionData> options = m_ResPackDropdown.options;
                options.Clear();

                for (int i = 0; i < names.Length; i++)
                {
                    options.Add(new TMP_Dropdown.OptionData(names[i]));
                }

                m_ResPackDropdown.RefreshShownValue();
            }
        }
    } 
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minecraft.UI
{
    public class Navigation : MonoBehaviour
    {
        [SerializeField] private ToggleGroup m_ToggleGroup;

        private void Start()
        {
            foreach (Toggle toggle in m_ToggleGroup.ActiveToggles())
            {
                toggle.onValueChanged.AddListener(value =>
                {
                    if (value)
                    {
                        FindObjectOfType<ToaruUnity.UI.UIManager>().SwitchView("Select World Menu", ToaruUnity.UI.SwitchViewMode.NavigateOrOpenNew);
                    }
                });
            }
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

namespace Minecraft.UI
{
    [RequireComponent(typeof(Animator))]
    public class NavigationToggle : Toggle
    {
        private const string OnAnimationName = "On";
        private const string OffAnimationName = "Off";

        [SerializeField] private Animator m_Animator;

        protected override void Start()
        {
            base.Start();

            m_Animator = GetComponent<Animator>();
            onValueChanged.AddListener(OnValueChanged);

            OnValueChanged(isOn);
        }

        protected override void OnDestroy()
        {
            onValueChanged.RemoveListener(OnValueChanged);

            base.OnDestroy();
        }

        private void OnValueChanged(bool value)
        {
            m_Animator.Play(value ? OnAnimationName : OffAnimationName);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649

namespace Minecraft
{
    [RequireComponent(typeof(AudioSource))]
    public sealed class UIButtonOnClick : MonoBehaviour
    {
        [SerializeField] private AudioClip m_OnClickSound;
        private AudioSource m_AudioSource;

        private void Start()
        {
            m_AudioSource = GetComponent<AudioSource>();
        }

        public void OnClick()
        {
            m_AudioSource.PlayOneShot(m_OnClickSound);
        }
    }
}
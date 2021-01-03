using System.Collections;
using ToaruUnity.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

#pragma warning disable CS0649

namespace Minecraft.UI
{
    //[InjectActions(typeof(EnterGameMenuActions))]
    public sealed class EnterGameMenu : TweenUGUIView, IPointerDownHandler
    {
        [SerializeField] private RawImage m_CGImage;
        [SerializeField] private VideoPlayer m_VideoPlayer;

        private RenderTexture m_CGTempTexture;

        protected override void OnCreate()
        {
            base.OnCreate();

            int width = (int)m_VideoPlayer.width;
            int height = (int)m_VideoPlayer.height;

            m_CGTempTexture = RenderTexture.GetTemporary(width, height);
            m_VideoPlayer.targetTexture = m_CGTempTexture;
            m_CGImage.texture = m_CGTempTexture;
        }

        protected override IEnumerator OnOpen(object data)
        {
            m_VideoPlayer.Play();
            return base.OnOpen(data);
        }

        protected override IEnumerator OnClose(object data)
        {
            m_VideoPlayer.Stop();
            return base.OnClose(data);
        }

        protected override IEnumerator OnResume(object data)
        {
            m_VideoPlayer.Play();
            return base.OnResume(data);
        }

        protected override IEnumerator OnSuspend(object data)
        {
            m_VideoPlayer.Pause();
            return base.OnSuspend(data);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            RenderTexture.ReleaseTemporary(m_CGTempTexture);
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            
        }
    }
}
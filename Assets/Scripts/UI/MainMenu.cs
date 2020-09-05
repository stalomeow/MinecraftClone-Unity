using UnityEngine;

#pragma warning disable CS0649

namespace Minecraft
{
    public sealed class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject m_SelectWorldMenu;
        [SerializeField] private GameObject m_OptionsMenu;

        public void Quit()
        {
            Application.Quit();
        }

        public void Play()
        {
            m_SelectWorldMenu.SetActive(true);
            Destroy(gameObject);
        }

        public void Options()
        {
            m_OptionsMenu.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
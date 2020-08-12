using UnityEngine;

#pragma warning disable CS0649

namespace Minecraft
{
    public sealed class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject m_SelectWorldMenu;

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

        }
    }
}
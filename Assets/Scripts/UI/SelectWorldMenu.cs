using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#pragma warning disable CS0649

namespace Minecraft
{
    public sealed class SelectWorldMenu : MonoBehaviour
    {
        [SerializeField] private GameObject m_NewWorldMenu;
        [SerializeField] private Transform m_Content;
        [SerializeField] private GameObject m_WorldTemplate;

        private void Start()
        {
            string folder = Application.persistentDataPath + "/Worlds";

            if (!Directory.Exists(folder))
            	Directory.CreateDirectory(folder);

            string[] worlds = Directory.GetDirectories(folder);

            for (int i = 0; i < worlds.Length; i++)
            {
                GameObject go = Instantiate(m_WorldTemplate, m_Content, true);
                go.GetComponentInChildren<TextMeshProUGUI>().text = Path.GetFileNameWithoutExtension(worlds[i]);

                byte[] bytes = File.ReadAllBytes(worlds[i] + "/Thumbnail.png");
                Texture2D thumbnail = new Texture2D(1920, 1080);
                thumbnail.LoadImage(bytes);
                go.GetComponentInChildren<RawImage>().texture = thumbnail;

                go.GetComponent<Button>().onClick.AddListener(() =>
                {
                    string name = go.GetComponentInChildren<TextMeshProUGUI>().text;
                    string json = File.ReadAllText(Application.persistentDataPath + "/Worlds/" + name + "/settings.json");
                    WorldSettings.Active = JsonUtility.FromJson<WorldSettings>(json);
                    SceneManager.LoadScene(1);
                });

                go.SetActive(true);
            }
        }

        public void NewWorld()
        {
            m_NewWorldMenu.SetActive(true);
            Destroy(gameObject);
        }
    }
}
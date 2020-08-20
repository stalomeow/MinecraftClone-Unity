using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS0649

namespace Minecraft
{
    public sealed class ResourcePackageSelector : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_Label;
        [SerializeField] private Transform m_ItemListContent;
        [SerializeField] private GameObject m_ItemTemplate;

        public string Selected => m_Label.text;

        private void Start()
        {
            string path = Path.Combine(Application.streamingAssetsPath, WorldConsts.ResourcePackagesFolderName);
            string[] packs = Directory.GetDirectories(path);

            for (int i = 0; i < packs.Length; i++)
            {
                string pack = packs[i];
                string name = Path.GetFileNameWithoutExtension(pack);

                string iconPath = Path.Combine(pack, WorldConsts.ResourcePackageIconName);
                byte[] bytes = File.ReadAllBytes(iconPath);

                Texture2D icon = new Texture2D(256, 256);
                icon.LoadImage(bytes);

                GameObject item = Instantiate(m_ItemTemplate, m_ItemListContent, true);
                TextMeshProUGUI text = item.GetComponentInChildren<TextMeshProUGUI>();
                text.text = name;

                item.GetComponentInChildren<RawImage>().texture = icon;
                item.GetComponent<Button>().onClick.AddListener(() => m_Label.text = text.text);

                item.SetActive(true);

                if (i == 0)
                {
                    m_Label.text = name;
                }
            }
        }
    }
}
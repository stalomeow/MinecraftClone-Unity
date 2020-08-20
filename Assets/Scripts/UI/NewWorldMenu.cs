using System;
using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

#pragma warning disable CS0649

namespace Minecraft
{
    public sealed class NewWorldMenu : MonoBehaviour
    {
        [SerializeField] private TMP_InputField m_NameInput;
        [SerializeField] private TMP_InputField m_SeedInput;
        [SerializeField] private TMP_Dropdown m_WorldTypeDropdown;
        [SerializeField] private ResourcePackageSelector m_ResPackSelector;

        public void Play()
        {
            string name = m_NameInput.text;
            string s = m_SeedInput.text;

            if (string.IsNullOrEmpty(name))
                return;

            string folder = Application.dataPath + "/Worlds/" + name;

            if (Directory.Exists(folder))
                return;

            if (!int.TryParse(s, out int seed))
            {
                seed = string.IsNullOrEmpty(s) ? (Process.GetCurrentProcess().Id +  DateTime.Now.GetHashCode()): s.GetHashCode();
            }

            WorldType worldType;

            switch (m_WorldTypeDropdown.captionText.text)
            {
                case "Endless":
                    worldType = WorldType.Normal;
                    break;
                case "Plain":
                    worldType = WorldType.Plain;
                    break;
                case "Old":
                    worldType = WorldType.Fixed;
                    break;
                default:
                    return;
            }

            string resPackName = m_ResPackSelector.Selected;

            if (string.IsNullOrEmpty(resPackName))
            {
                resPackName = WorldConsts.DefaultResourcePackageName;
            }

            WorldSettings.Active = new WorldSettings
            {
                Name = name,
                Type = worldType,
                Mode = PlayMode.Creative,
                Seed = seed,
                RenderChunkRadius = 6,
                HorizontalFOVInDEG = 90,
                MaxChunkCountInMemory = 700,
                EnableDestroyEffect = true,
                Position = Vector3.down,
                ResourcePackageName = resPackName
            };

            SceneManager.LoadScene(1);
        }
    }
}
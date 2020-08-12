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

            WorldSettings.Active = new WorldSettings
            {
                Name = name,
                Type = WorldType.Normal,
                Mode = PlayMode.Creative,
                Seed = seed,
                RenderChunkRadius = 6,
                HorizontalFOVInDEG = 90,
                MaxChunkCountInMemory = 700,
                EnableDestroyEffect = true,
                Position = Vector3.down
            };

            SceneManager.LoadScene(1);
        }
    }
}
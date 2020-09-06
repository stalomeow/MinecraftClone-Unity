using System;
using System.IO;
using UnityEngine;

namespace Minecraft
{
    [Serializable]
    public sealed class GlobalSettings
    {
        public int RenderChunkRadius;
        public float HorizontalFOVInDEG;
        public int MaxChunkCountInMemory;
        public int MaxTaskCountPerFrame;
        public bool EnableDestroyEffect;

        public int RenderRadius => RenderChunkRadius * WorldConsts.ChunkWidth;

        public Color DefaultAmbientColor => new Color(0.3632075f, 0.6424405f, 1f, 1f);


        public static GlobalSettings Instance { get; }

        static GlobalSettings()
        {
            string path = GetSettingsSavingPath();

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                Instance = JsonUtility.FromJson<GlobalSettings>(json);
            }
            else
            {
                Instance = new GlobalSettings
                {
                    RenderChunkRadius = 6,
                    HorizontalFOVInDEG = 90,
                    MaxChunkCountInMemory = 700,
                    MaxTaskCountPerFrame = 10,
                    EnableDestroyEffect = true
                };

                SaveSettings();
            }
        }

        public static string GetSettingsSavingPath()
        {
            return Application.persistentDataPath + "/global_settings.json";
        }

        public static void SaveSettings()
        {
            string path = GetSettingsSavingPath();
            string json = JsonUtility.ToJson(Instance);
            File.WriteAllText(path, json);
        }
    }
}
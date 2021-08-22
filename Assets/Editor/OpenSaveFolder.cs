using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace MinecraftEditor
{
    public sealed class OpenSaveFolder
    {
        [MenuItem("Minecraft-Unity/Open Save Folder")]
        public static void OpenWorldSavingFolder()
        {
            Process.Start(Application.persistentDataPath);
        }
    }
}
using UnityEditor;
using UnityEngine;

namespace MinecraftEditor
{
    public sealed class ScreenShot
    {
        [MenuItem("Minecraft-Unity/ScreenShot")]
        private static void Capture()
        {
            string path = EditorUtility.SaveFilePanel("Save", Application.dataPath, "Capture", "png");

            if (!string.IsNullOrEmpty(path))
            {
                ScreenCapture.CaptureScreenshot(path);
            }
        }
    }
}
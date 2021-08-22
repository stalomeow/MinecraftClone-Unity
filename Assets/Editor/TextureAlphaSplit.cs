using System.IO;
using UnityEditor;
using UnityEngine;

namespace MinecraftEditor
{
    public sealed class TextureAlphaSplit : EditorWindow
    {
        [MenuItem("Minecraft-Unity/Textures/Split Texture Alpha")]
        private static void Init()
        {
            GetWindow<TextureAlphaSplit>(true, "Split Texture Alpha");
        }

        private Texture2D m_Target = null;

        private void OnGUI()
        {
            m_Target = EditorGUILayout.ObjectField("Texture", m_Target, typeof(Texture2D), true) as Texture2D;

            EditorGUILayout.Space();

            using (new EditorGUI.DisabledGroupScope(m_Target == null))
            {
                if (GUILayout.Button("Split"))
                {
                    try
                    {
                        Do();
                    }
                    catch
                    {
                        EditorUtility.ClearProgressBar();
                        throw;
                    }
                }
            }
        }

        private void Do()
        {
            Texture2D rgb = new Texture2D(m_Target.width, m_Target.height, TextureFormat.RGB24, false);
            Texture2D alpha = new Texture2D(m_Target.width, m_Target.height, TextureFormat.RGB24, false);

            int totalCount = m_Target.width * m_Target.height;
            int i = 0;

            for (int x = 0; x < m_Target.width; x++)
            {
                for (int y = 0; y < m_Target.height; y++)
                {
                    Color color = m_Target.GetPixel(x, y);

                    rgb.SetPixel(x, y, new Color(color.r, color.g, color.b));
                    alpha.SetPixel(x, y, new Color(color.a, color.a, color.a));

                    i++;

                    if (EditorUtility.DisplayCancelableProgressBar("Hold On...", "Splitting Texture Alpha...", (float)i / totalCount))
                    {
                        EditorUtility.ClearProgressBar();
                        return;
                    }
                }
            }

            rgb.Apply();
            alpha.Apply();

            string path = AssetDatabase.GetAssetPath(m_Target);
            string folder = Path.GetDirectoryName(path);
            string name = Path.GetFileNameWithoutExtension(path);

            byte[] rgbBytes = rgb.EncodeToPNG();
            byte[] alphaBytes = alpha.EncodeToPNG();

            string rgbPath = Path.Combine(folder, name + "_rgb.png");
            string alphaPath = Path.Combine(folder, name + "_alpha.png");

            File.WriteAllBytes(rgbPath, rgbBytes);
            File.WriteAllBytes(alphaPath, alphaBytes);

            EditorUtility.ClearProgressBar();

            AssetDatabase.Refresh();

            Debug.LogWarning("Set Filter Mode!");
        }
    }
}

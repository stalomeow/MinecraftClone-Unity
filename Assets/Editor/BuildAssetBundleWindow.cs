using UnityEditor;
using UnityEngine;

namespace Minecraft.AssetManagement
{
    public sealed class BuildAssetBundleWindow : EditorWindow
    {
        [MenuItem("Minecraft-Unity/BuildAssetBundle")]
        private static void Init()
        {
            GetWindow<BuildAssetBundleWindow>(false, "Build AssetBundles", true);
        }

        [SerializeField] private string m_OutputPath;
        [SerializeField] private BuildTarget m_BuildTarget = BuildTarget.StandaloneWindows;

        private void OnEnable()
        {
            if (string.IsNullOrEmpty(m_OutputPath))
            {
                m_OutputPath = Application.streamingAssetsPath;
            }
        }

        private void OnGUI()
        {
            m_OutputPath = EditorGUILayout.TextField("Output Path", m_OutputPath);

            m_BuildTarget = (BuildTarget)EditorGUILayout.EnumPopup("Build Target", m_BuildTarget);

            if (GUILayout.Button("Build"))
            {
                AssetBundleBuilder.Build(m_OutputPath, m_BuildTarget);
            }
        }
    }
}
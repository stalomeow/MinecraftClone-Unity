using UnityEditor;
using UnityEngine;

namespace Minecraft.AssetManagement
{
    public sealed class BuildAssetBundleWindow : EditorWindow
    {
        [MenuItem("Minecraft-Unity/Build Resource Package")]
        private static void Init()
        {
            GetWindow<BuildAssetBundleWindow>(false, "Build Resource Package", true);
        }

        [SerializeField] private string m_Name;
        [SerializeField] private Texture2D m_Icon;
        [SerializeField] private string m_OutputPath;
        [SerializeField] private BuildTarget m_BuildTarget = BuildTarget.StandaloneWindows;

        private void OnEnable()
        {
            if (string.IsNullOrEmpty(m_Name))
            {
                m_Name = WorldConsts.DefaultResourcePackageName;
            }

            if (string.IsNullOrEmpty(m_OutputPath))
            {
                m_OutputPath = Application.streamingAssetsPath + "/" + WorldConsts.ResourcePackagesFolderName;
            }
        }

        private void OnGUI()
        {
            m_Name = EditorGUILayout.TextField("Package Name", m_Name);
            m_Icon = EditorGUILayout.ObjectField("Icon", m_Icon, typeof(Texture2D), false) as Texture2D;
            m_OutputPath = EditorGUILayout.TextField("Output Path", m_OutputPath);

            m_BuildTarget = (BuildTarget)EditorGUILayout.EnumPopup("Build Target", m_BuildTarget);

            if (GUILayout.Button("Build"))
            {
                if (m_Icon == null)
                {
                    Debug.LogError("icon");
                    return;
                }

                //AssetBundleBuilder.Build(m_OutputPath + "/" + m_Name, m_BuildTarget, m_Icon);
            }
        }
    }
}
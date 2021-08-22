using UnityEditor;
using UnityEngine;

namespace MinecraftEditor
{
    public class MaterialCleaner : EditorWindow
    {
        [MenuItem("Minecraft-Unity/Material Cleaner")]
        public static void Init()
        {
            GetWindow<MaterialCleaner>();
        }


        private Material m_Material = null;
        private SerializedObject m_SerializedObject = null;
        private Vector2 m_ScrollPos = Vector2.zero;

        private void OnEnable()
        {
            if (m_Material)
            {
                m_SerializedObject = new SerializedObject(m_Material);
            }
        }

        private void OnGUI()
        {
            EditorGUI.BeginChangeCheck();
            m_Material = EditorGUILayout.ObjectField("Material", m_Material, typeof(Material), false) as Material;

            if (EditorGUI.EndChangeCheck() && m_Material)
            {
                m_SerializedObject = new SerializedObject(m_Material);
            }

            if (!m_Material)
            {
                return;
            }

            m_SerializedObject.Update();
            EditorGUILayout.Space();

            m_ScrollPos = EditorGUILayout.BeginScrollView(m_ScrollPos);

            EditorGUILayout.LabelField("Textures", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            ProcessProperties("m_SavedProperties.m_TexEnvs");
            EditorGUI.indentLevel--;

            EditorGUILayout.LabelField("Floats", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            ProcessProperties("m_SavedProperties.m_Floats");
            EditorGUI.indentLevel--;

            EditorGUILayout.LabelField("Colors", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            ProcessProperties("m_SavedProperties.m_Colors");
            EditorGUI.indentLevel--;

            EditorGUILayout.EndScrollView();
        }

        private void ProcessProperties(string path)
        {
            SerializedProperty property = m_SerializedObject.FindProperty(path);

            if (property == null || !property.isArray)
            {
                return;
            }

            for (int i = 0; i < property.arraySize; i++)
            {
                string elementName = property.GetArrayElementAtIndex(i).displayName;
                bool exist = m_Material.HasProperty(elementName);

                if (exist)
                {
                    EditorGUILayout.LabelField(elementName, "Exist");
                }
                else
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.LabelField(elementName, "Old Reference", "CN StatusWarn");

                        if (GUILayout.Button("Remove", GUILayout.Width(80)))
                        {
                            property.DeleteArrayElementAtIndex(i);
                            m_SerializedObject.ApplyModifiedProperties();
                            GUIUtility.ExitGUI();
                        }
                    }
                }
            }
        }
    }
}

using Minecraft;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace MinecraftEditor
{
    [CustomEditor(typeof(Block))]
    public sealed class BlockEditor : Editor
    {
        private SerializedProperty m_StepSoundsProperty;
        private SerializedProperty m_ExtraAssetsProperty;
        private ReorderableList m_StepSounds;
        private ReorderableList m_ExtraAssets;

        private void OnEnable()
        {
            m_StepSoundsProperty = serializedObject.FindProperty("m_StepAudios");
            m_StepSounds = new ReorderableList(serializedObject, m_StepSoundsProperty, true, true, true, true)
            {
                drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Steps"),
                drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    SerializedProperty p = m_StepSoundsProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(rect, p);
                }
            };

            m_ExtraAssetsProperty = serializedObject.FindProperty("m_ExtraAssets");
            m_ExtraAssets = new ReorderableList(serializedObject, m_ExtraAssetsProperty, true, true, true, true)
            {
                drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Extra Assets"),
                drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    SerializedProperty p = m_ExtraAssetsProperty.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(rect, p);
                }
            };
        }

        public override void OnInspectorGUI()
        {
            SerializedProperty idProperty = serializedObject.FindProperty("m_Id");

            EditorGUILayout.PropertyField(idProperty);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_NameKey"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Flags"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Logics"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_MeshWriter"));

            if (idProperty.intValue == Block.AirId)
            {
                EditorGUILayout.HelpBox("This is an air block", MessageType.Error);
            }
            else
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_MoveResistance"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_LightOpacity"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_LightValue"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Hardness"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_DestoryEffectColor"));

                EditorGUILayout.Space();
                TextureFields();

                EditorGUILayout.Space();
                AudioFields();

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Others", EditorStyles.boldLabel);
                m_ExtraAssets.DoLayoutList();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void TextureFields()
        {
            string[] textures = (target as Block).MeshWriter?.RequiredTextureNames;
            SerializedProperty texArray = serializedObject.FindProperty("m_Textures");

            if (textures == null || textures.Length == 0)
            {
                texArray.ClearArray();
            }
            else
            {
                EditorGUILayout.LabelField("Textures", EditorStyles.boldLabel);
                texArray.arraySize = textures.Length;

                for (int i = 0; i < textures.Length; i++)
                {
                    SerializedProperty element = texArray.GetArrayElementAtIndex(i);
                    GUIContent label = new GUIContent(textures[i]);

                    EditorGUILayout.PropertyField(element, label);
                }
            }
        }

        private void AudioFields()
        {
            EditorGUILayout.LabelField("Audios", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_DigAudio"), new GUIContent("Dig"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_PlaceAudio"), new GUIContent("Place"));
            m_StepSounds.DoLayoutList();
        }


        protected override bool ShouldHideOpenButton()
        {
            return true;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Minecraft.Configurations;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace MinecraftEditor.Configurations.Blocks
{
    [CustomEditor(typeof(BlockMesh))]
    public class BlockMeshEditor : Editor
    {
        public static readonly GUIContent FaceContent = new GUIContent("Face");
        public static readonly GUIContent AddContent = new GUIContent("Add");
        public static readonly GUIContent RemoveContent = new GUIContent("Remove");

        private IEnumerable<GUIContent> m_AllFaces;
        private List<string> m_ExistedFaces;
        private string[] m_FieldList;
        private int m_SelectedFaceArrayIndex;
        private int m_SelectedFieldIndex;
        private ReorderableList m_VertexOrIndexList;
        private MethodInfo m_ClearListCacheMethod;
        private SerializedProperty m_FaceArray;
        private SerializedProperty m_CurrentFace;

        private void OnEnable()
        {
            m_AllFaces = Enum.GetNames(typeof(BlockFace)).Select(face => new GUIContent(face));
            m_ExistedFaces = new List<string>();
            m_FieldList = new string[]{
                nameof(BlockMesh.FaceData.Vertices),
                nameof(BlockMesh.FaceData.Indices)
            };
            m_SelectedFaceArrayIndex = 0;
            m_SelectedFieldIndex = 0;
            m_VertexOrIndexList = new ReorderableList(serializedObject, null, true, false, true, true);
            m_VertexOrIndexList.elementHeightCallback = GetElementHeight;
            m_VertexOrIndexList.drawElementCallback = DrawElement;
            m_ClearListCacheMethod = typeof(ReorderableList).GetMethod("ClearCacheRecursive", BindingFlags.Instance | BindingFlags.NonPublic);
            m_FaceArray = serializedObject.FindProperty(nameof(BlockMesh.Faces));
            m_CurrentFace = null;
        }

        public override void OnInspectorGUI()
        {
            DrawPivotField();
            DrawBoundingBoxField();
            InitAndSelectExistedFace();
            DrawNeverClipField();
            DrawAddAndRemoveButtons();
            EditorGUILayout.Space();
            DrawVerticesAndIndices();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawPivotField()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(BlockMesh.Pivot)));
            EditorGUILayout.Space();
        }

        private void DrawBoundingBoxField()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(BlockMesh.BoundingBox)));
            EditorGUILayout.Space();
        }

        private SerializedProperty GetFaceProperty(SerializedProperty property)
        {
            return property.FindPropertyRelative(nameof(BlockMesh.FaceData.Face));
        }

        private void SetCurrentFace(int index)
        {
            m_SelectedFaceArrayIndex = index;
            m_CurrentFace = m_FaceArray.arraySize == 0 ? null : m_FaceArray.GetArrayElementAtIndex(m_SelectedFaceArrayIndex);
        }

        private void InitAndSelectExistedFace()
        {
            m_ExistedFaces.Clear();

            if (m_FaceArray.arraySize > 0)
            {
                for (int i = 0; i < m_FaceArray.arraySize; i++)
                {
                    SerializedProperty face = GetFaceProperty(m_FaceArray.GetArrayElementAtIndex(i));
                    m_ExistedFaces.Add(face.enumNames[face.enumValueIndex]);
                }

                int index = EditorGUILayout.Popup(FaceContent, m_SelectedFaceArrayIndex, m_ExistedFaces.ToArray());
                SetCurrentFace(index);
            }
        }

        private void DrawNeverClipField()
        {
            if (m_CurrentFace == null)
            {
                return;
            }

            SerializedProperty neverClip = m_CurrentFace.FindPropertyRelative(nameof(BlockMesh.FaceData.NeverClip));
            EditorGUILayout.PropertyField(neverClip);

            if (neverClip.boolValue)
            {
                EditorGUILayout.HelpBox("This face will never be clipped, whether it is visible or not.", MessageType.Warning);
            }
        }

        private void DrawAddAndRemoveButtons()
        {
            if (m_FaceArray.arraySize == 0)
            {
                EditorGUILayout.HelpBox("There are no faces in this mesh, so it will never be rendered.", MessageType.Warning);
            }
            else
            {
                EditorGUILayout.HelpBox($"{m_FaceArray.arraySize} faces in mesh.", MessageType.Info);
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();


                if (EditorGUILayout.DropdownButton(AddContent, FocusType.Passive))
                {
                    GenericMenu menu = new GenericMenu();

                    foreach (GUIContent face in m_AllFaces)
                    {
                        if (m_ExistedFaces.Contains(face.text))
                        {
                            menu.AddDisabledItem(face, false);
                        }
                        else
                        {
                            menu.AddItem(face, false, AddFaceToArray, face.text);
                        }
                    }

                    menu.ShowAsContext();
                }

                using (new EditorGUI.DisabledGroupScope(m_FaceArray.arraySize == 0))
                {
                    if (GUILayout.Button(RemoveContent) && EditorUtility.DisplayDialog("Remove Operation", $"Are you sure to remove <{m_ExistedFaces[m_SelectedFaceArrayIndex]}>?", "Yes", "No"))
                    {
                        m_FaceArray.DeleteArrayElementAtIndex(m_SelectedFaceArrayIndex);
                        SetCurrentFace(Mathf.Clamp(m_SelectedFaceArrayIndex, 0, m_FaceArray.arraySize - 1));
                    }
                }
            }
        }

        private void AddFaceToArray(object faceName)
        {
            m_FaceArray.InsertArrayElementAtIndex(m_FaceArray.arraySize);
            SetCurrentFace(m_FaceArray.arraySize - 1);
            SerializedProperty face = GetFaceProperty(m_CurrentFace);
            face.enumValueIndex = Array.IndexOf(face.enumNames, faceName as string);
        }

        private void DrawVerticesAndIndices()
        {
            if (m_CurrentFace == null)
            {
                return;
            }

            int fieldIndex = GUILayout.Toolbar(m_SelectedFieldIndex, m_FieldList);
            m_VertexOrIndexList.serializedProperty = m_CurrentFace.FindPropertyRelative(m_FieldList[fieldIndex]);

            if (fieldIndex != m_SelectedFieldIndex)
            {
                m_SelectedFieldIndex = fieldIndex;
                m_ClearListCacheMethod.Invoke(m_VertexOrIndexList, Array.Empty<object>()); // 清除列表元素的高度缓存
            }

            EditorGUILayout.HelpBox($"{m_VertexOrIndexList.serializedProperty.arraySize} {m_FieldList[fieldIndex]} will be written in order.", MessageType.None);
            m_VertexOrIndexList.DoLayoutList();
        }

        private float GetElementHeight(int index)
        {
            SerializedProperty element = m_VertexOrIndexList.serializedProperty.GetArrayElementAtIndex(index);

            if (element.propertyType == SerializedPropertyType.Integer)
            {
                return EditorGUI.GetPropertyHeight(element, GUIContent.none, true);
            }

            float result = 0;
            element.NextVisible(true);

            do result += EditorGUI.GetPropertyHeight(element);
            while (element.NextVisible(false) && typeof(BlockVertexData).GetField(element.name) != null);
            return result;
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty element = m_VertexOrIndexList.serializedProperty.GetArrayElementAtIndex(index);

            if (element.propertyType == SerializedPropertyType.Integer)
            {
                EditorGUI.PropertyField(rect, element);
            }
            else
            {
                element.NextVisible(true);

                do
                {
                    float height = EditorGUI.GetPropertyHeight(element);
                    Rect rect0 = new Rect(rect.x, rect.y, rect.width, height);
                    rect.y += rect0.height;
                    rect.height -= rect0.height;
                    EditorGUI.PropertyField(rect0, element);
                } while (element.NextVisible(false) && typeof(BlockVertexData).GetField(element.name) != null);
            }
        }

        protected override bool ShouldHideOpenButton()
        {
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using ToaruUnity.UI;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ToaruUnityEditor.UI
{
    [CustomEditor(typeof(AbstractView), true)]
    internal sealed class ViewEditor : Editor
    {
        private const string TransitionsFieldName = "m_Transitions";
        private const string OnBeforeTransitionFieldName = "m_OnBeforeTransition";
        private const string OnAfterTransitionFieldName = "m_OnAfterTransition";

        private ReorderableList m_Transitions;

        private void OnEnable()
        {
            m_Transitions = new ReorderableList(serializedObject, serializedObject.FindProperty(TransitionsFieldName), true, true, true, true);
            m_Transitions.drawHeaderCallback = (Rect rect) =>
            {
                GUIContent content = new GUIContent($"Transitions ({m_Transitions.serializedProperty.arraySize})", m_Transitions.serializedProperty.tooltip);
                EditorGUI.LabelField(rect, content);
            };
            m_Transitions.elementHeightCallback = (int index) =>
            {
                SerializedProperty property = m_Transitions.serializedProperty.GetArrayElementAtIndex(index);
                return EditorGUI.GetPropertyHeight(property) + EditorGUIUtility.singleLineHeight * 0.5f;
            };
            m_Transitions.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                SerializedProperty property = m_Transitions.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(rect, property);
            };
        }

        public override void OnInspectorGUI()
        {
            SerializedProperty it = serializedObject.GetIterator();
            it.NextVisible(true);

            while (it.NextVisible(false))
            {
                if (it.name != TransitionsFieldName && it.name != OnBeforeTransitionFieldName && it.name != OnAfterTransitionFieldName)
                {
                    EditorGUILayout.PropertyField(it);
                }
            }

            EditorGUILayout.Space();

            m_Transitions.DoLayoutList();

            EditorGUILayout.PropertyField(serializedObject.FindProperty(OnBeforeTransitionFieldName));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(OnAfterTransitionFieldName));
            
            serializedObject.ApplyModifiedProperties();
        }
    }

    public static class ViewGUIUtility
    {

        public static void DrawViewDebugInfo(AbstractView view, ref bool foldout)
        {
            if (!EditorApplication.isPlaying)
                return;

            if (PrefabUtility.IsPartOfPrefabAsset(view))
                return;

            foldout = EditorGUILayout.BeginFoldoutHeaderGroup(foldout, "Runtime");

            if (foldout)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    //EditorGUILayout.LabelField($"Key: {GetObjString(view.InternalObjKey)}");

                    EditorGUILayout.LabelField("ViewState", EditorStyles.boldLabel);
                    if (view.IsTransiting)
                    {
                        EditorGUILayout.HelpBox($"Transforming({view.RemainingTransitionCount} Remains)", MessageType.Warning);
                    }
                    else
                    {
                        EditorGUILayout.HelpBox(view.State.ToString(), MessageType.Info);
                    }

                    if (view.Actions != null)
                    {
                        EditorGUILayout.LabelField("Action Center", EditorStyles.boldLabel);

                        EditorGUILayout.LabelField(view.Actions.GetType().ToString());
                        EditorGUILayout.LabelField($"{view.Actions.ExecutingCoroutineCount} Executing Coroutines");

                        if (view.Actions.ActionCount > 0)
                        {
                            EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);

                            foreach (KeyValuePair<string, Delegate> pair in view.Actions.ActionMap)
                            {
                                EditorGUILayout.LabelField($"[{pair.Key}] {pair.Value.Method.Name}");
                            }
                        }
                    }

                    EditorGUILayout.Space();
                }
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private static string GetObjString(object obj)
        {
            return obj == null ? "Null" : obj.ToString();
        }
    }
}
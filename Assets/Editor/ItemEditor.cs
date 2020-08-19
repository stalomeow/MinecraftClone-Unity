using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Minecraft.ItemsData
{
    [CustomEditor(typeof(Item))]
    public sealed class ItemEditor : Editor
    {
        private const string ItemTypeScriptPath = "/Scripts/ItemsData/ItemType.cs";

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_ItemName"));

            ShowItemTypeEnum(serializedObject.FindProperty("m_Type"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_MappedBlock"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_MaxStackSize"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_DigSpeed"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Icon"));

            serializedObject.ApplyModifiedProperties();
        }

        private void ShowItemTypeEnum(SerializedProperty property)
        {
            Rect rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight);
            Rect enumRect = new Rect(rect.x, rect.y, rect.width - 40 - 5, rect.height);
            Rect buttonRect = new Rect(enumRect.xMax + 5, rect.y, 40, rect.height);

            EditorGUI.PropertyField(enumRect, property);

            if (GUI.Button(buttonRect, "New"))
            {
                if (Enum.TryParse<ItemType>(target.name, true, out _))
                    return;

                string scriptPath = Application.dataPath + ItemTypeScriptPath;
                ItemType[] itemTypes = Enum.GetValues(typeof(ItemType)) as ItemType[];

                string script = "using XLua;\n\nnamespace Minecraft.ItemsData\n{\n    [LuaCallCSharp]\n    public enum ItemType : byte\n    {\n";

                foreach (var t in itemTypes)
                {
                    script += string.Format($"        {t.ToString()} = {((byte)t).ToString()},\n");
                }

                script += string.Format($"        {target.name} = {itemTypes.Length.ToString()},\n");
                script += "    }\n}";

                File.WriteAllText(scriptPath, script);
                AssetDatabase.Refresh();
            }
        }
    }
}
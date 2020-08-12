using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Minecraft.ItemsData
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Item))]
    public sealed class ItemEditor : Editor
    {
        private const string ItemRegistryPath = "Assets/_Minecraft/Items/_Registry.asset";
        private const string ItemTypeScriptPath = "/Scripts/ItemsData/ItemType.cs";

        public override void OnInspectorGUI()
        {
            ItemRegistry registry = GetRegistry();

            if (targets.Length == 1)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_ItemName"));

                ShowItemTypeEnum(serializedObject.FindProperty("m_Type"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_MappedBlock"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_MaxStackSize"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_DigSpeed"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Icon"));
            }
            else
            {
                ShowAllItemNames(registry);
            }

            EditorGUILayout.Space();

            RegisterBlockButton(registry);

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

                string script = "namespace Minecraft.ItemsData\n{\n    public enum ItemType : byte\n    {\n";

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

        private void ShowAllItemNames(ItemRegistry registry)
        {
            EditorGUILayout.LabelField("Item", "Registered");

            EditorGUILayout.Separator();

            foreach (var obj in targets)
            {
                Item item = obj as Item;
                EditorGUILayout.Toggle(item.ItemName, registry.RegisteredItems.Contains(item));
            }
        }

        private ItemRegistry GetRegistry()
        {
            ItemRegistry registry = AssetDatabase.LoadAssetAtPath<ItemRegistry>(ItemRegistryPath);

            if (registry == null)
            {
                registry = CreateInstance<ItemRegistry>();
                AssetDatabase.CreateAsset(registry, ItemRegistryPath);
            }

            return registry;
        }

        private void RegisterBlockButton(ItemRegistry registry)
        {
            bool disabled = registry.RegisteredItems != null && targets.All(obj => registry.RegisteredItems.Contains(obj as Item));

            using (new EditorGUI.DisabledGroupScope(disabled))
            {
                if (GUILayout.Button("Register"))
                {
                    List<Item> items = new List<Item>();

                    if (registry.RegisteredItems != null)
                    {
                        items.AddRange(registry.RegisteredItems);
                    }

                    for (int i = 0; i < targets.Length; i++)
                    {
                        Item item = targets[i] as Item;

                        if (item == null || items.Contains(item))
                            continue;

                        items.Add(item);
                    }

                    registry.RegisteredItems = (from i in items orderby i.Type ascending select i).ToArray();
                    EditorUtility.SetDirty(registry);
                }
            }

            disabled = registry.RegisteredItems == null || targets.All(obj => !registry.RegisteredItems.Contains(obj as Item));

            using (new EditorGUI.DisabledGroupScope(disabled))
            {
                if (GUILayout.Button("Unregister"))
                {
                    List<Item> items = new List<Item>(registry.RegisteredItems);

                    for (int i = 0; i < targets.Length; i++)
                    {
                        Item item = targets[i] as Item;

                        if (item == null)
                            continue;

                        items.Remove(item);
                    }

                    registry.RegisteredItems = (from i in items orderby i.Type ascending select i).ToArray();
                    EditorUtility.SetDirty(registry);
                }
            }
        }
    }
}
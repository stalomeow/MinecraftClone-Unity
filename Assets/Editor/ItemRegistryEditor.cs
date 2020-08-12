using UnityEditor;

namespace Minecraft.ItemsData
{
    [CustomEditor(typeof(ItemRegistry))]
    public sealed class ItemRegistryEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Item[] items = (target as ItemRegistry).RegisteredItems;

            int itemCount = items == null ? 0 : items.Length;
            EditorGUILayout.LabelField($"Items ({itemCount})", EditorStyles.boldLabel);

            if (itemCount == 0)
                return;

            EditorGUILayout.Space();

            using (new EditorGUI.DisabledGroupScope(true))
            {
                foreach (var item in items)
                {
                    EditorGUILayout.ObjectField(item, typeof(Item), true);
                }
            }
        }
    }
}
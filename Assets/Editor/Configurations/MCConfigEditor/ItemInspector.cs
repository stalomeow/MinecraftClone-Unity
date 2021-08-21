using Minecraft.Configurations;
using MinecraftEditor.Assets;
using UnityEditor;
using UnityEngine;

namespace MinecraftEditor.Configurations.MCConfigEditor
{
    public class ItemInspector : WindowInspector
    {
        private static GUIContent s_IDContent = new GUIContent("ID");
        private static GUIContent s_InternalNameContent = new GUIContent("Internal Name");
        private static GUIContent s_IconContent = new GUIContent("Icon");
        private static GUIContent s_BlockContent = new GUIContent("Block");
        private static GUIContent s_AttackContent = new GUIContent("Attack");
        private static GUIContent s_RarityContent = new GUIContent("Rarity");
        private static GUIContent s_MaxStackCountContent = new GUIContent("Max Stack Count");
        private static GUIContent s_DescriptionContent = new GUIContent("Description");
        private static GUIContent s_UsageContent = new GUIContent("Usage");
        private static GUIContent s_ObtainApproachContent = new GUIContent("Obtain Approach");

        private static float s_TextAreaHeight = EditorGUIUtility.singleLineHeight * 5;


        public ItemInspector(MainWindow mainWindow) : base(mainWindow) { }

        protected override void OnScrollableGUI(ref VerticalGUIRect rect, int index)
        {
            ItemData item = MainWnd.Items[index];

            rect.Space(10);

            using (new EditorGUI.DisabledGroupScope(true))
            {
                EditorGUI.IntField(rect.Next, s_IDContent, item.ID);
            }

            item.InternalName = EditorGUI.TextField(rect.Next, s_InternalNameContent, item.InternalName);
            item.Icon = EditorAssetUtility.AssetPtrField(rect.Next, s_IconContent, item.Icon, typeof(Sprite));
            item.Block = EditorGUI.TextField(rect.Next, s_BlockContent, item.Block);
            item.Attack = EditorGUI.IntField(rect.Next, s_AttackContent, item.Attack);
            item.Rarity = EditorGUI.IntSlider(rect.Next, s_RarityContent, item.Rarity, 1, 6);
            item.MaxStackCount = EditorGUI.IntSlider(rect.Next, s_MaxStackCountContent, item.MaxStackCount, 1, 99);
            rect.Space(10);

            EditorGUI.LabelField(rect.Next, s_DescriptionContent, EditorStyles.boldLabel);
            item.Description = EditorGUI.TextArea(rect.GetNext(s_TextAreaHeight, true, true), item.Description);
            rect.Space(10);

            EditorGUI.LabelField(rect.Next, s_UsageContent, EditorStyles.boldLabel);
            item.Usage = EditorGUI.TextArea(rect.GetNext(s_TextAreaHeight, true, true), item.Usage);
            rect.Space(10);

            EditorGUI.LabelField(rect.Next, s_ObtainApproachContent, EditorStyles.boldLabel);
            item.ObtainApproach = EditorGUI.TextArea(rect.GetNext(s_TextAreaHeight, true, true), item.ObtainApproach);
            rect.Space(10);
        }
    }
}

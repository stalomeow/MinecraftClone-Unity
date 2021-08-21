using Minecraft.Configurations;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace MinecraftEditor.Configurations.MCConfigEditor
{
    public class WindowTreeView : TreeView
    {
        private readonly MainWindow m_MainWindow;

        public WindowTreeView(MainWindow mainWindow, TreeViewState state) : base(state)
        {
            showBorder = false;
            showAlternatingRowBackgrounds = false;
            rowHeight = 30;

            m_MainWindow = mainWindow;
        }

        protected override TreeViewItem BuildRoot()
        {
            TreeViewItem root = new TreeViewItem(-1, -1, "root");

            if (m_MainWindow.GUIMode == WindowGUIMode.Blocks)
            {
                foreach (BlockData block in m_MainWindow.Blocks)
                {
                    root.AddChild(new BlockTreeViewItem(0, block));
                }
            }
            else if (m_MainWindow.GUIMode == WindowGUIMode.Items)
            {
                foreach (ItemData item in m_MainWindow.Items)
                {
                    root.AddChild(new ItemTreeViewItem(0, item));
                }
            }
            else
            {
                foreach (BiomeData biome in m_MainWindow.Biomes)
                {
                    root.AddChild(new BiomeTreeViewItem(0, biome));
                }
            }

            return root;
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            Rect rect = args.rowRect;
            float heightDelta = (rect.height - EditorGUIUtility.singleLineHeight);

            rect.x += 10;
            rect.width -= 20;
            rect.y += heightDelta * 0.5f;
            rect.height -= heightDelta;

            Rect iconRect = new Rect(rect.x, rect.y, rect.height, rect.height);
            EditorGUI.LabelField(iconRect, EditorGUIUtility.IconContent("Prefab Icon"));

            Rect labelRect = new Rect(iconRect.xMax, rect.y, rect.width - iconRect.width, rect.height);
            EditorGUI.LabelField(labelRect, args.item.displayName, EditorStyles.largeLabel);
        }

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return false;
        }
    }
}

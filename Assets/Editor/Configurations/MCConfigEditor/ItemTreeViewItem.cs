using Minecraft.Configurations;
using UnityEditor.IMGUI.Controls;

namespace MinecraftEditor.Configurations.MCConfigEditor
{
    public class ItemTreeViewItem : TreeViewItem
    {
        public ItemData Item { get; }

        public override int id
        {
            get => Item.ID;
            set => Item.ID = value;
        }

        public override string displayName
        {
            get => Item.InternalName;
            set => Item.InternalName = value;
        }

        public ItemTreeViewItem(int depth, ItemData item)
        {
            base.depth = depth;
            Item = item;
        }
    }
}

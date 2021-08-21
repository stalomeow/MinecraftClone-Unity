using Minecraft.Configurations;
using UnityEditor.IMGUI.Controls;

namespace MinecraftEditor.Configurations.MCConfigEditor
{
    public class BlockTreeViewItem : TreeViewItem
    {
        public BlockData Block { get; }

        public override int id
        {
            get => Block.ID;
            set => Block.ID = value;
        }

        public override string displayName
        {
            get => Block.InternalName;
            set => Block.InternalName = value;
        }

        public BlockTreeViewItem(int depth, BlockData block)
        {
            base.depth = depth;
            Block = block;
        }
    }
}

using Minecraft.Configurations;
using UnityEditor.IMGUI.Controls;

namespace MinecraftEditor.Configurations.MCConfigEditor
{
    public class BiomeTreeViewItem : TreeViewItem
    {
        public BiomeData Biome { get; }

        public override int id
        {
            get => Biome.ID;
            set => Biome.ID = value;
        }

        public override string displayName
        {
            get => Biome.InternalName;
            set => Biome.InternalName = value;
        }

        public BiomeTreeViewItem(int depth, BiomeData biome)
        {
            base.depth = depth;
            Biome = biome;
        }
    }
}

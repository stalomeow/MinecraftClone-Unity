using System;
using Minecraft.Assets;

namespace Minecraft.Configurations
{
    [Serializable]
    public class ItemData : IOrderedConfigData
    {
        public int ID;
        public string InternalName;
        public AssetPtr Icon;
        public string Block;
        public int Attack = 5;
        public int Rarity = 1;
        public int MaxStackCount = 64;
        public string Description;
        public string Usage;
        public string ObtainApproach;

        int IOrderedConfigData.ID
        {
            get => ID;
            set => ID = value;
        }

        string IOrderedConfigData.InternalName
        {
            get => InternalName;
            set => InternalName = value;
        }
    }
}

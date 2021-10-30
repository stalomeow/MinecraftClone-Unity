using System;
using System.Collections.Generic;
using Minecraft.Assets;
using Minecraft.PhysicSystem;

namespace Minecraft.Configurations
{
    [Serializable]
    public class BlockData : IOrderedConfigData
    {
        public int ID;
        public string InternalName;

        public string RewardItem;
        public BlockFlags Flags;
        public BlockRotationAxes RotationAxes;
        public BlockEntityConversion EntityConversion;

        public int Hardness = 16;
        public int LightValue = 0;
        public int LightOpacity = 15;
        public PhysicState PhysicState;
        public PhysicMaterial PhysicMaterial;

        public int? Mesh;
        public int? Material;
        public int?[][] Textures;

        public AssetPtr DigAudio;
        public AssetPtr PlaceAudio;
        public List<AssetPtr> StepAudios;

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

using System;

namespace Minecraft.Configurations
{
    [Serializable]
    public class BiomeData : IOrderedConfigData
    {
        public int ID;
        public string InternalName;

        /** The base height of this biome. Default 0.1. */
        public float BaseHeight = 0.1f;
        /** The variation from the base height of the biome. Default 0.2. */
        public float HeightVariation = 0.2f;
        /** The temperature of this biome. */
        public float Temperature = 0.5f;
        /** The rainfall in this biome. */
        public float Rainfall = 0.5f;
        /** Set to true if snow is enabled for this biome. */
        public bool EnableSnow = false;
        /** Is true (default) if the biome support rain (desert and nether can't have rain) */
        public bool EnableRain = true;
        /** The block expected to be on the top of this biome */
        public string TopBlock;
        /** The block to fill spots in when not on the top */
        public string FillerBlock;

        public int TreesPerChunk;
        public float ExtraTreeChance = 0.05f;
        public int GrassPerChunk = 10;
        public int FlowersPerChunk = 4;
        public int MushroomsPerChunk;

        public int DeadBushPerChunk = 2;
        public int ReedsPerChunk = 50;
        public int CactiPerChunk = 10;

        public int ClayPerChunk;
        public int WaterlilyPerChunk;
        public int SandPatchesPerChunk;
        public int GravelPatchesPerChunk;

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

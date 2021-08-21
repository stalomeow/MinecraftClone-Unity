using System;
using Minecraft.Configurations;
using Minecraft.Lua;
using Random = System.Random;

namespace Minecraft.ScriptableWorldGeneration
{
    public class GenerationContext : ILuaCallCSharp
    {
        public float[,,] DensityMap { get; }
        public float[,,] DepthMap { get; }
        public float[,,] MainNoiseMap { get; }
        public float[,,] MinLimitMap { get; }
        public float[,,] MaxLimitMap { get; }
        public float[,,] SurfaceMap { get; }
        public BiomeData[,] Biomes { get; } // 10x10 or 16 x 16

        public Random Rand { get; protected set; }

        public GenerationContext()
        {
            DensityMap = new float[5, 33, 5];
            DepthMap = new float[5, 1, 5];
            MainNoiseMap = new float[5, 33, 5];
            MinLimitMap = new float[5, 33, 5];
            MaxLimitMap = new float[5, 33, 5];
            SurfaceMap = new float[16, 1, 16];
            Biomes = new BiomeData[16, 16];
            Rand = null;
        }

        public void Initialize(int seed)
        {
            Array.Clear(DensityMap, 0, DensityMap.Length);
            Array.Clear(DepthMap, 0, DepthMap.Length);
            Array.Clear(MainNoiseMap, 0, MainNoiseMap.Length);
            Array.Clear(MinLimitMap, 0, MinLimitMap.Length);
            Array.Clear(MaxLimitMap, 0, MaxLimitMap.Length);
            Array.Clear(SurfaceMap, 0, SurfaceMap.Length);
            Array.Clear(Biomes, 0, Biomes.Length);
            Rand = new Random(seed);
        }
    }
}

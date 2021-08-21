using Minecraft.Lua;
using Minecraft.Noises;
using Minecraft.ScriptableWorldGeneration.GenLayers;

namespace Minecraft.ScriptableWorldGeneration
{
    public class GenerationHelper : ILuaCallCSharp
    {
        public int Seed;

        public GenericNoise<PerlinNoise> DepthNoise;
        public GenericNoise<PerlinNoise> MainNoise;
        public GenericNoise<PerlinNoise> MaxNoise;
        public GenericNoise<PerlinNoise> MinNoise;
        public GenericNoise<PerlinNoise> SurfaceNoise;

        public float[,] BiomeWeights;
        public StatelessGenLayer GenLayers;
    }
}

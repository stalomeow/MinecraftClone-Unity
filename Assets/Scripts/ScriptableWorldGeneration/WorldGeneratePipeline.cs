using System;
using System.Collections.Concurrent;
using Minecraft.Collections;
using Minecraft.Configurations;
using Minecraft.Lua;
using Minecraft.Noises;
using Minecraft.ScriptableWorldGeneration.GenLayers;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

namespace Minecraft.ScriptableWorldGeneration
{
    [CreateAssetMenu(menuName = "Minecraft/WorldGeneration/WorldGeneratePipeline")]
    public class WorldGeneratePipeline : ScriptableObject, ILuaCallCSharp
    {
        public bool GenerateStructure = true;
        public bool UseCaves = true;
        public bool UseRavines = true;
        public bool UseMineShafts = true;
        public bool UseVillages = true;
        public bool UseStrongholds = true;
        public bool UseTemples = true;
        public bool UseMonuments = true;
        public bool UseMansions = true;

        [Space]

        [SerializeField] protected StatelessGenerator m_TerrainGenerator;
        [SerializeField] protected StatelessGenerator m_CaveGenerator;
        [SerializeField] [FormerlySerializedAs("m_MineGenerators")] protected StatelessGenerator[] m_ExtraGenerators;

        [NonSerialized] private IWorld m_World;
        [NonSerialized] private int m_Seed;
        [NonSerialized] private GenerationHelper m_GenHelper;
        [NonSerialized] private ConcurrentBag<GenerationContext> m_GenContextPool;


        public void Initialize(IWorld world, int seed)
        {
            m_World = world;
            m_Seed = seed;
            m_GenHelper = CreateGenerationHelper(seed);
            m_GenContextPool = new ConcurrentBag<GenerationContext>();
        }

        protected virtual GenerationHelper CreateGenerationHelper(int seed)
        {
            Random random = new Random(seed);
            GenerationHelper helper = new GenerationHelper
            {
                Seed = seed,
                DepthNoise = new GenericNoise<PerlinNoise>(new PerlinNoise(random.Next()), 8, 0.5f),
                MainNoise = new GenericNoise<PerlinNoise>(new PerlinNoise(random.Next()), 8, 0.5f),
                MaxNoise = new GenericNoise<PerlinNoise>(new PerlinNoise(random.Next()), 8, 0.5f),
                MinNoise = new GenericNoise<PerlinNoise>(new PerlinNoise(random.Next()), 8, 0.5f),
                SurfaceNoise = new GenericNoise<PerlinNoise>(new PerlinNoise(random.Next()), 8, 0.5f),
                BiomeWeights = new float[5, 5],
                GenLayers = CreateGenLayers(seed)
            };

            InitBiomeWeights(helper.BiomeWeights);
            return helper;
        }

        protected virtual void InitBiomeWeights(float[,] weights)
        {
            for (int i = -2; i <= 2; i++)
            {
                for (int j = -2; j <= 2; j++)
                {
                    weights[i + 2, j + 2] = 10.0f / Mathf.Sqrt((i * i + j * j) + 0.2f);
                }
            }
        }

        protected virtual StatelessGenLayer CreateGenLayers(int seed)
        {
            StatelessGenLayer addIsland0 = new IslandLayer(seed, null);
            StatelessGenLayer zoomed0 = new ZoomLayer(seed, addIsland0);

            StatelessGenLayer biomesAdded = new BiomeLayer(seed, zoomed0);
            StatelessGenLayer addIsland1 = new AddIslandLayer(2, biomesAdded);
            StatelessGenLayer zoomed1 = new ZoomLayer(seed, addIsland1);
            StatelessGenLayer addIsland2 = new AddIslandLayer(50, zoomed1);

            // GenLayer zoomed2 = new GenLayerZoom(seed, zoomed0);
            StatelessGenLayer zoomed2 = ZoomLayer.Magnify(seed, addIsland2, 4);

            StatelessGenLayer addBeach = new AddBeachLayer(50, zoomed2);
            StatelessGenLayer zoomed3 = new ZoomLayer(seed, addBeach);
            StatelessGenLayer addRiver = new AddRiverLayer(1000, zoomed3);

            StatelessGenLayer result = ZoomLayer.Magnify(seed, addRiver, 2);

            // GenLayer biomesAdded = new GenLayerBiome(seed, zoomed0);
            // GenLayer zoomed2 = new GenLayerZoom(seed, zoomed1);
            return result;
        }


        public virtual void GenerateChunk(Chunk chunk)
        {
            GenerationContext context = GetGenerationContext();
            chunk.GetRawDataNoCheck(out ChunkPos transform, out IWorld world, out BlockData[,,] blocks, out Quaternion[,,] rotations, out NibbleArray skyLights, out byte[,] heightMap);
            InitializeBlockRotations(rotations);

            m_TerrainGenerator.Generate(world, transform, blocks, rotations, heightMap, m_GenHelper, context);

            // 生成洞穴
            if (UseCaves)
            {
                m_CaveGenerator.Generate(world, transform, blocks, rotations, heightMap, m_GenHelper, context);
            }

            for (int i = 0; i < m_ExtraGenerators.Length; i++)
            {
                m_ExtraGenerators[i].Generate(world, transform, blocks, rotations, heightMap, m_GenHelper, context);
            }

            RecycleGenerationContext(context);
        }

        private void InitializeBlockRotations(Quaternion[,,] rotations)
        {
            int len0 = rotations.GetLength(0);
            int len1 = rotations.GetLength(1);
            int len2 = rotations.GetLength(2);

            for (int i = 0; i < len0; i++)
            {
                for (int j = 0; j < len1; j++)
                {
                    for (int k = 0; k < len2; k++)
                    {
                        rotations[i, j, k] = Quaternion.identity;
                    }
                }
            }
        }

        protected GenerationContext GetGenerationContext()
        {
            if (!m_GenContextPool.TryTake(out GenerationContext context))
            {
                context = new GenerationContext();
            }

            context.Initialize(m_Seed);
            return context;
        }

        protected void RecycleGenerationContext(GenerationContext context)
        {
            m_GenContextPool.Add(context);
        }
    }
}

using Minecraft.Configurations;
using Minecraft.ScriptableWorldGeneration.GenLayers;
using Unity.Collections;
using UnityEngine;
using static Minecraft.WorldConsts;

namespace Minecraft.ScriptableWorldGeneration
{
    [CreateAssetMenu(menuName = "Minecraft/WorldGeneration/TerrainGenerator")]
    public class TerrainGenerator : StatelessGenerator
    {
        public string AirBlock;
        public string StoneBlock;
        public string WaterBlock;
        public string GravelBlock;
        public string BedrockBlock;

        [Space]

        public int SeaLevel = 63;
        public float CoordinateScale = 0.05f; // mc = 684.412F;0.05
        public float HeightScale = 0.05f; // mc = 684.412F;0.05
        public float BiomeDepthOffset;
        public float BiomeDepthWeight = 1f;
        public float BiomeScaleOffset;
        public float BiomeScaleWeight = 1f;
        public float BaseSize = 8.5f;
        public float StretchY = 12.0f;
        public float LowerLimitScale = 512.0f;
        public float UpperLimitScale = 512.0f;
        public int BiomeSize = 4;
        public int RiverSize = 4;
        public Vector3 DepthNoiseScale = new Vector3(200f, 200f, 200f);
        public Vector3 MainNoiseScale = new Vector3(1 / 80f, 1 / 160f, 1 / 80f);


        public override void Generate(IWorld world, ChunkPos pos, BlockData[,,] blocks, Quaternion[,,] rotations, byte[,] heightMap, GenerationHelper helper, GenerationContext context)
        {
            // 生物群系生成
            NativeInt2DArray biomeIds = helper.GenLayers.GetInts(pos.X - 8, pos.Z - 8, 32, 32, Allocator.TempJob);

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    context.Biomes[j, i] = world.BiomeDataTable.GetBiome(biomeIds[(int)(0.861111f * j * 4), (int)(0.861111f * i * 4)]);
                }
            }

            biomeIds.Dispose();

            // 基本地形生成
            GenerateBasicTerrain(world, pos, blocks, helper, context);

            // 获取生物群系
            biomeIds = helper.GenLayers.GetInts(pos.X, pos.Z, 16, 16, Allocator.TempJob);

            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    context.Biomes[j, i] = world.BiomeDataTable.GetBiome(biomeIds[j, i]);
                }
            }

            biomeIds.Dispose();

            // 设置生物群系
            // for (int height = 0; height < 64; ++height)
            // {
            //     for (int i = 0; i < 4; ++i)
            //     {
            //         for (int j = 0; j < 4; ++j)
            //         {
            //             columns[(height * 4 + i) * 4 + j].BiomeConfigData = (int)_biomesForGeneration[j * 4, i * 4].GetBiomeId();
            //         }
            //     }
            // }

            // 添加生物群系特有方块
            ReplaceBiomeBlocks(world, pos, blocks, helper, context);
        }

        private void GenerateBasicTerrain(IWorld world, ChunkPos pos, BlockData[,,] blocks, GenerationHelper helper, GenerationContext context)
        {
            int x = pos.X / 16;
            int z = pos.Z / 16;

            // 产生高度图
            GenerateDensityMap(new Vector3Int(x * 4, 0, z * 4), helper, context);

            BlockData airBlock = world.BlockDataTable.GetBlock(AirBlock);
            BlockData stoneBlock = world.BlockDataTable.GetBlock(StoneBlock);
            BlockData waterBlock = world.BlockDataTable.GetBlock(WaterBlock);

            // 进行线性插值
            for (int xHigh = 0; xHigh < 4; xHigh++)
            {
                for (int zHigh = 0; zHigh < 4; zHigh++)
                {
                    for (int yHigh = 0; yHigh < 32; yHigh++)
                    {
                        float yPart111 = context.DensityMap[xHigh, yHigh, zHigh];
                        float yPart121 = context.DensityMap[xHigh, yHigh, zHigh + 1];
                        float yPart211 = context.DensityMap[xHigh + 1, yHigh, zHigh];
                        float yPart221 = context.DensityMap[xHigh + 1, yHigh, zHigh + 1];
                        float yDensityStep11 = (context.DensityMap[xHigh, yHigh + 1, zHigh] - yPart111) * 0.125f;
                        float yDensityStep12 = (context.DensityMap[xHigh, yHigh + 1, zHigh + 1] - yPart121) * 0.125f;
                        float yDensityStep21 = (context.DensityMap[xHigh + 1, yHigh + 1, zHigh] - yPart211) * 0.125f;
                        float yDensityStep22 = (context.DensityMap[xHigh + 1, yHigh + 1, zHigh + 1] - yPart221) * 0.125f;

                        for (int yLow = 0; yLow < 8; yLow++)
                        {
                            float density111 = yPart111;
                            float density121 = yPart121;
                            float xDensityStep11 = (yPart211 - yPart111) * 0.25f;
                            float xDensityStep21 = (yPart221 - yPart121) * 0.25f;

                            for (int xLow = 0; xLow < 4; xLow++)
                            {
                                float zDensityStep11 = (density121 - density111) * 0.25f;
                                float blockValue = density111 - zDensityStep11;

                                for (int zLow = 0; zLow < 4; zLow++)
                                {
                                    int posX = xHigh * 4 + xLow;
                                    int posY = yHigh * 8 + yLow;
                                    int posZ = zHigh * 4 + zLow;

                                    if ((blockValue += zDensityStep11) > 0)
                                    {
                                        blocks[posX, posY, posZ] = stoneBlock;
                                    }
                                    else if (posY < SeaLevel)
                                    {
                                        blocks[posX, posY, posZ] = waterBlock;
                                    }
                                    else
                                    {
                                        blocks[posX, posY, posZ] = airBlock;
                                    }
                                }

                                density111 += xDensityStep11;
                                density121 += xDensityStep21;
                            }

                            yPart111 += yDensityStep11;
                            yPart121 += yDensityStep12;
                            yPart211 += yDensityStep21;
                            yPart221 += yDensityStep22;
                        }
                    }
                }
            }
        }

        private void GenerateDensityMap(Vector3Int noiseOffset, GenerationHelper helper, GenerationContext context)
        {
            Vector3 depthOffset = new Vector3(noiseOffset.x + 0.1f, 0.0f, noiseOffset.z + 0.1f);
            Vector3 depthScale = new Vector3(DepthNoiseScale.x, 1.0f, DepthNoiseScale.z);
            helper.DepthNoise.Noise(context.DepthMap, depthOffset, depthScale);

            Vector3 noiseScale = new Vector3(CoordinateScale, HeightScale, CoordinateScale);

            // 生成 3 个 5 * 5 * 33 的噪声
            helper.MainNoise.Noise(context.MainNoiseMap, noiseOffset, Vector3.Scale(noiseScale, MainNoiseScale));
            helper.MinNoise.Noise(context.MinLimitMap, noiseOffset, noiseScale);
            helper.MaxNoise.Noise(context.MaxLimitMap, noiseOffset, noiseScale);

            // chunk遍历
            for (int x1 = 0; x1 < 5; x1++)
            {
                for (int z1 = 0; z1 < 5; z1++)
                {
                    float scale = 0;
                    float groundYOffset = 0;
                    float totalWeight = 0;

                    // 中心点生物群系
                    BiomeData centerBiome = context.Biomes[z1 + 2, x1 + 2];

                    // 求 scale 和 groundYOffset 的加权平均值
                    for (int x2 = 0; x2 < 5; x2++)
                    {
                        for (int z2 = 0; z2 < 5; z2++)
                        {
                            BiomeData biome = context.Biomes[z1 + z2, x1 + x2];
                            float curGroundYOffset = BiomeDepthOffset + biome.BaseHeight * BiomeDepthWeight; // biomeDepthOffSet = 0
                            float curScale = BiomeScaleOffset + biome.HeightVariation * BiomeScaleWeight; // biomeScaleOffset = 0

                            // parabolicField 为 10 / √(该点到中心点的距离^2 + 0.2)
                            float weight = helper.BiomeWeights[z2, x2] / (curGroundYOffset + 2.0f);

                            if (biome.BaseHeight > centerBiome.BaseHeight)
                            {
                                weight *= 0.5f;
                            }

                            scale += curScale * weight;
                            groundYOffset += curGroundYOffset * weight;
                            totalWeight += weight;
                        }
                    }

                    scale = scale / totalWeight;
                    groundYOffset = groundYOffset / totalWeight;
                    scale = scale * 0.9f + 0.1f;
                    groundYOffset = (groundYOffset * 4.0f - 1.0f) / 8.0f;

                    // 取一个 -0.36 ~ 0.125 的随机数，这个随机数决定了起伏的地表
                    float random = (context.DepthMap[x1, 0, z1] - 0.5f) * 2 / 8000f;

                    if (random < 0)
                    {
                        random *= -0.3f;
                    }

                    random = random * 3.0f - 2.0f;

                    if (random < 0)
                    {
                        random *= 0.5f;

                        if (random < -1)
                        {
                            random = -1f;
                        }

                        random /= 1.4f;
                        random /= 2.0f;
                    }
                    else
                    {
                        if (random > 1)
                        {
                            random = 1f;
                        }

                        random *= 0.125f;
                    }

                    float groundYOffset1 = groundYOffset;
                    float scale1 = scale;

                    // groundYOffset 有 -0.072 ~ 0.025 的变动量
                    groundYOffset1 = groundYOffset1 + random * 0.2f;
                    groundYOffset1 = groundYOffset1 * BaseSize / 8.0f;

                    // 这个是大概的地面 y 坐标
                    float groundY = BaseSize + groundYOffset1 * 4.0f; // baseSize = 8.5，应该代表了平均地表高度 68

                    // 注意这个 y * 8 才是最终的y坐标
                    for (int y = 0; y < 33; y++)
                    {
                        // result 偏移量，这个是负数则趋向固体，是正数则趋向液体和空气
                        float offset = (y - groundY) * StretchY * 128.0f / 256.0f / scale1; // scale 大概在 0.1 ~ 0.2 这样...

                        if (offset < 0)
                        {
                            offset *= 4f;
                        }

                        // 并不保证 lowerLimit < upperLimit，不过没有影响
                        float lowerLimit = (context.MinLimitMap[x1, y, z1] - 0.5f) * 160000 / LowerLimitScale; // lowerLimitScale = 512
                        float upperLimit = (context.MaxLimitMap[x1, y, z1] - 0.5f) * 160000 / UpperLimitScale; // upperLimitScale = 512
                        float t = ((context.MainNoiseMap[x1, y, z1] - 0.5f) * 160000 / 10.0f + 1.0f) / 2.0f;

                        // 这个函数 t < 0 则取 lowerLimit，t > 1 则取 upperLimit，否则以 t 为参数在上下限间线性插值
                        float result = Mathf.Lerp(lowerLimit, upperLimit, t) - offset;

                        // y = 30 ~ 32
                        if (y > 29)
                        {
                            // 在原 result 和 -10 之间线性插值，这样 y > 240 的方块就会越来越少，最后全变成空气
                            float t2 = (float)(y - 29) / 3f;
                            result = result * (1f - t2) + -10f * t2;
                        }

                        context.DensityMap[x1, y, z1] = (float)result;
                    }
                }
            }
        }

        private void ReplaceBiomeBlocks(IWorld world, ChunkPos pos, BlockData[,,] blocks, GenerationHelper helper, GenerationContext context)
        {
            helper.SurfaceNoise.Noise(context.SurfaceMap, new Vector3(pos.X + 0.1f, 0, pos.Z + 0.1f), new Vector3(0.0625f, 1f, 0.0625f));

            for (int x1 = 0; x1 < 16; x1++)
            {
                for (int z1 = 0; z1 < 16; z1++)
                {
                    GenerateBiomeTerrain(x1, z1, world, blocks, context);
                }
            }
        }

        private void GenerateBiomeTerrain(int columnX, int columnZ, IWorld world, BlockData[,,] blocks, GenerationContext context)
        {
            float noise = (context.SurfaceMap[columnX, 0, columnZ] - 0.5f) * 2;
            BiomeData biome = context.Biomes[columnZ, columnX];

            BlockData topBlock = world.BlockDataTable.GetBlock(biome.TopBlock);
            BlockData fillerBlock = world.BlockDataTable.GetBlock(biome.FillerBlock);
            BlockData airBlock = world.BlockDataTable.GetBlock(AirBlock);
            BlockData stoneBlock = world.BlockDataTable.GetBlock(StoneBlock);
            BlockData gravelBlock = world.BlockDataTable.GetBlock(GravelBlock);
            BlockData bedrockBlock = world.BlockDataTable.GetBlock(BedrockBlock);

            BlockData currentTopBlock = topBlock;
            BlockData currentFillerBlock = fillerBlock;

            int surfaceFlag = -1;
            int surfaceDepth = (int)(noise / 3.0f + 3.0f + (float)context.Rand.NextDouble() * 0.25f);

            for (int y = ChunkHeight - 1; y >= 0; y--)
            {
                if (y <= context.Rand.Next(5))
                {
                    blocks[columnX, y, columnZ] = bedrockBlock;
                }
                else
                {
                    BlockData block = blocks[columnX, y, columnZ];

                    if (block.InternalName == AirBlock)
                    {
                        surfaceFlag = -1;
                    }
                    else if (block.InternalName == StoneBlock)
                    {
                        if (surfaceFlag == -1)
                        {
                            if (surfaceDepth <= 0)
                            {
                                currentTopBlock = airBlock;
                                currentFillerBlock = stoneBlock;
                            }
                            else if (y >= SeaLevel - 4 && y <= SeaLevel + 1)
                            {
                                currentTopBlock = topBlock;
                                currentFillerBlock = fillerBlock;
                            }

                            surfaceFlag = surfaceDepth;

                            if (y >= SeaLevel - 1)
                            {
                                blocks[columnX, y, columnZ] = currentTopBlock;
                            }
                            else if (y < SeaLevel - 7 - surfaceDepth)
                            {
                                currentTopBlock = airBlock;
                                currentFillerBlock = stoneBlock;
                                blocks[columnX, y, columnZ] = gravelBlock;
                            }
                            else
                            {
                                blocks[columnX, y, columnZ] = currentFillerBlock;
                            }
                        }
                        else if (surfaceFlag > 0)
                        {
                            --surfaceFlag;
                            blocks[columnX, y, columnZ] = currentFillerBlock;
                        }
                    }
                }
            }
        }

        private int GetDensityMapIndex(int x, int y, int z)
        {
            return (x * 5 + z) * 33 + y;
        }

        private double GetDensityMapValue(double[] densityMap, int x, int y, int z)
        {
            return densityMap[(x * 5 + z) * 33 + y];
        }
    }
}
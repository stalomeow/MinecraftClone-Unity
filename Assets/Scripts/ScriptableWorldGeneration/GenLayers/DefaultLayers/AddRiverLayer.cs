using System;
using Minecraft.Configurations;
using Unity.Collections;

namespace Minecraft.ScriptableWorldGeneration.GenLayers
{
    public class AddRiverLayer : StatelessGenLayer
    {
        public AddRiverLayer(int seed, StatelessGenLayer parent) : base(seed, parent) { }

        public override NativeInt2DArray GetInts(int areaX, int areaY, int areaWidth, int areaHeight, Allocator allocator)
        {
            int parentAreaX = areaX - 1;
            int parentAreaY = areaY - 1;
            int parentWidth = areaWidth + 2;
            int parentHeight = areaHeight + 2;

            NativeInt2DArray parentRes = m_Parent.GetInts(parentAreaX, parentAreaY, parentWidth, parentHeight, allocator);
            NativeInt2DArray result = new NativeInt2DArray(areaHeight, areaWidth, allocator);

            for (int y = 0; y < areaHeight; ++y)
            {
                for (int x = 0; x < areaWidth; ++x)
                {
                    int randomValueY1 = parentRes[y + 1, x];
                    int randomValueX2Y1 = parentRes[y + 1, x + 2];
                    int randomValueX1 = parentRes[y, x + 1];
                    int randomValueX1Y2 = parentRes[y + 2, x + 1];
                    int randomValueX1Y1 = parentRes[y + 1, x + 1];

                    int tempX, tempY;

                    if (x + areaX >= 0)
                    {
                        tempX = (x + areaX) / 64;
                    }
                    else
                    {
                        tempX = (x + areaX) / 64 - 1;
                    }
                    if (y + areaY >= 0)
                    {
                        tempY = (y + areaY) / 64;
                    }
                    else
                    {
                        tempY = (y + areaY) / 64 - 1;
                    }

                    int seed = GetChunkSeed(tempX, tempY);
                    Random rand = new Random(seed);

                    if (randomValueX1Y1 == randomValueY1 && randomValueX1Y1 == randomValueX1 && randomValueX1Y1 == randomValueX2Y1 && randomValueX1Y1 == randomValueX1Y2)
                    {
                        result[y, x] = randomValueX1Y1;
                    }
                    else if (rand.Next(5) == 0)
                    {
                        if (randomValueX1Y1 != (int)BiomeId.Ocean && randomValueX1Y1 != (int)BiomeId.Beach)
                        {
                            result[y, x] = (int)BiomeId.River;
                        }
                    }
                    else
                    {
                        result[y, x] = randomValueX1Y1;
                    }
                }
            }

            parentRes.Dispose();
            return result;
        }
    }
}

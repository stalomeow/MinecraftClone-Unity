using System;
using Unity.Collections;

namespace Minecraft.ScriptableWorldGeneration.GenLayers
{
    public class AddIslandLayer : StatelessGenLayer
    {
        public AddIslandLayer(int seed, StatelessGenLayer parent) : base(seed, parent) { }

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
                    int parentValue = parentRes[y, x];
                    int parentValueX2 = parentRes[y, x + 2];
                    int parentValueY2 = parentRes[y + 2, x];
                    int parentValueX2Y2 = parentRes[y + 2, x + 2];
                    int parentValueX1Y1 = parentRes[y + 1, x + 1];
                    int randomSeed = GetChunkSeed(x + areaX, y + areaY);

                    Random random = new Random(randomSeed);

                    if (parentValueX1Y1 != 0 || (parentValue == 0 && parentValueX2 == 0 && parentValueY2 == 0 && parentValueX2Y2 == 0))
                    {
                        if (parentValueX1Y1 > 0 && (parentValue == 0 || parentValueX2 == 0 || parentValueY2 == 0 || parentValueX2Y2 == 0))
                        {
                            if (random.Next(5) == 0)
                            {
                                if (parentValueX1Y1 == 4)
                                {
                                    result[y, x] = 4;
                                }
                                else
                                {
                                    result[y, x] = 0;
                                }
                            }
                            else
                            {
                                result[y, x] = parentValueX1Y1;
                            }
                        }
                        else
                        {
                            result[y, x] = parentValueX1Y1;
                        }
                    }
                    else
                    {
                        int deno = 1;
                        int value = 1;

                        if (parentValue != 0 && random.Next(deno++) == 0)
                        {
                            value = parentValue;
                        }

                        if (parentValueX2 != 0 && random.Next(deno++) == 0)
                        {
                            value = parentValueX2;
                        }

                        if (parentValueY2 != 0 && random.Next(deno++) == 0)
                        {
                            value = parentValueY2;
                        }

                        if (parentValueX2Y2 != 0 && random.Next(deno++) == 0)
                        {
                            value = parentValueX2Y2;
                        }

                        if (random.Next(3) == 0)
                        {
                            result[y, x] = value;
                        }
                        else if (value == 4)
                        {
                            result[y, x] = 4;
                        }
                        else
                        {
                            result[y, x] = 0;
                        }
                    }
                }
            }

            parentRes.Dispose();
            return result;
        }
    }
}

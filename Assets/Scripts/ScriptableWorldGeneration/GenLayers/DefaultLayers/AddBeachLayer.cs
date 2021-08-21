using Minecraft.Configurations;
using Unity.Collections;

namespace Minecraft.ScriptableWorldGeneration.GenLayers
{
    public class AddBeachLayer : StatelessGenLayer
    {
        public AddBeachLayer(int seed, StatelessGenLayer parent) : base(seed, parent) { }

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

                    if (parentValueX1Y1 != 0 && (parentValue == 0 || parentValueX2 == 0 || parentValueY2 == 0 || parentValueX2Y2 == 0))
                    {
                        if (parentValueX1Y1 != (int)BiomeId.Mountains)
                        {
                            result[y, x] = (int)BiomeId.Beach;
                        }
                    }
                    else
                    {
                        result[y, x] = parentValueX1Y1;
                    }
                }
            }

            parentRes.Dispose();
            return result;
        }
    }
}

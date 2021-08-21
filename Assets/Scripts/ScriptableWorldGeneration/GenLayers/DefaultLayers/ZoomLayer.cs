using Unity.Collections;
using UnityEngine;

namespace Minecraft.ScriptableWorldGeneration.GenLayers
{
    public class ZoomLayer : StatelessGenLayer
    {
        public ZoomLayer(int seed, StatelessGenLayer parent) : base(seed, parent) { }

        public unsafe override NativeInt2DArray GetInts(int areaX, int areaY, int areaWidth, int areaHeight, Allocator allocator)
        {
            int parentAreaX = areaX > 0 ? areaX / 2 : (areaX - 1) / 2;
            int parentAreaY = areaY > 0 ? areaY / 2 : (areaY - 1) / 2;

            int parentWidth = areaWidth / 2 + 2;
            int parentHeight = areaHeight / 2 + 2;

            NativeInt2DArray parentRes = m_Parent.GetInts(parentAreaX, parentAreaY, parentWidth, parentHeight, allocator);
            int tempWidth = (parentWidth - 1) * 2;
            int tempHeight = (parentHeight - 1) * 2;

            NativeInt2DArray temp = new NativeInt2DArray(tempHeight, tempWidth, allocator);

            for (int parentY = 0; parentY < parentHeight - 1; ++parentY)
            {
                int parentValue = parentRes[parentY, 0];
                int parentValueY1 = parentRes[parentY + 1, 0];

                for (int parentX = 0; parentX < parentWidth - 1; ++parentX)
                {
                    int randomSeed = GetChunkSeed((parentX + parentAreaX) * 2, (parentY + parentAreaY) * 2);

                    int parentValueX1 = parentRes[parentY, parentX + 1];
                    int parentValueX1Y1 = parentRes[parentY + 1, parentX + 1];

                    temp[parentY * 2, parentX * 2] = parentValue;

                    int* array = stackalloc int[2] { parentValue, parentValueY1 };
                    temp[parentY * 2 + 1, parentX * 2] = SelectRandom(randomSeed, array, 2);

                    array[1] = parentValueX1;
                    temp[parentY * 2, parentX * 2 + 1] = SelectRandom(randomSeed, array, 2);

                    temp[parentY * 2 + 1, parentX * 2 + 1] = SelectModeOrRandom(randomSeed, parentValue, parentValueX1, parentValueY1, parentValueX1Y1);

                    parentValue = parentValueX1;
                    parentValueY1 = parentValueX1Y1;
                }
            }

            NativeInt2DArray result = new NativeInt2DArray(areaHeight, areaWidth, allocator);
            int areaOffsetX = Mathf.Abs(areaX % 2);
            int areaOffsetY = Mathf.Abs(areaY % 2);

            for (int resultY = 0; resultY < areaHeight; ++resultY)
            {
                for (int resultX = 0; resultX < areaWidth; ++resultX)
                {
                    result[resultY, resultX] = temp[resultY + areaOffsetY, resultX + areaOffsetX];
                }
            }

            parentRes.Dispose();
            temp.Dispose();
            return result;
        }

        public static StatelessGenLayer Magnify(int seed, StatelessGenLayer layer, int times)
        {
            for (int i = 0; i < times; i++)
            {
                layer = new ZoomLayer(seed + 1, layer);
            }

            return layer;
        }
    }
}

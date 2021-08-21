using Minecraft.Lua;
using Unity.Collections;
using Random = System.Random;

namespace Minecraft.ScriptableWorldGeneration.GenLayers
{
    public abstract class StatelessGenLayer : ILuaCallCSharp
    {
        protected readonly int m_Seed;
        protected readonly StatelessGenLayer m_Parent;

        protected StatelessGenLayer(int seed, StatelessGenLayer parent)
        {
            m_Seed = seed;
            m_Parent = parent;
        }

        public abstract NativeInt2DArray GetInts(int areaX, int areaY, int areaWidth, int areaHeight, Allocator allocator);

        public static unsafe int SelectRandom(int seed, int* array, int count)
        {
            return array[new Random(seed).Next(count)];
        }

        public static unsafe int SelectModeOrRandom(int seed, int a, int b, int c, int d)
        {
            if (a == b || a == c || a == d)
            {
                return a;
            }
            else if (b == c || b == d)
            {
                return b;
            }
            else if (c == d)
            {
                return c;
            }
            else
            {
                int* array = stackalloc int[4] { a, b, c, d };
                return SelectRandom(seed, array, 4);
            }
        }

        public static int GetChunkSeed(int x, int z)
        {
            return z * 16384 + x;
        }
    }
}
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Minecraft.Noises
{
    public sealed class PerlinNoise : INoise
    {
        private static readonly int[] s_Perm =
        {
            151, 160, 137, 91, 90, 15, 131, 13, 201 ,95, 96, 53, 194, 233, 7, 225, 140, 36,
            103, 30, 69, 142, 8, 99, 37, 240, 21, 10, 23, 190, 6, 148, 247, 120, 234, 75, 0,
            26, 197, 62, 94, 252, 219, 203, 117, 35, 11, 32, 57, 177, 33, 88, 237, 149, 56,
            87, 174, 20, 125, 136, 171, 168,  68, 175, 74, 165, 71, 134, 139, 48, 27, 166, 
            77, 146, 158, 231, 83, 111, 229, 122, 60, 211, 133, 230, 220, 105, 92, 41, 55,
            46, 245, 40, 244, 102, 143, 54,  65, 25, 63, 161,  1, 216, 80, 73, 209, 76, 132,
            187, 208,  89, 18, 169, 200, 196, 135, 130, 116, 188, 159, 86, 164, 100, 109, 198,
            173, 186,  3, 64, 52, 217, 226, 250, 124, 123, 5, 202, 38, 147, 118, 126, 255, 82,
            85, 212, 207, 206, 59, 227, 47, 16, 58, 17, 182, 189, 28, 42, 223, 183, 170, 213,
            119, 248, 152, 2, 44, 154, 163, 70, 221, 153, 101, 155, 167, 43, 172, 9, 129, 22,
            39, 253,  19, 98, 108, 110, 79, 113, 224, 232, 178, 185, 112, 104, 218, 246, 97,
            228, 251, 34, 242, 193, 238, 210, 144, 12, 191, 179, 162, 241, 81, 51, 145, 235,
            249, 14, 239, 107, 49, 192, 214,  31, 181, 199, 106, 157, 184, 84, 204, 176, 115,
            121, 50, 45, 127, 4, 150, 254, 138, 236, 205, 93, 222, 114, 67, 29, 24, 72, 243,
            141, 128, 195, 78, 66, 215, 61, 156, 180, 151
        };

        public PerlinNoise() { }

        public float Get(float x, float y, int octave, float persistence)
        {
            float total = 0;
            float frequency = 1;
            float amplitude = 1;
            //用于将结果归一化

            float maxValue = 0;

            for (int i = 0; i < octave; i++)
            {
                total += amplitude * Perlin(x * frequency, y * frequency);
                maxValue += amplitude;
                frequency *= 2;
                amplitude *= persistence;
            }

            return total / maxValue;
        }

        public float Get(float x, float y, float z, int octave, float persistence)
        {
            float total = 0;
            float frequency = 1;
            float amplitude = 1;
            //用于将结果归一化

            float maxValue = 0;

            for (int i = 0; i < octave; i++)
            {
                total += amplitude * Perlin(x * frequency, y * frequency, z * frequency);
                maxValue += amplitude;
                frequency *= 2;
                amplitude *= persistence;
            }

            return total / maxValue;
        }

        private float Perlin(float x, float y)
        {
            float fx = Mathf.Floor(x);
            float fy = Mathf.Floor(y);

            int flooredX = (int)fx & 0xff;
            int flooredY = (int)fy & 0xff;

            //立方体中的位置(0,1)。
            x -= fx;
            y -= fy;

            //hash
            int a = s_Perm[flooredX] & 0xff;
            int b = s_Perm[flooredX + 1] & 0xff;
            int aa = (s_Perm[a] + flooredY) & 0xff;
            int ba = (s_Perm[b] + flooredY) & 0xff;

            int aaa = s_Perm[aa];
            int baa = s_Perm[ba];
            int aab = s_Perm[aa + 1];
            int bab = s_Perm[ba + 1];

            //fade
            float u = x * x * x * (x * (x * 6 - 15) + 10);
            float v = y * y * y * (y * (y * 6 - 15) + 10);

            float x1, x2;

            x1 = Mathf.Lerp(Grad(aaa, x, y), Grad(baa, x - 1, y), u);
            x2 = Mathf.Lerp(Grad(aab, x, y - 1), Grad(bab, x - 1, y - 1), u);

            //为了方便起见，我们将结果范围设为0~1(理论上之前的min/max是[-1，1])。
            return (Mathf.Lerp(x1, x2, v) + 1) * 0.5f;
        }

        private float Perlin(float x, float y, float z)
        {
            float fx = Mathf.Floor(x);
            float fy = Mathf.Floor(y);
            float fz = Mathf.Floor(z);

            int flooredX = (int)fx & 0xff;
            int flooredY = (int)fy & 0xff;
            int flooredZ = (int)fz & 0xff;

            //立方体中的位置(0,1)。
            x -= fx;
            y -= fy;
            z -= fz;

            //hash
            int a = (s_Perm[flooredX] + flooredY) & 0xff;
            int b = (s_Perm[flooredX + 1] + flooredY) & 0xff;
            int aa = (s_Perm[a] + flooredZ) & 0xff;
            int ba = (s_Perm[b] + flooredZ) & 0xff;
            int ab = (s_Perm[a + 1] + flooredZ) & 0xff;
            int bb = (s_Perm[b + 1] + flooredZ) & 0xff;

            int aaa = s_Perm[aa];
            int baa = s_Perm[ba];
            int aba = s_Perm[ab];
            int bba = s_Perm[bb];
            int aab = s_Perm[aa + 1];
            int bab = s_Perm[ba + 1];
            int abb = s_Perm[ab + 1];
            int bbb = s_Perm[bb + 1];

            //fade
            float u = x * x * x * (x * (x * 6 - 15) + 10);
            float v = y * y * y * (y * (y * 6 - 15) + 10);
            float w = z * z * z * (z * (z * 6 - 15) + 10);

            float x1, x2, y1, y2;

            x1 = Mathf.Lerp(Grad(aaa, x, y, z), Grad(baa, x - 1, y, z), u);
            x2 = Mathf.Lerp(Grad(aba, x, y - 1, z), Grad(bba, x - 1, y - 1, z), u);
            y1 = Mathf.Lerp(x1, x2, v);

            x1 = Mathf.Lerp(Grad(aab, x, y, z - 1), Grad(bab, x - 1, y, z - 1), u);
            x2 = Mathf.Lerp(Grad(abb, x, y - 1, z - 1), Grad(bbb, x - 1, y - 1, z - 1), u);
            y2 = Mathf.Lerp(x1, x2, v);

            //为了方便起见，我们将结果范围设为0~1(理论上之前的min/max是[-1，1])。
            return (Mathf.Lerp(y1, y2, w) + 1) * 0.5f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float Grad(int hash, float x, float y)
        {
            switch (hash & 0x3)
            {
                case 0x0: return x + y;
                case 0x1: return -x + y;
                case 0x2: return x - y;
                case 0x3: return -x - y;
                default: return 0; // never happens
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float Grad(int hash, float x, float y, float z)
        {
            switch (hash & 0xF)
            {
                case 0x0: return x + y;
                case 0x1: return -x + y;
                case 0x2: return x - y;
                case 0x3: return -x - y;
                case 0x4: return x + z;
                case 0x5: return -x + z;
                case 0x6: return x - z;
                case 0x7: return -x - z;
                case 0x8: return y + z;
                case 0x9: return -y + z;
                case 0xA: return y - z;
                case 0xB: return -y - z;
                case 0xC: return y + x;
                case 0xD: return -y + z;
                case 0xE: return y - x;
                case 0xF: return -y - z;
                default: return 0; // never happens
            }
        }
    }
}
using System;
using UnityEngine;

namespace Minecraft.Noises
{
    /// <summary>
    /// Implementation for Improved Perlin Noise (http://mrl.nyu.edu/~perlin/noise/).
    /// </summary>
    public class PerlinNoise : INoise
    {
        /// <summary>
        /// Permutation.
        /// </summary>
        private readonly byte[] m_Permutation = new byte[512];

        /// <summary>
        /// Initializes a new instance of the <see cref="PerlinNoise"/> class.
        /// </summary>
        /// <param name="seed">Seed for generating permutation.</param>
        public PerlinNoise(int seed)
        {
            var random = new UniformRNG((ulong)seed);

            for (int i = 0; i < 256; i++)
            {
                m_Permutation[i + 256] = m_Permutation[i] = (byte)(random.NextUInt32() % 256);
            }
        }

        public float Noise(float x, float y, float z)
        {
            var xCoords = Split(x);
            var yCoords = Split(y);
            var zCoords = Split(z);

            var u = Fade(xCoords.remainder);
            var v = Fade(yCoords.remainder);
            var w = Fade(zCoords.remainder);

            int a = m_Permutation[xCoords.integer];
            int b = m_Permutation[xCoords.integer + 1];
            int aa = m_Permutation[a + yCoords.integer];
            int ab = m_Permutation[a + yCoords.integer + 1];
            int ba = m_Permutation[b + yCoords.integer];
            int bb = m_Permutation[b + yCoords.integer + 1];

            int aaa = m_Permutation[aa + zCoords.integer];
            int aba = m_Permutation[ab + zCoords.integer];
            int aab = m_Permutation[aa + zCoords.integer + 1];
            int abb = m_Permutation[ab + zCoords.integer + 1];
            int baa = m_Permutation[ba + zCoords.integer];
            int bba = m_Permutation[bb + zCoords.integer];
            int bab = m_Permutation[ba + zCoords.integer + 1];
            int bbb = m_Permutation[bb + zCoords.integer + 1];

            var xa = new Vector4(
                Grad(aaa, xCoords.remainder, yCoords.remainder, zCoords.remainder),
                Grad(aba, xCoords.remainder, yCoords.remainder - 1, zCoords.remainder),
                Grad(aab, xCoords.remainder, yCoords.remainder, zCoords.remainder - 1),
                Grad(abb, xCoords.remainder, yCoords.remainder - 1, zCoords.remainder - 1)
            );
            var xb = new Vector4(
                Grad(baa, xCoords.remainder - 1, yCoords.remainder, zCoords.remainder),
                Grad(bba, xCoords.remainder - 1, yCoords.remainder - 1, zCoords.remainder),
                Grad(bab, xCoords.remainder - 1, yCoords.remainder, zCoords.remainder - 1),
                Grad(bbb, xCoords.remainder - 1, yCoords.remainder - 1, zCoords.remainder - 1)
            );
            var xl = Vector4.Lerp(xa, xb, u);
            var ya = new Vector2(xl.x, xl.z);
            var yb = new Vector2(xl.y, xl.w);
            var yl = Vector2.Lerp(ya, yb, v);

            return (Mathf.Lerp(yl.x, yl.y, w) + 1) * 0.5f;
        }

        public void Noise(float[,,] noise, Vector3 offset, Vector3 scale)
        {
            Noise(noise, offset, scale, 1, false);
        }

        public void Noise(float[,,] noise, Vector3 offset, Vector3 scale, float noiseScale, bool add)
        {
            var xLength = noise.GetLength(0);
            var yLength = noise.GetLength(1);
            var zLength = noise.GetLength(2);

            for (int x = 0; x < xLength; x++)
            {
                var xOffset = offset.x + x * scale.x;
                var xCoords = Split(xOffset);
                var u = Fade(xCoords.remainder);

                int a = m_Permutation[xCoords.integer];
                int b = m_Permutation[xCoords.integer + 1];

                for (int y = 0; y < yLength; y++)
                {
                    var yOffset = offset.y + y * scale.y;
                    var yCoords = Split(yOffset);
                    var v = Fade(yCoords.remainder);

                    int aa = m_Permutation[a + yCoords.integer];
                    int ab = m_Permutation[a + yCoords.integer + 1];
                    int ba = m_Permutation[b + yCoords.integer];
                    int bb = m_Permutation[b + yCoords.integer + 1];

                    for (int z = 0; z < zLength; z++)
                    {
                        var zOffset = offset.z + z * scale.z;
                        var zCoords = Split(zOffset);
                        var w = Fade(zCoords.remainder);

                        int aaa = m_Permutation[aa + zCoords.integer];
                        int aba = m_Permutation[ab + zCoords.integer];
                        int aab = m_Permutation[aa + zCoords.integer + 1];
                        int abb = m_Permutation[ab + zCoords.integer + 1];
                        int baa = m_Permutation[ba + zCoords.integer];
                        int bba = m_Permutation[bb + zCoords.integer];
                        int bab = m_Permutation[ba + zCoords.integer + 1];
                        int bbb = m_Permutation[bb + zCoords.integer + 1];

                        var xa = new Vector4(
                            Grad(aaa, xCoords.remainder, yCoords.remainder, zCoords.remainder),
                            Grad(aba, xCoords.remainder, yCoords.remainder - 1, zCoords.remainder),
                            Grad(aab, xCoords.remainder, yCoords.remainder, zCoords.remainder - 1),
                            Grad(abb, xCoords.remainder, yCoords.remainder - 1, zCoords.remainder - 1)
                        );
                        var xb = new Vector4(
                            Grad(baa, xCoords.remainder - 1, yCoords.remainder, zCoords.remainder),
                            Grad(bba, xCoords.remainder - 1, yCoords.remainder - 1, zCoords.remainder),
                            Grad(bab, xCoords.remainder - 1, yCoords.remainder, zCoords.remainder - 1),
                            Grad(bbb, xCoords.remainder - 1, yCoords.remainder - 1, zCoords.remainder - 1)
                        );
                        var xl = Vector4.Lerp(xa, xb, u);
                        var ya = new Vector2(xl.x, xl.z);
                        var yb = new Vector2(xl.y, xl.w);
                        var yl = Vector2.Lerp(ya, yb, v);
                        var value = (Mathf.Lerp(yl.x, yl.y, w) + 1) * 0.5f * noiseScale;

                        if (add)
                        {
                            noise[x, y, z] += value;
                        }
                        else
                        {
                            noise[x, y, z] = value;
                        }
                    }
                }
            }
        }

        private static (int integer, float remainder) Split(float value)
        {
            value = value % 256;

            if (value < 0)
            {
                value += 256;
            }

            var integer = (int)value;
            var remainder = value - integer;
            return (integer, remainder);
        }

        private static float Fade(float t)
        {
            // 6t^5 - 15t^4 + 10t^3
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        private static float Grad(int hash, float x, float y, float z)
        {
            // Source: http://riven8192.blogspot.com/2010/08/calculate-perlinnoise-twice-as-fast.html

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
                default: throw new ArgumentOutOfRangeException(nameof(hash));
            }
        }
    }
}
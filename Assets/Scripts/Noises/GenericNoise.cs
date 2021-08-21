using UnityEngine;

namespace Minecraft.Noises
{
    public class GenericNoise<T> : INoise where T : INoise
    {
        private readonly T m_Noise;
        private readonly int m_Octaves;
        private readonly float m_Persistence;

        public GenericNoise(T noise, int octaves, float persistence)
        {
            m_Noise = noise;
            m_Octaves = octaves;
            m_Persistence = persistence;
        }

        public float Noise(float x, float y, float z)
        {
            float total = 0;
            float maxValue = 0;
            float amplitude = 1;
            int frequency = 1;

            for (int i = 0; i < m_Octaves; i++)
            {
                total += amplitude * m_Noise.Noise(x * frequency, y * frequency, z * frequency);
                maxValue += amplitude;
                amplitude *= m_Persistence;
                frequency *= 2;
            }

            return total / maxValue;
        }

        public void Noise(float[,,] noise, Vector3 offset, Vector3 scale)
        {
            float maxValue = (m_Persistence == 1) ? m_Octaves : ((1 - Mathf.Pow(m_Persistence, m_Octaves)) / (1 - m_Persistence));
            float amplitude = 1;
            int frequency = 1;

            for (int i = 0; i < m_Octaves; i++)
            {
                m_Noise.Noise(noise, Vector3.Scale(offset, scale) * frequency, scale * frequency, amplitude / maxValue, i != 0);
                amplitude *= m_Persistence;
                frequency *= 2;
            }
        }

        void INoise.Noise(float[,,] noise, Vector3 offset, Vector3 scale, float noiseScale, bool add)
        {
            throw new System.NotImplementedException();
        }
    }
}

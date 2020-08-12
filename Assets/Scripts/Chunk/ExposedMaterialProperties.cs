using System;
using UnityEngine;

namespace Minecraft
{
    public readonly struct ExposedMaterialProperties
    {
        private readonly MaterialPropertyBlock m_ChunkProperties;
        private readonly MaterialPropertyBlock m_LiquidProperties;

        public ExposedMaterialProperties(MaterialPropertyBlock chunkPropertyBlock, MaterialPropertyBlock liquidPropertyBlock)
        {
            m_ChunkProperties = chunkPropertyBlock ?? throw new ArgumentNullException(nameof(chunkPropertyBlock));
            m_LiquidProperties = liquidPropertyBlock ?? throw new ArgumentNullException(nameof(liquidPropertyBlock));
        }

        public void SetRenderRadius(int value)
        {
            m_ChunkProperties.SetInt("_RenderRadius", value);
            m_LiquidProperties.SetInt("_RenderRadius", value);
        }

        public void SetAmbientColor(Color value)
        {
            m_ChunkProperties.SetColor("_AmbientColor", value);
            m_LiquidProperties.SetColor("_AmbientColor", value);
        }

        public MaterialPropertyBlock GetChunkPropertyBlock()
        {
            return m_ChunkProperties;
        }

        public MaterialPropertyBlock GetLiquidPropertyBlock()
        {
            return m_LiquidProperties;
        }
    }
}
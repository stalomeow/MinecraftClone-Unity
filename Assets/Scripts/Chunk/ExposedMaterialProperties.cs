using System;
using UnityEngine;

namespace Minecraft
{
    public readonly struct ExposedMaterialProperties
    {
        private readonly MaterialPropertyBlock m_SolidProperties;
        private readonly MaterialPropertyBlock m_LiquidProperties;

        public ExposedMaterialProperties(MaterialPropertyBlock solidPropertyBlock, MaterialPropertyBlock liquidPropertyBlock)
        {
            m_SolidProperties = solidPropertyBlock ?? throw new ArgumentNullException(nameof(solidPropertyBlock));
            m_LiquidProperties = liquidPropertyBlock ?? throw new ArgumentNullException(nameof(liquidPropertyBlock));
        }

        public void SetRenderRadius(int value)
        {
            m_SolidProperties.SetInt("_RenderRadius", value);
            m_LiquidProperties.SetInt("_RenderRadius", value);
        }

        public void SetAmbientColor(Color value)
        {
            m_SolidProperties.SetColor("_AmbientColor", value);
            m_LiquidProperties.SetColor("_AmbientColor", value);
        }

        public MaterialPropertyBlock GetSolidPropertyBlock()
        {
            return m_SolidProperties;
        }

        public MaterialPropertyBlock GetLiquidPropertyBlock()
        {
            return m_LiquidProperties;
        }
    }
}
namespace Minecraft.Rendering
{
    public readonly ref struct VertexLightingData
    {
        public float LeftBottom { get; }

        public float RightBottom { get; }

        public float RightTop { get; }

        public float LeftTop { get; }

        public VertexLightingData(byte light)
        {
            float value = LightingUtility.GetLightInShaders(light);

            LeftBottom = value;
            RightBottom = value;
            RightTop = value;
            LeftTop = value;
        }

        public VertexLightingData(byte leftBottom, byte rightBottom, byte rightTop, byte leftTop)
        {
            LeftBottom = LightingUtility.GetLightInShaders(leftBottom);
            RightBottom = LightingUtility.GetLightInShaders(rightBottom);
            RightTop = LightingUtility.GetLightInShaders(rightTop);
            LeftTop = LightingUtility.GetLightInShaders(leftTop);
        }
    }
}
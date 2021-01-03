using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Minecraft.Rendering
{
    public sealed class BlockTextureTable
    {
        private readonly Dictionary<Texture2D, int> m_TextureMap;

        public Texture2DArray TextureArray { get; }

        public int TextureCount => m_TextureMap.Count;

        public int this[Block block, int textureIndex] => GetTextureOffset(block, textureIndex);


        public BlockTextureTable(IEnumerable<Block> blocks)
        {
            Profiler.BeginSample("Build BlockTextureTable");

            HashSet<Texture2D> textures = GetTextures(blocks, out Texture2D configTex);

            int index = 0;
            m_TextureMap = new Dictionary<Texture2D, int>(textures.Count); // 防止浪费过多内存
            TextureArray = new Texture2DArray(configTex.width, configTex.height, textures.Count, configTex.format, false)
            {
                anisoLevel = configTex.anisoLevel,
                mipMapBias = configTex.mipMapBias,
                wrapMode = configTex.wrapMode,
                filterMode = configTex.filterMode
            };

            foreach (Texture2D texture in textures)
            {
                m_TextureMap.Add(texture, index);
                Graphics.CopyTexture(texture, 0, 0, TextureArray, index, 0);
                index++;
            }

            Profiler.EndSample();
        }

        public int GetTextureOffset(Block block, int textureIndex)
        {
            Texture2D texture = block.GetTexture(textureIndex);
            return m_TextureMap[texture];
        }

        public static BlockTextureTable BuildTable(params Block[] blocks)
        {
            return new BlockTextureTable(blocks);
        }

        private static HashSet<Texture2D> GetTextures(IEnumerable<Block> blocks, out Texture2D configTex)
        {
            HashSet<Texture2D> textures = new HashSet<Texture2D>();
            configTex = null;

            foreach (Block block in blocks)
            {
                foreach (Texture2D texture in block.Textures)
                {
                    configTex = texture;
                    textures.Add(texture);
                }
            }

            return textures;
        }
    }
}
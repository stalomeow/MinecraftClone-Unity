using System;
using UnityEngine;

namespace Minecraft
{
    [Serializable]
    public class WorldSettings
    {
        public string Name; // 别改

        public WorldType Type; // 别改
        public PlayMode Mode;

        public int Seed; // 别改

        public int RenderChunkRadius; // 别在加载完的世界里改
        public float HorizontalFOVInDEG; // 水平视角大小（角度制），别在加载完的世界里改

        public int MaxChunkCountInMemory;

        public bool EnableDestroyEffect;

        public Vector3 Position;

        public string ResourcePackageName;

        public int RenderRadius => RenderChunkRadius * WorldConsts.ChunkWidth;

        public Color DefaultAmbientColor => new Color(0.3632075f, 0.6424405f, 1f, 1f);

        public static WorldSettings Active = null;
    }
}
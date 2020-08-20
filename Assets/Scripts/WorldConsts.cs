namespace Minecraft
{
    public static class WorldConsts
    {
        public const int WorldHeight = 256;
        public const int BaseHeight = 60;

        public const int ChunkWidth = 16;
        public const int SectionHeight = 16;

        public const float OverChunkWidth = 1f / ChunkWidth;
        public const float OverSectionHeight = 1f / SectionHeight;

        public const int SectionCountInChunk = WorldHeight / SectionHeight;
        public const int BlockCountInChunk = ChunkWidth * WorldHeight * ChunkWidth;

        public const int MaxTickBlockCountPerFrame = 500;

        public const int MaxLightBlockCountPerFrame = 500;

        public const int ChunkFileSize = 4 + 4 + BlockCountInChunk;

        public const int SqrMinChunkDistance = ChunkWidth * ChunkWidth * 9; // 很近的距离，如果chunk距离比这个值小，则必须被渲染

        public const int BlockLayer = 8;
        public const int PlayerLayer = 9;

        public const byte MaxLight = 15;
        public const byte SkyLightSubtracted = 2; // temp
        public const float OverMaxLight = 1f / MaxLight;
        public const byte SkyLight = MaxLight;

        /// <summary>
        /// 方块光照传播时的最小阻挡值
        /// </summary>
        public const int MinBlockLightOpacity = 1;

        /// <summary>
        /// 非空气方块受到的最大的天空光照值
        /// </summary>
        /// 
        /// <remarks>
        /// 方块光照最高也只能到14级（固体方块光源的发光等级是15，但仅仅是光源本身所在位置是这个等级）
        /// https://minecraft-zh.gamepedia.com/%E4%BA%AE%E5%BA%A6
        /// </remarks>
        public const int MaxNonAirBlockSkyLightValue = MaxLight - 1;

        public const string ResourcePackagesFolderName = "resource_packs";

        public const string ResourcePackageIconName = "icon.png";

        public const string DefaultResourcePackageName = "default";
    }
}
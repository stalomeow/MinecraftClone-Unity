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

        

        public const string ResourcePackagesFolderName = "resource_packs";

        public const string ResourcePackageIconName = "icon.png";

        public const string DefaultResourcePackageName = "default";
    }
}
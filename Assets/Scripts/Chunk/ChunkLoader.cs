using System.IO;
using static Minecraft.WorldConsts;

namespace Minecraft
{
    public sealed class ChunkLoader
    {
        public int Seed { get; }

        public string ChunkSavingDirectory { get; }

        public ChunkLoader(int seed, string chunkSavingDirectory)
        {
            Seed = seed;
            ChunkSavingDirectory = chunkSavingDirectory;

            Directory.CreateDirectory(chunkSavingDirectory);//不存在，则创建
        }

        public Chunk LoadChunk(Chunk chunk, int x, int z)
        {
            string filePath = GetChunkFilePath(x, z);

            if (File.Exists(filePath))
            {
                ReadChunkFromFile(chunk, filePath);
            }
            else
            {
                chunk.Init(x, z, Seed);
            }

            return chunk;
        }

        public void SaveChunk(Chunk chunk, bool checkModified = true)
        {
            if (!chunk.IsModified && checkModified)
            {
                return;
            }

            string path = GetChunkFilePath(chunk.PositionX, chunk.PositionZ);
            WriteChunkToFile(chunk, path);
            chunk.OnSaved();
        }

        private string GetChunkFilePath(int x, int z)
        {
            return $"{ChunkSavingDirectory}/{Chunk.GetUniqueIdByPosition(x, z).ToString()}";
        }

        private void WriteChunkToFile(Chunk chunk, string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                fs.WriteByte((byte)chunk.PositionX);
                fs.WriteByte((byte)(chunk.PositionX >> 8));
                fs.WriteByte((byte)(chunk.PositionX >> 16));
                fs.WriteByte((byte)(chunk.PositionX >> 24));

                fs.WriteByte((byte)chunk.PositionZ);
                fs.WriteByte((byte)(chunk.PositionZ >> 8));
                fs.WriteByte((byte)(chunk.PositionZ >> 16));
                fs.WriteByte((byte)(chunk.PositionZ >> 24));

                fs.Write(chunk.GetRawBlockData(), 0, BlockCountInChunk);
            }
        }

        private void ReadChunkFromFile(Chunk chunk, string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                int x_b0 = fs.ReadByte();
                int x_b1 = fs.ReadByte();
                int x_b2 = fs.ReadByte();
                int x_b3 = fs.ReadByte();

                int posX = x_b0 | (x_b1 << 8) | (x_b2 << 16) | (x_b3 << 24);

                int z_b0 = fs.ReadByte();
                int z_b1 = fs.ReadByte();
                int z_b2 = fs.ReadByte();
                int z_b3 = fs.ReadByte();

                int posZ = z_b0 | (z_b1 << 8) | (z_b2 << 16) | (z_b3 << 24);

                byte[] blocks = new byte[BlockCountInChunk];

                int count = 0;

                do
                {
                    count += fs.Read(blocks, count, BlockCountInChunk);

                } while (count < BlockCountInChunk);

                chunk.Init(posX, posZ, blocks);
            }
        }
    }
}
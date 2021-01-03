using System.IO;

namespace Minecraft.Serialization
{
    public interface IBinarySerializable
    {
        void Serialize(Stream stream);

        void Deserialize(World world, Stream stream);
    }
}
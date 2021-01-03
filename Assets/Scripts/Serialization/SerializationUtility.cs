using System.IO;
using System.Runtime.CompilerServices;

namespace Minecraft.Serialization
{
    public static class SerializationUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteObject(this Stream stream, IBinarySerializable obj)
        {
            obj.Serialize(stream);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadObject(this Stream stream, World world, IBinarySerializable obj)
        {
            obj.Deserialize(world, stream);
        }
    }
}
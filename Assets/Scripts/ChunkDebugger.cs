using UnityEngine;

namespace Minecraft
{
    [DisallowMultipleComponent]
    public class ChunkDebugger : MonoBehaviour
    {
        public void OnChunkLoaded(Chunk chunk)
        {
            print("Load" + chunk.Position);
        }

        public void OnChunkUnloaded(ChunkPos pos)
        {
            print("Unload " + pos);
        }
    }
}

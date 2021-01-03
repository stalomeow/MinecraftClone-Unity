using UnityEngine;

namespace Minecraft
{
    public abstract class BlockLogics : ScriptableObject
    {
        public abstract void Tick(int x, int y, int z, Block block);

        public abstract void RandomTick(int x, int y, int z, Block block);

        public abstract void Destroy(int x, int y, int z, Block block);

        public abstract void Place(int x, int y, int z, BlockDirection direction, Block block);

        public abstract void Click(int x, int y, int z, BlockDirection direction, Block block);

        public abstract float GetHarvestTime(int x, int y, int z, Block block);


        protected BlockLogics() { }
    }
}
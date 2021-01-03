using UnityEngine;

namespace Minecraft
{
    [CreateAssetMenu(menuName = "Minecraft/Block Logics/Default", fileName = "Default")]
    public class DefaultBlockLogics : BlockLogics
    {
        public override void Click(int x, int y, int z, BlockDirection direction, Block block) { }

        public override void Destroy(int x, int y, int z, Block block) { }

        public override float GetHarvestTime(int x, int y, int z, Block block)
        {
            return block.Hardness / 10f;
        }

        public override void Place(int x, int y, int z, BlockDirection direction, Block block) { }

        public override void RandomTick(int x, int y, int z, Block block) { }

        public override void Tick(int x, int y, int z, Block block) { }
    }
}
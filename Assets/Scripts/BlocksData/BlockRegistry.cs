using UnityEngine;

namespace Minecraft.BlocksData
{
    [CreateAssetMenu(menuName = "Minecraft/BlockRegistry", fileName = "_Registry")]
    public sealed class BlockRegistry : ScriptableObject
    {
        public Block[] RegisteredBlocks;
    }
}
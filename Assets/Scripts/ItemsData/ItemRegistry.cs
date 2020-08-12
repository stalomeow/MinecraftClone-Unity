using UnityEngine;

namespace Minecraft.ItemsData
{
    [CreateAssetMenu(menuName = "Minecraft/ItemRegistry", fileName = "_Registry")]
    public sealed class ItemRegistry : ScriptableObject
    {
        public Item[] RegisteredItems;
    }
}
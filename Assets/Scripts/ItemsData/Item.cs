using Minecraft.BlocksData;
using UnityEngine;

#pragma warning disable CS0649

namespace Minecraft.ItemsData
{
    [CreateAssetMenu(menuName = "Minecraft/Item")]
    public sealed class Item : ScriptableObject
    {
        [SerializeField] private string m_ItemName;
        [SerializeField] private ItemType m_Type;
        [SerializeField] private BlockType m_MappedBlock;
        [SerializeField] [Range(1, 64)] private int m_MaxStackSize = 64;
        [SerializeField] [Range(0, 60)] private float m_DigSpeed = 3;
        [SerializeField] private Sprite m_Icon;


        public string ItemName => m_ItemName;

        public ItemType Type => m_Type;

        public BlockType MappedBlockType => m_MappedBlock;

        public int MaxStackSize => m_MaxStackSize;

        public float DiggingSpeed => m_DigSpeed;

        public Sprite Icon => m_Icon;
    }
}
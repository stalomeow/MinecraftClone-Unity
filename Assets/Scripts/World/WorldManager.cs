using Minecraft.BlocksData;
using Minecraft.ItemsData;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#pragma warning disable CS0649

namespace Minecraft
{
    public sealed class WorldManager : MonoBehaviour
    {
        private sealed class BlockTypeComparer : IEqualityComparer<BlockType>
        {
            public bool Equals(BlockType x, BlockType y)
            {
                return x == y;
            }

            public int GetHashCode(BlockType obj)
            {
                return (int)obj;
            }
        }

        private sealed class ItemTypeComparer : IEqualityComparer<ItemType>
        {
            public bool Equals(ItemType x, ItemType y)
            {
                return x == y;
            }

            public int GetHashCode(ItemType obj)
            {
                return (int)obj;
            }
        }


        public static WorldManager Active { get; private set; }


        [SerializeField] private Material m_BlockEntityMaterial;
        [SerializeField] private Material m_ChunkMaterial;
        [SerializeField] private Material m_LiquidMaterial;
        [SerializeField] private PlayerEntity m_Player;
        [SerializeField] private GameObject m_LoadingMenu;
        [SerializeField] private Camera m_MainCamera;
        [SerializeField] private BlockRegistry m_BlockRegistry;
        [SerializeField] private ItemRegistry m_ItemRegistry;
        [SerializeField] private InventoryManager m_InventoryManager;

        private Transform m_PlayerTransform;
        private Vector3 m_PlayerPositionRecorded;


        private readonly Dictionary<BlockType, Block> m_BlocksMap = new Dictionary<BlockType, Block>(new BlockTypeComparer());

        private readonly Dictionary<ItemType, Item> m_ItemsMap = new Dictionary<ItemType, Item>(new ItemTypeComparer());


        public Camera MainCamera => m_MainCamera;

        public WorldSettings Settings { get; private set; }

        public string WorldSettingsSavingPath { get; private set; }

        public ChunkManager ChunkManager { get; private set; }

        public EntityManager EntityManager { get; private set; }

        public InventoryManager InventoryManager => m_InventoryManager;


        private void Awake()
        {
            Block[] blocks = m_BlockRegistry.RegisteredBlocks;

            for (int i = 0; i < blocks.Length; i++)
            {
                Block block = blocks[i];
                m_BlocksMap.Add(block.Type, block);

                // 添加重力tick

                if (block.HasAnyFlag(BlockFlags.AffectedByGravity))
                {
                    block.OnTickEvent += (x, y, z, b) =>
                    {
                        WorldManager world = Active;

                        if (world.GetBlock(x, y - 1, z).HasAnyFlag(BlockFlags.IgnoreCollisions))
                        {
                            world.SetBlockType(x, y, z, BlockType.Air);
                            GravityBlockEntity e = world.EntityManager.CreateEntity<GravityBlockEntity>();
                            e.Initialize(x, y, z, b);
                        }
                    };
                }
            }

            Item[] items = m_ItemRegistry.RegisteredItems;

            for (int i = 0; i < items.Length; i++)
            {
                Item item = items[i];
                m_ItemsMap.Add(item.Type, item);
            }

            Active = this;
        }

        private void Start()
        {
            if (WorldSettings.Active == null)
                return;

            m_PlayerTransform = m_Player.transform;
            Initialize(WorldSettings.Active);
        }


        private void Update()
        {
            ChunkManager.SyncUpdateOnMainThread();
            m_PlayerPositionRecorded = m_PlayerTransform.localPosition;
        }

        private void FixedUpdate()
        {
            ChunkManager.SyncFixedUpdateOnMainThread();
        }

#if UNITY_EDITOR
        private void OnDestroy()
#else
        private void OnApplicationQuit()
#endif
        {
            Settings.Position = m_PlayerPositionRecorded;

            ChunkManager.Destroy();

            string settingsPath = WorldSettingsSavingPath + "/" + Settings.Name + "/settings.json";
            string json = JsonUtility.ToJson(Settings, false);
            File.WriteAllText(settingsPath, json);

            //ScreenCapture.CaptureScreenshot(WorldSettingsSavingPath + "/" + Settings.Name + "/Thumbnail.png");

            Active = null;
        }

        public void Initialize(WorldSettings settings)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            WorldSettingsSavingPath = Application.persistentDataPath + "/Worlds";

            string chunkSavingDirectory = WorldSettingsSavingPath + $"/{settings.Name}/Chunks"; 
            ChunkManager = new ChunkManager(settings, MainCamera, m_PlayerTransform, chunkSavingDirectory, m_ChunkMaterial, m_LiquidMaterial);
            EntityManager = new EntityManager(m_BlockEntityMaterial, m_Player);

            m_PlayerTransform.position = (settings.Position.y < 0 || settings.Position.y >= WorldConsts.WorldHeight) ? new Vector3(0, WorldConsts.WorldHeight, 0) : settings.Position;

            ChunkManager.OnChunksReadyWhenStartingUp += () =>
            {
                ScreenCapture.CaptureScreenshot(WorldSettingsSavingPath + "/" + Settings.Name + "/Thumbnail.png"); // 没办法

                if (Settings.Position.y < 0 || Settings.Position.y >= WorldConsts.WorldHeight)
                {
                    Chunk chunk = ChunkManager.GetChunkByNormalizedPosition(0, 0);
                    m_PlayerTransform.position = new Vector3(0, chunk.GetHighestNonAirY(0, 0) + 5, 0);
                }

                m_Player.enabled = true;

                Destroy(m_LoadingMenu);
            };

            ChunkManager.StartChunksUpdatingThread();
            ChunkManager.StartChunksBuildingThread();
        }


        public Block GetBlockByType(BlockType type)
        {
            return m_BlocksMap.TryGetValue(type, out Block block) ? block : null;
        }

        public Item GetItemByType(ItemType type)
        {
            return m_ItemsMap.TryGetValue(type, out Item item) ? item : null;
        }

        public ItemType GetCurrentItemType()
        {
            return InventoryManager.CurrentItem;
        }

        public Item GetCurrentItem()
        {
            return GetItemByType(InventoryManager.CurrentItem);
        }

        public void SetItemType(int index, ItemType type)
        {
            InventoryManager.SetItem(index, type);
        }

        public bool SetBlockType(int x, int y, int z, BlockType block, bool lightBlocks = true, bool tickBlocks = true, bool updateNeighborChunks = true)
        {
            Chunk chunk = ChunkManager.GetChunk(x, z);
            return chunk.SetBlockType(x, y, z, block, lightBlocks, tickBlocks, updateNeighborChunks);
        }

        public BlockType GetBlockType(int x, int y, int z)
        {
            Chunk chunk = ChunkManager.GetChunk(x, z);
            return chunk == null ? BlockType.Air : chunk.GetBlockType(x, y, z);
        }

        public Block GetBlock(int x, int y, int z)
        {
            BlockType type = GetBlockType(x, y, z);
            return GetBlockByType(type);
        }

        public byte GetFinalLightLevel(int x, int y, int z)
        {
            Chunk chunk = ChunkManager.GetChunk(x, z);
            return chunk == null ? WorldConsts.MaxLight : chunk.GetFinalLightLevel(x, y, z);
        }

        public byte GetBlockLight(int x, int y, int z)
        {
            Chunk chunk = ChunkManager.GetChunk(x, z);
            return chunk == null ? WorldConsts.MaxLight : chunk.GetBlockLight(x, y, z);
        }

        public void SetBlockLight(int x, int y, int z, byte value)
        {
            Chunk chunk = ChunkManager.GetChunk(x, z);
            chunk?.SetBlockLight(x, y, z, value);
        }

        public bool IsBlockTransparent(int x, int y, int z)
        {
            Block block = GetBlock(x, y, z);
            return block.LightOpacity < WorldConsts.MaxLight && block.LightValue == 0;
        }

        public bool IsBlockTransparentAndNotWater(int x, int y, int z)
        {
            Block block = GetBlock(x, y, z);
            return block.LightOpacity < WorldConsts.MaxLight && block.LightValue == 0 && block.Type != BlockType.Water;
        }
    }
}
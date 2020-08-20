using Minecraft.BlocksData;
using Minecraft.ItemsData;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using XLua;

#pragma warning disable CS0649

namespace Minecraft
{
    [LuaCallCSharp]
    public sealed class WorldManager : MonoBehaviour
    {
        public static WorldManager Active { get; private set; }

        [SerializeField] private PlayerEntity m_Player;
        [SerializeField] private GameObject m_LoadingMenu;
        [SerializeField] private Camera m_MainCamera;
        [SerializeField] private InventoryManager m_InventoryManager;

        private Transform m_PlayerTransform;
        private Vector3 m_PlayerPositionRecorded;


        public bool Initialized { get; private set; }

        public DataManager DataManager { get; private set; }

        public WorldSettings Settings { get; private set; }

        public string WorldSettingsSavingPath { get; private set; }

        public ChunkManager ChunkManager { get; private set; }

        public EntityManager EntityManager { get; private set; }

        public Camera MainCamera => m_MainCamera;

        public InventoryManager InventoryManager => m_InventoryManager;


        private IEnumerator Start()
        {
            WorldSettings settings = WorldSettings.Active;
            DataManager = new DataManager(settings.ResourcePackageName);

            yield return DataManager.InitBlocks();
            yield return DataManager.InitItems();
            yield return DataManager.InitMaterials();

            Initialized = false;
            Active = this;
            m_PlayerTransform = m_Player.transform;

            Initialize(settings);

            yield return DataManager.DoLua();

            DataManager.LuaFullGC();
            GC.Collect();
        }

        private void Update()
        {
            if (!Initialized)
                return;

            ChunkManager.SyncUpdateOnMainThread();
            m_PlayerPositionRecorded = m_PlayerTransform.localPosition;
        }

        private void FixedUpdate()
        {
            if (!Initialized)
                return;

            ChunkManager.SyncFixedUpdateOnMainThread();
        }

#if UNITY_EDITOR
        private void OnDestroy()
#else
        private void OnApplicationQuit()
#endif
        {
            Active = null;
            Settings.Position = m_PlayerPositionRecorded;

            ChunkManager.Dispose();
            DataManager.Dispose();

            string settingsPath = WorldSettingsSavingPath + "/" + Settings.Name + "/settings.json";
            string json = JsonUtility.ToJson(Settings, false);
            File.WriteAllText(settingsPath, json);

            //ScreenCapture.CaptureScreenshot(WorldSettingsSavingPath + "/" + Settings.Name + "/Thumbnail.png");
        }

        private void Initialize(WorldSettings settings)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            WorldSettingsSavingPath = Application.persistentDataPath + "/Worlds";

            string chunkSavingDirectory = WorldSettingsSavingPath + $"/{settings.Name}/Chunks"; 
            ChunkManager = new ChunkManager(settings, MainCamera, m_PlayerTransform, chunkSavingDirectory, DataManager.ChunkMaterial, DataManager.LiquidMaterial);
            EntityManager = new EntityManager(DataManager.BlockEntityMaterial, m_Player);

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

            Initialized = true;
        }


        public ItemType GetCurrentItemType()
        {
            return InventoryManager.CurrentItem;
        }

        public Item GetCurrentItem()
        {
            return DataManager.GetItemByType(InventoryManager.CurrentItem);
        }

        public void SetItemType(int index, ItemType type)
        {
            InventoryManager.SetItem(index, type);
        }

        public bool SetBlockType(int x, int y, int z, BlockType block, byte state = 0, bool lightBlocks = true, bool tickBlocks = true, bool updateNeighborChunks = true)
        {
            Chunk chunk = ChunkManager.GetChunk(x, z);
            return chunk.SetBlockType(x, y, z, block, state, lightBlocks, tickBlocks, updateNeighborChunks);
        }

        public BlockType GetBlockType(int x, int y, int z)
        {
            Chunk chunk = ChunkManager.GetChunk(x, z);
            return chunk == null ? BlockType.Air : chunk.GetBlockType(x, y, z);
        }

        public void SetBlockState(int x, int y, int z, byte value)
        {
            Chunk chunk = ChunkManager.GetChunk(x, z);
            chunk.SetBlockState(x, y, z, value);
        }

        public byte GetBlockState(int x, int y, int z)
        {
            Chunk chunk = ChunkManager.GetChunk(x, z);
            return chunk.GetBlockState(x, y, z);
        }

        public Block GetBlock(int x, int y, int z)
        {
            BlockType type = GetBlockType(x, y, z);
            return DataManager.GetBlockByType(type);
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
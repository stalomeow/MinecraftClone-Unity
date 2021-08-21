using System;
using System.Collections.Generic;
using Minecraft.Collections;
using Minecraft.Lua;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Profiling;

namespace Minecraft
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ChunkBuilder))]
    public class ChunkManager : MonoBehaviour, ILuaCallCSharp
    {
        [Serializable] private class ChunkEvent : UnityEvent<Chunk> { }

        [Serializable] private class ChunkPosEvent : UnityEvent<ChunkPos> { }


        [SerializeField] private int m_MaxChunkCountInMemory = 3000; // 内存中的最大 Chunk 数量
        [SerializeField] private ChunkEvent m_OnChunkLoaded;
        [SerializeField] private ChunkPosEvent m_OnChunkUnloaded;


        private ChunkBuilder m_ChunkBuilder;
        private LRULinkedMap<ChunkPos, Chunk> m_Chunks;
        private Dictionary<ChunkPos, Chunk3x3Accessor> m_Chunk3x3Accessors;
        private Stack<Chunk3x3Accessor> m_Chunk3x3Pool;
        private HashSet<ChunkPos> m_LoadingChunks;
        private Action<Chunk> m_GetBuildedChunkCallback;
        private IWorld m_World;


        public event UnityAction<Chunk> OnChunkLoaded
        {
            add => m_OnChunkLoaded.AddListener(value);
            remove => m_OnChunkLoaded.RemoveListener(value);
        }

        public event UnityAction<ChunkPos> OnChunkUnloaded
        {
            add => m_OnChunkUnloaded.AddListener(value);
            remove => m_OnChunkUnloaded.RemoveListener(value);
        }


        public void Initialize(IWorld world)
        {
            m_ChunkBuilder = GetComponent<ChunkBuilder>();
            m_Chunks = new LRULinkedMap<ChunkPos, Chunk>(m_MaxChunkCountInMemory);
            m_Chunk3x3Accessors = new Dictionary<ChunkPos, Chunk3x3Accessor>();
            m_Chunk3x3Pool = new Stack<Chunk3x3Accessor>();
            m_LoadingChunks = new HashSet<ChunkPos>();
            m_GetBuildedChunkCallback = null;
            m_World = world;

            m_ChunkBuilder.Initialize(world);
            m_Chunks.OnValueRemoved += OnWillUnloadChunk;
        }

        private void OnWillUnloadChunk(ChunkPos key, Chunk value)
        {
            // 卸载所有与该 chunk 有关联的 Chunk3x3Accessor
            for (int x = Chunk3x3Accessor.XOffsetBegin; x <= Chunk3x3Accessor.XOffsetEnd; x++)
            {
                for (int z = Chunk3x3Accessor.ZOffsetBegin; z <= Chunk3x3Accessor.ZOffsetEnd; z++)
                {
                    ChunkPos neighbor = key.AddOffset(x, z);
                    TryRecycleChunk3x3Accessor(neighbor);
                }
            }

            m_OnChunkUnloaded.Invoke(value.Position);
            m_ChunkBuilder.RecycleChunk(value);
        }

        private void Update()
        {
            if (m_World != null)
            {
                m_ChunkBuilder.GetBuildedChunks(m_GetBuildedChunkCallback ??= chunk =>
                {
                    lock (m_Chunks)
                    {
                        m_Chunks.AddOrSet(chunk.Position, chunk);
                    }

                    lock (m_LoadingChunks)
                    {
                        m_LoadingChunks.Remove(chunk.Position);
                    }

                    chunk.PostLightAllBlocks();
                    m_OnChunkLoaded.Invoke(chunk);
                });
            }
        }

        public bool GetChunk(ChunkPos pos, bool load, out Chunk chunk)
        {
            lock (m_Chunks)
            {
                if (m_Chunks.TryGetValue(pos, out chunk))
                {
                    return true;
                }
            }

            if (load)
            {
                LoadChunk(pos);
            }
            return false;
        }

        private void LoadChunk(ChunkPos pos)
        {
            bool shouldLoadChunk;

            lock (m_LoadingChunks)
            {
                shouldLoadChunk = m_LoadingChunks.Add(pos);
            }

            if (shouldLoadChunk)
            {
                m_ChunkBuilder.BuildChunk(pos);
            }
        }

        public bool GetChunk3x3Accessor(ChunkPos pos, bool load, out Chunk3x3Accessor accessor)
        {
            Profiler.BeginSample("ChunkManager.GetChunk3x3Accessor");

            lock (m_Chunk3x3Accessors)
            {
                if (m_Chunk3x3Accessors.TryGetValue(pos, out accessor))
                {
                    return true;
                }
            }

            bool result = load && LoadChunk3x3Accessor(pos, out accessor);
            Profiler.EndSample();
            return result;
        }

        private bool LoadChunk3x3Accessor(ChunkPos pos, out Chunk3x3Accessor accessor)
        {
            bool canLoad = true;

            lock (m_Chunks)
            {
                for (int x = Chunk3x3Accessor.XOffsetBegin; x <= Chunk3x3Accessor.XOffsetEnd; x++)
                {
                    for (int z = Chunk3x3Accessor.ZOffsetBegin; z <= Chunk3x3Accessor.ZOffsetEnd; z++)
                    {
                        ChunkPos neighbor = pos.AddOffset(x, z);

                        if (!m_Chunks.ContainsKey(neighbor))
                        {
                            LoadChunk(neighbor);
                            canLoad = false;
                        }
                    }
                }
            }

            if (canLoad)
            {
                accessor = AllocateChunk3x3Accessor();
                accessor.Initialize(pos, this);

                lock (m_Chunk3x3Accessors)
                {
                    m_Chunk3x3Accessors.Add(pos, accessor);
                }
                return true;
            }

            accessor = default;
            return false;
        }

        private Chunk3x3Accessor AllocateChunk3x3Accessor()
        {
            lock (m_Chunk3x3Pool)
            {
                if (m_Chunk3x3Pool.Count > 0)
                {
                    return m_Chunk3x3Pool.Pop();
                }
            }
            return new Chunk3x3Accessor();
        }

        private void TryRecycleChunk3x3Accessor(ChunkPos pos)
        {
            Chunk3x3Accessor accessor = null;

            lock (m_Chunk3x3Accessors)
            {
                if (m_Chunk3x3Accessors.TryGetValue(pos, out accessor))
                {
                    m_Chunk3x3Accessors.Remove(pos);
                }
            }

            if (accessor != null)
            {
                accessor.Dispose();

                lock (m_Chunk3x3Pool)
                {
                    m_Chunk3x3Pool.Push(accessor);
                }
            }
        }
    }
}
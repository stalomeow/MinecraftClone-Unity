using System;
using System.Collections.Generic;
using Minecraft.Configurations;
using Minecraft.Lua;
using UnityEngine;
using UnityEngine.Rendering;
using XLua;

namespace Minecraft.Entities
{
    public class EntityManager : MonoBehaviour, ILuaCallCSharp
    {
        [SerializeField] private int m_RenderLayer;
        [SerializeField] private Camera m_MainCamera;
        [NonSerialized] private Queue<LuaBlockEntity> m_BlockEntityPool;
        [NonSerialized] private HashSet<IRenderableEntity> m_RenderableEntities;

        public void Initialize()
        {
            m_BlockEntityPool = new Queue<LuaBlockEntity>();
            m_RenderableEntities = new HashSet<IRenderableEntity>();
        }

        public LuaBlockEntity CreateBlockEntityAt(int x, int y, int z, BlockData block)
        {
            LuaBlockEntity entity;

            if (m_BlockEntityPool.Count > 0)
            {
                entity = m_BlockEntityPool.Dequeue();
            }
            else
            {
                GameObject go = new GameObject("Block Entity");
                entity = go.AddComponent<LuaBlockEntity>();
            }

            entity.SetBlockAndPosition(block, new Vector3Int(x, y, z));
            m_RenderableEntities.Add(entity);
            entity.gameObject.SetActive(true);
            return entity;
        }

        public void DestroyEntity(IAABBEntity entity)
        {
            entity.OnRecycle();

            if (entity is IRenderableEntity renderableEntity)
            {
                m_RenderableEntities.Remove(renderableEntity);
            }

            if (entity is LuaBlockEntity blockEntity)
            {
                blockEntity.gameObject.SetActive(false);
                m_BlockEntityPool.Enqueue(blockEntity);
            }
        }

        private void Update()
        {
            if (m_RenderableEntities != null)
            {
                var iterator = m_RenderableEntities.GetEnumerator();

                while (iterator.MoveNext())
                {
                    var entity = iterator.Current;

                    if (entity.EnableRendering)
                    {
                        entity.Render(m_RenderLayer, m_MainCamera, ShadowCastingMode.On, true);
                    }
                }

                iterator.Dispose();
            }
        }
    }
}

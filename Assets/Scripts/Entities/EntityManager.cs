using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Minecraft
{
    [LuaCallCSharp]
    public sealed class EntityManager
    {
        private sealed class EntityComparer : IEqualityComparer<Entity>
        {
            public bool Equals(Entity x, Entity y)
            {
                return x == y;
            }

            public int GetHashCode(Entity obj)
            {
                return obj.GetHashCode();
            }
        }


        private readonly HashSet<Entity> m_Entities;

        public Material BlockEntityMaterial { get; }

        public PlayerEntity PlayerObj { get; }


        public EntityManager(Material blockEntityMaterial, PlayerEntity player)
        {
            m_Entities = new HashSet<Entity>(new EntityComparer()) { player };
            BlockEntityMaterial = blockEntityMaterial;
            PlayerObj = player;
        }

        public T CreateEntity<T>() where T : Entity
        {
            GameObject go = new GameObject();
            T entity = go.AddComponent<T>();
            return m_Entities.Add(entity) ? entity : null;
        }

        public Entity CreateEntity(Type type)
        {
            GameObject go = new GameObject();
            Entity entity = go.AddComponent(type) as Entity;
            return m_Entities.Add(entity) ? entity : null;
        }

        public T CreateEntity<T>(T prefab) where T : Entity
        {
            T entity = GameObject.Instantiate<T>(prefab, null);
            return m_Entities.Add(entity) ? entity : null;
        }

        public void DestroyEntity(Entity entity)
        {
            if (entity)
            {
                m_Entities.Remove(entity);
                GameObject.Destroy(entity.gameObject);
            }
        }

        public EntityEnumerator EnumerateEntities()
        {
            return new EntityEnumerator(m_Entities.GetEnumerator(), null);
        }

        public EntityEnumerator EnumerateOtherEntities(Entity self)
        {
            return new EntityEnumerator(m_Entities.GetEnumerator(), self);
        }


        public struct EntityEnumerator : IEnumerator<Entity>
        {
            private HashSet<Entity>.Enumerator m_Enumerator;
            private readonly Entity m_Self;

            public EntityEnumerator(HashSet<Entity>.Enumerator enumerator, Entity self)
            {
                m_Enumerator = enumerator;
                m_Self = self;
            }

            public Entity Current => m_Enumerator.Current;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                m_Enumerator.Dispose();
            }

            public bool MoveNext()
            {
                if (m_Self == null)
                {
                    return m_Enumerator.MoveNext();
                }

                while (m_Enumerator.MoveNext())
                {
                    Entity current = m_Enumerator.Current;

                    if (current && current != m_Self)
                    {
                        return true;
                    }
                }

                return false;
            }

            void IEnumerator.Reset()
            {
                (m_Enumerator as IEnumerator).Reset();
            }
        }
    }
}
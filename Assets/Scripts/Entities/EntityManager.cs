using System.Collections.Generic;
using UnityEngine;

namespace Minecraft
{
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

        public T CreateEntity<T>(T prefab) where T : Entity
        {
            T entity = GameObject.Instantiate<T>(prefab, null);
            return m_Entities.Add(entity) ? entity : null;
        }

        public void DestroyEntity(Entity entity)
        {
            m_Entities.Remove(entity);
            GameObject.Destroy(entity.gameObject);
        }

        public IEnumerator<Entity> EnumerateEntities()
        {
            return m_Entities.GetEnumerator();
        }
    }
}
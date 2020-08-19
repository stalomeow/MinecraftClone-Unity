using Minecraft.BlocksData;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace Minecraft
{
    [DisallowMultipleComponent]
    public sealed class TNTBlockEntity : BlockEntity
    {
        private MaterialPropertyBlock m_PropertyBlock;
        private AudioSource m_AudioSource;
        private bool m_Started;
        private bool m_ShouldRenderBlock;
        private AudioClip m_Fuse;
        private AudioClip m_Explode;
        private GameObject m_Effect;

        protected override MaterialPropertyBlock MaterialPropertyBlock => m_PropertyBlock;

        protected override bool ShouldRenderBlock => m_ShouldRenderBlock;

        protected override void OnInitialize()
        {
            m_PropertyBlock = new MaterialPropertyBlock();
            m_AudioSource = gameObject.AddComponent<AudioSource>();
            m_Started = false;
            m_ShouldRenderBlock = true;

            m_Fuse = Block.ExtraAssets[0] as AudioClip;
            m_Explode = Block.ExtraAssets[1] as AudioClip;
            m_Effect = Instantiate(Block.ExtraAssets[2]) as GameObject;

            m_AudioSource.clip = m_Fuse;
            m_AudioSource.Play();
        }

        private void OnDestroy()
        {
            Destroy(m_Effect);
        }

        protected override void OnCollisions(CollisionFlags flags)
        {
            if (!m_Started)
            {
                m_Started = true;
                StartCoroutine(Explode());
            }
        }

        private IEnumerator Explode()
        {
            float time = 0;

            while (time < 3)
            {
                time += Time.deltaTime;

                float t = Mathf.Sin(time * Mathf.PI * 3);
                Color color = Color.Lerp(Color.grey, Color.white, t);
                m_PropertyBlock.SetColor("_Color", color);

                yield return null;
            }

            int radius = 5;

            WorldManager world = WorldManager.Active;
            EntityManager entityManager = world.EntityManager;

            Vector3Int pos = BlockPosition;

            int x = pos.x;
            int y = pos.y;
            int z = pos.z;

            for (int dx = -radius; dx <= radius; dx++)
            {
                for (int dz = -radius; dz <= radius; dz++)
                {
                    for (int dy = -radius; dy <= radius; dy++)
                    {
                        if (dx * dx + dy * dy + dz * dz <= radius * radius)
                        {
                            Block block = world.GetBlock(x + dx, y + dy, z + dz);

                            if (block.HasAnyFlag(BlockFlags.IgnoreExplosions))
                                continue;

                            world.SetBlockType(x + dx, y + dy, z + dz, BlockType.Air);
                        }
                    }
                }
            }

            var iterator = entityManager.EnumerateEntities();

            while (iterator.MoveNext())
            {
                Entity entity = iterator.Current;
                Vector3 dir = entity.transform.position - pos;

                if (dir.sqrMagnitude <= radius * radius)
                {
                    float vx = Random.Range(-10f, 10f);//[-10, 10]
                    float vy = Random.Range(1, 10f);
                    float vz = Random.Range(-10f, 10f);

                    entity.Velocity = new Vector3(vx, vy, vz);
                }
            }

            iterator.Dispose();

            yield return null;

            m_ShouldRenderBlock = false;

            m_AudioSource.clip = m_Explode;
            m_AudioSource.Play();

            ParticleSystem particle = m_Effect.GetComponent<ParticleSystem>();
            m_Effect.transform.position = transform.position;
            particle.Play();

            while (m_AudioSource.isPlaying || particle)
            {
                yield return null;
            }

            entityManager.DestroyEntity(this);
        }
    }
}
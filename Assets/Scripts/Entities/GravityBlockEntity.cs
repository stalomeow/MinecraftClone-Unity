using System.Collections;
using UnityEngine;
using XLua;

namespace Minecraft
{
    [LuaCallCSharp]
    [DisallowMultipleComponent]
    public class GravityBlockEntity : BlockEntity
    {
        private bool m_IsStarted;

        protected override void OnInitialize()
        {
            m_IsStarted = false;
        }

        protected override void OnGrounded()
        {
            if (!m_IsStarted)
            {
                m_IsStarted = true;
                StartCoroutine(SetBlockAndDestroy());
            }
        }

        private IEnumerator SetBlockAndDestroy()
        {
            Vector3Int pos = BlockPosition;
            WorldManager world = WorldManager.Active;

            world.SetBlockType(pos.x, pos.y, pos.z, Block.Type);

            yield return null; // 等mesh更新
            yield return null; // 等mesh更新
            yield return null; // 等mesh更新

            world.EntityManager.DestroyEntity(this);
        }
    }
}
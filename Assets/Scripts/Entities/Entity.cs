using Minecraft.XPhysics;
using UnityEngine;
using XLua;

namespace Minecraft.Entities
{
    [LuaCallCSharp]
    [DisallowMultipleComponent]
    public abstract class Entity : MonoBehaviour
    {
        public abstract AABB BoundingBox { get; }

        public abstract bool IsGrounded { get; }

        public abstract Vector3 Velocity { get; set; }

        public float GravityMultiplier { get; set; }
    }
}
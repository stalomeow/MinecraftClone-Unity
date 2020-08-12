using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Minecraft.WorldConsts;
using Minecraft.BlocksData;

namespace Minecraft
{
    [DisallowMultipleComponent]
    public abstract class Entity : MonoBehaviour
    {
        public abstract AABB BoundingBox { get; }

        public abstract bool IsGrounded { get; }

        public abstract Vector3 Velocity { get; set; }

        public float GravityMultiplier { get; set; }
    }
}
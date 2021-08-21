using System;
using Minecraft.Lua;
using UnityEngine;

namespace Minecraft.PhysicSystem
{
    [Serializable]
    [XLua.GCOptimize]
    public struct AABB : ILuaCallCSharp
    {
        [SerializeField] private Vector3 m_Min;
        [SerializeField] private Vector3 m_Max;

        public Vector3 Min
        {
            get => m_Min;
            set => m_Min = value;
        }

        public Vector3 Max
        {
            get => m_Max;
            set => m_Max = value;
        }

        public Vector3 Center => (Min + Max) * 0.5f;

        public AABB(Vector3 min, Vector3 max)
        {
            m_Min = min;
            m_Max = max;
        }

        public bool Intersects(AABB aabb)
        {
            return ((Min.x >= aabb.Min.x && Min.x <= aabb.Max.x) || (aabb.Min.x >= Min.x && aabb.Min.x <= Max.x))
                   && ((Min.y >= aabb.Min.y && Min.y <= aabb.Max.y) || (aabb.Min.y >= Min.y && aabb.Min.y <= Max.y))
                   && ((Min.z >= aabb.Min.z && Min.z <= aabb.Max.z) || (aabb.Min.z >= Min.z && aabb.Min.z <= Max.z));
        }

        public static AABB Merge(AABB left, AABB right)
        {
            return new AABB(Vector3.Min(left.Min, right.Min), Vector3.Max(left.Max, right.Max));
        }

        public static AABB Transform(AABB aabb, Vector3 transform)
        {
            return new AABB(aabb.Min + transform, aabb.Max + transform);
        }

        public static AABB operator +(AABB left, AABB right) => Merge(left, right);

        public static AABB operator +(AABB aabb, Vector3 transform) => Transform(aabb, transform);

        public static AABB operator -(AABB aabb, Vector3 transform) => Transform(aabb, -transform);
    }
}
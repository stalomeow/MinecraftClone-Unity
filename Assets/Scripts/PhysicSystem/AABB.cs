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

        public Vector3 Size => Max - Min;

        public AABB(Vector3 min, Vector3 max)
        {
            m_Min = min;
            m_Max = max;
        }

        public override bool Equals(object obj)
        {
            return obj is AABB aabb && (aabb == this);
        }

        public override int GetHashCode()
        {
            return m_Min.GetHashCode() ^ m_Max.GetHashCode();
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

        public static AABB Translate(AABB aabb, Vector3 translation)
        {
            return new AABB(aabb.Min + translation, aabb.Max + translation);
        }

        public static AABB Rotate(AABB aabb, Quaternion rotation)
        {
            return Rotate(aabb, rotation, aabb.Center);
        }

        public static AABB Rotate(AABB aabb, Quaternion rotation, Vector3 pivot)
        {
            Vector3 step = aabb.Max - aabb.Min;
            Vector3 min = Vector3.positiveInfinity;
            Vector3 max = Vector3.negativeInfinity;

            for (float x = aabb.Min.x; x <= aabb.Max.x; x += step.x)
            {
                for (float y = aabb.Min.y; y <= aabb.Max.y; y += step.y)
                {
                    for (float z = aabb.Min.z; z <= aabb.Max.z; z += step.z)
                    {
                        Vector3 point = MathUtility.RotatePoint(new Vector3(x, y, z), rotation, pivot);
                        min = Vector3.Min(min, point);
                        max = Vector3.Max(max, point);
                    }
                }
            }

            return new AABB(min, max);
        }

        public static AABB operator +(AABB left, AABB right) => Merge(left, right);

        public static AABB operator +(AABB aabb, Vector3 translation) => Translate(aabb, translation);

        public static AABB operator -(AABB aabb, Vector3 translation) => Translate(aabb, -translation);

        public static AABB operator *(AABB aabb, Quaternion rotation) => Rotate(aabb, rotation);

        public static bool operator ==(AABB left, AABB right) => left.m_Min == right.m_Min && left.m_Max == right.m_Max;

        public static bool operator !=(AABB left, AABB right) => left.m_Min != right.m_Min || left.m_Max != right.m_Max;
    }
}
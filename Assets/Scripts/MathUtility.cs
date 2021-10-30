using UnityEngine;

namespace Minecraft
{
    [XLua.LuaCallCSharp]
    public static class MathUtility
    {
        public static Vector3Int FloorToInt(this Vector3 vec)
        {
            int x = Mathf.FloorToInt(vec.x);
            int y = Mathf.FloorToInt(vec.y);
            int z = Mathf.FloorToInt(vec.z);
            return new Vector3Int(x, y, z);
        }

        public static Vector3Int RoundToInt(this Vector3 vec)
        {
            int x = Mathf.RoundToInt(vec.x);
            int y = Mathf.RoundToInt(vec.y);
            int z = Mathf.RoundToInt(vec.z);
            return new Vector3Int(x, y, z);
        }

        public static Vector3 RotatePoint(Vector3 point, Quaternion rotation, Vector3 pivot)
        {
            return rotation * (point - pivot) + pivot;
        }
    }
}

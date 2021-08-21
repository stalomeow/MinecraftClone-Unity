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
    }
}

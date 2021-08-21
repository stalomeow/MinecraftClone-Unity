using UnityEngine;

namespace Minecraft
{
    [XLua.LuaCallCSharp]
    public static class WorldUtility
    {
        public static void AccessorSpaceToAccessorSpacePosition(this IWorldRAccessor self, IWorldRAccessor accessor, ref int x, ref int y, ref int z)
        {
            Vector3Int origin = self.WorldSpaceOrigin - accessor.WorldSpaceOrigin;
            x += origin.x;
            y += origin.y;
            z += origin.z;
        }

        public static void WorldSpaceToAccessorSpacePosition(this IWorldRAccessor accessor, ref int x, ref int y, ref int z)
        {
            WorldSpaceToAccessorSpacePosition(accessor.WorldSpaceOrigin, ref x, ref y, ref z);
        }

        public static void AccessorSpaceToWorldSpacePosition(this IWorldRAccessor accessor, ref int x, ref int y, ref int z)
        {
            AccessorSpaceToWorldSpacePosition(accessor.WorldSpaceOrigin, ref x, ref y, ref z);
        }

        public static void WorldSpaceToAccessorSpacePosition(Vector3Int accessorWorldSpaceOrigin, ref int x, ref int y, ref int z)
        {
            x -= accessorWorldSpaceOrigin.x;
            y -= accessorWorldSpaceOrigin.y;
            z -= accessorWorldSpaceOrigin.z;
        }

        public static void AccessorSpaceToWorldSpacePosition(Vector3Int accessorWorldSpaceOrigin, ref int x, ref int y, ref int z)
        {
            x += accessorWorldSpaceOrigin.x;
            y += accessorWorldSpaceOrigin.y;
            z += accessorWorldSpaceOrigin.z;
        }
    }
}

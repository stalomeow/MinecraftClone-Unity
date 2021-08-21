using UnityEngine;
using XLua;

namespace Minecraft.Lua
{
    [LuaCallCSharp]
    public static class LuaUtility
    {
        public static ParticleSystem GetParticleSystem(GameObject go)
        {
            return go.GetComponent<ParticleSystem>();
        }
    }
}

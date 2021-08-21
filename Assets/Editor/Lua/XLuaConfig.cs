using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Minecraft.Lua;
using XLua;

namespace MinecraftEditor.Lua
{
    public static class XLuaConfig
    {
        [CSharpCallLua]
        public static List<Type> CSharpCallLuaTypeList
        {
            get => (from type in Assembly.Load("Assembly-CSharp").GetTypes()
                    where typeof(ICSharpCallLua).IsAssignableFrom(type)
                    select type).ToList();
        }

        [LuaCallCSharp]
        public static List<Type> LuaCallCSharpTypeList
        {
            get => (from type in Assembly.Load("Assembly-CSharp").GetTypes()
                    where typeof(ILuaCallCSharp).IsAssignableFrom(type)
                    select type).ToList();
        }

        [Hotfix]
        public static List<Type> HotfixTypeList
        {
            get => (from type in Assembly.Load("Assembly-CSharp").GetTypes()
                    where typeof(IHotfixable).IsAssignableFrom(type)
                    select type).ToList();
        }

        [CSharpCallLua]
        public static List<Type> CSharpCallLuaExtraTypeList = new List<Type>()
        {
            typeof(IEnumerator)
        };
    }
}

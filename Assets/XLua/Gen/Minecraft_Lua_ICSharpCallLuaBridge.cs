#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System;


namespace XLua.CSObjectWrap
{
    public class MinecraftLuaICSharpCallLuaBridge : LuaBase, Minecraft.Lua.ICSharpCallLua
    {
	    public static LuaBase __Create(int reference, LuaEnv luaenv)
		{
		    return new MinecraftLuaICSharpCallLuaBridge(reference, luaenv);
		}
		
		public MinecraftLuaICSharpCallLuaBridge(int reference, LuaEnv luaenv) : base(reference, luaenv)
        {
        }
		
        

        
        
        
		
		
	}
}

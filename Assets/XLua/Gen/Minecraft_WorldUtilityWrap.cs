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
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class MinecraftWorldUtilityWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Minecraft.WorldUtility);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 4, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "AccessorSpaceToAccessorSpacePosition", _m_AccessorSpaceToAccessorSpacePosition_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "WorldSpaceToAccessorSpacePosition", _m_WorldSpaceToAccessorSpacePosition_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AccessorSpaceToWorldSpacePosition", _m_AccessorSpaceToWorldSpacePosition_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Minecraft.WorldUtility does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AccessorSpaceToAccessorSpacePosition_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Minecraft.IWorldRAccessor _self = (Minecraft.IWorldRAccessor)translator.GetObject(L, 1, typeof(Minecraft.IWorldRAccessor));
                    Minecraft.IWorldRAccessor _accessor = (Minecraft.IWorldRAccessor)translator.GetObject(L, 2, typeof(Minecraft.IWorldRAccessor));
                    int _x = LuaAPI.xlua_tointeger(L, 3);
                    int _y = LuaAPI.xlua_tointeger(L, 4);
                    int _z = LuaAPI.xlua_tointeger(L, 5);
                    
                    Minecraft.WorldUtility.AccessorSpaceToAccessorSpacePosition( _self, _accessor, ref _x, ref _y, ref _z );
                    LuaAPI.xlua_pushinteger(L, _x);
                        
                    LuaAPI.xlua_pushinteger(L, _y);
                        
                    LuaAPI.xlua_pushinteger(L, _z);
                        
                    
                    
                    
                    return 3;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WorldSpaceToAccessorSpacePosition_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<Minecraft.IWorldRAccessor>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    Minecraft.IWorldRAccessor _accessor = (Minecraft.IWorldRAccessor)translator.GetObject(L, 1, typeof(Minecraft.IWorldRAccessor));
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _z = LuaAPI.xlua_tointeger(L, 4);
                    
                    Minecraft.WorldUtility.WorldSpaceToAccessorSpacePosition( _accessor, ref _x, ref _y, ref _z );
                    LuaAPI.xlua_pushinteger(L, _x);
                        
                    LuaAPI.xlua_pushinteger(L, _y);
                        
                    LuaAPI.xlua_pushinteger(L, _z);
                        
                    
                    
                    
                    return 3;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3Int>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Vector3Int _accessorWorldSpaceOrigin;translator.Get(L, 1, out _accessorWorldSpaceOrigin);
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _z = LuaAPI.xlua_tointeger(L, 4);
                    
                    Minecraft.WorldUtility.WorldSpaceToAccessorSpacePosition( _accessorWorldSpaceOrigin, ref _x, ref _y, ref _z );
                    LuaAPI.xlua_pushinteger(L, _x);
                        
                    LuaAPI.xlua_pushinteger(L, _y);
                        
                    LuaAPI.xlua_pushinteger(L, _z);
                        
                    
                    
                    
                    return 3;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Minecraft.WorldUtility.WorldSpaceToAccessorSpacePosition!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AccessorSpaceToWorldSpacePosition_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<Minecraft.IWorldRAccessor>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    Minecraft.IWorldRAccessor _accessor = (Minecraft.IWorldRAccessor)translator.GetObject(L, 1, typeof(Minecraft.IWorldRAccessor));
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _z = LuaAPI.xlua_tointeger(L, 4);
                    
                    Minecraft.WorldUtility.AccessorSpaceToWorldSpacePosition( _accessor, ref _x, ref _y, ref _z );
                    LuaAPI.xlua_pushinteger(L, _x);
                        
                    LuaAPI.xlua_pushinteger(L, _y);
                        
                    LuaAPI.xlua_pushinteger(L, _z);
                        
                    
                    
                    
                    return 3;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3Int>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.Vector3Int _accessorWorldSpaceOrigin;translator.Get(L, 1, out _accessorWorldSpaceOrigin);
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _z = LuaAPI.xlua_tointeger(L, 4);
                    
                    Minecraft.WorldUtility.AccessorSpaceToWorldSpacePosition( _accessorWorldSpaceOrigin, ref _x, ref _y, ref _z );
                    LuaAPI.xlua_pushinteger(L, _x);
                        
                    LuaAPI.xlua_pushinteger(L, _y);
                        
                    LuaAPI.xlua_pushinteger(L, _z);
                        
                    
                    
                    
                    return 3;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Minecraft.WorldUtility.AccessorSpaceToWorldSpacePosition!");
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}

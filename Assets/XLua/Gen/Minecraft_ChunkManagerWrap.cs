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
    public class MinecraftChunkManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Minecraft.ChunkManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 5, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Initialize", _m_Initialize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetChunk", _m_GetChunk);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetChunk3x3Accessor", _m_GetChunk3x3Accessor);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnChunkLoaded", _e_OnChunkLoaded);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnChunkUnloaded", _e_OnChunkUnloaded);
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new Minecraft.ChunkManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Minecraft.ChunkManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Initialize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.ChunkManager gen_to_be_invoked = (Minecraft.ChunkManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Minecraft.IWorld _world = (Minecraft.IWorld)translator.GetObject(L, 2, typeof(Minecraft.IWorld));
                    
                    gen_to_be_invoked.Initialize( _world );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetChunk(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.ChunkManager gen_to_be_invoked = (Minecraft.ChunkManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Minecraft.ChunkPos _pos;translator.Get(L, 2, out _pos);
                    bool _load = LuaAPI.lua_toboolean(L, 3);
                    Minecraft.Chunk _chunk;
                    
                        var gen_ret = gen_to_be_invoked.GetChunk( _pos, _load, out _chunk );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _chunk);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetChunk3x3Accessor(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.ChunkManager gen_to_be_invoked = (Minecraft.ChunkManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Minecraft.ChunkPos _pos;translator.Get(L, 2, out _pos);
                    bool _load = LuaAPI.lua_toboolean(L, 3);
                    Minecraft.Chunk3x3Accessor _accessor;
                    
                        var gen_ret = gen_to_be_invoked.GetChunk3x3Accessor( _pos, _load, out _accessor );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _accessor);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnChunkLoaded(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			Minecraft.ChunkManager gen_to_be_invoked = (Minecraft.ChunkManager)translator.FastGetCSObj(L, 1);
                UnityEngine.Events.UnityAction<Minecraft.Chunk> gen_delegate = translator.GetDelegate<UnityEngine.Events.UnityAction<Minecraft.Chunk>>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need UnityEngine.Events.UnityAction<Minecraft.Chunk>!");
                }
				
				if (gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						gen_to_be_invoked.OnChunkLoaded += gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						gen_to_be_invoked.OnChunkLoaded -= gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to Minecraft.ChunkManager.OnChunkLoaded!");
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnChunkUnloaded(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			Minecraft.ChunkManager gen_to_be_invoked = (Minecraft.ChunkManager)translator.FastGetCSObj(L, 1);
                UnityEngine.Events.UnityAction<Minecraft.ChunkPos> gen_delegate = translator.GetDelegate<UnityEngine.Events.UnityAction<Minecraft.ChunkPos>>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need UnityEngine.Events.UnityAction<Minecraft.ChunkPos>!");
                }
				
				if (gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						gen_to_be_invoked.OnChunkUnloaded += gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						gen_to_be_invoked.OnChunkUnloaded -= gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to Minecraft.ChunkManager.OnChunkUnloaded!");
            return 0;
        }
        
		
		
    }
}

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
    public class MinecraftIWorldWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Minecraft.IWorld);
			Utils.BeginObjectRegister(type, L, translator, 0, 3, 14, 2);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LightBlock", _m_LightBlock);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "TickBlock", _m_TickBlock);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "MarkBlockMeshDirty", _m_MarkBlockMeshDirty);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Initialized", _g_get_Initialized);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "RWAccessor", _g_get_RWAccessor);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PlayerTransform", _g_get_PlayerTransform);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MainCamera", _g_get_MainCamera);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AudioManager", _g_get_AudioManager);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LuaManager", _g_get_LuaManager);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ChunkManager", _g_get_ChunkManager);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "RenderingManager", _g_get_RenderingManager);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EntityManager", _g_get_EntityManager);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BlockDataTable", _g_get_BlockDataTable);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BiomeDataTable", _g_get_BiomeDataTable);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "WorldGenPipeline", _g_get_WorldGenPipeline);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MaxTickBlockCountPerFrame", _g_get_MaxTickBlockCountPerFrame);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MaxLightBlockCountPerFrame", _g_get_MaxLightBlockCountPerFrame);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "MaxTickBlockCountPerFrame", _s_set_MaxTickBlockCountPerFrame);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "MaxLightBlockCountPerFrame", _s_set_MaxLightBlockCountPerFrame);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Minecraft.IWorld does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LightBlock(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.IWorld gen_to_be_invoked = (Minecraft.IWorld)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _z = LuaAPI.xlua_tointeger(L, 4);
                    Minecraft.ModificationSource _source;translator.Get(L, 5, out _source);
                    
                    gen_to_be_invoked.LightBlock( _x, _y, _z, _source );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TickBlock(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.IWorld gen_to_be_invoked = (Minecraft.IWorld)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _z = LuaAPI.xlua_tointeger(L, 4);
                    
                    gen_to_be_invoked.TickBlock( _x, _y, _z );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_MarkBlockMeshDirty(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.IWorld gen_to_be_invoked = (Minecraft.IWorld)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _z = LuaAPI.xlua_tointeger(L, 4);
                    Minecraft.ModificationSource _source;translator.Get(L, 5, out _source);
                    
                    gen_to_be_invoked.MarkBlockMeshDirty( _x, _y, _z, _source );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Initialized(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.IWorld gen_to_be_invoked = (Minecraft.IWorld)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.Initialized);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_RWAccessor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.IWorld gen_to_be_invoked = (Minecraft.IWorld)translator.FastGetCSObj(L, 1);
                translator.PushAny(L, gen_to_be_invoked.RWAccessor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PlayerTransform(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.IWorld gen_to_be_invoked = (Minecraft.IWorld)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PlayerTransform);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MainCamera(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.IWorld gen_to_be_invoked = (Minecraft.IWorld)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.MainCamera);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AudioManager(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.IWorld gen_to_be_invoked = (Minecraft.IWorld)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.AudioManager);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LuaManager(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.IWorld gen_to_be_invoked = (Minecraft.IWorld)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.LuaManager);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ChunkManager(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.IWorld gen_to_be_invoked = (Minecraft.IWorld)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ChunkManager);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_RenderingManager(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.IWorld gen_to_be_invoked = (Minecraft.IWorld)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.RenderingManager);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EntityManager(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.IWorld gen_to_be_invoked = (Minecraft.IWorld)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.EntityManager);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BlockDataTable(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.IWorld gen_to_be_invoked = (Minecraft.IWorld)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.BlockDataTable);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BiomeDataTable(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.IWorld gen_to_be_invoked = (Minecraft.IWorld)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.BiomeDataTable);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_WorldGenPipeline(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.IWorld gen_to_be_invoked = (Minecraft.IWorld)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.WorldGenPipeline);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MaxTickBlockCountPerFrame(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.IWorld gen_to_be_invoked = (Minecraft.IWorld)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.MaxTickBlockCountPerFrame);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MaxLightBlockCountPerFrame(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.IWorld gen_to_be_invoked = (Minecraft.IWorld)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.MaxLightBlockCountPerFrame);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_MaxTickBlockCountPerFrame(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.IWorld gen_to_be_invoked = (Minecraft.IWorld)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.MaxTickBlockCountPerFrame = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_MaxLightBlockCountPerFrame(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.IWorld gen_to_be_invoked = (Minecraft.IWorld)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.MaxLightBlockCountPerFrame = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}

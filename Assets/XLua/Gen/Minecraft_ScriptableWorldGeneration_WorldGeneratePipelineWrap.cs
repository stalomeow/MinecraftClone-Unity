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
    public class MinecraftScriptableWorldGenerationWorldGeneratePipelineWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline);
			Utils.BeginObjectRegister(type, L, translator, 0, 2, 9, 9);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Initialize", _m_Initialize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GenerateChunk", _m_GenerateChunk);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "GenerateStructure", _g_get_GenerateStructure);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UseCaves", _g_get_UseCaves);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UseRavines", _g_get_UseRavines);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UseMineShafts", _g_get_UseMineShafts);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UseVillages", _g_get_UseVillages);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UseStrongholds", _g_get_UseStrongholds);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UseTemples", _g_get_UseTemples);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UseMonuments", _g_get_UseMonuments);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UseMansions", _g_get_UseMansions);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "GenerateStructure", _s_set_GenerateStructure);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UseCaves", _s_set_UseCaves);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UseRavines", _s_set_UseRavines);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UseMineShafts", _s_set_UseMineShafts);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UseVillages", _s_set_UseVillages);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UseStrongholds", _s_set_UseStrongholds);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UseTemples", _s_set_UseTemples);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UseMonuments", _s_set_UseMonuments);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UseMansions", _s_set_UseMansions);
            
			
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
					
					var gen_ret = new Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Initialize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Minecraft.IWorld _world = (Minecraft.IWorld)translator.GetObject(L, 2, typeof(Minecraft.IWorld));
                    int _seed = LuaAPI.xlua_tointeger(L, 3);
                    
                    gen_to_be_invoked.Initialize( _world, _seed );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GenerateChunk(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Minecraft.Chunk _chunk = (Minecraft.Chunk)translator.GetObject(L, 2, typeof(Minecraft.Chunk));
                    
                    gen_to_be_invoked.GenerateChunk( _chunk );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_GenerateStructure(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.GenerateStructure);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UseCaves(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.UseCaves);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UseRavines(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.UseRavines);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UseMineShafts(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.UseMineShafts);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UseVillages(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.UseVillages);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UseStrongholds(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.UseStrongholds);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UseTemples(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.UseTemples);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UseMonuments(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.UseMonuments);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UseMansions(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.UseMansions);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_GenerateStructure(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.GenerateStructure = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UseCaves(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UseCaves = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UseRavines(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UseRavines = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UseMineShafts(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UseMineShafts = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UseVillages(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UseVillages = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UseStrongholds(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UseStrongholds = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UseTemples(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UseTemples = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UseMonuments(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UseMonuments = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UseMansions(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UseMansions = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}

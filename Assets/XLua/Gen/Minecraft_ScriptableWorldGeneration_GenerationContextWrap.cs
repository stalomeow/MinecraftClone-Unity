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
    public class MinecraftScriptableWorldGenerationGenerationContextWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Minecraft.ScriptableWorldGeneration.GenerationContext);
			Utils.BeginObjectRegister(type, L, translator, 0, 1, 8, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Initialize", _m_Initialize);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "DensityMap", _g_get_DensityMap);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DepthMap", _g_get_DepthMap);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MainNoiseMap", _g_get_MainNoiseMap);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MinLimitMap", _g_get_MinLimitMap);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MaxLimitMap", _g_get_MaxLimitMap);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SurfaceMap", _g_get_SurfaceMap);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Biomes", _g_get_Biomes);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Rand", _g_get_Rand);
            
			
			
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
					
					var gen_ret = new Minecraft.ScriptableWorldGeneration.GenerationContext();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Minecraft.ScriptableWorldGeneration.GenerationContext constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Initialize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.ScriptableWorldGeneration.GenerationContext gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.GenerationContext)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _seed = LuaAPI.xlua_tointeger(L, 2);
                    
                    gen_to_be_invoked.Initialize( _seed );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DensityMap(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.GenerationContext gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.GenerationContext)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.DensityMap);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DepthMap(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.GenerationContext gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.GenerationContext)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.DepthMap);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MainNoiseMap(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.GenerationContext gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.GenerationContext)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.MainNoiseMap);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MinLimitMap(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.GenerationContext gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.GenerationContext)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.MinLimitMap);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MaxLimitMap(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.GenerationContext gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.GenerationContext)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.MaxLimitMap);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SurfaceMap(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.GenerationContext gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.GenerationContext)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SurfaceMap);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Biomes(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.GenerationContext gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.GenerationContext)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Biomes);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Rand(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.GenerationContext gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.GenerationContext)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Rand);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}

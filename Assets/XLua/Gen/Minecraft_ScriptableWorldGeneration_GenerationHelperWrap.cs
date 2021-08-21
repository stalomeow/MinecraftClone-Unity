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
    public class MinecraftScriptableWorldGenerationGenerationHelperWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Minecraft.ScriptableWorldGeneration.GenerationHelper);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 8, 8);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Seed", _g_get_Seed);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DepthNoise", _g_get_DepthNoise);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MainNoise", _g_get_MainNoise);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MaxNoise", _g_get_MaxNoise);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MinNoise", _g_get_MinNoise);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SurfaceNoise", _g_get_SurfaceNoise);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BiomeWeights", _g_get_BiomeWeights);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "GenLayers", _g_get_GenLayers);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "Seed", _s_set_Seed);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "DepthNoise", _s_set_DepthNoise);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "MainNoise", _s_set_MainNoise);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "MaxNoise", _s_set_MaxNoise);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "MinNoise", _s_set_MinNoise);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SurfaceNoise", _s_set_SurfaceNoise);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BiomeWeights", _s_set_BiomeWeights);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "GenLayers", _s_set_GenLayers);
            
			
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
					
					var gen_ret = new Minecraft.ScriptableWorldGeneration.GenerationHelper();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Minecraft.ScriptableWorldGeneration.GenerationHelper constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Seed(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.GenerationHelper gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.GenerationHelper)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.Seed);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DepthNoise(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.GenerationHelper gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.GenerationHelper)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.DepthNoise);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MainNoise(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.GenerationHelper gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.GenerationHelper)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.MainNoise);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MaxNoise(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.GenerationHelper gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.GenerationHelper)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.MaxNoise);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MinNoise(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.GenerationHelper gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.GenerationHelper)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.MinNoise);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SurfaceNoise(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.GenerationHelper gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.GenerationHelper)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SurfaceNoise);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BiomeWeights(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.GenerationHelper gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.GenerationHelper)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.BiomeWeights);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_GenLayers(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.GenerationHelper gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.GenerationHelper)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.GenLayers);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Seed(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.GenerationHelper gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.GenerationHelper)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Seed = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DepthNoise(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.GenerationHelper gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.GenerationHelper)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.DepthNoise = (Minecraft.Noises.GenericNoise<Minecraft.Noises.PerlinNoise>)translator.GetObject(L, 2, typeof(Minecraft.Noises.GenericNoise<Minecraft.Noises.PerlinNoise>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_MainNoise(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.GenerationHelper gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.GenerationHelper)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.MainNoise = (Minecraft.Noises.GenericNoise<Minecraft.Noises.PerlinNoise>)translator.GetObject(L, 2, typeof(Minecraft.Noises.GenericNoise<Minecraft.Noises.PerlinNoise>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_MaxNoise(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.GenerationHelper gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.GenerationHelper)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.MaxNoise = (Minecraft.Noises.GenericNoise<Minecraft.Noises.PerlinNoise>)translator.GetObject(L, 2, typeof(Minecraft.Noises.GenericNoise<Minecraft.Noises.PerlinNoise>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_MinNoise(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.GenerationHelper gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.GenerationHelper)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.MinNoise = (Minecraft.Noises.GenericNoise<Minecraft.Noises.PerlinNoise>)translator.GetObject(L, 2, typeof(Minecraft.Noises.GenericNoise<Minecraft.Noises.PerlinNoise>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SurfaceNoise(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.GenerationHelper gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.GenerationHelper)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SurfaceNoise = (Minecraft.Noises.GenericNoise<Minecraft.Noises.PerlinNoise>)translator.GetObject(L, 2, typeof(Minecraft.Noises.GenericNoise<Minecraft.Noises.PerlinNoise>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BiomeWeights(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.GenerationHelper gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.GenerationHelper)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.BiomeWeights = (float[,])translator.GetObject(L, 2, typeof(float[,]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_GenLayers(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.GenerationHelper gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.GenerationHelper)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.GenLayers = (Minecraft.ScriptableWorldGeneration.GenLayers.StatelessGenLayer)translator.GetObject(L, 2, typeof(Minecraft.ScriptableWorldGeneration.GenLayers.StatelessGenLayer));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}

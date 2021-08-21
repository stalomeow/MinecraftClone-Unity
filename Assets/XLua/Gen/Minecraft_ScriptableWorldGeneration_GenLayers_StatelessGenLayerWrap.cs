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
    public class MinecraftScriptableWorldGenerationGenLayersStatelessGenLayerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Minecraft.ScriptableWorldGeneration.GenLayers.StatelessGenLayer);
			Utils.BeginObjectRegister(type, L, translator, 0, 1, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetInts", _m_GetInts);
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 3, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "SelectModeOrRandom", _m_SelectModeOrRandom_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetChunkSeed", _m_GetChunkSeed_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Minecraft.ScriptableWorldGeneration.GenLayers.StatelessGenLayer does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetInts(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.ScriptableWorldGeneration.GenLayers.StatelessGenLayer gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.GenLayers.StatelessGenLayer)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _areaX = LuaAPI.xlua_tointeger(L, 2);
                    int _areaY = LuaAPI.xlua_tointeger(L, 3);
                    int _areaWidth = LuaAPI.xlua_tointeger(L, 4);
                    int _areaHeight = LuaAPI.xlua_tointeger(L, 5);
                    Unity.Collections.Allocator _allocator;translator.Get(L, 6, out _allocator);
                    
                        var gen_ret = gen_to_be_invoked.GetInts( _areaX, _areaY, _areaWidth, _areaHeight, _allocator );
                        translator.PushMinecraftScriptableWorldGenerationGenLayersNativeInt2DArray(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SelectModeOrRandom_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _seed = LuaAPI.xlua_tointeger(L, 1);
                    int _a = LuaAPI.xlua_tointeger(L, 2);
                    int _b = LuaAPI.xlua_tointeger(L, 3);
                    int _c = LuaAPI.xlua_tointeger(L, 4);
                    int _d = LuaAPI.xlua_tointeger(L, 5);
                    
                        var gen_ret = Minecraft.ScriptableWorldGeneration.GenLayers.StatelessGenLayer.SelectModeOrRandom( _seed, _a, _b, _c, _d );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetChunkSeed_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 1);
                    int _z = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = Minecraft.ScriptableWorldGeneration.GenLayers.StatelessGenLayer.GetChunkSeed( _x, _z );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}

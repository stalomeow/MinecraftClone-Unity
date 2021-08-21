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
    public class MinecraftScriptableWorldGenerationGenLayersZoomLayerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Minecraft.ScriptableWorldGeneration.GenLayers.ZoomLayer);
			Utils.BeginObjectRegister(type, L, translator, 0, 1, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetInts", _m_GetInts);
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "Magnify", _m_Magnify_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 3 && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2) && translator.Assignable<Minecraft.ScriptableWorldGeneration.GenLayers.StatelessGenLayer>(L, 3))
				{
					int _seed = LuaAPI.xlua_tointeger(L, 2);
					Minecraft.ScriptableWorldGeneration.GenLayers.StatelessGenLayer _parent = (Minecraft.ScriptableWorldGeneration.GenLayers.StatelessGenLayer)translator.GetObject(L, 3, typeof(Minecraft.ScriptableWorldGeneration.GenLayers.StatelessGenLayer));
					
					var gen_ret = new Minecraft.ScriptableWorldGeneration.GenLayers.ZoomLayer(_seed, _parent);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Minecraft.ScriptableWorldGeneration.GenLayers.ZoomLayer constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetInts(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.ScriptableWorldGeneration.GenLayers.ZoomLayer gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.GenLayers.ZoomLayer)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_Magnify_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _seed = LuaAPI.xlua_tointeger(L, 1);
                    Minecraft.ScriptableWorldGeneration.GenLayers.StatelessGenLayer _layer = (Minecraft.ScriptableWorldGeneration.GenLayers.StatelessGenLayer)translator.GetObject(L, 2, typeof(Minecraft.ScriptableWorldGeneration.GenLayers.StatelessGenLayer));
                    int _times = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = Minecraft.ScriptableWorldGeneration.GenLayers.ZoomLayer.Magnify( _seed, _layer, _times );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}

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
    public class MinecraftRenderingLightingUtilityWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Minecraft.Rendering.LightingUtility);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 10, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "MapLight01", _m_MapLight01_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetBlockedLight", _m_GetBlockedLight_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AmbientOcclusion", _m_AmbientOcclusion_xlua_st_);
            
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MaxLight", Minecraft.Rendering.LightingUtility.MaxLight);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SkyLight", Minecraft.Rendering.LightingUtility.SkyLight);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MaxBlockFaceCount", Minecraft.Rendering.LightingUtility.MaxBlockFaceCount);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MaxBlockFaceCornerCount", Minecraft.Rendering.LightingUtility.MaxBlockFaceCornerCount);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AmbientLightSampleCount", Minecraft.Rendering.LightingUtility.AmbientLightSampleCount);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AmbientLightSampleDirections", Minecraft.Rendering.LightingUtility.AmbientLightSampleDirections);
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Minecraft.Rendering.LightingUtility does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_MapLight01_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 1&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 1)) 
                {
                    float _light = (float)LuaAPI.lua_tonumber(L, 1);
                    
                        var gen_ret = Minecraft.Rendering.LightingUtility.MapLight01( _light );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 1&& translator.Assignable<UnityEngine.Vector2>(L, 1)) 
                {
                    UnityEngine.Vector2 _light;translator.Get(L, 1, out _light);
                    
                        var gen_ret = Minecraft.Rendering.LightingUtility.MapLight01( _light );
                        translator.PushUnityEngineVector2(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Minecraft.Rendering.LightingUtility.MapLight01!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBlockedLight_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _light = LuaAPI.xlua_tointeger(L, 1);
                    Minecraft.Configurations.BlockData _block = (Minecraft.Configurations.BlockData)translator.GetObject(L, 2, typeof(Minecraft.Configurations.BlockData));
                    
                        var gen_ret = Minecraft.Rendering.LightingUtility.GetBlockedLight( _light, _block );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AmbientOcclusion_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 1);
                    int _y = LuaAPI.xlua_tointeger(L, 2);
                    int _z = LuaAPI.xlua_tointeger(L, 3);
                    Minecraft.Configurations.BlockFace _face;translator.Get(L, 4, out _face);
                    Minecraft.Configurations.BlockFaceCorner _corner;translator.Get(L, 5, out _corner);
                    Minecraft.IWorldRAccessor _accessor = (Minecraft.IWorldRAccessor)translator.GetObject(L, 6, typeof(Minecraft.IWorldRAccessor));
                    
                        var gen_ret = Minecraft.Rendering.LightingUtility.AmbientOcclusion( _x, _y, _z, _face, _corner, _accessor );
                        translator.PushUnityEngineVector2(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}

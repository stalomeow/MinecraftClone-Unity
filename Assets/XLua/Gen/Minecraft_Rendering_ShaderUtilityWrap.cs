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
    public class MinecraftRenderingShaderUtilityWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Minecraft.Rendering.ShaderUtility);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 7, 7);
			
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "BlockTextures", _g_get_BlockTextures);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "RenderDistance", _g_get_RenderDistance);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "ViewDistance", _g_get_ViewDistance);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "LightLimits", _g_get_LightLimits);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "WorldAmbientColor", _g_get_WorldAmbientColor);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "TargetedBlockPosition", _g_get_TargetedBlockPosition);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "DigProgress", _g_get_DigProgress);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "BlockTextures", _s_set_BlockTextures);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "RenderDistance", _s_set_RenderDistance);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "ViewDistance", _s_set_ViewDistance);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "LightLimits", _s_set_LightLimits);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "WorldAmbientColor", _s_set_WorldAmbientColor);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "TargetedBlockPosition", _s_set_TargetedBlockPosition);
            Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "DigProgress", _s_set_DigProgress);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Minecraft.Rendering.ShaderUtility does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BlockTextures(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Minecraft.Rendering.ShaderUtility.BlockTextures);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_RenderDistance(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, Minecraft.Rendering.ShaderUtility.RenderDistance);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ViewDistance(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, Minecraft.Rendering.ShaderUtility.ViewDistance);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LightLimits(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.PushUnityEngineVector2(L, Minecraft.Rendering.ShaderUtility.LightLimits);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_WorldAmbientColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.PushUnityEngineColor(L, Minecraft.Rendering.ShaderUtility.WorldAmbientColor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TargetedBlockPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.PushUnityEngineVector3(L, Minecraft.Rendering.ShaderUtility.TargetedBlockPosition);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DigProgress(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.xlua_pushinteger(L, Minecraft.Rendering.ShaderUtility.DigProgress);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BlockTextures(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    Minecraft.Rendering.ShaderUtility.BlockTextures = (UnityEngine.Texture2DArray)translator.GetObject(L, 1, typeof(UnityEngine.Texture2DArray));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_RenderDistance(RealStatePtr L)
        {
		    try {
                
			    Minecraft.Rendering.ShaderUtility.RenderDistance = LuaAPI.xlua_tointeger(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ViewDistance(RealStatePtr L)
        {
		    try {
                
			    Minecraft.Rendering.ShaderUtility.ViewDistance = LuaAPI.xlua_tointeger(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LightLimits(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			UnityEngine.Vector2 gen_value;translator.Get(L, 1, out gen_value);
				Minecraft.Rendering.ShaderUtility.LightLimits = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_WorldAmbientColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			UnityEngine.Color gen_value;translator.Get(L, 1, out gen_value);
				Minecraft.Rendering.ShaderUtility.WorldAmbientColor = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_TargetedBlockPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			UnityEngine.Vector3 gen_value;translator.Get(L, 1, out gen_value);
				Minecraft.Rendering.ShaderUtility.TargetedBlockPosition = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DigProgress(RealStatePtr L)
        {
		    try {
                
			    Minecraft.Rendering.ShaderUtility.DigProgress = LuaAPI.xlua_tointeger(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}

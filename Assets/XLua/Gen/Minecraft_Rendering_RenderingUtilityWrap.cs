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
    public class MinecraftRenderingRenderingUtilityWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Minecraft.Rendering.RenderingUtility);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 10, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "GetSectionIndex", _m_GetSectionIndex_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetSectionY", _m_GetSectionY_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetSection", _m_GetSection_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CalculateFrustumPlanes", _m_CalculateFrustumPlanes_xlua_st_);
            
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FrustumPlaneCount", Minecraft.Rendering.RenderingUtility.FrustumPlaneCount);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SectionHeight", Minecraft.Rendering.RenderingUtility.SectionHeight);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SectionCountInChunk", Minecraft.Rendering.RenderingUtility.SectionCountInChunk);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SectionMeshTopology", Minecraft.Rendering.RenderingUtility.SectionMeshTopology);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SectionMeshUpdateFlags", Minecraft.Rendering.RenderingUtility.SectionMeshUpdateFlags);
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Minecraft.Rendering.RenderingUtility does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSectionIndex_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    float _y = (float)LuaAPI.lua_tonumber(L, 1);
                    
                        var gen_ret = Minecraft.Rendering.RenderingUtility.GetSectionIndex( _y );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSectionY_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    int _sectionIndex = LuaAPI.xlua_tointeger(L, 1);
                    
                        var gen_ret = Minecraft.Rendering.RenderingUtility.GetSectionY( _sectionIndex );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSection_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    float _x = (float)LuaAPI.lua_tonumber(L, 1);
                    float _y = (float)LuaAPI.lua_tonumber(L, 2);
                    float _z = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        var gen_ret = Minecraft.Rendering.RenderingUtility.GetSection( _x, _y, _z );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CalculateFrustumPlanes_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 1, typeof(UnityEngine.Camera));
                    UnityEngine.Transform _transform = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
                    Unity.Collections.NativeArray<Unity.Mathematics.float4> _planes;translator.Get(L, 3, out _planes);
                    
                    Minecraft.Rendering.RenderingUtility.CalculateFrustumPlanes( _camera, _transform, _planes );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}

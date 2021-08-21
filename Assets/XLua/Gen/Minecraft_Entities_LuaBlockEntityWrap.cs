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
    public class MinecraftEntitiesLuaBlockEntityWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Minecraft.Entities.LuaBlockEntity);
			Utils.BeginObjectRegister(type, L, translator, 0, 3, 4, 1);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetBlockAndPosition", _m_SetBlockAndPosition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnRecycle", _m_OnRecycle);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Render", _m_Render);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "EnableRendering", _g_get_EnableRendering);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SharedMesh", _g_get_SharedMesh);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SharedMaterial", _g_get_SharedMaterial);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MaterialProperty", _g_get_MaterialProperty);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "EnableRendering", _s_set_EnableRendering);
            
			
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
					
					var gen_ret = new Minecraft.Entities.LuaBlockEntity();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Minecraft.Entities.LuaBlockEntity constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetBlockAndPosition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Entities.LuaBlockEntity gen_to_be_invoked = (Minecraft.Entities.LuaBlockEntity)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Minecraft.Configurations.BlockData _block = (Minecraft.Configurations.BlockData)translator.GetObject(L, 2, typeof(Minecraft.Configurations.BlockData));
                    UnityEngine.Vector3Int _position;translator.Get(L, 3, out _position);
                    
                    gen_to_be_invoked.SetBlockAndPosition( _block, _position );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnRecycle(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Entities.LuaBlockEntity gen_to_be_invoked = (Minecraft.Entities.LuaBlockEntity)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnRecycle(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Render(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Entities.LuaBlockEntity gen_to_be_invoked = (Minecraft.Entities.LuaBlockEntity)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _layer = LuaAPI.xlua_tointeger(L, 2);
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 3, typeof(UnityEngine.Camera));
                    UnityEngine.Rendering.ShadowCastingMode _castShadows;translator.Get(L, 4, out _castShadows);
                    bool _receiveShadows = LuaAPI.lua_toboolean(L, 5);
                    
                    gen_to_be_invoked.Render( _layer, _camera, _castShadows, _receiveShadows );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EnableRendering(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Entities.LuaBlockEntity gen_to_be_invoked = (Minecraft.Entities.LuaBlockEntity)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.EnableRendering);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SharedMesh(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Entities.LuaBlockEntity gen_to_be_invoked = (Minecraft.Entities.LuaBlockEntity)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SharedMesh);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SharedMaterial(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Entities.LuaBlockEntity gen_to_be_invoked = (Minecraft.Entities.LuaBlockEntity)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.SharedMaterial);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MaterialProperty(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Entities.LuaBlockEntity gen_to_be_invoked = (Minecraft.Entities.LuaBlockEntity)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.MaterialProperty);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_EnableRendering(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Entities.LuaBlockEntity gen_to_be_invoked = (Minecraft.Entities.LuaBlockEntity)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.EnableRendering = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}

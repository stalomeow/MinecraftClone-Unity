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
    public class MinecraftAssetsAssetManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Minecraft.Assets.AssetManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 10, 2, 1);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Update", _m_Update);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LogAssetCatalog", _m_LogAssetCatalog);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadAssetBundle", _m_LoadAssetBundle);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnloadAssetBundle", _m_UnloadAssetBundle);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadAsset", _m_LoadAsset);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadAssets", _m_LoadAssets);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadAllAssets", _m_LoadAllAssets);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnloadAsset", _m_UnloadAsset);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnloadAssets", _m_UnloadAssets);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnloadAll", _m_UnloadAll);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "AssetBundleDirectory", _g_get_AssetBundleDirectory);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EnableLog", _g_get_EnableLog);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "EnableLog", _s_set_EnableLog);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 1, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "InitializeIfNeeded", _m_InitializeIfNeeded_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "Instance", _g_get_Instance);
            
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Minecraft.Assets.AssetManager does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitializeIfNeeded_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _assetBundleDirectory = LuaAPI.lua_tostring(L, 1);
                    
                    Minecraft.Assets.AssetManager.InitializeIfNeeded( _assetBundleDirectory );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Update(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Assets.AssetManager gen_to_be_invoked = (Minecraft.Assets.AssetManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Update(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LogAssetCatalog(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Assets.AssetManager gen_to_be_invoked = (Minecraft.Assets.AssetManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.LogAssetCatalog(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadAssetBundle(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Assets.AssetManager gen_to_be_invoked = (Minecraft.Assets.AssetManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _name = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.LoadAssetBundle( _name );
                        translator.PushAny(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnloadAssetBundle(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Assets.AssetManager gen_to_be_invoked = (Minecraft.Assets.AssetManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Minecraft.Assets.IAssetBundle _assetBundle = (Minecraft.Assets.IAssetBundle)translator.GetObject(L, 2, typeof(Minecraft.Assets.IAssetBundle));
                    
                    gen_to_be_invoked.UnloadAssetBundle( _assetBundle );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadAsset(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Assets.AssetManager gen_to_be_invoked = (Minecraft.Assets.AssetManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Type>(L, 3)) 
                {
                    string _name = LuaAPI.lua_tostring(L, 2);
                    System.Type _type = (System.Type)translator.GetObject(L, 3, typeof(System.Type));
                    
                        var gen_ret = gen_to_be_invoked.LoadAsset( _name, _type );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<Minecraft.Assets.AssetPtr>(L, 2)&& translator.Assignable<System.Type>(L, 3)) 
                {
                    Minecraft.Assets.AssetPtr _ptr = (Minecraft.Assets.AssetPtr)translator.GetObject(L, 2, typeof(Minecraft.Assets.AssetPtr));
                    System.Type _type = (System.Type)translator.GetObject(L, 3, typeof(System.Type));
                    
                        var gen_ret = gen_to_be_invoked.LoadAsset( _ptr, _type );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Minecraft.Assets.AssetManager.LoadAsset!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadAssets(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Assets.AssetManager gen_to_be_invoked = (Minecraft.Assets.AssetManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count >= 2&& translator.Assignable<System.Type>(L, 2)&& (LuaTypes.LUA_TNONE == LuaAPI.lua_type(L, 3) || (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING))) 
                {
                    System.Type _type = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    string[] _names = translator.GetParams<string>(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.LoadAssets( _type, _names );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count >= 2&& translator.Assignable<System.Type>(L, 2)&& (LuaTypes.LUA_TNONE == LuaAPI.lua_type(L, 3) || translator.Assignable<Minecraft.Assets.AssetPtr>(L, 3))) 
                {
                    System.Type _type = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    Minecraft.Assets.AssetPtr[] _ptrs = translator.GetParams<Minecraft.Assets.AssetPtr>(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.LoadAssets( _type, _ptrs );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Minecraft.Assets.AssetManager.LoadAssets!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadAllAssets(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Assets.AssetManager gen_to_be_invoked = (Minecraft.Assets.AssetManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _assetBundleName = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.LoadAllAssets( _assetBundleName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnloadAsset(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Assets.AssetManager gen_to_be_invoked = (Minecraft.Assets.AssetManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _name = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.UnloadAsset( _name );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<Minecraft.Assets.AssetPtr>(L, 2)) 
                {
                    Minecraft.Assets.AssetPtr _ptr = (Minecraft.Assets.AssetPtr)translator.GetObject(L, 2, typeof(Minecraft.Assets.AssetPtr));
                    
                    gen_to_be_invoked.UnloadAsset( _ptr );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<Minecraft.Assets.AsyncAsset>(L, 2)) 
                {
                    Minecraft.Assets.AsyncAsset _asset = (Minecraft.Assets.AsyncAsset)translator.GetObject(L, 2, typeof(Minecraft.Assets.AsyncAsset));
                    
                    gen_to_be_invoked.UnloadAsset( _asset );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Minecraft.Assets.AssetManager.UnloadAsset!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnloadAssets(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Assets.AssetManager gen_to_be_invoked = (Minecraft.Assets.AssetManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count >= 1&& (LuaTypes.LUA_TNONE == LuaAPI.lua_type(L, 2) || (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING))) 
                {
                    string[] _names = translator.GetParams<string>(L, 2);
                    
                    gen_to_be_invoked.UnloadAssets( _names );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count >= 1&& (LuaTypes.LUA_TNONE == LuaAPI.lua_type(L, 2) || translator.Assignable<Minecraft.Assets.AssetPtr>(L, 2))) 
                {
                    Minecraft.Assets.AssetPtr[] _ptrs = translator.GetParams<Minecraft.Assets.AssetPtr>(L, 2);
                    
                    gen_to_be_invoked.UnloadAssets( _ptrs );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count >= 1&& (LuaTypes.LUA_TNONE == LuaAPI.lua_type(L, 2) || translator.Assignable<Minecraft.Assets.AsyncAsset>(L, 2))) 
                {
                    Minecraft.Assets.AsyncAsset[] _assets = translator.GetParams<Minecraft.Assets.AsyncAsset>(L, 2);
                    
                    gen_to_be_invoked.UnloadAssets( _assets );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Minecraft.Assets.AssetManager.UnloadAssets!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnloadAll(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Assets.AssetManager gen_to_be_invoked = (Minecraft.Assets.AssetManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.UnloadAll(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Instance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Minecraft.Assets.AssetManager.Instance);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AssetBundleDirectory(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Assets.AssetManager gen_to_be_invoked = (Minecraft.Assets.AssetManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.AssetBundleDirectory);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EnableLog(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Assets.AssetManager gen_to_be_invoked = (Minecraft.Assets.AssetManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.EnableLog);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_EnableLog(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Assets.AssetManager gen_to_be_invoked = (Minecraft.Assets.AssetManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.EnableLog = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}

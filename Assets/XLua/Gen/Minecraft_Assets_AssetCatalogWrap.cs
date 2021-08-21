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
    public class MinecraftAssetsAssetCatalogWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Minecraft.Assets.AssetCatalog);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 2, 2);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Assets", _g_get_Assets);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AssetBundles", _g_get_AssetBundles);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "Assets", _s_set_Assets);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AssetBundles", _s_set_AssetBundles);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 0, 0);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FileName", Minecraft.Assets.AssetCatalog.FileName);
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new Minecraft.Assets.AssetCatalog();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Minecraft.Assets.AssetCatalog constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Assets(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Assets.AssetCatalog gen_to_be_invoked = (Minecraft.Assets.AssetCatalog)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Assets);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AssetBundles(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Assets.AssetCatalog gen_to_be_invoked = (Minecraft.Assets.AssetCatalog)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.AssetBundles);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Assets(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Assets.AssetCatalog gen_to_be_invoked = (Minecraft.Assets.AssetCatalog)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Assets = (System.Collections.Generic.Dictionary<string, Minecraft.Assets.AssetInfo>)translator.GetObject(L, 2, typeof(System.Collections.Generic.Dictionary<string, Minecraft.Assets.AssetInfo>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AssetBundles(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Assets.AssetCatalog gen_to_be_invoked = (Minecraft.Assets.AssetCatalog)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AssetBundles = (System.Collections.Generic.Dictionary<string, Minecraft.Assets.AssetBundleInfo>)translator.GetObject(L, 2, typeof(System.Collections.Generic.Dictionary<string, Minecraft.Assets.AssetBundleInfo>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}

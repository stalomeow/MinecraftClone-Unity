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
    public class MinecraftEntityManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Minecraft.EntityManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 4, 2, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreateEntity", _m_CreateEntity);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "DestroyEntity", _m_DestroyEntity);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "EnumerateEntities", _m_EnumerateEntities);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "EnumerateOtherEntities", _m_EnumerateOtherEntities);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "BlockEntityMaterial", _g_get_BlockEntityMaterial);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PlayerObj", _g_get_PlayerObj);
            
			
			
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
				if(LuaAPI.lua_gettop(L) == 3 && translator.Assignable<UnityEngine.Material>(L, 2) && translator.Assignable<Minecraft.PlayerEntity>(L, 3))
				{
					UnityEngine.Material _blockEntityMaterial = (UnityEngine.Material)translator.GetObject(L, 2, typeof(UnityEngine.Material));
					Minecraft.PlayerEntity _player = (Minecraft.PlayerEntity)translator.GetObject(L, 3, typeof(Minecraft.PlayerEntity));
					
					Minecraft.EntityManager gen_ret = new Minecraft.EntityManager(_blockEntityMaterial, _player);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Minecraft.EntityManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateEntity(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.EntityManager gen_to_be_invoked = (Minecraft.EntityManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<System.Type>(L, 2)) 
                {
                    System.Type _type = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    
                        Minecraft.Entity gen_ret = gen_to_be_invoked.CreateEntity( _type );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<Minecraft.Entity>(L, 2)) 
                {
                    Minecraft.Entity _prefab = (Minecraft.Entity)translator.GetObject(L, 2, typeof(Minecraft.Entity));
                    
                        Minecraft.Entity gen_ret = gen_to_be_invoked.CreateEntity( _prefab );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Minecraft.EntityManager.CreateEntity!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DestroyEntity(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.EntityManager gen_to_be_invoked = (Minecraft.EntityManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Minecraft.Entity _entity = (Minecraft.Entity)translator.GetObject(L, 2, typeof(Minecraft.Entity));
                    
                    gen_to_be_invoked.DestroyEntity( _entity );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_EnumerateEntities(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.EntityManager gen_to_be_invoked = (Minecraft.EntityManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        Minecraft.EntityManager.EntityEnumerator gen_ret = gen_to_be_invoked.EnumerateEntities(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_EnumerateOtherEntities(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.EntityManager gen_to_be_invoked = (Minecraft.EntityManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Minecraft.Entity _self = (Minecraft.Entity)translator.GetObject(L, 2, typeof(Minecraft.Entity));
                    
                        Minecraft.EntityManager.EntityEnumerator gen_ret = gen_to_be_invoked.EnumerateOtherEntities( _self );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BlockEntityMaterial(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.EntityManager gen_to_be_invoked = (Minecraft.EntityManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.BlockEntityMaterial);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PlayerObj(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.EntityManager gen_to_be_invoked = (Minecraft.EntityManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PlayerObj);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}

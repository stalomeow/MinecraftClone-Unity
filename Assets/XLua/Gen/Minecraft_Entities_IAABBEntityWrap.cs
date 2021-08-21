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
    public class MinecraftEntitiesIAABBEntityWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Minecraft.Entities.IAABBEntity);
			Utils.BeginObjectRegister(type, L, translator, 0, 5, 11, 6);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "InitializeEntityIfNot", _m_InitializeEntityIfNot);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnRecycle", _m_OnRecycle);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetIsGrounded", _m_GetIsGrounded);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddInstantForce", _m_AddInstantForce);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnCollisions", _e_OnCollisions);
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Initialized", _g_get_Initialized);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Mass", _g_get_Mass);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BoundingBox", _g_get_BoundingBox);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "GravityMultiplier", _g_get_GravityMultiplier);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UseGravity", _g_get_UseGravity);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PhysicMaterial", _g_get_PhysicMaterial);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsGrounded", _g_get_IsGrounded);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Velocity", _g_get_Velocity);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Position", _g_get_Position);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LocalPosition", _g_get_LocalPosition);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "World", _g_get_World);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "Mass", _s_set_Mass);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BoundingBox", _s_set_BoundingBox);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "GravityMultiplier", _s_set_GravityMultiplier);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UseGravity", _s_set_UseGravity);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Position", _s_set_Position);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LocalPosition", _s_set_LocalPosition);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Minecraft.Entities.IAABBEntity does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_InitializeEntityIfNot(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Entities.IAABBEntity gen_to_be_invoked = (Minecraft.Entities.IAABBEntity)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.InitializeEntityIfNot(  );
                    
                    
                    
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
            
            
                Minecraft.Entities.IAABBEntity gen_to_be_invoked = (Minecraft.Entities.IAABBEntity)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.OnRecycle(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetIsGrounded(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Entities.IAABBEntity gen_to_be_invoked = (Minecraft.Entities.IAABBEntity)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Minecraft.Configurations.BlockData _groundBlock;
                    
                        var gen_ret = gen_to_be_invoked.GetIsGrounded( out _groundBlock );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _groundBlock);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddInstantForce(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Entities.IAABBEntity gen_to_be_invoked = (Minecraft.Entities.IAABBEntity)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3 _force;translator.Get(L, 2, out _force);
                    
                    gen_to_be_invoked.AddInstantForce( _force );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Initialized(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Entities.IAABBEntity gen_to_be_invoked = (Minecraft.Entities.IAABBEntity)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.Initialized);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Mass(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Entities.IAABBEntity gen_to_be_invoked = (Minecraft.Entities.IAABBEntity)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.Mass);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BoundingBox(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Entities.IAABBEntity gen_to_be_invoked = (Minecraft.Entities.IAABBEntity)translator.FastGetCSObj(L, 1);
                translator.PushMinecraftPhysicSystemAABB(L, gen_to_be_invoked.BoundingBox);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_GravityMultiplier(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Entities.IAABBEntity gen_to_be_invoked = (Minecraft.Entities.IAABBEntity)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.GravityMultiplier);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UseGravity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Entities.IAABBEntity gen_to_be_invoked = (Minecraft.Entities.IAABBEntity)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.UseGravity);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PhysicMaterial(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Entities.IAABBEntity gen_to_be_invoked = (Minecraft.Entities.IAABBEntity)translator.FastGetCSObj(L, 1);
                translator.PushMinecraftPhysicSystemPhysicMaterial(L, gen_to_be_invoked.PhysicMaterial);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsGrounded(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Entities.IAABBEntity gen_to_be_invoked = (Minecraft.Entities.IAABBEntity)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.IsGrounded);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Velocity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Entities.IAABBEntity gen_to_be_invoked = (Minecraft.Entities.IAABBEntity)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.Velocity);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Position(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Entities.IAABBEntity gen_to_be_invoked = (Minecraft.Entities.IAABBEntity)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.Position);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LocalPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Entities.IAABBEntity gen_to_be_invoked = (Minecraft.Entities.IAABBEntity)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.LocalPosition);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_World(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Entities.IAABBEntity gen_to_be_invoked = (Minecraft.Entities.IAABBEntity)translator.FastGetCSObj(L, 1);
                translator.PushAny(L, gen_to_be_invoked.World);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Mass(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Entities.IAABBEntity gen_to_be_invoked = (Minecraft.Entities.IAABBEntity)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Mass = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BoundingBox(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Entities.IAABBEntity gen_to_be_invoked = (Minecraft.Entities.IAABBEntity)translator.FastGetCSObj(L, 1);
                Minecraft.PhysicSystem.AABB gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.BoundingBox = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_GravityMultiplier(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Entities.IAABBEntity gen_to_be_invoked = (Minecraft.Entities.IAABBEntity)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.GravityMultiplier = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UseGravity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Entities.IAABBEntity gen_to_be_invoked = (Minecraft.Entities.IAABBEntity)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UseGravity = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Position(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Entities.IAABBEntity gen_to_be_invoked = (Minecraft.Entities.IAABBEntity)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector3 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.Position = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LocalPosition(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Entities.IAABBEntity gen_to_be_invoked = (Minecraft.Entities.IAABBEntity)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector3 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.LocalPosition = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnCollisions(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			Minecraft.Entities.IAABBEntity gen_to_be_invoked = (Minecraft.Entities.IAABBEntity)translator.FastGetCSObj(L, 1);
                UnityEngine.Events.UnityAction<UnityEngine.CollisionFlags> gen_delegate = translator.GetDelegate<UnityEngine.Events.UnityAction<UnityEngine.CollisionFlags>>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need UnityEngine.Events.UnityAction<UnityEngine.CollisionFlags>!");
                }
				
				if (gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						gen_to_be_invoked.OnCollisions += gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						gen_to_be_invoked.OnCollisions -= gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to Minecraft.Entities.IAABBEntity.OnCollisions!");
            return 0;
        }
        
		
		
    }
}

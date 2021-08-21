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
    public class MinecraftPhysicSystemAABBWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Minecraft.PhysicSystem.AABB);
			Utils.BeginObjectRegister(type, L, translator, 2, 1, 3, 2);
			Utils.RegisterFunc(L, Utils.OBJ_META_IDX, "__add", __AddMeta);
            Utils.RegisterFunc(L, Utils.OBJ_META_IDX, "__sub", __SubMeta);
            
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Intersects", _m_Intersects);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Min", _g_get_Min);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Max", _g_get_Max);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Center", _g_get_Center);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "Min", _s_set_Min);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Max", _s_set_Max);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 3, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "Merge", _m_Merge_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Transform", _m_Transform_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 3 && translator.Assignable<UnityEngine.Vector3>(L, 2) && translator.Assignable<UnityEngine.Vector3>(L, 3))
				{
					UnityEngine.Vector3 _min;translator.Get(L, 2, out _min);
					UnityEngine.Vector3 _max;translator.Get(L, 3, out _max);
					
					var gen_ret = new Minecraft.PhysicSystem.AABB(_min, _max);
					translator.PushMinecraftPhysicSystemAABB(L, gen_ret);
                    
					return 1;
				}
				
				if (LuaAPI.lua_gettop(L) == 1)
				{
				    translator.PushMinecraftPhysicSystemAABB(L, default(Minecraft.PhysicSystem.AABB));
			        return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Minecraft.PhysicSystem.AABB constructor!");
            
        }
        
		
        
		
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __AddMeta(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
			
				if (translator.Assignable<Minecraft.PhysicSystem.AABB>(L, 1) && translator.Assignable<Minecraft.PhysicSystem.AABB>(L, 2))
				{
					Minecraft.PhysicSystem.AABB leftside;translator.Get(L, 1, out leftside);
					Minecraft.PhysicSystem.AABB rightside;translator.Get(L, 2, out rightside);
					
					translator.PushMinecraftPhysicSystemAABB(L, leftside + rightside);
					
					return 1;
				}
            
			
				if (translator.Assignable<Minecraft.PhysicSystem.AABB>(L, 1) && translator.Assignable<UnityEngine.Vector3>(L, 2))
				{
					Minecraft.PhysicSystem.AABB leftside;translator.Get(L, 1, out leftside);
					UnityEngine.Vector3 rightside;translator.Get(L, 2, out rightside);
					
					translator.PushMinecraftPhysicSystemAABB(L, leftside + rightside);
					
					return 1;
				}
            
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to right hand of + operator, need Minecraft.PhysicSystem.AABB!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __SubMeta(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
			
				if (translator.Assignable<Minecraft.PhysicSystem.AABB>(L, 1) && translator.Assignable<UnityEngine.Vector3>(L, 2))
				{
					Minecraft.PhysicSystem.AABB leftside;translator.Get(L, 1, out leftside);
					UnityEngine.Vector3 rightside;translator.Get(L, 2, out rightside);
					
					translator.PushMinecraftPhysicSystemAABB(L, leftside - rightside);
					
					return 1;
				}
            
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to right hand of - operator, need Minecraft.PhysicSystem.AABB!");
            
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Intersects(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.PhysicSystem.AABB gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
            
            
                
                {
                    Minecraft.PhysicSystem.AABB _aabb;translator.Get(L, 2, out _aabb);
                    
                        var gen_ret = gen_to_be_invoked.Intersects( _aabb );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                        translator.UpdateMinecraftPhysicSystemAABB(L, 1, gen_to_be_invoked);
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Merge_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Minecraft.PhysicSystem.AABB _left;translator.Get(L, 1, out _left);
                    Minecraft.PhysicSystem.AABB _right;translator.Get(L, 2, out _right);
                    
                        var gen_ret = Minecraft.PhysicSystem.AABB.Merge( _left, _right );
                        translator.PushMinecraftPhysicSystemAABB(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Transform_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    Minecraft.PhysicSystem.AABB _aabb;translator.Get(L, 1, out _aabb);
                    UnityEngine.Vector3 _transform;translator.Get(L, 2, out _transform);
                    
                        var gen_ret = Minecraft.PhysicSystem.AABB.Transform( _aabb, _transform );
                        translator.PushMinecraftPhysicSystemAABB(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Min(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.PhysicSystem.AABB gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.Min);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Max(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.PhysicSystem.AABB gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.Max);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Center(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.PhysicSystem.AABB gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.Center);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Min(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.PhysicSystem.AABB gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                UnityEngine.Vector3 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.Min = gen_value;
            
                translator.UpdateMinecraftPhysicSystemAABB(L, 1, gen_to_be_invoked);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Max(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.PhysicSystem.AABB gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
                UnityEngine.Vector3 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.Max = gen_value;
            
                translator.UpdateMinecraftPhysicSystemAABB(L, 1, gen_to_be_invoked);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}

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
    public class MinecraftPhysicSystemPhysicsWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Minecraft.PhysicSystem.Physics);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 4, 1, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "RaycastBlock", _m_RaycastBlock_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CheckGroundedAABB", _m_CheckGroundedAABB_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CollideWithTerrainAABB", _m_CollideWithTerrainAABB_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "Gravity", _g_get_Gravity);
            
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "Minecraft.PhysicSystem.Physics does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RaycastBlock_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Ray>(L, 1)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& translator.Assignable<Minecraft.IWorld>(L, 3)&& translator.Assignable<System.Func<Minecraft.Configurations.BlockData, bool>>(L, 4)) 
                {
                    UnityEngine.Ray _ray;translator.Get(L, 1, out _ray);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 2);
                    Minecraft.IWorld _world = (Minecraft.IWorld)translator.GetObject(L, 3, typeof(Minecraft.IWorld));
                    System.Func<Minecraft.Configurations.BlockData, bool> _selectBlock = translator.GetDelegate<System.Func<Minecraft.Configurations.BlockData, bool>>(L, 4);
                    Minecraft.PhysicSystem.BlockRaycastHit _hit;
                    
                        var gen_ret = Minecraft.PhysicSystem.Physics.RaycastBlock( _ray, _maxDistance, _world, _selectBlock, out _hit );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _hit);
                        
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& translator.Assignable<Minecraft.IWorld>(L, 3)&& translator.Assignable<System.Func<Minecraft.Configurations.BlockData, bool>>(L, 4)) 
                {
                    UnityEngine.Vector3 _start;translator.Get(L, 1, out _start);
                    UnityEngine.Vector3 _end;translator.Get(L, 2, out _end);
                    Minecraft.IWorld _world = (Minecraft.IWorld)translator.GetObject(L, 3, typeof(Minecraft.IWorld));
                    System.Func<Minecraft.Configurations.BlockData, bool> _selectBlock = translator.GetDelegate<System.Func<Minecraft.Configurations.BlockData, bool>>(L, 4);
                    Minecraft.PhysicSystem.BlockRaycastHit _hit;
                    
                        var gen_ret = Minecraft.PhysicSystem.Physics.RaycastBlock( _start, _end, _world, _selectBlock, out _hit );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _hit);
                        
                    
                    
                    
                    return 2;
                }
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.Vector3>(L, 1)&& translator.Assignable<UnityEngine.Vector3>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& translator.Assignable<Minecraft.IWorld>(L, 4)&& translator.Assignable<System.Func<Minecraft.Configurations.BlockData, bool>>(L, 5)) 
                {
                    UnityEngine.Vector3 _origin;translator.Get(L, 1, out _origin);
                    UnityEngine.Vector3 _direction;translator.Get(L, 2, out _direction);
                    float _maxDistance = (float)LuaAPI.lua_tonumber(L, 3);
                    Minecraft.IWorld _world = (Minecraft.IWorld)translator.GetObject(L, 4, typeof(Minecraft.IWorld));
                    System.Func<Minecraft.Configurations.BlockData, bool> _selectBlock = translator.GetDelegate<System.Func<Minecraft.Configurations.BlockData, bool>>(L, 5);
                    Minecraft.PhysicSystem.BlockRaycastHit _hit;
                    
                        var gen_ret = Minecraft.PhysicSystem.Physics.RaycastBlock( _origin, _direction, _maxDistance, _world, _selectBlock, out _hit );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _hit);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Minecraft.PhysicSystem.Physics.RaycastBlock!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CheckGroundedAABB_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 _position;translator.Get(L, 1, out _position);
                    Minecraft.PhysicSystem.AABB _aabb;translator.Get(L, 2, out _aabb);
                    Minecraft.IWorld _world = (Minecraft.IWorld)translator.GetObject(L, 3, typeof(Minecraft.IWorld));
                    Minecraft.Configurations.BlockData _groundBlock;
                    
                        var gen_ret = Minecraft.PhysicSystem.Physics.CheckGroundedAABB( _position, _aabb, _world, out _groundBlock );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    translator.Push(L, _groundBlock);
                        
                    
                    
                    
                    return 2;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CollideWithTerrainAABB_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Vector3 _position;translator.Get(L, 1, out _position);
                    Minecraft.PhysicSystem.AABB _aabb;translator.Get(L, 2, out _aabb);
                    Minecraft.IWorld _world = (Minecraft.IWorld)translator.GetObject(L, 3, typeof(Minecraft.IWorld));
                    Minecraft.PhysicSystem.PhysicMaterial _material;translator.Get(L, 4, out _material);
                    float _time = (float)LuaAPI.lua_tonumber(L, 5);
                    UnityEngine.Vector3 _velocity;translator.Get(L, 6, out _velocity);
                    UnityEngine.Vector3 _movement;
                    
                        var gen_ret = Minecraft.PhysicSystem.Physics.CollideWithTerrainAABB( _position, _aabb, _world, _material, _time, ref _velocity, out _movement );
                        translator.Push(L, gen_ret);
                    translator.PushUnityEngineVector3(L, _velocity);
                        translator.UpdateUnityEngineVector3(L, 6, _velocity);
                        
                    translator.PushUnityEngineVector3(L, _movement);
                        
                    
                    
                    
                    return 3;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Gravity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.PushUnityEngineVector3(L, Minecraft.PhysicSystem.Physics.Gravity);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}

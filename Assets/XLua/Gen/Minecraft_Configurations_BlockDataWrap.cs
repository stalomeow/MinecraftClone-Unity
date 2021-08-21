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
using Minecraft.Rendering;using Minecraft.PhysicSystem;using Minecraft.Configurations;

namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class MinecraftConfigurationsBlockDataWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Minecraft.Configurations.BlockData);
			Utils.BeginObjectRegister(type, L, translator, 0, 13, 16, 16);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsOpaqueBlock", _m_IsOpaqueBlock);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetEmissionValue", _m_GetEmissionValue);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetBoundingBox", _m_GetBoundingBox);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "HasFlag", _m_HasFlag);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Tick", _m_Tick);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Place", _m_Place);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Destroy", _m_Destroy);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Click", _m_Click);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "EntityInit", _m_EntityInit);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "EntityDestroy", _m_EntityDestroy);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "EntityUpdate", _m_EntityUpdate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "EntityFixedUpdate", _m_EntityFixedUpdate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "EntityOnCollisions", _m_EntityOnCollisions);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "ID", _g_get_ID);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "InternalName", _g_get_InternalName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "RewardItem", _g_get_RewardItem);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Flags", _g_get_Flags);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EntityConversion", _g_get_EntityConversion);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Hardness", _g_get_Hardness);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LightValue", _g_get_LightValue);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LightOpacity", _g_get_LightOpacity);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PhysicState", _g_get_PhysicState);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PhysicMaterial", _g_get_PhysicMaterial);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Mesh", _g_get_Mesh);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Material", _g_get_Material);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Textures", _g_get_Textures);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DigAudio", _g_get_DigAudio);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PlaceAudio", _g_get_PlaceAudio);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "StepAudios", _g_get_StepAudios);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "ID", _s_set_ID);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "InternalName", _s_set_InternalName);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "RewardItem", _s_set_RewardItem);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Flags", _s_set_Flags);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "EntityConversion", _s_set_EntityConversion);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Hardness", _s_set_Hardness);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LightValue", _s_set_LightValue);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LightOpacity", _s_set_LightOpacity);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PhysicState", _s_set_PhysicState);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PhysicMaterial", _s_set_PhysicMaterial);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Mesh", _s_set_Mesh);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Material", _s_set_Material);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Textures", _s_set_Textures);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "DigAudio", _s_set_DigAudio);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PlaceAudio", _s_set_PlaceAudio);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "StepAudios", _s_set_StepAudios);
            
			
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
					
					var gen_ret = new Minecraft.Configurations.BlockData();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Minecraft.Configurations.BlockData constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsOpaqueBlock(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.IsOpaqueBlock(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetEmissionValue(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetEmissionValue(  );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBoundingBox(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    float _x = (float)LuaAPI.lua_tonumber(L, 2);
                    float _y = (float)LuaAPI.lua_tonumber(L, 3);
                    float _z = (float)LuaAPI.lua_tonumber(L, 4);
                    
                        var gen_ret = gen_to_be_invoked.GetBoundingBox( _x, _y, _z );
                        translator.PushMinecraftPhysicSystemAABB(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.Vector3>(L, 2)) 
                {
                    UnityEngine.Vector3 _position;translator.Get(L, 2, out _position);
                    
                        var gen_ret = gen_to_be_invoked.GetBoundingBox( _position );
                        translator.PushMinecraftPhysicSystemAABB(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Minecraft.Configurations.BlockData.GetBoundingBox!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HasFlag(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Minecraft.Configurations.BlockFlags _flag;translator.Get(L, 2, out _flag);
                    
                        var gen_ret = gen_to_be_invoked.HasFlag( _flag );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Tick(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Minecraft.IWorld _world = (Minecraft.IWorld)translator.GetObject(L, 2, typeof(Minecraft.IWorld));
                    int _x = LuaAPI.xlua_tointeger(L, 3);
                    int _y = LuaAPI.xlua_tointeger(L, 4);
                    int _z = LuaAPI.xlua_tointeger(L, 5);
                    
                    gen_to_be_invoked.Tick( _world, _x, _y, _z );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Place(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Minecraft.IWorld _world = (Minecraft.IWorld)translator.GetObject(L, 2, typeof(Minecraft.IWorld));
                    int _x = LuaAPI.xlua_tointeger(L, 3);
                    int _y = LuaAPI.xlua_tointeger(L, 4);
                    int _z = LuaAPI.xlua_tointeger(L, 5);
                    
                    gen_to_be_invoked.Place( _world, _x, _y, _z );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Destroy(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Minecraft.IWorld _world = (Minecraft.IWorld)translator.GetObject(L, 2, typeof(Minecraft.IWorld));
                    int _x = LuaAPI.xlua_tointeger(L, 3);
                    int _y = LuaAPI.xlua_tointeger(L, 4);
                    int _z = LuaAPI.xlua_tointeger(L, 5);
                    
                    gen_to_be_invoked.Destroy( _world, _x, _y, _z );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Click(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Minecraft.IWorld _world = (Minecraft.IWorld)translator.GetObject(L, 2, typeof(Minecraft.IWorld));
                    int _x = LuaAPI.xlua_tointeger(L, 3);
                    int _y = LuaAPI.xlua_tointeger(L, 4);
                    int _z = LuaAPI.xlua_tointeger(L, 5);
                    
                    gen_to_be_invoked.Click( _world, _x, _y, _z );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_EntityInit(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Minecraft.IWorld _world = (Minecraft.IWorld)translator.GetObject(L, 2, typeof(Minecraft.IWorld));
                    Minecraft.Entities.IAABBEntity _entity = (Minecraft.Entities.IAABBEntity)translator.GetObject(L, 3, typeof(Minecraft.Entities.IAABBEntity));
                    XLua.LuaTable _context = (XLua.LuaTable)translator.GetObject(L, 4, typeof(XLua.LuaTable));
                    
                    gen_to_be_invoked.EntityInit( _world, _entity, _context );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_EntityDestroy(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Minecraft.IWorld _world = (Minecraft.IWorld)translator.GetObject(L, 2, typeof(Minecraft.IWorld));
                    Minecraft.Entities.IAABBEntity _entity = (Minecraft.Entities.IAABBEntity)translator.GetObject(L, 3, typeof(Minecraft.Entities.IAABBEntity));
                    XLua.LuaTable _context = (XLua.LuaTable)translator.GetObject(L, 4, typeof(XLua.LuaTable));
                    
                    gen_to_be_invoked.EntityDestroy( _world, _entity, _context );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_EntityUpdate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Minecraft.IWorld _world = (Minecraft.IWorld)translator.GetObject(L, 2, typeof(Minecraft.IWorld));
                    Minecraft.Entities.IAABBEntity _entity = (Minecraft.Entities.IAABBEntity)translator.GetObject(L, 3, typeof(Minecraft.Entities.IAABBEntity));
                    XLua.LuaTable _context = (XLua.LuaTable)translator.GetObject(L, 4, typeof(XLua.LuaTable));
                    
                    gen_to_be_invoked.EntityUpdate( _world, _entity, _context );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_EntityFixedUpdate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Minecraft.IWorld _world = (Minecraft.IWorld)translator.GetObject(L, 2, typeof(Minecraft.IWorld));
                    Minecraft.Entities.IAABBEntity _entity = (Minecraft.Entities.IAABBEntity)translator.GetObject(L, 3, typeof(Minecraft.Entities.IAABBEntity));
                    XLua.LuaTable _context = (XLua.LuaTable)translator.GetObject(L, 4, typeof(XLua.LuaTable));
                    
                    gen_to_be_invoked.EntityFixedUpdate( _world, _entity, _context );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_EntityOnCollisions(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Minecraft.IWorld _world = (Minecraft.IWorld)translator.GetObject(L, 2, typeof(Minecraft.IWorld));
                    Minecraft.Entities.IAABBEntity _entity = (Minecraft.Entities.IAABBEntity)translator.GetObject(L, 3, typeof(Minecraft.Entities.IAABBEntity));
                    UnityEngine.CollisionFlags _flags;translator.Get(L, 4, out _flags);
                    XLua.LuaTable _context = (XLua.LuaTable)translator.GetObject(L, 5, typeof(XLua.LuaTable));
                    
                    gen_to_be_invoked.EntityOnCollisions( _world, _entity, _flags, _context );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ID(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.ID);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_InternalName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.InternalName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_RewardItem(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.RewardItem);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Flags(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                translator.PushMinecraftConfigurationsBlockFlags(L, gen_to_be_invoked.Flags);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EntityConversion(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                translator.PushMinecraftConfigurationsBlockEntityConversion(L, gen_to_be_invoked.EntityConversion);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Hardness(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.Hardness);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LightValue(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.LightValue);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LightOpacity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.LightOpacity);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PhysicState(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                translator.PushMinecraftPhysicSystemPhysicState(L, gen_to_be_invoked.PhysicState);
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
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                translator.PushMinecraftPhysicSystemPhysicMaterial(L, gen_to_be_invoked.PhysicMaterial);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Mesh(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                translator.PushAny(L, gen_to_be_invoked.Mesh);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Material(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                translator.PushAny(L, gen_to_be_invoked.Material);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Textures(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Textures);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DigAudio(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.DigAudio);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PlaceAudio(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PlaceAudio);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_StepAudios(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.StepAudios);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ID(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ID = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_InternalName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.InternalName = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_RewardItem(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.RewardItem = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Flags(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                Minecraft.Configurations.BlockFlags gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.Flags = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_EntityConversion(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                Minecraft.Configurations.BlockEntityConversion gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.EntityConversion = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Hardness(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Hardness = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LightValue(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LightValue = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LightOpacity(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LightOpacity = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PhysicState(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                Minecraft.PhysicSystem.PhysicState gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.PhysicState = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PhysicMaterial(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                Minecraft.PhysicSystem.PhysicMaterial gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.PhysicMaterial = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Mesh(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                System.Nullable<int> gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.Mesh = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Material(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                System.Nullable<int> gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.Material = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Textures(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Textures = (System.Nullable<int>[][])translator.GetObject(L, 2, typeof(System.Nullable<int>[][]));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DigAudio(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.DigAudio = (Minecraft.Assets.AssetPtr)translator.GetObject(L, 2, typeof(Minecraft.Assets.AssetPtr));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PlaceAudio(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PlaceAudio = (Minecraft.Assets.AssetPtr)translator.GetObject(L, 2, typeof(Minecraft.Assets.AssetPtr));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_StepAudios(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BlockData gen_to_be_invoked = (Minecraft.Configurations.BlockData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.StepAudios = (System.Collections.Generic.List<Minecraft.Assets.AssetPtr>)translator.GetObject(L, 2, typeof(System.Collections.Generic.List<Minecraft.Assets.AssetPtr>));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}

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
    
    public class MinecraftModificationSourceWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Minecraft.ModificationSource), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Minecraft.ModificationSource), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Minecraft.ModificationSource), L, null, 3, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "InternalOrSystem", Minecraft.ModificationSource.InternalOrSystem);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PlayerAction", Minecraft.ModificationSource.PlayerAction);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Minecraft.ModificationSource), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushMinecraftModificationSource(L, (Minecraft.ModificationSource)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "InternalOrSystem"))
                {
                    translator.PushMinecraftModificationSource(L, Minecraft.ModificationSource.InternalOrSystem);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "PlayerAction"))
                {
                    translator.PushMinecraftModificationSource(L, Minecraft.ModificationSource.PlayerAction);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Minecraft.ModificationSource!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Minecraft.ModificationSource! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class MinecraftPhysicSystemPhysicStateWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Minecraft.PhysicSystem.PhysicState), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Minecraft.PhysicSystem.PhysicState), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Minecraft.PhysicSystem.PhysicState), L, null, 3, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Solid", Minecraft.PhysicSystem.PhysicState.Solid);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Fluid", Minecraft.PhysicSystem.PhysicState.Fluid);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Minecraft.PhysicSystem.PhysicState), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushMinecraftPhysicSystemPhysicState(L, (Minecraft.PhysicSystem.PhysicState)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Solid"))
                {
                    translator.PushMinecraftPhysicSystemPhysicState(L, Minecraft.PhysicSystem.PhysicState.Solid);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Fluid"))
                {
                    translator.PushMinecraftPhysicSystemPhysicState(L, Minecraft.PhysicSystem.PhysicState.Fluid);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Minecraft.PhysicSystem.PhysicState!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Minecraft.PhysicSystem.PhysicState! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class MinecraftConfigurationsBiomeIdWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Minecraft.Configurations.BiomeId), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Minecraft.Configurations.BiomeId), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Minecraft.Configurations.BiomeId), L, null, 52, 0, 0);

            Utils.RegisterEnumType(L, typeof(Minecraft.Configurations.BiomeId));

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Minecraft.Configurations.BiomeId), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushMinecraftConfigurationsBiomeId(L, (Minecraft.Configurations.BiomeId)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

                try
				{
                    translator.TranslateToEnumToTop(L, typeof(Minecraft.Configurations.BiomeId), 1);
				}
				catch (System.Exception e)
				{
					return LuaAPI.luaL_error(L, "cast to " + typeof(Minecraft.Configurations.BiomeId) + " exception:" + e);
				}

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Minecraft.Configurations.BiomeId! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class MinecraftConfigurationsBlockEntityConversionWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Minecraft.Configurations.BlockEntityConversion), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Minecraft.Configurations.BlockEntityConversion), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Minecraft.Configurations.BlockEntityConversion), L, null, 4, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Never", Minecraft.Configurations.BlockEntityConversion.Never);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Initial", Minecraft.Configurations.BlockEntityConversion.Initial);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Conditional", Minecraft.Configurations.BlockEntityConversion.Conditional);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Minecraft.Configurations.BlockEntityConversion), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushMinecraftConfigurationsBlockEntityConversion(L, (Minecraft.Configurations.BlockEntityConversion)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Never"))
                {
                    translator.PushMinecraftConfigurationsBlockEntityConversion(L, Minecraft.Configurations.BlockEntityConversion.Never);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Initial"))
                {
                    translator.PushMinecraftConfigurationsBlockEntityConversion(L, Minecraft.Configurations.BlockEntityConversion.Initial);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Conditional"))
                {
                    translator.PushMinecraftConfigurationsBlockEntityConversion(L, Minecraft.Configurations.BlockEntityConversion.Conditional);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Minecraft.Configurations.BlockEntityConversion!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Minecraft.Configurations.BlockEntityConversion! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class MinecraftConfigurationsBlockFaceWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Minecraft.Configurations.BlockFace), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Minecraft.Configurations.BlockFace), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Minecraft.Configurations.BlockFace), L, null, 7, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PositiveX", Minecraft.Configurations.BlockFace.PositiveX);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PositiveY", Minecraft.Configurations.BlockFace.PositiveY);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PositiveZ", Minecraft.Configurations.BlockFace.PositiveZ);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "NegativeX", Minecraft.Configurations.BlockFace.NegativeX);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "NegativeY", Minecraft.Configurations.BlockFace.NegativeY);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "NegativeZ", Minecraft.Configurations.BlockFace.NegativeZ);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Minecraft.Configurations.BlockFace), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushMinecraftConfigurationsBlockFace(L, (Minecraft.Configurations.BlockFace)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "PositiveX"))
                {
                    translator.PushMinecraftConfigurationsBlockFace(L, Minecraft.Configurations.BlockFace.PositiveX);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "PositiveY"))
                {
                    translator.PushMinecraftConfigurationsBlockFace(L, Minecraft.Configurations.BlockFace.PositiveY);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "PositiveZ"))
                {
                    translator.PushMinecraftConfigurationsBlockFace(L, Minecraft.Configurations.BlockFace.PositiveZ);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "NegativeX"))
                {
                    translator.PushMinecraftConfigurationsBlockFace(L, Minecraft.Configurations.BlockFace.NegativeX);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "NegativeY"))
                {
                    translator.PushMinecraftConfigurationsBlockFace(L, Minecraft.Configurations.BlockFace.NegativeY);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "NegativeZ"))
                {
                    translator.PushMinecraftConfigurationsBlockFace(L, Minecraft.Configurations.BlockFace.NegativeZ);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Minecraft.Configurations.BlockFace!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Minecraft.Configurations.BlockFace! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class MinecraftConfigurationsBlockFaceCornerWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Minecraft.Configurations.BlockFaceCorner), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Minecraft.Configurations.BlockFaceCorner), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Minecraft.Configurations.BlockFaceCorner), L, null, 5, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LeftBottom", Minecraft.Configurations.BlockFaceCorner.LeftBottom);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "RightBottom", Minecraft.Configurations.BlockFaceCorner.RightBottom);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LeftTop", Minecraft.Configurations.BlockFaceCorner.LeftTop);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "RightTop", Minecraft.Configurations.BlockFaceCorner.RightTop);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Minecraft.Configurations.BlockFaceCorner), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushMinecraftConfigurationsBlockFaceCorner(L, (Minecraft.Configurations.BlockFaceCorner)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "LeftBottom"))
                {
                    translator.PushMinecraftConfigurationsBlockFaceCorner(L, Minecraft.Configurations.BlockFaceCorner.LeftBottom);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "RightBottom"))
                {
                    translator.PushMinecraftConfigurationsBlockFaceCorner(L, Minecraft.Configurations.BlockFaceCorner.RightBottom);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "LeftTop"))
                {
                    translator.PushMinecraftConfigurationsBlockFaceCorner(L, Minecraft.Configurations.BlockFaceCorner.LeftTop);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "RightTop"))
                {
                    translator.PushMinecraftConfigurationsBlockFaceCorner(L, Minecraft.Configurations.BlockFaceCorner.RightTop);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Minecraft.Configurations.BlockFaceCorner!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Minecraft.Configurations.BlockFaceCorner! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class MinecraftConfigurationsBlockFlagsWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Minecraft.Configurations.BlockFlags), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Minecraft.Configurations.BlockFlags), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Minecraft.Configurations.BlockFlags), L, null, 8, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", Minecraft.Configurations.BlockFlags.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "IgnoreCollisions", Minecraft.Configurations.BlockFlags.IgnoreCollisions);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "IgnorePlaceBlockRaycast", Minecraft.Configurations.BlockFlags.IgnorePlaceBlockRaycast);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "IgnoreDestroyBlockRaycast", Minecraft.Configurations.BlockFlags.IgnoreDestroyBlockRaycast);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Reserved", Minecraft.Configurations.BlockFlags.Reserved);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "IgnoreExplosions", Minecraft.Configurations.BlockFlags.IgnoreExplosions);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AlwaysInvisible", Minecraft.Configurations.BlockFlags.AlwaysInvisible);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Minecraft.Configurations.BlockFlags), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushMinecraftConfigurationsBlockFlags(L, (Minecraft.Configurations.BlockFlags)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushMinecraftConfigurationsBlockFlags(L, Minecraft.Configurations.BlockFlags.None);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "IgnoreCollisions"))
                {
                    translator.PushMinecraftConfigurationsBlockFlags(L, Minecraft.Configurations.BlockFlags.IgnoreCollisions);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "IgnorePlaceBlockRaycast"))
                {
                    translator.PushMinecraftConfigurationsBlockFlags(L, Minecraft.Configurations.BlockFlags.IgnorePlaceBlockRaycast);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "IgnoreDestroyBlockRaycast"))
                {
                    translator.PushMinecraftConfigurationsBlockFlags(L, Minecraft.Configurations.BlockFlags.IgnoreDestroyBlockRaycast);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Reserved"))
                {
                    translator.PushMinecraftConfigurationsBlockFlags(L, Minecraft.Configurations.BlockFlags.Reserved);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "IgnoreExplosions"))
                {
                    translator.PushMinecraftConfigurationsBlockFlags(L, Minecraft.Configurations.BlockFlags.IgnoreExplosions);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "AlwaysInvisible"))
                {
                    translator.PushMinecraftConfigurationsBlockFlags(L, Minecraft.Configurations.BlockFlags.AlwaysInvisible);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Minecraft.Configurations.BlockFlags!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Minecraft.Configurations.BlockFlags! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
}
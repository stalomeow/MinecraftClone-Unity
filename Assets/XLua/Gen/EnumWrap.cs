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
    
    public class MinecraftItemsDataItemTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Minecraft.ItemsData.ItemType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Minecraft.ItemsData.ItemType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Minecraft.ItemsData.ItemType), L, null, 13, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", Minecraft.ItemsData.ItemType.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Glass", Minecraft.ItemsData.ItemType.Glass);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Log_Oak", Minecraft.ItemsData.ItemType.Log_Oak);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TNT", Minecraft.ItemsData.ItemType.TNT);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "GlowStone", Minecraft.ItemsData.ItemType.GlowStone);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "BookShelf", Minecraft.ItemsData.ItemType.BookShelf);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "CobbleStone", Minecraft.ItemsData.ItemType.CobbleStone);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Planks_Oak", Minecraft.ItemsData.ItemType.Planks_Oak);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "CraftingTable", Minecraft.ItemsData.ItemType.CraftingTable);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "DiamondBlock", Minecraft.ItemsData.ItemType.DiamondBlock);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Noteblock", Minecraft.ItemsData.ItemType.Noteblock);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Bilibili_SanLian", Minecraft.ItemsData.ItemType.Bilibili_SanLian);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Minecraft.ItemsData.ItemType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushMinecraftItemsDataItemType(L, (Minecraft.ItemsData.ItemType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushMinecraftItemsDataItemType(L, Minecraft.ItemsData.ItemType.None);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Glass"))
                {
                    translator.PushMinecraftItemsDataItemType(L, Minecraft.ItemsData.ItemType.Glass);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Log_Oak"))
                {
                    translator.PushMinecraftItemsDataItemType(L, Minecraft.ItemsData.ItemType.Log_Oak);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "TNT"))
                {
                    translator.PushMinecraftItemsDataItemType(L, Minecraft.ItemsData.ItemType.TNT);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "GlowStone"))
                {
                    translator.PushMinecraftItemsDataItemType(L, Minecraft.ItemsData.ItemType.GlowStone);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "BookShelf"))
                {
                    translator.PushMinecraftItemsDataItemType(L, Minecraft.ItemsData.ItemType.BookShelf);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "CobbleStone"))
                {
                    translator.PushMinecraftItemsDataItemType(L, Minecraft.ItemsData.ItemType.CobbleStone);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Planks_Oak"))
                {
                    translator.PushMinecraftItemsDataItemType(L, Minecraft.ItemsData.ItemType.Planks_Oak);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "CraftingTable"))
                {
                    translator.PushMinecraftItemsDataItemType(L, Minecraft.ItemsData.ItemType.CraftingTable);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "DiamondBlock"))
                {
                    translator.PushMinecraftItemsDataItemType(L, Minecraft.ItemsData.ItemType.DiamondBlock);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Noteblock"))
                {
                    translator.PushMinecraftItemsDataItemType(L, Minecraft.ItemsData.ItemType.Noteblock);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Bilibili_SanLian"))
                {
                    translator.PushMinecraftItemsDataItemType(L, Minecraft.ItemsData.ItemType.Bilibili_SanLian);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Minecraft.ItemsData.ItemType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Minecraft.ItemsData.ItemType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class MinecraftBlocksDataBlockFlagsWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Minecraft.BlocksData.BlockFlags), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Minecraft.BlocksData.BlockFlags), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Minecraft.BlocksData.BlockFlags), L, null, 10, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", Minecraft.BlocksData.BlockFlags.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "IgnoreCollisions", Minecraft.BlocksData.BlockFlags.IgnoreCollisions);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "IgnorePlaceBlockRaycast", Minecraft.BlocksData.BlockFlags.IgnorePlaceBlockRaycast);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "IgnoreDestroyBlockRaycast", Minecraft.BlocksData.BlockFlags.IgnoreDestroyBlockRaycast);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "AffectedByGravity", Minecraft.BlocksData.BlockFlags.AffectedByGravity);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "NeedsRandomTick", Minecraft.BlocksData.BlockFlags.NeedsRandomTick);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Liquid", Minecraft.BlocksData.BlockFlags.Liquid);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "FlowersAndPlants", Minecraft.BlocksData.BlockFlags.FlowersAndPlants);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "IgnoreExplosions", Minecraft.BlocksData.BlockFlags.IgnoreExplosions);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Minecraft.BlocksData.BlockFlags), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushMinecraftBlocksDataBlockFlags(L, (Minecraft.BlocksData.BlockFlags)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushMinecraftBlocksDataBlockFlags(L, Minecraft.BlocksData.BlockFlags.None);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "IgnoreCollisions"))
                {
                    translator.PushMinecraftBlocksDataBlockFlags(L, Minecraft.BlocksData.BlockFlags.IgnoreCollisions);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "IgnorePlaceBlockRaycast"))
                {
                    translator.PushMinecraftBlocksDataBlockFlags(L, Minecraft.BlocksData.BlockFlags.IgnorePlaceBlockRaycast);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "IgnoreDestroyBlockRaycast"))
                {
                    translator.PushMinecraftBlocksDataBlockFlags(L, Minecraft.BlocksData.BlockFlags.IgnoreDestroyBlockRaycast);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "AffectedByGravity"))
                {
                    translator.PushMinecraftBlocksDataBlockFlags(L, Minecraft.BlocksData.BlockFlags.AffectedByGravity);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "NeedsRandomTick"))
                {
                    translator.PushMinecraftBlocksDataBlockFlags(L, Minecraft.BlocksData.BlockFlags.NeedsRandomTick);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Liquid"))
                {
                    translator.PushMinecraftBlocksDataBlockFlags(L, Minecraft.BlocksData.BlockFlags.Liquid);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "FlowersAndPlants"))
                {
                    translator.PushMinecraftBlocksDataBlockFlags(L, Minecraft.BlocksData.BlockFlags.FlowersAndPlants);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "IgnoreExplosions"))
                {
                    translator.PushMinecraftBlocksDataBlockFlags(L, Minecraft.BlocksData.BlockFlags.IgnoreExplosions);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Minecraft.BlocksData.BlockFlags!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Minecraft.BlocksData.BlockFlags! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class MinecraftBlocksDataBlockTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Minecraft.BlocksData.BlockType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Minecraft.BlocksData.BlockType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Minecraft.BlocksData.BlockType), L, null, 28, 0, 0);

            foreach(var e in System.Enum.GetValues(typeof(Minecraft.BlocksData.BlockType)))
			{
			    Utils.RegisterObject(L, translator, Utils.CLS_IDX, e.ToString(), e);
			}

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Minecraft.BlocksData.BlockType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushMinecraftBlocksDataBlockType(L, (Minecraft.BlocksData.BlockType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

                try
				{
                    translator.TranslateToEnumToTop(L, typeof(Minecraft.BlocksData.BlockType), 1);
				}
				catch (System.Exception e)
				{
					return LuaAPI.luaL_error(L, "cast to " + typeof(Minecraft.BlocksData.BlockType) + " exception:" + e);
				}

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Minecraft.BlocksData.BlockType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class MinecraftBlocksDataBlockVertexTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(Minecraft.BlocksData.BlockVertexType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(Minecraft.BlocksData.BlockVertexType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(Minecraft.BlocksData.BlockVertexType), L, null, 4, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "None", Minecraft.BlocksData.BlockVertexType.None);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Cube", Minecraft.BlocksData.BlockVertexType.Cube);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PerpendicularQuads", Minecraft.BlocksData.BlockVertexType.PerpendicularQuads);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(Minecraft.BlocksData.BlockVertexType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushMinecraftBlocksDataBlockVertexType(L, (Minecraft.BlocksData.BlockVertexType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "None"))
                {
                    translator.PushMinecraftBlocksDataBlockVertexType(L, Minecraft.BlocksData.BlockVertexType.None);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Cube"))
                {
                    translator.PushMinecraftBlocksDataBlockVertexType(L, Minecraft.BlocksData.BlockVertexType.Cube);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "PerpendicularQuads"))
                {
                    translator.PushMinecraftBlocksDataBlockVertexType(L, Minecraft.BlocksData.BlockVertexType.PerpendicularQuads);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for Minecraft.BlocksData.BlockVertexType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for Minecraft.BlocksData.BlockVertexType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
}
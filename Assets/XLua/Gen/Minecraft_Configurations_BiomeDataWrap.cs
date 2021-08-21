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
    public class MinecraftConfigurationsBiomeDataWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Minecraft.Configurations.BiomeData);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 22, 22);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "ID", _g_get_ID);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "InternalName", _g_get_InternalName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BaseHeight", _g_get_BaseHeight);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "HeightVariation", _g_get_HeightVariation);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Temperature", _g_get_Temperature);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Rainfall", _g_get_Rainfall);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EnableSnow", _g_get_EnableSnow);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EnableRain", _g_get_EnableRain);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TopBlock", _g_get_TopBlock);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FillerBlock", _g_get_FillerBlock);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TreesPerChunk", _g_get_TreesPerChunk);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ExtraTreeChance", _g_get_ExtraTreeChance);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "GrassPerChunk", _g_get_GrassPerChunk);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "FlowersPerChunk", _g_get_FlowersPerChunk);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MushroomsPerChunk", _g_get_MushroomsPerChunk);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DeadBushPerChunk", _g_get_DeadBushPerChunk);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ReedsPerChunk", _g_get_ReedsPerChunk);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CactiPerChunk", _g_get_CactiPerChunk);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ClayPerChunk", _g_get_ClayPerChunk);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "WaterlilyPerChunk", _g_get_WaterlilyPerChunk);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SandPatchesPerChunk", _g_get_SandPatchesPerChunk);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "GravelPatchesPerChunk", _g_get_GravelPatchesPerChunk);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "ID", _s_set_ID);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "InternalName", _s_set_InternalName);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BaseHeight", _s_set_BaseHeight);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "HeightVariation", _s_set_HeightVariation);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Temperature", _s_set_Temperature);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Rainfall", _s_set_Rainfall);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "EnableSnow", _s_set_EnableSnow);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "EnableRain", _s_set_EnableRain);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "TopBlock", _s_set_TopBlock);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "FillerBlock", _s_set_FillerBlock);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "TreesPerChunk", _s_set_TreesPerChunk);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ExtraTreeChance", _s_set_ExtraTreeChance);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "GrassPerChunk", _s_set_GrassPerChunk);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "FlowersPerChunk", _s_set_FlowersPerChunk);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "MushroomsPerChunk", _s_set_MushroomsPerChunk);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "DeadBushPerChunk", _s_set_DeadBushPerChunk);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ReedsPerChunk", _s_set_ReedsPerChunk);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CactiPerChunk", _s_set_CactiPerChunk);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ClayPerChunk", _s_set_ClayPerChunk);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "WaterlilyPerChunk", _s_set_WaterlilyPerChunk);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SandPatchesPerChunk", _s_set_SandPatchesPerChunk);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "GravelPatchesPerChunk", _s_set_GravelPatchesPerChunk);
            
			
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
					
					var gen_ret = new Minecraft.Configurations.BiomeData();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Minecraft.Configurations.BiomeData constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ID(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
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
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.InternalName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BaseHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.BaseHeight);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_HeightVariation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.HeightVariation);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Temperature(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.Temperature);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Rainfall(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.Rainfall);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EnableSnow(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.EnableSnow);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EnableRain(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.EnableRain);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TopBlock(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.TopBlock);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FillerBlock(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.FillerBlock);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TreesPerChunk(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.TreesPerChunk);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ExtraTreeChance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.ExtraTreeChance);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_GrassPerChunk(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.GrassPerChunk);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_FlowersPerChunk(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.FlowersPerChunk);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MushroomsPerChunk(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.MushroomsPerChunk);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DeadBushPerChunk(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.DeadBushPerChunk);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ReedsPerChunk(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.ReedsPerChunk);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CactiPerChunk(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.CactiPerChunk);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ClayPerChunk(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.ClayPerChunk);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_WaterlilyPerChunk(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.WaterlilyPerChunk);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SandPatchesPerChunk(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.SandPatchesPerChunk);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_GravelPatchesPerChunk(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.GravelPatchesPerChunk);
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
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
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
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.InternalName = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BaseHeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.BaseHeight = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_HeightVariation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.HeightVariation = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Temperature(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Temperature = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Rainfall(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Rainfall = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_EnableSnow(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.EnableSnow = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_EnableRain(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.EnableRain = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_TopBlock(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.TopBlock = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_FillerBlock(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.FillerBlock = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_TreesPerChunk(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.TreesPerChunk = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ExtraTreeChance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ExtraTreeChance = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_GrassPerChunk(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.GrassPerChunk = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_FlowersPerChunk(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.FlowersPerChunk = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_MushroomsPerChunk(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.MushroomsPerChunk = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DeadBushPerChunk(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.DeadBushPerChunk = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ReedsPerChunk(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ReedsPerChunk = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CactiPerChunk(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CactiPerChunk = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ClayPerChunk(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ClayPerChunk = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_WaterlilyPerChunk(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.WaterlilyPerChunk = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SandPatchesPerChunk(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SandPatchesPerChunk = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_GravelPatchesPerChunk(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.Configurations.BiomeData gen_to_be_invoked = (Minecraft.Configurations.BiomeData)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.GravelPatchesPerChunk = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}

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
    public class MinecraftScriptableWorldGenerationTerrainGeneratorWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Minecraft.ScriptableWorldGeneration.TerrainGenerator);
			Utils.BeginObjectRegister(type, L, translator, 0, 1, 20, 20);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Generate", _m_Generate);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "AirBlock", _g_get_AirBlock);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "StoneBlock", _g_get_StoneBlock);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "WaterBlock", _g_get_WaterBlock);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "GravelBlock", _g_get_GravelBlock);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BedrockBlock", _g_get_BedrockBlock);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SeaLevel", _g_get_SeaLevel);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CoordinateScale", _g_get_CoordinateScale);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "HeightScale", _g_get_HeightScale);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BiomeDepthOffset", _g_get_BiomeDepthOffset);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BiomeDepthWeight", _g_get_BiomeDepthWeight);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BiomeScaleOffset", _g_get_BiomeScaleOffset);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BiomeScaleWeight", _g_get_BiomeScaleWeight);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BaseSize", _g_get_BaseSize);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "StretchY", _g_get_StretchY);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LowerLimitScale", _g_get_LowerLimitScale);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UpperLimitScale", _g_get_UpperLimitScale);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BiomeSize", _g_get_BiomeSize);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "RiverSize", _g_get_RiverSize);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DepthNoiseScale", _g_get_DepthNoiseScale);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MainNoiseScale", _g_get_MainNoiseScale);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "AirBlock", _s_set_AirBlock);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "StoneBlock", _s_set_StoneBlock);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "WaterBlock", _s_set_WaterBlock);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "GravelBlock", _s_set_GravelBlock);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BedrockBlock", _s_set_BedrockBlock);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SeaLevel", _s_set_SeaLevel);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CoordinateScale", _s_set_CoordinateScale);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "HeightScale", _s_set_HeightScale);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BiomeDepthOffset", _s_set_BiomeDepthOffset);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BiomeDepthWeight", _s_set_BiomeDepthWeight);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BiomeScaleOffset", _s_set_BiomeScaleOffset);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BiomeScaleWeight", _s_set_BiomeScaleWeight);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BaseSize", _s_set_BaseSize);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "StretchY", _s_set_StretchY);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "LowerLimitScale", _s_set_LowerLimitScale);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UpperLimitScale", _s_set_UpperLimitScale);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BiomeSize", _s_set_BiomeSize);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "RiverSize", _s_set_RiverSize);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "DepthNoiseScale", _s_set_DepthNoiseScale);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "MainNoiseScale", _s_set_MainNoiseScale);
            
			
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
					
					var gen_ret = new Minecraft.ScriptableWorldGeneration.TerrainGenerator();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Minecraft.ScriptableWorldGeneration.TerrainGenerator constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Generate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Minecraft.IWorld _world = (Minecraft.IWorld)translator.GetObject(L, 2, typeof(Minecraft.IWorld));
                    Minecraft.ChunkPos _pos;translator.Get(L, 3, out _pos);
                    Minecraft.Configurations.BlockData[,,] _blocks = (Minecraft.Configurations.BlockData[,,])translator.GetObject(L, 4, typeof(Minecraft.Configurations.BlockData[,,]));
                    byte[,] _heightMap = (byte[,])translator.GetObject(L, 5, typeof(byte[,]));
                    Minecraft.ScriptableWorldGeneration.GenerationHelper _helper = (Minecraft.ScriptableWorldGeneration.GenerationHelper)translator.GetObject(L, 6, typeof(Minecraft.ScriptableWorldGeneration.GenerationHelper));
                    Minecraft.ScriptableWorldGeneration.GenerationContext _context = (Minecraft.ScriptableWorldGeneration.GenerationContext)translator.GetObject(L, 7, typeof(Minecraft.ScriptableWorldGeneration.GenerationContext));
                    
                    gen_to_be_invoked.Generate( _world, _pos, _blocks, _heightMap, _helper, _context );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AirBlock(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.AirBlock);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_StoneBlock(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.StoneBlock);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_WaterBlock(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.WaterBlock);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_GravelBlock(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.GravelBlock);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BedrockBlock(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.BedrockBlock);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SeaLevel(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.SeaLevel);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CoordinateScale(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.CoordinateScale);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_HeightScale(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.HeightScale);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BiomeDepthOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.BiomeDepthOffset);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BiomeDepthWeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.BiomeDepthWeight);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BiomeScaleOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.BiomeScaleOffset);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BiomeScaleWeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.BiomeScaleWeight);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BaseSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.BaseSize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_StretchY(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.StretchY);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LowerLimitScale(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.LowerLimitScale);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UpperLimitScale(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.UpperLimitScale);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BiomeSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.BiomeSize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_RiverSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.RiverSize);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DepthNoiseScale(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.DepthNoiseScale);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MainNoiseScale(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                translator.PushUnityEngineVector3(L, gen_to_be_invoked.MainNoiseScale);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AirBlock(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AirBlock = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_StoneBlock(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.StoneBlock = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_WaterBlock(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.WaterBlock = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_GravelBlock(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.GravelBlock = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BedrockBlock(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.BedrockBlock = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SeaLevel(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SeaLevel = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CoordinateScale(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CoordinateScale = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_HeightScale(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.HeightScale = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BiomeDepthOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.BiomeDepthOffset = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BiomeDepthWeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.BiomeDepthWeight = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BiomeScaleOffset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.BiomeScaleOffset = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BiomeScaleWeight(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.BiomeScaleWeight = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BaseSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.BaseSize = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_StretchY(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.StretchY = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_LowerLimitScale(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.LowerLimitScale = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UpperLimitScale(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UpperLimitScale = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BiomeSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.BiomeSize = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_RiverSize(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.RiverSize = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_DepthNoiseScale(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector3 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.DepthNoiseScale = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_MainNoiseScale(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.ScriptableWorldGeneration.TerrainGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.TerrainGenerator)translator.FastGetCSObj(L, 1);
                UnityEngine.Vector3 gen_value;translator.Get(L, 2, out gen_value);
				gen_to_be_invoked.MainNoiseScale = gen_value;
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}

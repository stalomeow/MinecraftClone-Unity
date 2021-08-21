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
    public class MinecraftScriptableWorldGenerationMineGeneratorWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Minecraft.ScriptableWorldGeneration.MineGenerator);
			Utils.BeginObjectRegister(type, L, translator, 0, 1, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Generate", _m_Generate);
			
			
			
			
			
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
					
					var gen_ret = new Minecraft.ScriptableWorldGeneration.MineGenerator();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Minecraft.ScriptableWorldGeneration.MineGenerator constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Generate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.ScriptableWorldGeneration.MineGenerator gen_to_be_invoked = (Minecraft.ScriptableWorldGeneration.MineGenerator)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 8&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<Minecraft.Configurations.BlockData[,,]>(L, 5)&& translator.Assignable<System.Random>(L, 6)&& translator.Assignable<Minecraft.Configurations.BlockData>(L, 7)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 8)) 
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _z = LuaAPI.xlua_tointeger(L, 4);
                    Minecraft.Configurations.BlockData[,,] _blocks = (Minecraft.Configurations.BlockData[,,])translator.GetObject(L, 5, typeof(Minecraft.Configurations.BlockData[,,]));
                    System.Random _random = (System.Random)translator.GetObject(L, 6, typeof(System.Random));
                    Minecraft.Configurations.BlockData _ore = (Minecraft.Configurations.BlockData)translator.GetObject(L, 7, typeof(Minecraft.Configurations.BlockData));
                    int _count = LuaAPI.xlua_tointeger(L, 8);
                    
                    gen_to_be_invoked.Generate( _x, _y, _z, _blocks, _random, _ore, _count );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 7&& translator.Assignable<Minecraft.IWorld>(L, 2)&& translator.Assignable<Minecraft.ChunkPos>(L, 3)&& translator.Assignable<Minecraft.Configurations.BlockData[,,]>(L, 4)&& translator.Assignable<byte[,]>(L, 5)&& translator.Assignable<Minecraft.ScriptableWorldGeneration.GenerationHelper>(L, 6)&& translator.Assignable<Minecraft.ScriptableWorldGeneration.GenerationContext>(L, 7)) 
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
            
            return LuaAPI.luaL_error(L, "invalid arguments to Minecraft.ScriptableWorldGeneration.MineGenerator.Generate!");
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}

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
    public class MinecraftNoisesUniformRNGWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Minecraft.Noises.UniformRNG);
			Utils.BeginObjectRegister(type, L, translator, 0, 7, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "NextInt32", _m_NextInt32);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "NextUInt32", _m_NextUInt32);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "NextInt64", _m_NextInt64);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "NextUInt64", _m_NextUInt64);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "NextSingle", _m_NextSingle);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "NextDouble", _m_NextDouble);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Uniform", _m_Uniform);
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 3, 0, 0);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Multiplier", Minecraft.Noises.UniformRNG.Multiplier);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Increment", Minecraft.Noises.UniformRNG.Increment);
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 2 && (LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2) || LuaAPI.lua_isuint64(L, 2)))
				{
					ulong _state = LuaAPI.lua_touint64(L, 2);
					
					var gen_ret = new Minecraft.Noises.UniformRNG(_state);
					translator.PushMinecraftNoisesUniformRNG(L, gen_ret);
                    
					return 1;
				}
				
				if (LuaAPI.lua_gettop(L) == 1)
				{
				    translator.PushMinecraftNoisesUniformRNG(L, default(Minecraft.Noises.UniformRNG));
			        return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Minecraft.Noises.UniformRNG constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NextInt32(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Noises.UniformRNG gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.NextInt32(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                        translator.UpdateMinecraftNoisesUniformRNG(L, 1, gen_to_be_invoked);
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NextUInt32(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Noises.UniformRNG gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.NextUInt32(  );
                        LuaAPI.xlua_pushuint(L, gen_ret);
                    
                    
                        translator.UpdateMinecraftNoisesUniformRNG(L, 1, gen_to_be_invoked);
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NextInt64(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Noises.UniformRNG gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.NextInt64(  );
                        LuaAPI.lua_pushint64(L, gen_ret);
                    
                    
                        translator.UpdateMinecraftNoisesUniformRNG(L, 1, gen_to_be_invoked);
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NextUInt64(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Noises.UniformRNG gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.NextUInt64(  );
                        LuaAPI.lua_pushuint64(L, gen_ret);
                    
                    
                        translator.UpdateMinecraftNoisesUniformRNG(L, 1, gen_to_be_invoked);
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NextSingle(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Noises.UniformRNG gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.NextSingle(  );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                        translator.UpdateMinecraftNoisesUniformRNG(L, 1, gen_to_be_invoked);
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_NextDouble(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Noises.UniformRNG gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.NextDouble(  );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                        translator.UpdateMinecraftNoisesUniformRNG(L, 1, gen_to_be_invoked);
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Uniform(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Noises.UniformRNG gen_to_be_invoked;translator.Get(L, 1, out gen_to_be_invoked);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    int _a = LuaAPI.xlua_tointeger(L, 2);
                    int _b = LuaAPI.xlua_tointeger(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.Uniform( _a, _b );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                        translator.UpdateMinecraftNoisesUniformRNG(L, 1, gen_to_be_invoked);
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    uint _a = LuaAPI.xlua_touint(L, 2);
                    uint _b = LuaAPI.xlua_touint(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.Uniform( _a, _b );
                        LuaAPI.xlua_pushuint(L, gen_ret);
                    
                    
                        translator.UpdateMinecraftNoisesUniformRNG(L, 1, gen_to_be_invoked);
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& (LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2) || LuaAPI.lua_isint64(L, 2))&& (LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3) || LuaAPI.lua_isint64(L, 3))) 
                {
                    long _a = LuaAPI.lua_toint64(L, 2);
                    long _b = LuaAPI.lua_toint64(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.Uniform( _a, _b );
                        LuaAPI.lua_pushint64(L, gen_ret);
                    
                    
                        translator.UpdateMinecraftNoisesUniformRNG(L, 1, gen_to_be_invoked);
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& (LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2) || LuaAPI.lua_isuint64(L, 2))&& (LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3) || LuaAPI.lua_isuint64(L, 3))) 
                {
                    ulong _a = LuaAPI.lua_touint64(L, 2);
                    ulong _b = LuaAPI.lua_touint64(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.Uniform( _a, _b );
                        LuaAPI.lua_pushuint64(L, gen_ret);
                    
                    
                        translator.UpdateMinecraftNoisesUniformRNG(L, 1, gen_to_be_invoked);
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    float _a = (float)LuaAPI.lua_tonumber(L, 2);
                    float _b = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.Uniform( _a, _b );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                        translator.UpdateMinecraftNoisesUniformRNG(L, 1, gen_to_be_invoked);
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)) 
                {
                    double _a = LuaAPI.lua_tonumber(L, 2);
                    double _b = LuaAPI.lua_tonumber(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.Uniform( _a, _b );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                        translator.UpdateMinecraftNoisesUniformRNG(L, 1, gen_to_be_invoked);
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Minecraft.Noises.UniformRNG.Uniform!");
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}

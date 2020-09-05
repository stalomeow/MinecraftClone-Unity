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
    public class MinecraftBlocksDataBlockWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Minecraft.BlocksData.Block);
			Utils.BeginObjectRegister(type, L, translator, 0, 24, 9, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetExtraAsset", _m_GetExtraAsset);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnTick", _m_OnTick);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnRandomTick", _m_OnRandomTick);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnBlockDestroy", _m_OnBlockDestroy);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnBlockPlace", _m_OnBlockPlace);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnClick", _m_OnClick);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ClearEvents", _m_ClearEvents);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "HasAllFlags", _m_HasAllFlags);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "HasAnyFlag", _m_HasAnyFlag);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetMainUVForPerpendicularQuadsVertex", _m_GetMainUVForPerpendicularQuadsVertex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetPositiveXUVForCubeVertex", _m_GetPositiveXUVForCubeVertex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetPositiveYUVForCubeVertex", _m_GetPositiveYUVForCubeVertex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetPositiveZUVForCubeVertex", _m_GetPositiveZUVForCubeVertex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetNegativeXUVForCubeVertex", _m_GetNegativeXUVForCubeVertex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetNegativeYUVForCubeVertex", _m_GetNegativeYUVForCubeVertex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetNegativeZUVForCubeVertex", _m_GetNegativeZUVForCubeVertex);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayDigAudio", _m_PlayDigAudio);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayPlaceAudio", _m_PlayPlaceAudio);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PlayStepAutio", _m_PlayStepAutio);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnTickEvent", _e_OnTickEvent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnRandomTickEvent", _e_OnRandomTickEvent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnBlockDestroyEvent", _e_OnBlockDestroyEvent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnBlockPlaceEvent", _e_OnBlockPlaceEvent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnClickEvent", _e_OnClickEvent);
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "BlockName", _g_get_BlockName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Type", _g_get_Type);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Flags", _g_get_Flags);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "VertexType", _g_get_VertexType);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MoveResistance", _g_get_MoveResistance);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LightOpacity", _g_get_LightOpacity);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LightValue", _g_get_LightValue);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Hardness", _g_get_Hardness);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DestoryEffectColor", _g_get_DestoryEffectColor);
            
			
			
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
					
					Minecraft.BlocksData.Block gen_ret = new Minecraft.BlocksData.Block();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Minecraft.BlocksData.Block constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetExtraAsset(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    
                        UnityEngine.Object gen_ret = gen_to_be_invoked.GetExtraAsset( _index );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnTick(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _z = LuaAPI.xlua_tointeger(L, 4);
                    
                    gen_to_be_invoked.OnTick( _x, _y, _z );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnRandomTick(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _z = LuaAPI.xlua_tointeger(L, 4);
                    
                    gen_to_be_invoked.OnRandomTick( _x, _y, _z );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnBlockDestroy(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _z = LuaAPI.xlua_tointeger(L, 4);
                    
                    gen_to_be_invoked.OnBlockDestroy( _x, _y, _z );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnBlockPlace(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _z = LuaAPI.xlua_tointeger(L, 4);
                    
                    gen_to_be_invoked.OnBlockPlace( _x, _y, _z );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnClick(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _z = LuaAPI.xlua_tointeger(L, 4);
                    
                    gen_to_be_invoked.OnClick( _x, _y, _z );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearEvents(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ClearEvents(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HasAllFlags(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Minecraft.BlocksData.BlockFlags _flags;translator.Get(L, 2, out _flags);
                    
                        bool gen_ret = gen_to_be_invoked.HasAllFlags( _flags );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_HasAnyFlag(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Minecraft.BlocksData.BlockFlags _flags;translator.Get(L, 2, out _flags);
                    
                        bool gen_ret = gen_to_be_invoked.HasAnyFlag( _flags );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetMainUVForPerpendicularQuadsVertex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector2 _lb;
                    UnityEngine.Vector2 _rb;
                    UnityEngine.Vector2 _rt;
                    UnityEngine.Vector2 _lt;
                    
                    gen_to_be_invoked.GetMainUVForPerpendicularQuadsVertex( out _lb, out _rb, out _rt, out _lt );
                    translator.PushUnityEngineVector2(L, _lb);
                        
                    translator.PushUnityEngineVector2(L, _rb);
                        
                    translator.PushUnityEngineVector2(L, _rt);
                        
                    translator.PushUnityEngineVector2(L, _lt);
                        
                    
                    
                    
                    return 4;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPositiveXUVForCubeVertex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector2 _lb;
                    UnityEngine.Vector2 _rb;
                    UnityEngine.Vector2 _rt;
                    UnityEngine.Vector2 _lt;
                    
                    gen_to_be_invoked.GetPositiveXUVForCubeVertex( out _lb, out _rb, out _rt, out _lt );
                    translator.PushUnityEngineVector2(L, _lb);
                        
                    translator.PushUnityEngineVector2(L, _rb);
                        
                    translator.PushUnityEngineVector2(L, _rt);
                        
                    translator.PushUnityEngineVector2(L, _lt);
                        
                    
                    
                    
                    return 4;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPositiveYUVForCubeVertex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector2 _lb;
                    UnityEngine.Vector2 _rb;
                    UnityEngine.Vector2 _rt;
                    UnityEngine.Vector2 _lt;
                    
                    gen_to_be_invoked.GetPositiveYUVForCubeVertex( out _lb, out _rb, out _rt, out _lt );
                    translator.PushUnityEngineVector2(L, _lb);
                        
                    translator.PushUnityEngineVector2(L, _rb);
                        
                    translator.PushUnityEngineVector2(L, _rt);
                        
                    translator.PushUnityEngineVector2(L, _lt);
                        
                    
                    
                    
                    return 4;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetPositiveZUVForCubeVertex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector2 _lb;
                    UnityEngine.Vector2 _rb;
                    UnityEngine.Vector2 _rt;
                    UnityEngine.Vector2 _lt;
                    
                    gen_to_be_invoked.GetPositiveZUVForCubeVertex( out _lb, out _rb, out _rt, out _lt );
                    translator.PushUnityEngineVector2(L, _lb);
                        
                    translator.PushUnityEngineVector2(L, _rb);
                        
                    translator.PushUnityEngineVector2(L, _rt);
                        
                    translator.PushUnityEngineVector2(L, _lt);
                        
                    
                    
                    
                    return 4;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetNegativeXUVForCubeVertex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector2 _lb;
                    UnityEngine.Vector2 _rb;
                    UnityEngine.Vector2 _rt;
                    UnityEngine.Vector2 _lt;
                    
                    gen_to_be_invoked.GetNegativeXUVForCubeVertex( out _lb, out _rb, out _rt, out _lt );
                    translator.PushUnityEngineVector2(L, _lb);
                        
                    translator.PushUnityEngineVector2(L, _rb);
                        
                    translator.PushUnityEngineVector2(L, _rt);
                        
                    translator.PushUnityEngineVector2(L, _lt);
                        
                    
                    
                    
                    return 4;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetNegativeYUVForCubeVertex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector2 _lb;
                    UnityEngine.Vector2 _rb;
                    UnityEngine.Vector2 _rt;
                    UnityEngine.Vector2 _lt;
                    
                    gen_to_be_invoked.GetNegativeYUVForCubeVertex( out _lb, out _rb, out _rt, out _lt );
                    translator.PushUnityEngineVector2(L, _lb);
                        
                    translator.PushUnityEngineVector2(L, _rb);
                        
                    translator.PushUnityEngineVector2(L, _rt);
                        
                    translator.PushUnityEngineVector2(L, _lt);
                        
                    
                    
                    
                    return 4;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetNegativeZUVForCubeVertex(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector2 _lb;
                    UnityEngine.Vector2 _rb;
                    UnityEngine.Vector2 _rt;
                    UnityEngine.Vector2 _lt;
                    
                    gen_to_be_invoked.GetNegativeZUVForCubeVertex( out _lb, out _rb, out _rt, out _lt );
                    translator.PushUnityEngineVector2(L, _lb);
                        
                    translator.PushUnityEngineVector2(L, _rb);
                        
                    translator.PushUnityEngineVector2(L, _rt);
                        
                    translator.PushUnityEngineVector2(L, _lt);
                        
                    
                    
                    
                    return 4;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayDigAudio(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.AudioSource _source = (UnityEngine.AudioSource)translator.GetObject(L, 2, typeof(UnityEngine.AudioSource));
                    
                    gen_to_be_invoked.PlayDigAudio( _source );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayPlaceAudio(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.AudioSource _source = (UnityEngine.AudioSource)translator.GetObject(L, 2, typeof(UnityEngine.AudioSource));
                    
                    gen_to_be_invoked.PlayPlaceAudio( _source );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PlayStepAutio(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.AudioSource _source = (UnityEngine.AudioSource)translator.GetObject(L, 2, typeof(UnityEngine.AudioSource));
                    
                    gen_to_be_invoked.PlayStepAutio( _source );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BlockName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.BlockName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Type(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
                translator.PushMinecraftBlocksDataBlockType(L, gen_to_be_invoked.Type);
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
			
                Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
                translator.PushMinecraftBlocksDataBlockFlags(L, gen_to_be_invoked.Flags);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_VertexType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
                translator.PushMinecraftBlocksDataBlockVertexType(L, gen_to_be_invoked.VertexType);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MoveResistance(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.MoveResistance);
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
			
                Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.LightOpacity);
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
			
                Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.LightValue);
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
			
                Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.Hardness);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DestoryEffectColor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.DestoryEffectColor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnTickEvent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
                Minecraft.BlocksData.BlockEventAction gen_delegate = translator.GetDelegate<Minecraft.BlocksData.BlockEventAction>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need Minecraft.BlocksData.BlockEventAction!");
                }
				
				if (gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						gen_to_be_invoked.OnTickEvent += gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						gen_to_be_invoked.OnTickEvent -= gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to Minecraft.BlocksData.Block.OnTickEvent!");
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnRandomTickEvent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
                Minecraft.BlocksData.BlockEventAction gen_delegate = translator.GetDelegate<Minecraft.BlocksData.BlockEventAction>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need Minecraft.BlocksData.BlockEventAction!");
                }
				
				if (gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						gen_to_be_invoked.OnRandomTickEvent += gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						gen_to_be_invoked.OnRandomTickEvent -= gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to Minecraft.BlocksData.Block.OnRandomTickEvent!");
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnBlockDestroyEvent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
                Minecraft.BlocksData.BlockEventAction gen_delegate = translator.GetDelegate<Minecraft.BlocksData.BlockEventAction>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need Minecraft.BlocksData.BlockEventAction!");
                }
				
				if (gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						gen_to_be_invoked.OnBlockDestroyEvent += gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						gen_to_be_invoked.OnBlockDestroyEvent -= gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to Minecraft.BlocksData.Block.OnBlockDestroyEvent!");
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnBlockPlaceEvent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
                Minecraft.BlocksData.BlockEventAction gen_delegate = translator.GetDelegate<Minecraft.BlocksData.BlockEventAction>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need Minecraft.BlocksData.BlockEventAction!");
                }
				
				if (gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						gen_to_be_invoked.OnBlockPlaceEvent += gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						gen_to_be_invoked.OnBlockPlaceEvent -= gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to Minecraft.BlocksData.Block.OnBlockPlaceEvent!");
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _e_OnClickEvent(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    int gen_param_count = LuaAPI.lua_gettop(L);
			Minecraft.BlocksData.Block gen_to_be_invoked = (Minecraft.BlocksData.Block)translator.FastGetCSObj(L, 1);
                Minecraft.BlocksData.BlockEventAction gen_delegate = translator.GetDelegate<Minecraft.BlocksData.BlockEventAction>(L, 3);
                if (gen_delegate == null) {
                    return LuaAPI.luaL_error(L, "#3 need Minecraft.BlocksData.BlockEventAction!");
                }
				
				if (gen_param_count == 3)
				{
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "+")) {
						gen_to_be_invoked.OnClickEvent += gen_delegate;
						return 0;
					} 
					
					
					if (LuaAPI.xlua_is_eq_str(L, 2, "-")) {
						gen_to_be_invoked.OnClickEvent -= gen_delegate;
						return 0;
					} 
					
				}
			} catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
			LuaAPI.luaL_error(L, "invalid arguments to Minecraft.BlocksData.Block.OnClickEvent!");
            return 0;
        }
        
		
		
    }
}

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
    public class MinecraftWorldManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Minecraft.WorldManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 13, 8, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetCurrentItemType", _m_GetCurrentItemType);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetCurrentItem", _m_GetCurrentItem);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetItemType", _m_SetItemType);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetBlockType", _m_SetBlockType);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetBlockType", _m_GetBlockType);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetBlockState", _m_SetBlockState);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetBlockState", _m_GetBlockState);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetBlock", _m_GetBlock);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetFinalLightLevel", _m_GetFinalLightLevel);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetBlockLight", _m_GetBlockLight);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetBlockLight", _m_SetBlockLight);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsBlockTransparent", _m_IsBlockTransparent);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsBlockTransparentAndNotWater", _m_IsBlockTransparentAndNotWater);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "Initialized", _g_get_Initialized);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "DataManager", _g_get_DataManager);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Settings", _g_get_Settings);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "WorldSettingsSavingPath", _g_get_WorldSettingsSavingPath);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ChunkManager", _g_get_ChunkManager);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EntityManager", _g_get_EntityManager);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "MainCamera", _g_get_MainCamera);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "InventoryManager", _g_get_InventoryManager);
            
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 1, 0);
			
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "Active", _g_get_Active);
            
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					Minecraft.WorldManager gen_ret = new Minecraft.WorldManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Minecraft.WorldManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCurrentItemType(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.WorldManager gen_to_be_invoked = (Minecraft.WorldManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        Minecraft.ItemsData.ItemType gen_ret = gen_to_be_invoked.GetCurrentItemType(  );
                        translator.PushMinecraftItemsDataItemType(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetCurrentItem(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.WorldManager gen_to_be_invoked = (Minecraft.WorldManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        Minecraft.ItemsData.Item gen_ret = gen_to_be_invoked.GetCurrentItem(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetItemType(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.WorldManager gen_to_be_invoked = (Minecraft.WorldManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _index = LuaAPI.xlua_tointeger(L, 2);
                    Minecraft.ItemsData.ItemType _type;translator.Get(L, 3, out _type);
                    
                    gen_to_be_invoked.SetItemType( _index, _type );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetBlockType(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.WorldManager gen_to_be_invoked = (Minecraft.WorldManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 9&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<Minecraft.BlocksData.BlockType>(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 7)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 8)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 9)) 
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _z = LuaAPI.xlua_tointeger(L, 4);
                    Minecraft.BlocksData.BlockType _block;translator.Get(L, 5, out _block);
                    byte _state = (byte)LuaAPI.xlua_tointeger(L, 6);
                    bool _lightBlocks = LuaAPI.lua_toboolean(L, 7);
                    bool _tickBlocks = LuaAPI.lua_toboolean(L, 8);
                    bool _updateNeighborChunks = LuaAPI.lua_toboolean(L, 9);
                    
                        bool gen_ret = gen_to_be_invoked.SetBlockType( _x, _y, _z, _block, _state, _lightBlocks, _tickBlocks, _updateNeighborChunks );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 8&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<Minecraft.BlocksData.BlockType>(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 7)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 8)) 
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _z = LuaAPI.xlua_tointeger(L, 4);
                    Minecraft.BlocksData.BlockType _block;translator.Get(L, 5, out _block);
                    byte _state = (byte)LuaAPI.xlua_tointeger(L, 6);
                    bool _lightBlocks = LuaAPI.lua_toboolean(L, 7);
                    bool _tickBlocks = LuaAPI.lua_toboolean(L, 8);
                    
                        bool gen_ret = gen_to_be_invoked.SetBlockType( _x, _y, _z, _block, _state, _lightBlocks, _tickBlocks );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 7&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<Minecraft.BlocksData.BlockType>(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 7)) 
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _z = LuaAPI.xlua_tointeger(L, 4);
                    Minecraft.BlocksData.BlockType _block;translator.Get(L, 5, out _block);
                    byte _state = (byte)LuaAPI.xlua_tointeger(L, 6);
                    bool _lightBlocks = LuaAPI.lua_toboolean(L, 7);
                    
                        bool gen_ret = gen_to_be_invoked.SetBlockType( _x, _y, _z, _block, _state, _lightBlocks );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<Minecraft.BlocksData.BlockType>(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)) 
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _z = LuaAPI.xlua_tointeger(L, 4);
                    Minecraft.BlocksData.BlockType _block;translator.Get(L, 5, out _block);
                    byte _state = (byte)LuaAPI.xlua_tointeger(L, 6);
                    
                        bool gen_ret = gen_to_be_invoked.SetBlockType( _x, _y, _z, _block, _state );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<Minecraft.BlocksData.BlockType>(L, 5)) 
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _z = LuaAPI.xlua_tointeger(L, 4);
                    Minecraft.BlocksData.BlockType _block;translator.Get(L, 5, out _block);
                    
                        bool gen_ret = gen_to_be_invoked.SetBlockType( _x, _y, _z, _block );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to Minecraft.WorldManager.SetBlockType!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBlockType(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.WorldManager gen_to_be_invoked = (Minecraft.WorldManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _z = LuaAPI.xlua_tointeger(L, 4);
                    
                        Minecraft.BlocksData.BlockType gen_ret = gen_to_be_invoked.GetBlockType( _x, _y, _z );
                        translator.PushMinecraftBlocksDataBlockType(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetBlockState(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.WorldManager gen_to_be_invoked = (Minecraft.WorldManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _z = LuaAPI.xlua_tointeger(L, 4);
                    byte _value = (byte)LuaAPI.xlua_tointeger(L, 5);
                    
                    gen_to_be_invoked.SetBlockState( _x, _y, _z, _value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBlockState(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.WorldManager gen_to_be_invoked = (Minecraft.WorldManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _z = LuaAPI.xlua_tointeger(L, 4);
                    
                        byte gen_ret = gen_to_be_invoked.GetBlockState( _x, _y, _z );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBlock(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.WorldManager gen_to_be_invoked = (Minecraft.WorldManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _z = LuaAPI.xlua_tointeger(L, 4);
                    
                        Minecraft.BlocksData.Block gen_ret = gen_to_be_invoked.GetBlock( _x, _y, _z );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetFinalLightLevel(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.WorldManager gen_to_be_invoked = (Minecraft.WorldManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _z = LuaAPI.xlua_tointeger(L, 4);
                    
                        byte gen_ret = gen_to_be_invoked.GetFinalLightLevel( _x, _y, _z );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBlockLight(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.WorldManager gen_to_be_invoked = (Minecraft.WorldManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _z = LuaAPI.xlua_tointeger(L, 4);
                    
                        byte gen_ret = gen_to_be_invoked.GetBlockLight( _x, _y, _z );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetBlockLight(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.WorldManager gen_to_be_invoked = (Minecraft.WorldManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _z = LuaAPI.xlua_tointeger(L, 4);
                    byte _value = (byte)LuaAPI.xlua_tointeger(L, 5);
                    
                    gen_to_be_invoked.SetBlockLight( _x, _y, _z, _value );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsBlockTransparent(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.WorldManager gen_to_be_invoked = (Minecraft.WorldManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _z = LuaAPI.xlua_tointeger(L, 4);
                    
                        bool gen_ret = gen_to_be_invoked.IsBlockTransparent( _x, _y, _z );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsBlockTransparentAndNotWater(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.WorldManager gen_to_be_invoked = (Minecraft.WorldManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    int _x = LuaAPI.xlua_tointeger(L, 2);
                    int _y = LuaAPI.xlua_tointeger(L, 3);
                    int _z = LuaAPI.xlua_tointeger(L, 4);
                    
                        bool gen_ret = gen_to_be_invoked.IsBlockTransparentAndNotWater( _x, _y, _z );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Active(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, Minecraft.WorldManager.Active);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Initialized(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.WorldManager gen_to_be_invoked = (Minecraft.WorldManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.Initialized);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_DataManager(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.WorldManager gen_to_be_invoked = (Minecraft.WorldManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.DataManager);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Settings(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.WorldManager gen_to_be_invoked = (Minecraft.WorldManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Settings);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_WorldSettingsSavingPath(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.WorldManager gen_to_be_invoked = (Minecraft.WorldManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.WorldSettingsSavingPath);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ChunkManager(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.WorldManager gen_to_be_invoked = (Minecraft.WorldManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ChunkManager);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EntityManager(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.WorldManager gen_to_be_invoked = (Minecraft.WorldManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.EntityManager);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MainCamera(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.WorldManager gen_to_be_invoked = (Minecraft.WorldManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.MainCamera);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_InventoryManager(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Minecraft.WorldManager gen_to_be_invoked = (Minecraft.WorldManager)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.InventoryManager);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}

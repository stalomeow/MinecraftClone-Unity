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
    public class MinecraftRenderingSectionMeshManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Minecraft.Rendering.SectionMeshManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 4, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Initialize", _m_Initialize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "FreeSectionMesh", _m_FreeSectionMesh);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "MarkSectionMeshDirty", _m_MarkSectionMeshDirty);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetSectionMesh", _m_GetSectionMesh);
			
			
			
			
			
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
					
					var gen_ret = new Minecraft.Rendering.SectionMeshManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Minecraft.Rendering.SectionMeshManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Initialize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Rendering.SectionMeshManager gen_to_be_invoked = (Minecraft.Rendering.SectionMeshManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Minecraft.IWorld _world = (Minecraft.IWorld)translator.GetObject(L, 2, typeof(Minecraft.IWorld));
                    
                    gen_to_be_invoked.Initialize( _world );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FreeSectionMesh(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Rendering.SectionMeshManager gen_to_be_invoked = (Minecraft.Rendering.SectionMeshManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _section;translator.Get(L, 2, out _section);
                    
                    gen_to_be_invoked.FreeSectionMesh( _section );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_MarkSectionMeshDirty(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Rendering.SectionMeshManager gen_to_be_invoked = (Minecraft.Rendering.SectionMeshManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _section;translator.Get(L, 2, out _section);
                    bool _forceLoadMesh = LuaAPI.lua_toboolean(L, 3);
                    bool _important = LuaAPI.lua_toboolean(L, 4);
                    
                    gen_to_be_invoked.MarkSectionMeshDirty( _section, _forceLoadMesh, _important );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetSectionMesh(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Rendering.SectionMeshManager gen_to_be_invoked = (Minecraft.Rendering.SectionMeshManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Vector3Int _section;translator.Get(L, 2, out _section);
                    
                        var gen_ret = gen_to_be_invoked.GetSectionMesh( _section );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}

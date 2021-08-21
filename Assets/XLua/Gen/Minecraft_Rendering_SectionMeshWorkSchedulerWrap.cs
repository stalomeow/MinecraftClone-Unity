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
    public class MinecraftRenderingSectionMeshWorkSchedulerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Minecraft.Rendering.SectionMeshWorkScheduler);
			Utils.BeginObjectRegister(type, L, translator, 0, 4, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Initialize", _m_Initialize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ScheduleWork", _m_ScheduleWork);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ScheduleAsyncWork", _m_ScheduleAsyncWork);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ClearWorks", _m_ClearWorks);
			
			
			
			
			
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
					
					var gen_ret = new Minecraft.Rendering.SectionMeshWorkScheduler();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Minecraft.Rendering.SectionMeshWorkScheduler constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Initialize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Rendering.SectionMeshWorkScheduler gen_to_be_invoked = (Minecraft.Rendering.SectionMeshWorkScheduler)translator.FastGetCSObj(L, 1);
            
            
                
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
        static int _m_ScheduleWork(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Rendering.SectionMeshWorkScheduler gen_to_be_invoked = (Minecraft.Rendering.SectionMeshWorkScheduler)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Mesh _mesh = (UnityEngine.Mesh)translator.GetObject(L, 2, typeof(UnityEngine.Mesh));
                    UnityEngine.Vector3Int _section;translator.Get(L, 3, out _section);
                    Minecraft.Chunk3x3Accessor _accessor = (Minecraft.Chunk3x3Accessor)translator.GetObject(L, 4, typeof(Minecraft.Chunk3x3Accessor));
                    
                    gen_to_be_invoked.ScheduleWork( _mesh, _section, _accessor );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ScheduleAsyncWork(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Rendering.SectionMeshWorkScheduler gen_to_be_invoked = (Minecraft.Rendering.SectionMeshWorkScheduler)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Mesh _mesh = (UnityEngine.Mesh)translator.GetObject(L, 2, typeof(UnityEngine.Mesh));
                    UnityEngine.Vector3Int _section;translator.Get(L, 3, out _section);
                    float _priority = (float)LuaAPI.lua_tonumber(L, 4);
                    Minecraft.Chunk3x3Accessor _accessor = (Minecraft.Chunk3x3Accessor)translator.GetObject(L, 5, typeof(Minecraft.Chunk3x3Accessor));
                    
                    gen_to_be_invoked.ScheduleAsyncWork( _mesh, _section, _priority, _accessor );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearWorks(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Minecraft.Rendering.SectionMeshWorkScheduler gen_to_be_invoked = (Minecraft.Rendering.SectionMeshWorkScheduler)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Action<Minecraft.Rendering.SectionMeshWorkScheduler.AsyncWork> _callback = translator.GetDelegate<System.Action<Minecraft.Rendering.SectionMeshWorkScheduler.AsyncWork>>(L, 2);
                    
                    gen_to_be_invoked.ClearWorks( _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}

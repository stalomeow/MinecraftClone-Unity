#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using System;


namespace XLua
{
    public partial class ObjectTranslator
    {
        
        class IniterAdderUnityEngineVector2
        {
            static IniterAdderUnityEngineVector2()
            {
                LuaEnv.AddIniter(Init);
            }
			
			static void Init(LuaEnv luaenv, ObjectTranslator translator)
			{
			
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Vector2>(translator.PushUnityEngineVector2, translator.Get, translator.UpdateUnityEngineVector2);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Vector3>(translator.PushUnityEngineVector3, translator.Get, translator.UpdateUnityEngineVector3);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Vector4>(translator.PushUnityEngineVector4, translator.Get, translator.UpdateUnityEngineVector4);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Color>(translator.PushUnityEngineColor, translator.Get, translator.UpdateUnityEngineColor);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Quaternion>(translator.PushUnityEngineQuaternion, translator.Get, translator.UpdateUnityEngineQuaternion);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Ray>(translator.PushUnityEngineRay, translator.Get, translator.UpdateUnityEngineRay);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Bounds>(translator.PushUnityEngineBounds, translator.Get, translator.UpdateUnityEngineBounds);
				translator.RegisterPushAndGetAndUpdate<UnityEngine.Ray2D>(translator.PushUnityEngineRay2D, translator.Get, translator.UpdateUnityEngineRay2D);
				translator.RegisterPushAndGetAndUpdate<Minecraft.ChunkPos>(translator.PushMinecraftChunkPos, translator.Get, translator.UpdateMinecraftChunkPos);
				translator.RegisterPushAndGetAndUpdate<Minecraft.ModificationSource>(translator.PushMinecraftModificationSource, translator.Get, translator.UpdateMinecraftModificationSource);
				translator.RegisterPushAndGetAndUpdate<Minecraft.ScriptableWorldGeneration.GenLayers.NativeInt2DArray>(translator.PushMinecraftScriptableWorldGenerationGenLayersNativeInt2DArray, translator.Get, translator.UpdateMinecraftScriptableWorldGenerationGenLayersNativeInt2DArray);
				translator.RegisterPushAndGetAndUpdate<Minecraft.Rendering.SectionMeshVertexData>(translator.PushMinecraftRenderingSectionMeshVertexData, translator.Get, translator.UpdateMinecraftRenderingSectionMeshVertexData);
				translator.RegisterPushAndGetAndUpdate<Minecraft.PhysicSystem.AABB>(translator.PushMinecraftPhysicSystemAABB, translator.Get, translator.UpdateMinecraftPhysicSystemAABB);
				translator.RegisterPushAndGetAndUpdate<Minecraft.PhysicSystem.PhysicMaterial>(translator.PushMinecraftPhysicSystemPhysicMaterial, translator.Get, translator.UpdateMinecraftPhysicSystemPhysicMaterial);
				translator.RegisterPushAndGetAndUpdate<Minecraft.PhysicSystem.PhysicState>(translator.PushMinecraftPhysicSystemPhysicState, translator.Get, translator.UpdateMinecraftPhysicSystemPhysicState);
				translator.RegisterPushAndGetAndUpdate<Minecraft.Noises.UniformRNG>(translator.PushMinecraftNoisesUniformRNG, translator.Get, translator.UpdateMinecraftNoisesUniformRNG);
				translator.RegisterPushAndGetAndUpdate<Minecraft.Configurations.BiomeId>(translator.PushMinecraftConfigurationsBiomeId, translator.Get, translator.UpdateMinecraftConfigurationsBiomeId);
				translator.RegisterPushAndGetAndUpdate<Minecraft.Configurations.BlockEntityConversion>(translator.PushMinecraftConfigurationsBlockEntityConversion, translator.Get, translator.UpdateMinecraftConfigurationsBlockEntityConversion);
				translator.RegisterPushAndGetAndUpdate<Minecraft.Configurations.BlockFace>(translator.PushMinecraftConfigurationsBlockFace, translator.Get, translator.UpdateMinecraftConfigurationsBlockFace);
				translator.RegisterPushAndGetAndUpdate<Minecraft.Configurations.BlockFaceCorner>(translator.PushMinecraftConfigurationsBlockFaceCorner, translator.Get, translator.UpdateMinecraftConfigurationsBlockFaceCorner);
				translator.RegisterPushAndGetAndUpdate<Minecraft.Configurations.BlockFlags>(translator.PushMinecraftConfigurationsBlockFlags, translator.Get, translator.UpdateMinecraftConfigurationsBlockFlags);
			
				translator.RegisterCaster<Minecraft.Configurations.BlockVertexData>(translator.Get);
			}
        }
        
        static IniterAdderUnityEngineVector2 s_IniterAdderUnityEngineVector2_dumb_obj = new IniterAdderUnityEngineVector2();
        static IniterAdderUnityEngineVector2 IniterAdderUnityEngineVector2_dumb_obj {get{return s_IniterAdderUnityEngineVector2_dumb_obj;}}
        
        
        int UnityEngineVector2_TypeID = -1;
        public void PushUnityEngineVector2(RealStatePtr L, UnityEngine.Vector2 val)
        {
            if (UnityEngineVector2_TypeID == -1)
            {
			    bool is_first;
                UnityEngineVector2_TypeID = getTypeId(L, typeof(UnityEngine.Vector2), out is_first);
				
            }
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 8, UnityEngineVector2_TypeID);
            if (!CopyByValue.Pack(buff, 0, val))
            {
                throw new Exception("pack fail fail for UnityEngine.Vector2 ,value="+val);
            }
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Vector2 val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineVector2_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Vector2");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);if (!CopyByValue.UnPack(buff, 0, out val))
                {
                    throw new Exception("unpack fail for UnityEngine.Vector2");
                }
            }
			else if (type ==LuaTypes.LUA_TTABLE)
			{
			    CopyByValue.UnPack(this, L, index, out val);
			}
            else
            {
                val = (UnityEngine.Vector2)objectCasters.GetCaster(typeof(UnityEngine.Vector2))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineVector2(RealStatePtr L, int index, UnityEngine.Vector2 val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineVector2_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Vector2");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  val))
                {
                    throw new Exception("pack fail for UnityEngine.Vector2 ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineVector3_TypeID = -1;
        public void PushUnityEngineVector3(RealStatePtr L, UnityEngine.Vector3 val)
        {
            if (UnityEngineVector3_TypeID == -1)
            {
			    bool is_first;
                UnityEngineVector3_TypeID = getTypeId(L, typeof(UnityEngine.Vector3), out is_first);
				
            }
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 12, UnityEngineVector3_TypeID);
            if (!CopyByValue.Pack(buff, 0, val))
            {
                throw new Exception("pack fail fail for UnityEngine.Vector3 ,value="+val);
            }
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Vector3 val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineVector3_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Vector3");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);if (!CopyByValue.UnPack(buff, 0, out val))
                {
                    throw new Exception("unpack fail for UnityEngine.Vector3");
                }
            }
			else if (type ==LuaTypes.LUA_TTABLE)
			{
			    CopyByValue.UnPack(this, L, index, out val);
			}
            else
            {
                val = (UnityEngine.Vector3)objectCasters.GetCaster(typeof(UnityEngine.Vector3))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineVector3(RealStatePtr L, int index, UnityEngine.Vector3 val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineVector3_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Vector3");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  val))
                {
                    throw new Exception("pack fail for UnityEngine.Vector3 ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineVector4_TypeID = -1;
        public void PushUnityEngineVector4(RealStatePtr L, UnityEngine.Vector4 val)
        {
            if (UnityEngineVector4_TypeID == -1)
            {
			    bool is_first;
                UnityEngineVector4_TypeID = getTypeId(L, typeof(UnityEngine.Vector4), out is_first);
				
            }
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 16, UnityEngineVector4_TypeID);
            if (!CopyByValue.Pack(buff, 0, val))
            {
                throw new Exception("pack fail fail for UnityEngine.Vector4 ,value="+val);
            }
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Vector4 val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineVector4_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Vector4");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);if (!CopyByValue.UnPack(buff, 0, out val))
                {
                    throw new Exception("unpack fail for UnityEngine.Vector4");
                }
            }
			else if (type ==LuaTypes.LUA_TTABLE)
			{
			    CopyByValue.UnPack(this, L, index, out val);
			}
            else
            {
                val = (UnityEngine.Vector4)objectCasters.GetCaster(typeof(UnityEngine.Vector4))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineVector4(RealStatePtr L, int index, UnityEngine.Vector4 val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineVector4_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Vector4");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  val))
                {
                    throw new Exception("pack fail for UnityEngine.Vector4 ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineColor_TypeID = -1;
        public void PushUnityEngineColor(RealStatePtr L, UnityEngine.Color val)
        {
            if (UnityEngineColor_TypeID == -1)
            {
			    bool is_first;
                UnityEngineColor_TypeID = getTypeId(L, typeof(UnityEngine.Color), out is_first);
				
            }
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 16, UnityEngineColor_TypeID);
            if (!CopyByValue.Pack(buff, 0, val))
            {
                throw new Exception("pack fail fail for UnityEngine.Color ,value="+val);
            }
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Color val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineColor_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Color");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);if (!CopyByValue.UnPack(buff, 0, out val))
                {
                    throw new Exception("unpack fail for UnityEngine.Color");
                }
            }
			else if (type ==LuaTypes.LUA_TTABLE)
			{
			    CopyByValue.UnPack(this, L, index, out val);
			}
            else
            {
                val = (UnityEngine.Color)objectCasters.GetCaster(typeof(UnityEngine.Color))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineColor(RealStatePtr L, int index, UnityEngine.Color val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineColor_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Color");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  val))
                {
                    throw new Exception("pack fail for UnityEngine.Color ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineQuaternion_TypeID = -1;
        public void PushUnityEngineQuaternion(RealStatePtr L, UnityEngine.Quaternion val)
        {
            if (UnityEngineQuaternion_TypeID == -1)
            {
			    bool is_first;
                UnityEngineQuaternion_TypeID = getTypeId(L, typeof(UnityEngine.Quaternion), out is_first);
				
            }
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 16, UnityEngineQuaternion_TypeID);
            if (!CopyByValue.Pack(buff, 0, val))
            {
                throw new Exception("pack fail fail for UnityEngine.Quaternion ,value="+val);
            }
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Quaternion val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineQuaternion_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Quaternion");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);if (!CopyByValue.UnPack(buff, 0, out val))
                {
                    throw new Exception("unpack fail for UnityEngine.Quaternion");
                }
            }
			else if (type ==LuaTypes.LUA_TTABLE)
			{
			    CopyByValue.UnPack(this, L, index, out val);
			}
            else
            {
                val = (UnityEngine.Quaternion)objectCasters.GetCaster(typeof(UnityEngine.Quaternion))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineQuaternion(RealStatePtr L, int index, UnityEngine.Quaternion val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineQuaternion_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Quaternion");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  val))
                {
                    throw new Exception("pack fail for UnityEngine.Quaternion ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineRay_TypeID = -1;
        public void PushUnityEngineRay(RealStatePtr L, UnityEngine.Ray val)
        {
            if (UnityEngineRay_TypeID == -1)
            {
			    bool is_first;
                UnityEngineRay_TypeID = getTypeId(L, typeof(UnityEngine.Ray), out is_first);
				
            }
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 24, UnityEngineRay_TypeID);
            if (!CopyByValue.Pack(buff, 0, val))
            {
                throw new Exception("pack fail fail for UnityEngine.Ray ,value="+val);
            }
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Ray val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineRay_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Ray");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);if (!CopyByValue.UnPack(buff, 0, out val))
                {
                    throw new Exception("unpack fail for UnityEngine.Ray");
                }
            }
			else if (type ==LuaTypes.LUA_TTABLE)
			{
			    CopyByValue.UnPack(this, L, index, out val);
			}
            else
            {
                val = (UnityEngine.Ray)objectCasters.GetCaster(typeof(UnityEngine.Ray))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineRay(RealStatePtr L, int index, UnityEngine.Ray val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineRay_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Ray");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  val))
                {
                    throw new Exception("pack fail for UnityEngine.Ray ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineBounds_TypeID = -1;
        public void PushUnityEngineBounds(RealStatePtr L, UnityEngine.Bounds val)
        {
            if (UnityEngineBounds_TypeID == -1)
            {
			    bool is_first;
                UnityEngineBounds_TypeID = getTypeId(L, typeof(UnityEngine.Bounds), out is_first);
				
            }
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 24, UnityEngineBounds_TypeID);
            if (!CopyByValue.Pack(buff, 0, val))
            {
                throw new Exception("pack fail fail for UnityEngine.Bounds ,value="+val);
            }
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Bounds val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineBounds_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Bounds");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);if (!CopyByValue.UnPack(buff, 0, out val))
                {
                    throw new Exception("unpack fail for UnityEngine.Bounds");
                }
            }
			else if (type ==LuaTypes.LUA_TTABLE)
			{
			    CopyByValue.UnPack(this, L, index, out val);
			}
            else
            {
                val = (UnityEngine.Bounds)objectCasters.GetCaster(typeof(UnityEngine.Bounds))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineBounds(RealStatePtr L, int index, UnityEngine.Bounds val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineBounds_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Bounds");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  val))
                {
                    throw new Exception("pack fail for UnityEngine.Bounds ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int UnityEngineRay2D_TypeID = -1;
        public void PushUnityEngineRay2D(RealStatePtr L, UnityEngine.Ray2D val)
        {
            if (UnityEngineRay2D_TypeID == -1)
            {
			    bool is_first;
                UnityEngineRay2D_TypeID = getTypeId(L, typeof(UnityEngine.Ray2D), out is_first);
				
            }
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 16, UnityEngineRay2D_TypeID);
            if (!CopyByValue.Pack(buff, 0, val))
            {
                throw new Exception("pack fail fail for UnityEngine.Ray2D ,value="+val);
            }
			
        }
		
        public void Get(RealStatePtr L, int index, out UnityEngine.Ray2D val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineRay2D_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Ray2D");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);if (!CopyByValue.UnPack(buff, 0, out val))
                {
                    throw new Exception("unpack fail for UnityEngine.Ray2D");
                }
            }
			else if (type ==LuaTypes.LUA_TTABLE)
			{
			    CopyByValue.UnPack(this, L, index, out val);
			}
            else
            {
                val = (UnityEngine.Ray2D)objectCasters.GetCaster(typeof(UnityEngine.Ray2D))(L, index, null);
            }
        }
		
        public void UpdateUnityEngineRay2D(RealStatePtr L, int index, UnityEngine.Ray2D val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != UnityEngineRay2D_TypeID)
				{
				    throw new Exception("invalid userdata for UnityEngine.Ray2D");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  val))
                {
                    throw new Exception("pack fail for UnityEngine.Ray2D ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int MinecraftChunkPos_TypeID = -1;
        public void PushMinecraftChunkPos(RealStatePtr L, Minecraft.ChunkPos val)
        {
            if (MinecraftChunkPos_TypeID == -1)
            {
			    bool is_first;
                MinecraftChunkPos_TypeID = getTypeId(L, typeof(Minecraft.ChunkPos), out is_first);
				
            }
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 0, MinecraftChunkPos_TypeID);
            if (!CopyByValue.Pack(buff, 0, val))
            {
                throw new Exception("pack fail fail for Minecraft.ChunkPos ,value="+val);
            }
			
        }
		
        public void Get(RealStatePtr L, int index, out Minecraft.ChunkPos val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != MinecraftChunkPos_TypeID)
				{
				    throw new Exception("invalid userdata for Minecraft.ChunkPos");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);if (!CopyByValue.UnPack(buff, 0, out val))
                {
                    throw new Exception("unpack fail for Minecraft.ChunkPos");
                }
            }
			else if (type ==LuaTypes.LUA_TTABLE)
			{
			    CopyByValue.UnPack(this, L, index, out val);
			}
            else
            {
                val = (Minecraft.ChunkPos)objectCasters.GetCaster(typeof(Minecraft.ChunkPos))(L, index, null);
            }
        }
		
        public void UpdateMinecraftChunkPos(RealStatePtr L, int index, Minecraft.ChunkPos val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != MinecraftChunkPos_TypeID)
				{
				    throw new Exception("invalid userdata for Minecraft.ChunkPos");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  val))
                {
                    throw new Exception("pack fail for Minecraft.ChunkPos ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int MinecraftModificationSource_TypeID = -1;
		int MinecraftModificationSource_EnumRef = -1;
        
        public void PushMinecraftModificationSource(RealStatePtr L, Minecraft.ModificationSource val)
        {
            if (MinecraftModificationSource_TypeID == -1)
            {
			    bool is_first;
                MinecraftModificationSource_TypeID = getTypeId(L, typeof(Minecraft.ModificationSource), out is_first);
				
				if (MinecraftModificationSource_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Minecraft.ModificationSource));
				    MinecraftModificationSource_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, MinecraftModificationSource_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, MinecraftModificationSource_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Minecraft.ModificationSource ,value="+val);
            }
			
			LuaAPI.lua_getref(L, MinecraftModificationSource_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Minecraft.ModificationSource val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != MinecraftModificationSource_TypeID)
				{
				    throw new Exception("invalid userdata for Minecraft.ModificationSource");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Minecraft.ModificationSource");
                }
				val = (Minecraft.ModificationSource)e;
                
            }
            else
            {
                val = (Minecraft.ModificationSource)objectCasters.GetCaster(typeof(Minecraft.ModificationSource))(L, index, null);
            }
        }
		
        public void UpdateMinecraftModificationSource(RealStatePtr L, int index, Minecraft.ModificationSource val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != MinecraftModificationSource_TypeID)
				{
				    throw new Exception("invalid userdata for Minecraft.ModificationSource");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Minecraft.ModificationSource ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int MinecraftScriptableWorldGenerationGenLayersNativeInt2DArray_TypeID = -1;
        public void PushMinecraftScriptableWorldGenerationGenLayersNativeInt2DArray(RealStatePtr L, Minecraft.ScriptableWorldGeneration.GenLayers.NativeInt2DArray val)
        {
            if (MinecraftScriptableWorldGenerationGenLayersNativeInt2DArray_TypeID == -1)
            {
			    bool is_first;
                MinecraftScriptableWorldGenerationGenLayersNativeInt2DArray_TypeID = getTypeId(L, typeof(Minecraft.ScriptableWorldGeneration.GenLayers.NativeInt2DArray), out is_first);
				
            }
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 0, MinecraftScriptableWorldGenerationGenLayersNativeInt2DArray_TypeID);
            if (!CopyByValue.Pack(buff, 0, val))
            {
                throw new Exception("pack fail fail for Minecraft.ScriptableWorldGeneration.GenLayers.NativeInt2DArray ,value="+val);
            }
			
        }
		
        public void Get(RealStatePtr L, int index, out Minecraft.ScriptableWorldGeneration.GenLayers.NativeInt2DArray val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != MinecraftScriptableWorldGenerationGenLayersNativeInt2DArray_TypeID)
				{
				    throw new Exception("invalid userdata for Minecraft.ScriptableWorldGeneration.GenLayers.NativeInt2DArray");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);if (!CopyByValue.UnPack(buff, 0, out val))
                {
                    throw new Exception("unpack fail for Minecraft.ScriptableWorldGeneration.GenLayers.NativeInt2DArray");
                }
            }
			else if (type ==LuaTypes.LUA_TTABLE)
			{
			    CopyByValue.UnPack(this, L, index, out val);
			}
            else
            {
                val = (Minecraft.ScriptableWorldGeneration.GenLayers.NativeInt2DArray)objectCasters.GetCaster(typeof(Minecraft.ScriptableWorldGeneration.GenLayers.NativeInt2DArray))(L, index, null);
            }
        }
		
        public void UpdateMinecraftScriptableWorldGenerationGenLayersNativeInt2DArray(RealStatePtr L, int index, Minecraft.ScriptableWorldGeneration.GenLayers.NativeInt2DArray val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != MinecraftScriptableWorldGenerationGenLayersNativeInt2DArray_TypeID)
				{
				    throw new Exception("invalid userdata for Minecraft.ScriptableWorldGeneration.GenLayers.NativeInt2DArray");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  val))
                {
                    throw new Exception("pack fail for Minecraft.ScriptableWorldGeneration.GenLayers.NativeInt2DArray ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int MinecraftRenderingSectionMeshVertexData_TypeID = -1;
        public void PushMinecraftRenderingSectionMeshVertexData(RealStatePtr L, Minecraft.Rendering.SectionMeshVertexData val)
        {
            if (MinecraftRenderingSectionMeshVertexData_TypeID == -1)
            {
			    bool is_first;
                MinecraftRenderingSectionMeshVertexData_TypeID = getTypeId(L, typeof(Minecraft.Rendering.SectionMeshVertexData), out is_first);
				
            }
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 44, MinecraftRenderingSectionMeshVertexData_TypeID);
            if (!CopyByValue.Pack(buff, 0, val))
            {
                throw new Exception("pack fail fail for Minecraft.Rendering.SectionMeshVertexData ,value="+val);
            }
			
        }
		
        public void Get(RealStatePtr L, int index, out Minecraft.Rendering.SectionMeshVertexData val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != MinecraftRenderingSectionMeshVertexData_TypeID)
				{
				    throw new Exception("invalid userdata for Minecraft.Rendering.SectionMeshVertexData");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);if (!CopyByValue.UnPack(buff, 0, out val))
                {
                    throw new Exception("unpack fail for Minecraft.Rendering.SectionMeshVertexData");
                }
            }
			else if (type ==LuaTypes.LUA_TTABLE)
			{
			    CopyByValue.UnPack(this, L, index, out val);
			}
            else
            {
                val = (Minecraft.Rendering.SectionMeshVertexData)objectCasters.GetCaster(typeof(Minecraft.Rendering.SectionMeshVertexData))(L, index, null);
            }
        }
		
        public void UpdateMinecraftRenderingSectionMeshVertexData(RealStatePtr L, int index, Minecraft.Rendering.SectionMeshVertexData val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != MinecraftRenderingSectionMeshVertexData_TypeID)
				{
				    throw new Exception("invalid userdata for Minecraft.Rendering.SectionMeshVertexData");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  val))
                {
                    throw new Exception("pack fail for Minecraft.Rendering.SectionMeshVertexData ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int MinecraftPhysicSystemAABB_TypeID = -1;
        public void PushMinecraftPhysicSystemAABB(RealStatePtr L, Minecraft.PhysicSystem.AABB val)
        {
            if (MinecraftPhysicSystemAABB_TypeID == -1)
            {
			    bool is_first;
                MinecraftPhysicSystemAABB_TypeID = getTypeId(L, typeof(Minecraft.PhysicSystem.AABB), out is_first);
				
            }
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 0, MinecraftPhysicSystemAABB_TypeID);
            if (!CopyByValue.Pack(buff, 0, val))
            {
                throw new Exception("pack fail fail for Minecraft.PhysicSystem.AABB ,value="+val);
            }
			
        }
		
        public void Get(RealStatePtr L, int index, out Minecraft.PhysicSystem.AABB val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != MinecraftPhysicSystemAABB_TypeID)
				{
				    throw new Exception("invalid userdata for Minecraft.PhysicSystem.AABB");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);if (!CopyByValue.UnPack(buff, 0, out val))
                {
                    throw new Exception("unpack fail for Minecraft.PhysicSystem.AABB");
                }
            }
			else if (type ==LuaTypes.LUA_TTABLE)
			{
			    CopyByValue.UnPack(this, L, index, out val);
			}
            else
            {
                val = (Minecraft.PhysicSystem.AABB)objectCasters.GetCaster(typeof(Minecraft.PhysicSystem.AABB))(L, index, null);
            }
        }
		
        public void UpdateMinecraftPhysicSystemAABB(RealStatePtr L, int index, Minecraft.PhysicSystem.AABB val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != MinecraftPhysicSystemAABB_TypeID)
				{
				    throw new Exception("invalid userdata for Minecraft.PhysicSystem.AABB");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  val))
                {
                    throw new Exception("pack fail for Minecraft.PhysicSystem.AABB ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int MinecraftPhysicSystemPhysicMaterial_TypeID = -1;
        public void PushMinecraftPhysicSystemPhysicMaterial(RealStatePtr L, Minecraft.PhysicSystem.PhysicMaterial val)
        {
            if (MinecraftPhysicSystemPhysicMaterial_TypeID == -1)
            {
			    bool is_first;
                MinecraftPhysicSystemPhysicMaterial_TypeID = getTypeId(L, typeof(Minecraft.PhysicSystem.PhysicMaterial), out is_first);
				
            }
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 12, MinecraftPhysicSystemPhysicMaterial_TypeID);
            if (!CopyByValue.Pack(buff, 0, val))
            {
                throw new Exception("pack fail fail for Minecraft.PhysicSystem.PhysicMaterial ,value="+val);
            }
			
        }
		
        public void Get(RealStatePtr L, int index, out Minecraft.PhysicSystem.PhysicMaterial val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != MinecraftPhysicSystemPhysicMaterial_TypeID)
				{
				    throw new Exception("invalid userdata for Minecraft.PhysicSystem.PhysicMaterial");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);if (!CopyByValue.UnPack(buff, 0, out val))
                {
                    throw new Exception("unpack fail for Minecraft.PhysicSystem.PhysicMaterial");
                }
            }
			else if (type ==LuaTypes.LUA_TTABLE)
			{
			    CopyByValue.UnPack(this, L, index, out val);
			}
            else
            {
                val = (Minecraft.PhysicSystem.PhysicMaterial)objectCasters.GetCaster(typeof(Minecraft.PhysicSystem.PhysicMaterial))(L, index, null);
            }
        }
		
        public void UpdateMinecraftPhysicSystemPhysicMaterial(RealStatePtr L, int index, Minecraft.PhysicSystem.PhysicMaterial val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != MinecraftPhysicSystemPhysicMaterial_TypeID)
				{
				    throw new Exception("invalid userdata for Minecraft.PhysicSystem.PhysicMaterial");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  val))
                {
                    throw new Exception("pack fail for Minecraft.PhysicSystem.PhysicMaterial ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int MinecraftPhysicSystemPhysicState_TypeID = -1;
		int MinecraftPhysicSystemPhysicState_EnumRef = -1;
        
        public void PushMinecraftPhysicSystemPhysicState(RealStatePtr L, Minecraft.PhysicSystem.PhysicState val)
        {
            if (MinecraftPhysicSystemPhysicState_TypeID == -1)
            {
			    bool is_first;
                MinecraftPhysicSystemPhysicState_TypeID = getTypeId(L, typeof(Minecraft.PhysicSystem.PhysicState), out is_first);
				
				if (MinecraftPhysicSystemPhysicState_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Minecraft.PhysicSystem.PhysicState));
				    MinecraftPhysicSystemPhysicState_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, MinecraftPhysicSystemPhysicState_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, MinecraftPhysicSystemPhysicState_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Minecraft.PhysicSystem.PhysicState ,value="+val);
            }
			
			LuaAPI.lua_getref(L, MinecraftPhysicSystemPhysicState_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Minecraft.PhysicSystem.PhysicState val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != MinecraftPhysicSystemPhysicState_TypeID)
				{
				    throw new Exception("invalid userdata for Minecraft.PhysicSystem.PhysicState");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Minecraft.PhysicSystem.PhysicState");
                }
				val = (Minecraft.PhysicSystem.PhysicState)e;
                
            }
            else
            {
                val = (Minecraft.PhysicSystem.PhysicState)objectCasters.GetCaster(typeof(Minecraft.PhysicSystem.PhysicState))(L, index, null);
            }
        }
		
        public void UpdateMinecraftPhysicSystemPhysicState(RealStatePtr L, int index, Minecraft.PhysicSystem.PhysicState val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != MinecraftPhysicSystemPhysicState_TypeID)
				{
				    throw new Exception("invalid userdata for Minecraft.PhysicSystem.PhysicState");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Minecraft.PhysicSystem.PhysicState ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int MinecraftNoisesUniformRNG_TypeID = -1;
        public void PushMinecraftNoisesUniformRNG(RealStatePtr L, Minecraft.Noises.UniformRNG val)
        {
            if (MinecraftNoisesUniformRNG_TypeID == -1)
            {
			    bool is_first;
                MinecraftNoisesUniformRNG_TypeID = getTypeId(L, typeof(Minecraft.Noises.UniformRNG), out is_first);
				
            }
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 0, MinecraftNoisesUniformRNG_TypeID);
            if (!CopyByValue.Pack(buff, 0, val))
            {
                throw new Exception("pack fail fail for Minecraft.Noises.UniformRNG ,value="+val);
            }
			
        }
		
        public void Get(RealStatePtr L, int index, out Minecraft.Noises.UniformRNG val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != MinecraftNoisesUniformRNG_TypeID)
				{
				    throw new Exception("invalid userdata for Minecraft.Noises.UniformRNG");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);if (!CopyByValue.UnPack(buff, 0, out val))
                {
                    throw new Exception("unpack fail for Minecraft.Noises.UniformRNG");
                }
            }
			else if (type ==LuaTypes.LUA_TTABLE)
			{
			    CopyByValue.UnPack(this, L, index, out val);
			}
            else
            {
                val = (Minecraft.Noises.UniformRNG)objectCasters.GetCaster(typeof(Minecraft.Noises.UniformRNG))(L, index, null);
            }
        }
		
        public void UpdateMinecraftNoisesUniformRNG(RealStatePtr L, int index, Minecraft.Noises.UniformRNG val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != MinecraftNoisesUniformRNG_TypeID)
				{
				    throw new Exception("invalid userdata for Minecraft.Noises.UniformRNG");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  val))
                {
                    throw new Exception("pack fail for Minecraft.Noises.UniformRNG ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int MinecraftConfigurationsBiomeId_TypeID = -1;
		int MinecraftConfigurationsBiomeId_EnumRef = -1;
        
        public void PushMinecraftConfigurationsBiomeId(RealStatePtr L, Minecraft.Configurations.BiomeId val)
        {
            if (MinecraftConfigurationsBiomeId_TypeID == -1)
            {
			    bool is_first;
                MinecraftConfigurationsBiomeId_TypeID = getTypeId(L, typeof(Minecraft.Configurations.BiomeId), out is_first);
				
				if (MinecraftConfigurationsBiomeId_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Minecraft.Configurations.BiomeId));
				    MinecraftConfigurationsBiomeId_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, MinecraftConfigurationsBiomeId_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, MinecraftConfigurationsBiomeId_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Minecraft.Configurations.BiomeId ,value="+val);
            }
			
			LuaAPI.lua_getref(L, MinecraftConfigurationsBiomeId_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Minecraft.Configurations.BiomeId val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != MinecraftConfigurationsBiomeId_TypeID)
				{
				    throw new Exception("invalid userdata for Minecraft.Configurations.BiomeId");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Minecraft.Configurations.BiomeId");
                }
				val = (Minecraft.Configurations.BiomeId)e;
                
            }
            else
            {
                val = (Minecraft.Configurations.BiomeId)objectCasters.GetCaster(typeof(Minecraft.Configurations.BiomeId))(L, index, null);
            }
        }
		
        public void UpdateMinecraftConfigurationsBiomeId(RealStatePtr L, int index, Minecraft.Configurations.BiomeId val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != MinecraftConfigurationsBiomeId_TypeID)
				{
				    throw new Exception("invalid userdata for Minecraft.Configurations.BiomeId");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Minecraft.Configurations.BiomeId ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int MinecraftConfigurationsBlockEntityConversion_TypeID = -1;
		int MinecraftConfigurationsBlockEntityConversion_EnumRef = -1;
        
        public void PushMinecraftConfigurationsBlockEntityConversion(RealStatePtr L, Minecraft.Configurations.BlockEntityConversion val)
        {
            if (MinecraftConfigurationsBlockEntityConversion_TypeID == -1)
            {
			    bool is_first;
                MinecraftConfigurationsBlockEntityConversion_TypeID = getTypeId(L, typeof(Minecraft.Configurations.BlockEntityConversion), out is_first);
				
				if (MinecraftConfigurationsBlockEntityConversion_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Minecraft.Configurations.BlockEntityConversion));
				    MinecraftConfigurationsBlockEntityConversion_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, MinecraftConfigurationsBlockEntityConversion_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, MinecraftConfigurationsBlockEntityConversion_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Minecraft.Configurations.BlockEntityConversion ,value="+val);
            }
			
			LuaAPI.lua_getref(L, MinecraftConfigurationsBlockEntityConversion_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Minecraft.Configurations.BlockEntityConversion val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != MinecraftConfigurationsBlockEntityConversion_TypeID)
				{
				    throw new Exception("invalid userdata for Minecraft.Configurations.BlockEntityConversion");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Minecraft.Configurations.BlockEntityConversion");
                }
				val = (Minecraft.Configurations.BlockEntityConversion)e;
                
            }
            else
            {
                val = (Minecraft.Configurations.BlockEntityConversion)objectCasters.GetCaster(typeof(Minecraft.Configurations.BlockEntityConversion))(L, index, null);
            }
        }
		
        public void UpdateMinecraftConfigurationsBlockEntityConversion(RealStatePtr L, int index, Minecraft.Configurations.BlockEntityConversion val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != MinecraftConfigurationsBlockEntityConversion_TypeID)
				{
				    throw new Exception("invalid userdata for Minecraft.Configurations.BlockEntityConversion");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Minecraft.Configurations.BlockEntityConversion ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int MinecraftConfigurationsBlockFace_TypeID = -1;
		int MinecraftConfigurationsBlockFace_EnumRef = -1;
        
        public void PushMinecraftConfigurationsBlockFace(RealStatePtr L, Minecraft.Configurations.BlockFace val)
        {
            if (MinecraftConfigurationsBlockFace_TypeID == -1)
            {
			    bool is_first;
                MinecraftConfigurationsBlockFace_TypeID = getTypeId(L, typeof(Minecraft.Configurations.BlockFace), out is_first);
				
				if (MinecraftConfigurationsBlockFace_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Minecraft.Configurations.BlockFace));
				    MinecraftConfigurationsBlockFace_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, MinecraftConfigurationsBlockFace_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, MinecraftConfigurationsBlockFace_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Minecraft.Configurations.BlockFace ,value="+val);
            }
			
			LuaAPI.lua_getref(L, MinecraftConfigurationsBlockFace_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Minecraft.Configurations.BlockFace val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != MinecraftConfigurationsBlockFace_TypeID)
				{
				    throw new Exception("invalid userdata for Minecraft.Configurations.BlockFace");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Minecraft.Configurations.BlockFace");
                }
				val = (Minecraft.Configurations.BlockFace)e;
                
            }
            else
            {
                val = (Minecraft.Configurations.BlockFace)objectCasters.GetCaster(typeof(Minecraft.Configurations.BlockFace))(L, index, null);
            }
        }
		
        public void UpdateMinecraftConfigurationsBlockFace(RealStatePtr L, int index, Minecraft.Configurations.BlockFace val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != MinecraftConfigurationsBlockFace_TypeID)
				{
				    throw new Exception("invalid userdata for Minecraft.Configurations.BlockFace");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Minecraft.Configurations.BlockFace ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int MinecraftConfigurationsBlockFaceCorner_TypeID = -1;
		int MinecraftConfigurationsBlockFaceCorner_EnumRef = -1;
        
        public void PushMinecraftConfigurationsBlockFaceCorner(RealStatePtr L, Minecraft.Configurations.BlockFaceCorner val)
        {
            if (MinecraftConfigurationsBlockFaceCorner_TypeID == -1)
            {
			    bool is_first;
                MinecraftConfigurationsBlockFaceCorner_TypeID = getTypeId(L, typeof(Minecraft.Configurations.BlockFaceCorner), out is_first);
				
				if (MinecraftConfigurationsBlockFaceCorner_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Minecraft.Configurations.BlockFaceCorner));
				    MinecraftConfigurationsBlockFaceCorner_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, MinecraftConfigurationsBlockFaceCorner_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, MinecraftConfigurationsBlockFaceCorner_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Minecraft.Configurations.BlockFaceCorner ,value="+val);
            }
			
			LuaAPI.lua_getref(L, MinecraftConfigurationsBlockFaceCorner_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Minecraft.Configurations.BlockFaceCorner val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != MinecraftConfigurationsBlockFaceCorner_TypeID)
				{
				    throw new Exception("invalid userdata for Minecraft.Configurations.BlockFaceCorner");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Minecraft.Configurations.BlockFaceCorner");
                }
				val = (Minecraft.Configurations.BlockFaceCorner)e;
                
            }
            else
            {
                val = (Minecraft.Configurations.BlockFaceCorner)objectCasters.GetCaster(typeof(Minecraft.Configurations.BlockFaceCorner))(L, index, null);
            }
        }
		
        public void UpdateMinecraftConfigurationsBlockFaceCorner(RealStatePtr L, int index, Minecraft.Configurations.BlockFaceCorner val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != MinecraftConfigurationsBlockFaceCorner_TypeID)
				{
				    throw new Exception("invalid userdata for Minecraft.Configurations.BlockFaceCorner");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Minecraft.Configurations.BlockFaceCorner ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        int MinecraftConfigurationsBlockFlags_TypeID = -1;
		int MinecraftConfigurationsBlockFlags_EnumRef = -1;
        
        public void PushMinecraftConfigurationsBlockFlags(RealStatePtr L, Minecraft.Configurations.BlockFlags val)
        {
            if (MinecraftConfigurationsBlockFlags_TypeID == -1)
            {
			    bool is_first;
                MinecraftConfigurationsBlockFlags_TypeID = getTypeId(L, typeof(Minecraft.Configurations.BlockFlags), out is_first);
				
				if (MinecraftConfigurationsBlockFlags_EnumRef == -1)
				{
				    Utils.LoadCSTable(L, typeof(Minecraft.Configurations.BlockFlags));
				    MinecraftConfigurationsBlockFlags_EnumRef = LuaAPI.luaL_ref(L, LuaIndexes.LUA_REGISTRYINDEX);
				}
				
            }
			
			if (LuaAPI.xlua_tryget_cachedud(L, (int)val, MinecraftConfigurationsBlockFlags_EnumRef) == 1)
            {
			    return;
			}
			
            IntPtr buff = LuaAPI.xlua_pushstruct(L, 4, MinecraftConfigurationsBlockFlags_TypeID);
            if (!CopyByValue.Pack(buff, 0, (int)val))
            {
                throw new Exception("pack fail fail for Minecraft.Configurations.BlockFlags ,value="+val);
            }
			
			LuaAPI.lua_getref(L, MinecraftConfigurationsBlockFlags_EnumRef);
			LuaAPI.lua_pushvalue(L, -2);
			LuaAPI.xlua_rawseti(L, -2, (int)val);
			LuaAPI.lua_pop(L, 1);
			
        }
		
        public void Get(RealStatePtr L, int index, out Minecraft.Configurations.BlockFlags val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != MinecraftConfigurationsBlockFlags_TypeID)
				{
				    throw new Exception("invalid userdata for Minecraft.Configurations.BlockFlags");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
				int e;
                if (!CopyByValue.UnPack(buff, 0, out e))
                {
                    throw new Exception("unpack fail for Minecraft.Configurations.BlockFlags");
                }
				val = (Minecraft.Configurations.BlockFlags)e;
                
            }
            else
            {
                val = (Minecraft.Configurations.BlockFlags)objectCasters.GetCaster(typeof(Minecraft.Configurations.BlockFlags))(L, index, null);
            }
        }
		
        public void UpdateMinecraftConfigurationsBlockFlags(RealStatePtr L, int index, Minecraft.Configurations.BlockFlags val)
        {
		    
            if (LuaAPI.lua_type(L, index) == LuaTypes.LUA_TUSERDATA)
            {
			    if (LuaAPI.xlua_gettypeid(L, index) != MinecraftConfigurationsBlockFlags_TypeID)
				{
				    throw new Exception("invalid userdata for Minecraft.Configurations.BlockFlags");
				}
				
                IntPtr buff = LuaAPI.lua_touserdata(L, index);
                if (!CopyByValue.Pack(buff, 0,  (int)val))
                {
                    throw new Exception("pack fail for Minecraft.Configurations.BlockFlags ,value="+val);
                }
            }
			
            else
            {
                throw new Exception("try to update a data with lua type:" + LuaAPI.lua_type(L, index));
            }
        }
        
        
		// table cast optimze
		
		public void Get(RealStatePtr L, int index, out Minecraft.Configurations.BlockVertexData val)
        {
		    LuaTypes type = LuaAPI.lua_type(L, index);
            if (type == LuaTypes.LUA_TUSERDATA )
            {
			    val = (Minecraft.Configurations.BlockVertexData)FastGetCSObj(L, index);
            }
			else if (type == LuaTypes.LUA_TTABLE)
			{
			    val = new Minecraft.Configurations.BlockVertexData();
				int top = LuaAPI.lua_gettop(L);
				
				if (Utils.LoadField(L, index, "Position"))
				{
					Get(L, top + 1, out val.Position);
				}
				LuaAPI.lua_pop(L, 1);
				
				if (Utils.LoadField(L, index, "UV"))
				{
					Get(L, top + 1, out val.UV);
				}
				LuaAPI.lua_pop(L, 1);
				
				if (Utils.LoadField(L, index, "CornerInFace"))
				{
					Get(L, top + 1, out val.CornerInFace);
				}
				LuaAPI.lua_pop(L, 1);
				
			}
            else
            {
                throw new Exception("can not cast " + LuaAPI.lua_type(L, index) + " to " + typeof(Minecraft.Configurations.BlockVertexData));
            }
        }
		
        
    }
	
	public partial class StaticLuaCallbacks
    {
	    internal static bool __tryArrayGet(Type type, RealStatePtr L, ObjectTranslator translator, object obj, int index)
		{
		
			if (type == typeof(UnityEngine.Vector2[]))
			{
			    UnityEngine.Vector2[] array = obj as UnityEngine.Vector2[];
				translator.PushUnityEngineVector2(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Vector3[]))
			{
			    UnityEngine.Vector3[] array = obj as UnityEngine.Vector3[];
				translator.PushUnityEngineVector3(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Vector4[]))
			{
			    UnityEngine.Vector4[] array = obj as UnityEngine.Vector4[];
				translator.PushUnityEngineVector4(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Color[]))
			{
			    UnityEngine.Color[] array = obj as UnityEngine.Color[];
				translator.PushUnityEngineColor(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Quaternion[]))
			{
			    UnityEngine.Quaternion[] array = obj as UnityEngine.Quaternion[];
				translator.PushUnityEngineQuaternion(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Ray[]))
			{
			    UnityEngine.Ray[] array = obj as UnityEngine.Ray[];
				translator.PushUnityEngineRay(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Bounds[]))
			{
			    UnityEngine.Bounds[] array = obj as UnityEngine.Bounds[];
				translator.PushUnityEngineBounds(L, array[index]);
				return true;
			}
			else if (type == typeof(UnityEngine.Ray2D[]))
			{
			    UnityEngine.Ray2D[] array = obj as UnityEngine.Ray2D[];
				translator.PushUnityEngineRay2D(L, array[index]);
				return true;
			}
			else if (type == typeof(Minecraft.ChunkPos[]))
			{
			    Minecraft.ChunkPos[] array = obj as Minecraft.ChunkPos[];
				translator.PushMinecraftChunkPos(L, array[index]);
				return true;
			}
			else if (type == typeof(Minecraft.ModificationSource[]))
			{
			    Minecraft.ModificationSource[] array = obj as Minecraft.ModificationSource[];
				translator.PushMinecraftModificationSource(L, array[index]);
				return true;
			}
			else if (type == typeof(Minecraft.ScriptableWorldGeneration.GenLayers.NativeInt2DArray[]))
			{
			    Minecraft.ScriptableWorldGeneration.GenLayers.NativeInt2DArray[] array = obj as Minecraft.ScriptableWorldGeneration.GenLayers.NativeInt2DArray[];
				translator.PushMinecraftScriptableWorldGenerationGenLayersNativeInt2DArray(L, array[index]);
				return true;
			}
			else if (type == typeof(Minecraft.Rendering.SectionMeshVertexData[]))
			{
			    Minecraft.Rendering.SectionMeshVertexData[] array = obj as Minecraft.Rendering.SectionMeshVertexData[];
				translator.PushMinecraftRenderingSectionMeshVertexData(L, array[index]);
				return true;
			}
			else if (type == typeof(Minecraft.PhysicSystem.AABB[]))
			{
			    Minecraft.PhysicSystem.AABB[] array = obj as Minecraft.PhysicSystem.AABB[];
				translator.PushMinecraftPhysicSystemAABB(L, array[index]);
				return true;
			}
			else if (type == typeof(Minecraft.PhysicSystem.PhysicMaterial[]))
			{
			    Minecraft.PhysicSystem.PhysicMaterial[] array = obj as Minecraft.PhysicSystem.PhysicMaterial[];
				translator.PushMinecraftPhysicSystemPhysicMaterial(L, array[index]);
				return true;
			}
			else if (type == typeof(Minecraft.PhysicSystem.PhysicState[]))
			{
			    Minecraft.PhysicSystem.PhysicState[] array = obj as Minecraft.PhysicSystem.PhysicState[];
				translator.PushMinecraftPhysicSystemPhysicState(L, array[index]);
				return true;
			}
			else if (type == typeof(Minecraft.Noises.UniformRNG[]))
			{
			    Minecraft.Noises.UniformRNG[] array = obj as Minecraft.Noises.UniformRNG[];
				translator.PushMinecraftNoisesUniformRNG(L, array[index]);
				return true;
			}
			else if (type == typeof(Minecraft.Configurations.BiomeId[]))
			{
			    Minecraft.Configurations.BiomeId[] array = obj as Minecraft.Configurations.BiomeId[];
				translator.PushMinecraftConfigurationsBiomeId(L, array[index]);
				return true;
			}
			else if (type == typeof(Minecraft.Configurations.BlockEntityConversion[]))
			{
			    Minecraft.Configurations.BlockEntityConversion[] array = obj as Minecraft.Configurations.BlockEntityConversion[];
				translator.PushMinecraftConfigurationsBlockEntityConversion(L, array[index]);
				return true;
			}
			else if (type == typeof(Minecraft.Configurations.BlockFace[]))
			{
			    Minecraft.Configurations.BlockFace[] array = obj as Minecraft.Configurations.BlockFace[];
				translator.PushMinecraftConfigurationsBlockFace(L, array[index]);
				return true;
			}
			else if (type == typeof(Minecraft.Configurations.BlockFaceCorner[]))
			{
			    Minecraft.Configurations.BlockFaceCorner[] array = obj as Minecraft.Configurations.BlockFaceCorner[];
				translator.PushMinecraftConfigurationsBlockFaceCorner(L, array[index]);
				return true;
			}
			else if (type == typeof(Minecraft.Configurations.BlockFlags[]))
			{
			    Minecraft.Configurations.BlockFlags[] array = obj as Minecraft.Configurations.BlockFlags[];
				translator.PushMinecraftConfigurationsBlockFlags(L, array[index]);
				return true;
			}
            return false;
		}
		
		internal static bool __tryArraySet(Type type, RealStatePtr L, ObjectTranslator translator, object obj, int array_idx, int obj_idx)
		{
		
			if (type == typeof(UnityEngine.Vector2[]))
			{
			    UnityEngine.Vector2[] array = obj as UnityEngine.Vector2[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Vector3[]))
			{
			    UnityEngine.Vector3[] array = obj as UnityEngine.Vector3[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Vector4[]))
			{
			    UnityEngine.Vector4[] array = obj as UnityEngine.Vector4[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Color[]))
			{
			    UnityEngine.Color[] array = obj as UnityEngine.Color[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Quaternion[]))
			{
			    UnityEngine.Quaternion[] array = obj as UnityEngine.Quaternion[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Ray[]))
			{
			    UnityEngine.Ray[] array = obj as UnityEngine.Ray[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Bounds[]))
			{
			    UnityEngine.Bounds[] array = obj as UnityEngine.Bounds[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(UnityEngine.Ray2D[]))
			{
			    UnityEngine.Ray2D[] array = obj as UnityEngine.Ray2D[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Minecraft.ChunkPos[]))
			{
			    Minecraft.ChunkPos[] array = obj as Minecraft.ChunkPos[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Minecraft.ModificationSource[]))
			{
			    Minecraft.ModificationSource[] array = obj as Minecraft.ModificationSource[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Minecraft.ScriptableWorldGeneration.GenLayers.NativeInt2DArray[]))
			{
			    Minecraft.ScriptableWorldGeneration.GenLayers.NativeInt2DArray[] array = obj as Minecraft.ScriptableWorldGeneration.GenLayers.NativeInt2DArray[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Minecraft.Rendering.SectionMeshVertexData[]))
			{
			    Minecraft.Rendering.SectionMeshVertexData[] array = obj as Minecraft.Rendering.SectionMeshVertexData[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Minecraft.PhysicSystem.AABB[]))
			{
			    Minecraft.PhysicSystem.AABB[] array = obj as Minecraft.PhysicSystem.AABB[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Minecraft.PhysicSystem.PhysicMaterial[]))
			{
			    Minecraft.PhysicSystem.PhysicMaterial[] array = obj as Minecraft.PhysicSystem.PhysicMaterial[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Minecraft.PhysicSystem.PhysicState[]))
			{
			    Minecraft.PhysicSystem.PhysicState[] array = obj as Minecraft.PhysicSystem.PhysicState[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Minecraft.Noises.UniformRNG[]))
			{
			    Minecraft.Noises.UniformRNG[] array = obj as Minecraft.Noises.UniformRNG[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Minecraft.Configurations.BiomeId[]))
			{
			    Minecraft.Configurations.BiomeId[] array = obj as Minecraft.Configurations.BiomeId[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Minecraft.Configurations.BlockEntityConversion[]))
			{
			    Minecraft.Configurations.BlockEntityConversion[] array = obj as Minecraft.Configurations.BlockEntityConversion[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Minecraft.Configurations.BlockFace[]))
			{
			    Minecraft.Configurations.BlockFace[] array = obj as Minecraft.Configurations.BlockFace[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Minecraft.Configurations.BlockFaceCorner[]))
			{
			    Minecraft.Configurations.BlockFaceCorner[] array = obj as Minecraft.Configurations.BlockFaceCorner[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Minecraft.Configurations.BlockFlags[]))
			{
			    Minecraft.Configurations.BlockFlags[] array = obj as Minecraft.Configurations.BlockFlags[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
			else if (type == typeof(Minecraft.Configurations.BlockVertexData[]))
			{
			    Minecraft.Configurations.BlockVertexData[] array = obj as Minecraft.Configurations.BlockVertexData[];
				translator.Get(L, obj_idx, out array[array_idx]);
				return true;
			}
            return false;
		}
	}
}
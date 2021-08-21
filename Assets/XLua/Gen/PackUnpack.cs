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
    public static partial class CopyByValue
    {
        
		
		public static void UnPack(ObjectTranslator translator, RealStatePtr L, int idx, out UnityEngine.Vector2 val)
		{
		    val = new UnityEngine.Vector2();
            int top = LuaAPI.lua_gettop(L);
			
			if (Utils.LoadField(L, idx, "x"))
            {
			    
                translator.Get(L, top + 1, out val.x);
				
            }
            LuaAPI.lua_pop(L, 1);
			
			if (Utils.LoadField(L, idx, "y"))
            {
			    
                translator.Get(L, top + 1, out val.y);
				
            }
            LuaAPI.lua_pop(L, 1);
			
		}
		
        public static bool Pack(IntPtr buff, int offset, UnityEngine.Vector2 field)
        {
            
            if(!LuaAPI.xlua_pack_float2(buff, offset, field.x, field.y))
            {
                return false;
            }
            
            return true;
        }
        public static bool UnPack(IntPtr buff, int offset, out UnityEngine.Vector2 field)
        {
            field = default(UnityEngine.Vector2);
            
            float x = default(float);
            float y = default(float);
            
            if(!LuaAPI.xlua_unpack_float2(buff, offset, out x, out y))
            {
                return false;
            }
            field.x = x;
            field.y = y;
            
            
            return true;
        }
        
		
		public static void UnPack(ObjectTranslator translator, RealStatePtr L, int idx, out UnityEngine.Vector3 val)
		{
		    val = new UnityEngine.Vector3();
            int top = LuaAPI.lua_gettop(L);
			
			if (Utils.LoadField(L, idx, "x"))
            {
			    
                translator.Get(L, top + 1, out val.x);
				
            }
            LuaAPI.lua_pop(L, 1);
			
			if (Utils.LoadField(L, idx, "y"))
            {
			    
                translator.Get(L, top + 1, out val.y);
				
            }
            LuaAPI.lua_pop(L, 1);
			
			if (Utils.LoadField(L, idx, "z"))
            {
			    
                translator.Get(L, top + 1, out val.z);
				
            }
            LuaAPI.lua_pop(L, 1);
			
		}
		
        public static bool Pack(IntPtr buff, int offset, UnityEngine.Vector3 field)
        {
            
            if(!LuaAPI.xlua_pack_float3(buff, offset, field.x, field.y, field.z))
            {
                return false;
            }
            
            return true;
        }
        public static bool UnPack(IntPtr buff, int offset, out UnityEngine.Vector3 field)
        {
            field = default(UnityEngine.Vector3);
            
            float x = default(float);
            float y = default(float);
            float z = default(float);
            
            if(!LuaAPI.xlua_unpack_float3(buff, offset, out x, out y, out z))
            {
                return false;
            }
            field.x = x;
            field.y = y;
            field.z = z;
            
            
            return true;
        }
        
		
		public static void UnPack(ObjectTranslator translator, RealStatePtr L, int idx, out UnityEngine.Vector4 val)
		{
		    val = new UnityEngine.Vector4();
            int top = LuaAPI.lua_gettop(L);
			
			if (Utils.LoadField(L, idx, "x"))
            {
			    
                translator.Get(L, top + 1, out val.x);
				
            }
            LuaAPI.lua_pop(L, 1);
			
			if (Utils.LoadField(L, idx, "y"))
            {
			    
                translator.Get(L, top + 1, out val.y);
				
            }
            LuaAPI.lua_pop(L, 1);
			
			if (Utils.LoadField(L, idx, "z"))
            {
			    
                translator.Get(L, top + 1, out val.z);
				
            }
            LuaAPI.lua_pop(L, 1);
			
			if (Utils.LoadField(L, idx, "w"))
            {
			    
                translator.Get(L, top + 1, out val.w);
				
            }
            LuaAPI.lua_pop(L, 1);
			
		}
		
        public static bool Pack(IntPtr buff, int offset, UnityEngine.Vector4 field)
        {
            
            if(!LuaAPI.xlua_pack_float4(buff, offset, field.x, field.y, field.z, field.w))
            {
                return false;
            }
            
            return true;
        }
        public static bool UnPack(IntPtr buff, int offset, out UnityEngine.Vector4 field)
        {
            field = default(UnityEngine.Vector4);
            
            float x = default(float);
            float y = default(float);
            float z = default(float);
            float w = default(float);
            
            if(!LuaAPI.xlua_unpack_float4(buff, offset, out x, out y, out z, out w))
            {
                return false;
            }
            field.x = x;
            field.y = y;
            field.z = z;
            field.w = w;
            
            
            return true;
        }
        
		
		public static void UnPack(ObjectTranslator translator, RealStatePtr L, int idx, out UnityEngine.Color val)
		{
		    val = new UnityEngine.Color();
            int top = LuaAPI.lua_gettop(L);
			
			if (Utils.LoadField(L, idx, "r"))
            {
			    
                translator.Get(L, top + 1, out val.r);
				
            }
            LuaAPI.lua_pop(L, 1);
			
			if (Utils.LoadField(L, idx, "g"))
            {
			    
                translator.Get(L, top + 1, out val.g);
				
            }
            LuaAPI.lua_pop(L, 1);
			
			if (Utils.LoadField(L, idx, "b"))
            {
			    
                translator.Get(L, top + 1, out val.b);
				
            }
            LuaAPI.lua_pop(L, 1);
			
			if (Utils.LoadField(L, idx, "a"))
            {
			    
                translator.Get(L, top + 1, out val.a);
				
            }
            LuaAPI.lua_pop(L, 1);
			
		}
		
        public static bool Pack(IntPtr buff, int offset, UnityEngine.Color field)
        {
            
            if(!LuaAPI.xlua_pack_float4(buff, offset, field.r, field.g, field.b, field.a))
            {
                return false;
            }
            
            return true;
        }
        public static bool UnPack(IntPtr buff, int offset, out UnityEngine.Color field)
        {
            field = default(UnityEngine.Color);
            
            float r = default(float);
            float g = default(float);
            float b = default(float);
            float a = default(float);
            
            if(!LuaAPI.xlua_unpack_float4(buff, offset, out r, out g, out b, out a))
            {
                return false;
            }
            field.r = r;
            field.g = g;
            field.b = b;
            field.a = a;
            
            
            return true;
        }
        
		
		public static void UnPack(ObjectTranslator translator, RealStatePtr L, int idx, out UnityEngine.Quaternion val)
		{
		    val = new UnityEngine.Quaternion();
            int top = LuaAPI.lua_gettop(L);
			
			if (Utils.LoadField(L, idx, "x"))
            {
			    
                translator.Get(L, top + 1, out val.x);
				
            }
            LuaAPI.lua_pop(L, 1);
			
			if (Utils.LoadField(L, idx, "y"))
            {
			    
                translator.Get(L, top + 1, out val.y);
				
            }
            LuaAPI.lua_pop(L, 1);
			
			if (Utils.LoadField(L, idx, "z"))
            {
			    
                translator.Get(L, top + 1, out val.z);
				
            }
            LuaAPI.lua_pop(L, 1);
			
			if (Utils.LoadField(L, idx, "w"))
            {
			    
                translator.Get(L, top + 1, out val.w);
				
            }
            LuaAPI.lua_pop(L, 1);
			
		}
		
        public static bool Pack(IntPtr buff, int offset, UnityEngine.Quaternion field)
        {
            
            if(!LuaAPI.xlua_pack_float4(buff, offset, field.x, field.y, field.z, field.w))
            {
                return false;
            }
            
            return true;
        }
        public static bool UnPack(IntPtr buff, int offset, out UnityEngine.Quaternion field)
        {
            field = default(UnityEngine.Quaternion);
            
            float x = default(float);
            float y = default(float);
            float z = default(float);
            float w = default(float);
            
            if(!LuaAPI.xlua_unpack_float4(buff, offset, out x, out y, out z, out w))
            {
                return false;
            }
            field.x = x;
            field.y = y;
            field.z = z;
            field.w = w;
            
            
            return true;
        }
        
		
		public static void UnPack(ObjectTranslator translator, RealStatePtr L, int idx, out UnityEngine.Ray val)
		{
		    val = new UnityEngine.Ray();
            int top = LuaAPI.lua_gettop(L);
			
			if (Utils.LoadField(L, idx, "origin"))
            {
			    
				var origin = val.origin;
				translator.Get(L, top + 1, out origin);
				val.origin = origin;
				
            }
            LuaAPI.lua_pop(L, 1);
			
			if (Utils.LoadField(L, idx, "direction"))
            {
			    
				var direction = val.direction;
				translator.Get(L, top + 1, out direction);
				val.direction = direction;
				
            }
            LuaAPI.lua_pop(L, 1);
			
		}
		
        public static bool Pack(IntPtr buff, int offset, UnityEngine.Ray field)
        {
            
            if(!Pack(buff, offset, field.origin))
            {
                return false;
            }
            
            if(!Pack(buff, offset + 12, field.direction))
            {
                return false;
            }
            
            return true;
        }
        public static bool UnPack(IntPtr buff, int offset, out UnityEngine.Ray field)
        {
            field = default(UnityEngine.Ray);
            
            var origin = field.origin;
            if(!UnPack(buff, offset, out origin))
            {
                return false;
            }
            field.origin = origin;
            
            var direction = field.direction;
            if(!UnPack(buff, offset + 12, out direction))
            {
                return false;
            }
            field.direction = direction;
            
            return true;
        }
        
		
		public static void UnPack(ObjectTranslator translator, RealStatePtr L, int idx, out UnityEngine.Bounds val)
		{
		    val = new UnityEngine.Bounds();
            int top = LuaAPI.lua_gettop(L);
			
			if (Utils.LoadField(L, idx, "center"))
            {
			    
				var center = val.center;
				translator.Get(L, top + 1, out center);
				val.center = center;
				
            }
            LuaAPI.lua_pop(L, 1);
			
			if (Utils.LoadField(L, idx, "extents"))
            {
			    
				var extents = val.extents;
				translator.Get(L, top + 1, out extents);
				val.extents = extents;
				
            }
            LuaAPI.lua_pop(L, 1);
			
		}
		
        public static bool Pack(IntPtr buff, int offset, UnityEngine.Bounds field)
        {
            
            if(!Pack(buff, offset, field.center))
            {
                return false;
            }
            
            if(!Pack(buff, offset + 12, field.extents))
            {
                return false;
            }
            
            return true;
        }
        public static bool UnPack(IntPtr buff, int offset, out UnityEngine.Bounds field)
        {
            field = default(UnityEngine.Bounds);
            
            var center = field.center;
            if(!UnPack(buff, offset, out center))
            {
                return false;
            }
            field.center = center;
            
            var extents = field.extents;
            if(!UnPack(buff, offset + 12, out extents))
            {
                return false;
            }
            field.extents = extents;
            
            return true;
        }
        
		
		public static void UnPack(ObjectTranslator translator, RealStatePtr L, int idx, out UnityEngine.Ray2D val)
		{
		    val = new UnityEngine.Ray2D();
            int top = LuaAPI.lua_gettop(L);
			
			if (Utils.LoadField(L, idx, "origin"))
            {
			    
				var origin = val.origin;
				translator.Get(L, top + 1, out origin);
				val.origin = origin;
				
            }
            LuaAPI.lua_pop(L, 1);
			
			if (Utils.LoadField(L, idx, "direction"))
            {
			    
				var direction = val.direction;
				translator.Get(L, top + 1, out direction);
				val.direction = direction;
				
            }
            LuaAPI.lua_pop(L, 1);
			
		}
		
        public static bool Pack(IntPtr buff, int offset, UnityEngine.Ray2D field)
        {
            
            if(!Pack(buff, offset, field.origin))
            {
                return false;
            }
            
            if(!Pack(buff, offset + 8, field.direction))
            {
                return false;
            }
            
            return true;
        }
        public static bool UnPack(IntPtr buff, int offset, out UnityEngine.Ray2D field)
        {
            field = default(UnityEngine.Ray2D);
            
            var origin = field.origin;
            if(!UnPack(buff, offset, out origin))
            {
                return false;
            }
            field.origin = origin;
            
            var direction = field.direction;
            if(!UnPack(buff, offset + 8, out direction))
            {
                return false;
            }
            field.direction = direction;
            
            return true;
        }
        
		
		public static void UnPack(ObjectTranslator translator, RealStatePtr L, int idx, out Minecraft.ChunkPos val)
		{
		    val = new Minecraft.ChunkPos();
            int top = LuaAPI.lua_gettop(L);
			
		}
		
        public static bool Pack(IntPtr buff, int offset, Minecraft.ChunkPos field)
        {
            
            return true;
        }
        public static bool UnPack(IntPtr buff, int offset, out Minecraft.ChunkPos field)
        {
            field = default(Minecraft.ChunkPos);
            
            return true;
        }
        
		
		public static void UnPack(ObjectTranslator translator, RealStatePtr L, int idx, out Minecraft.ScriptableWorldGeneration.GenLayers.NativeInt2DArray val)
		{
		    val = new Minecraft.ScriptableWorldGeneration.GenLayers.NativeInt2DArray();
            int top = LuaAPI.lua_gettop(L);
			
		}
		
        public static bool Pack(IntPtr buff, int offset, Minecraft.ScriptableWorldGeneration.GenLayers.NativeInt2DArray field)
        {
            
            return true;
        }
        public static bool UnPack(IntPtr buff, int offset, out Minecraft.ScriptableWorldGeneration.GenLayers.NativeInt2DArray field)
        {
            field = default(Minecraft.ScriptableWorldGeneration.GenLayers.NativeInt2DArray);
            
            return true;
        }
        
		
		public static void UnPack(ObjectTranslator translator, RealStatePtr L, int idx, out Minecraft.Rendering.SectionMeshVertexData val)
		{
		    val = new Minecraft.Rendering.SectionMeshVertexData();
            int top = LuaAPI.lua_gettop(L);
			
			if (Utils.LoadField(L, idx, "PositionOS"))
            {
			    
                translator.Get(L, top + 1, out val.PositionOS);
				
            }
            LuaAPI.lua_pop(L, 1);
			
			if (Utils.LoadField(L, idx, "UV"))
            {
			    
                translator.Get(L, top + 1, out val.UV);
				
            }
            LuaAPI.lua_pop(L, 1);
			
			if (Utils.LoadField(L, idx, "TexIndices"))
            {
			    
                translator.Get(L, top + 1, out val.TexIndices);
				
            }
            LuaAPI.lua_pop(L, 1);
			
			if (Utils.LoadField(L, idx, "Lights"))
            {
			    
                translator.Get(L, top + 1, out val.Lights);
				
            }
            LuaAPI.lua_pop(L, 1);
			
			if (Utils.LoadField(L, idx, "BlockPositionWS"))
            {
			    
                translator.Get(L, top + 1, out val.BlockPositionWS);
				
            }
            LuaAPI.lua_pop(L, 1);
			
		}
		
        public static bool Pack(IntPtr buff, int offset, Minecraft.Rendering.SectionMeshVertexData field)
        {
            
            if(!Pack(buff, offset, field.PositionOS))
            {
                return false;
            }
            
            if(!Pack(buff, offset + 12, field.UV))
            {
                return false;
            }
            
            if(!Pack(buff, offset + 20, field.TexIndices))
            {
                return false;
            }
            
            if(!Pack(buff, offset + 20, field.Lights))
            {
                return false;
            }
            
            if(!Pack(buff, offset + 32, field.BlockPositionWS))
            {
                return false;
            }
            
            return true;
        }
        public static bool UnPack(IntPtr buff, int offset, out Minecraft.Rendering.SectionMeshVertexData field)
        {
            field = default(Minecraft.Rendering.SectionMeshVertexData);
            
            if(!UnPack(buff, offset, out field.PositionOS))
            {
                return false;
            }
            
            if(!UnPack(buff, offset + 12, out field.UV))
            {
                return false;
            }
            
            if(!UnPack(buff, offset + 20, out field.TexIndices))
            {
                return false;
            }
            
            if(!UnPack(buff, offset + 20, out field.Lights))
            {
                return false;
            }
            
            if(!UnPack(buff, offset + 32, out field.BlockPositionWS))
            {
                return false;
            }
            
            return true;
        }
        
		
        public static bool Pack(IntPtr buff, int offset, UnityEngine.Vector3Int field)
        {
            
            return true;
        }
        public static bool UnPack(IntPtr buff, int offset, out UnityEngine.Vector3Int field)
        {
            field = default(UnityEngine.Vector3Int);
            
            return true;
        }
        
		
		public static void UnPack(ObjectTranslator translator, RealStatePtr L, int idx, out Minecraft.PhysicSystem.AABB val)
		{
		    val = new Minecraft.PhysicSystem.AABB();
            int top = LuaAPI.lua_gettop(L);
			
		}
		
        public static bool Pack(IntPtr buff, int offset, Minecraft.PhysicSystem.AABB field)
        {
            
            return true;
        }
        public static bool UnPack(IntPtr buff, int offset, out Minecraft.PhysicSystem.AABB field)
        {
            field = default(Minecraft.PhysicSystem.AABB);
            
            return true;
        }
        
		
		public static void UnPack(ObjectTranslator translator, RealStatePtr L, int idx, out Minecraft.PhysicSystem.PhysicMaterial val)
		{
		    val = new Minecraft.PhysicSystem.PhysicMaterial();
            int top = LuaAPI.lua_gettop(L);
			
			if (Utils.LoadField(L, idx, "CoefficientOfRestitution"))
            {
			    
                translator.Get(L, top + 1, out val.CoefficientOfRestitution);
				
            }
            LuaAPI.lua_pop(L, 1);
			
			if (Utils.LoadField(L, idx, "CoefficientOfStaticFriction"))
            {
			    
                translator.Get(L, top + 1, out val.CoefficientOfStaticFriction);
				
            }
            LuaAPI.lua_pop(L, 1);
			
			if (Utils.LoadField(L, idx, "CoefficientOfDynamicFriction"))
            {
			    
                translator.Get(L, top + 1, out val.CoefficientOfDynamicFriction);
				
            }
            LuaAPI.lua_pop(L, 1);
			
		}
		
        public static bool Pack(IntPtr buff, int offset, Minecraft.PhysicSystem.PhysicMaterial field)
        {
            
            if(!LuaAPI.xlua_pack_float3(buff, offset, field.CoefficientOfRestitution, field.CoefficientOfStaticFriction, field.CoefficientOfDynamicFriction))
            {
                return false;
            }
            
            return true;
        }
        public static bool UnPack(IntPtr buff, int offset, out Minecraft.PhysicSystem.PhysicMaterial field)
        {
            field = default(Minecraft.PhysicSystem.PhysicMaterial);
            
            float CoefficientOfRestitution = default(float);
            float CoefficientOfStaticFriction = default(float);
            float CoefficientOfDynamicFriction = default(float);
            
            if(!LuaAPI.xlua_unpack_float3(buff, offset, out CoefficientOfRestitution, out CoefficientOfStaticFriction, out CoefficientOfDynamicFriction))
            {
                return false;
            }
            field.CoefficientOfRestitution = CoefficientOfRestitution;
            field.CoefficientOfStaticFriction = CoefficientOfStaticFriction;
            field.CoefficientOfDynamicFriction = CoefficientOfDynamicFriction;
            
            
            return true;
        }
        
		
		public static void UnPack(ObjectTranslator translator, RealStatePtr L, int idx, out Minecraft.Noises.UniformRNG val)
		{
		    val = new Minecraft.Noises.UniformRNG();
            int top = LuaAPI.lua_gettop(L);
			
		}
		
        public static bool Pack(IntPtr buff, int offset, Minecraft.Noises.UniformRNG field)
        {
            
            return true;
        }
        public static bool UnPack(IntPtr buff, int offset, out Minecraft.Noises.UniformRNG field)
        {
            field = default(Minecraft.Noises.UniformRNG);
            
            return true;
        }
        
    }
}
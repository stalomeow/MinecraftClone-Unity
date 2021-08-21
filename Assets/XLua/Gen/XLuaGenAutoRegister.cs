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
using System.Collections.Generic;
using System.Reflection;


namespace XLua.CSObjectWrap
{
    public class XLua_Gen_Initer_Register__
	{
        
        
        static void wrapInit0(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(Minecraft.MathUtility), MinecraftMathUtilityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.ModificationSource), MinecraftModificationSourceWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.WorldConsts), MinecraftWorldConstsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.WorldSetting), MinecraftWorldSettingWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.WorldUtility), MinecraftWorldUtilityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Utils.ArrayUtility), MinecraftUtilsArrayUtilityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Rendering.LightingUtility), MinecraftRenderingLightingUtilityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Rendering.RenderingUtility), MinecraftRenderingRenderingUtilityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Rendering.ShaderUtility), MinecraftRenderingShaderUtilityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.PhysicSystem.BlockPhysicsUtility), MinecraftPhysicSystemBlockPhysicsUtilityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.PhysicSystem.PhysicState), MinecraftPhysicSystemPhysicStateWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.PhysicSystem.Physics), MinecraftPhysicSystemPhysicsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Lua.LuaUtility), MinecraftLuaLuaUtilityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Entities.Entity), MinecraftEntitiesEntityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Entities.IAABBEntity), MinecraftEntitiesIAABBEntityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Entities.IRenderableEntity), MinecraftEntitiesIRenderableEntityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Configurations.BiomeId), MinecraftConfigurationsBiomeIdWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Configurations.BlockEntityConversion), MinecraftConfigurationsBlockEntityConversionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Configurations.BlockFace), MinecraftConfigurationsBlockFaceWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Configurations.BlockFaceCorner), MinecraftConfigurationsBlockFaceCornerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Configurations.BlockFlags), MinecraftConfigurationsBlockFlagsWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Configurations.BlockUtility), MinecraftConfigurationsBlockUtilityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Collections.HashUtility), MinecraftCollectionsHashUtilityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Chunk), MinecraftChunkWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Chunk3x3Accessor), MinecraftChunk3x3AccessorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.ChunkBuilder), MinecraftChunkBuilderWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.ChunkManager), MinecraftChunkManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.ChunkPos), MinecraftChunkPosWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.IWorld), MinecraftIWorldWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.IWorldRAccessor), MinecraftIWorldRAccessorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.IWorldRWAccessor), MinecraftIWorldRWAccessorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.World), MinecraftWorldWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.ScriptableWorldGeneration.CaveGenerator), MinecraftScriptableWorldGenerationCaveGeneratorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.ScriptableWorldGeneration.GenerationContext), MinecraftScriptableWorldGenerationGenerationContextWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.ScriptableWorldGeneration.GenerationHelper), MinecraftScriptableWorldGenerationGenerationHelperWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.ScriptableWorldGeneration.MineGenerator), MinecraftScriptableWorldGenerationMineGeneratorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.ScriptableWorldGeneration.PlantGenerator), MinecraftScriptableWorldGenerationPlantGeneratorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.ScriptableWorldGeneration.StatelessGenerator), MinecraftScriptableWorldGenerationStatelessGeneratorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.ScriptableWorldGeneration.TerrainGenerator), MinecraftScriptableWorldGenerationTerrainGeneratorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.ScriptableWorldGeneration.TreeGenerator), MinecraftScriptableWorldGenerationTreeGeneratorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.ScriptableWorldGeneration.WorldGeneratePipeline), MinecraftScriptableWorldGenerationWorldGeneratePipelineWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.ScriptableWorldGeneration.GenLayers.AddBeachLayer), MinecraftScriptableWorldGenerationGenLayersAddBeachLayerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.ScriptableWorldGeneration.GenLayers.AddIslandLayer), MinecraftScriptableWorldGenerationGenLayersAddIslandLayerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.ScriptableWorldGeneration.GenLayers.AddRiverLayer), MinecraftScriptableWorldGenerationGenLayersAddRiverLayerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.ScriptableWorldGeneration.GenLayers.BiomeLayer), MinecraftScriptableWorldGenerationGenLayersBiomeLayerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.ScriptableWorldGeneration.GenLayers.IslandLayer), MinecraftScriptableWorldGenerationGenLayersIslandLayerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.ScriptableWorldGeneration.GenLayers.ZoomLayer), MinecraftScriptableWorldGenerationGenLayersZoomLayerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.ScriptableWorldGeneration.GenLayers.NativeInt2DArray), MinecraftScriptableWorldGenerationGenLayersNativeInt2DArrayWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.ScriptableWorldGeneration.GenLayers.StatelessGenLayer), MinecraftScriptableWorldGenerationGenLayersStatelessGenLayerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Rendering.SectionMeshManager), MinecraftRenderingSectionMeshManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Rendering.SectionMeshVertexData), MinecraftRenderingSectionMeshVertexDataWrap.__Register);
        
        }
        
        static void wrapInit1(LuaEnv luaenv, ObjectTranslator translator)
        {
        
            translator.DelayWrapLoader(typeof(Minecraft.Rendering.SectionMeshWorkScheduler), MinecraftRenderingSectionMeshWorkSchedulerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Rendering.SectionRenderingManager), MinecraftRenderingSectionRenderingManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.PlayerControls.BlockInteraction), MinecraftPlayerControlsBlockInteractionWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.PlayerControls.FluidInteractor), MinecraftPlayerControlsFluidInteractorWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.PhysicSystem.AABB), MinecraftPhysicSystemAABBWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.PhysicSystem.BlockRaycastHit), MinecraftPhysicSystemBlockRaycastHitWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.PhysicSystem.PhysicMaterial), MinecraftPhysicSystemPhysicMaterialWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Noises.INoise), MinecraftNoisesINoiseWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Noises.PerlinNoise), MinecraftNoisesPerlinNoiseWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Noises.UniformRNG), MinecraftNoisesUniformRNGWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Lua.ILuaCallCSharp), MinecraftLuaILuaCallCSharpWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Lua.LuaManager), MinecraftLuaLuaManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Entities.EntityManager), MinecraftEntitiesEntityManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Entities.LuaBlockEntity), MinecraftEntitiesLuaBlockEntityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Entities.PlayerEntity), MinecraftEntitiesPlayerEntityWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Configurations.BiomeData), MinecraftConfigurationsBiomeDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Configurations.BiomeTable), MinecraftConfigurationsBiomeTableWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Configurations.BlockData), MinecraftConfigurationsBlockDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Configurations.BlockMesh), MinecraftConfigurationsBlockMeshWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Configurations.BlockTable), MinecraftConfigurationsBlockTableWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Configurations.BlockVertexData), MinecraftConfigurationsBlockVertexDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Configurations.IOrderedConfigData), MinecraftConfigurationsIOrderedConfigDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Configurations.ItemData), MinecraftConfigurationsItemDataWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Collections.NibbleArray), MinecraftCollectionsNibbleArrayWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Audio.AudioManager), MinecraftAudioAudioManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Assets.AssetBundleInfo), MinecraftAssetsAssetBundleInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Assets.AssetCatalog), MinecraftAssetsAssetCatalogWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Assets.AssetInfo), MinecraftAssetsAssetInfoWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Assets.AssetManager), MinecraftAssetsAssetManagerWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Assets.AssetPtr), MinecraftAssetsAssetPtrWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Assets.AsyncAsset), MinecraftAssetsAsyncAssetWrap.__Register);
        
        
            translator.DelayWrapLoader(typeof(Minecraft.Assets.IAssetBundle), MinecraftAssetsIAssetBundleWrap.__Register);
        
        
        
        }
        
        static void Init(LuaEnv luaenv, ObjectTranslator translator)
        {
            
            wrapInit0(luaenv, translator);
            
            wrapInit1(luaenv, translator);
            
            
            translator.AddInterfaceBridgeCreator(typeof(System.Collections.IEnumerator), SystemCollectionsIEnumeratorBridge.__Create);
            
            translator.AddInterfaceBridgeCreator(typeof(Minecraft.Lua.ICSharpCallLua), MinecraftLuaICSharpCallLuaBridge.__Create);
            
            translator.AddInterfaceBridgeCreator(typeof(Minecraft.Configurations.IBlockBehaviour), MinecraftConfigurationsIBlockBehaviourBridge.__Create);
            
        }
        
	    static XLua_Gen_Initer_Register__()
        {
		    XLua.LuaEnv.AddIniter(Init);
		}
		
		
	}
	
}
namespace XLua
{
	public partial class ObjectTranslator
	{
		static XLua.CSObjectWrap.XLua_Gen_Initer_Register__ s_gen_reg_dumb_obj = new XLua.CSObjectWrap.XLua_Gen_Initer_Register__();
		static XLua.CSObjectWrap.XLua_Gen_Initer_Register__ gen_reg_dumb_obj {get{return s_gen_reg_dumb_obj;}}
	}
	
	internal partial class InternalGlobals
    {
	    
		delegate UnityEngine.Vector3Int __GEN_DELEGATE0( UnityEngine.Vector3 vec);
		
	    static InternalGlobals()
		{
		    extensionMethodMap = new Dictionary<Type, IEnumerable<MethodInfo>>()
			{
			    
				{typeof(UnityEngine.Vector3), new List<MethodInfo>(){
				
				  new __GEN_DELEGATE0(Minecraft.MathUtility.FloorToInt)
#if UNITY_WSA && !UNITY_EDITOR
                                      .GetMethodInfo(),
#else
                                      .Method,
#endif
				
				}},
				
			};
			
			genTryArrayGetPtr = StaticLuaCallbacks.__tryArrayGet;
            genTryArraySetPtr = StaticLuaCallbacks.__tryArraySet;
		}
	}
}

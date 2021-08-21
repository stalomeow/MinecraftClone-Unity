using Minecraft.Audio;
using Minecraft.Configurations;
using Minecraft.Entities;
using Minecraft.Lua;
using Minecraft.Rendering;
using Minecraft.ScriptableWorldGeneration;
using UnityEngine;
using XLua;

namespace Minecraft
{
    public interface IWorld : ILuaCallCSharp, IHotfixable
    {
        bool Initialized { get; }

        IWorldRWAccessor RWAccessor { get; }

        Transform PlayerTransform { get; }

        Camera MainCamera { get; }

        AudioManager AudioManager { get; }

        LuaManager LuaManager { get; }

        ChunkManager ChunkManager { get; }

        SectionRenderingManager RenderingManager { get; }

        EntityManager EntityManager { get; }

        BlockTable BlockDataTable { get; }

        BiomeTable BiomeDataTable { get; }

        WorldGeneratePipeline WorldGenPipeline { get; }

        int MaxTickBlockCountPerFrame { get; set; }

        int MaxLightBlockCountPerFrame { get; set; }

        void LightBlock(int x, int y, int z, ModificationSource source);

        void TickBlock(int x, int y, int z);

        void MarkBlockMeshDirty(int x, int y, int z, ModificationSource source);
    }
}

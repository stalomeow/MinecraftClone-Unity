using UnityEngine;

namespace Minecraft.Rendering
{
    [XLua.LuaCallCSharp]
    public static class ShaderUtility
    {
        private static readonly int s_BlockTextures = Shader.PropertyToID("_BlockTextures");
        private static readonly int s_DigProgressTextures = Shader.PropertyToID("_DigProgressTextures");
        private static readonly int s_RenderDistance = Shader.PropertyToID("_RenderDistance");
        private static readonly int s_ViewDistance = Shader.PropertyToID("_ViewDistance");
        private static readonly int s_LightLimits = Shader.PropertyToID("_LightLimits");
        private static readonly int s_WorldAmbientColorDay = Shader.PropertyToID("_WorldAmbientColorDay");
        private static readonly int s_WorldAmbientColorNight = Shader.PropertyToID("_WorldAmbientColorNight");
        private static readonly int s_TargetBlockPosition = Shader.PropertyToID("_TargetBlockPosition");
        private static readonly int s_DigProgress = Shader.PropertyToID("_DigProgress");

        public static Texture2DArray BlockTextures
        {
            get => Shader.GetGlobalTexture(s_BlockTextures) as Texture2DArray;
            set => Shader.SetGlobalTexture(s_BlockTextures, value);
        }

        public static Texture2DArray DigProgressTextures
        {
            get => Shader.GetGlobalTexture(s_DigProgressTextures) as Texture2DArray;
            set => Shader.SetGlobalTexture(s_DigProgressTextures, value);
        }

        public static int RenderDistance
        {
            get => Shader.GetGlobalInt(s_RenderDistance);
            set => Shader.SetGlobalInt(s_RenderDistance, value);
        }

        public static int ViewDistance
        {
            get => Shader.GetGlobalInt(s_ViewDistance);
            set => Shader.SetGlobalInt(s_ViewDistance, value);
        }

        public static Vector2 LightLimits
        {
            get => Shader.GetGlobalVector(s_LightLimits);
            set => Shader.SetGlobalVector(s_LightLimits, value);
        }

        public static Color WorldAmbientColorDay
        {
            get => Shader.GetGlobalColor(s_WorldAmbientColorDay);
            set => Shader.SetGlobalColor(s_WorldAmbientColorDay, value);
        }

        public static Color WorldAmbientColorNight
        {
            get => Shader.GetGlobalColor(s_WorldAmbientColorNight);
            set => Shader.SetGlobalColor(s_WorldAmbientColorNight, value);
        }

        public static Vector3 TargetedBlockPosition
        {
            get => Shader.GetGlobalVector(s_TargetBlockPosition);
            set => Shader.SetGlobalVector(s_TargetBlockPosition, value);
        }

        public static int DigProgress
        {
            get => Shader.GetGlobalInt(s_DigProgress);
            set => Shader.SetGlobalInt(s_DigProgress, value);
        }
    }
}

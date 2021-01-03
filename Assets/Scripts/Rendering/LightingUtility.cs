using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace Minecraft.Rendering
{
    public static class LightingUtility
    {
        public const byte MaxLight = 15;
        public const byte SkyLight = MaxLight;

        private const byte SkyLightSubtracted = 2; // temp
        private const float OverMaxLight = 1f / MaxLight;

        /// <summary>
        /// 方块光照传播时的最小阻挡值
        /// </summary>
        private const int MinBlockLightOpacity = 1;

        /// <summary>
        /// 非空气方块受到的最大的天空光照值
        /// </summary>
        /// 
        /// <remarks>
        /// 方块光照最高也只能到14级（固体方块光源的发光等级是15，但仅仅是光源本身所在位置是这个等级）
        /// https://minecraft-zh.gamepedia.com/%E4%BA%AE%E5%BA%A6
        /// </remarks>
        private const int MaxNonAirBlockSkyLightValue = MaxLight - 1;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetLightInShaders(byte light)
        {
            return Mathf.Clamp01(light * OverMaxLight);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ClampBlockLight(int light)
        {
            return Mathf.Clamp(light, 0, MaxNonAirBlockSkyLightValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ClampSkyLight(int light)
        {
            return Mathf.Clamp(light - SkyLightSubtracted, 0, MaxLight);
        }
    }
}
#ifndef MINECRAFT_INPUT_INCLUDED
#define MINECRAFT_INPUT_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

// 所有方块纹理（包括法线贴图、PBR 贴图）
TEXTURE2D_ARRAY(_BlockTextures); SAMPLER(sampler_BlockTextures);

// 方块挖掘进度贴图
TEXTURE2D_ARRAY(_DigProgressTextures); SAMPLER(sampler_DigProgressTextures);

// 方块的挖掘进度（贴图的索引）
int _DigProgress;

// 当前玩家准心瞄准的方块的世界坐标
float3 _TargetBlockPosition;

// 渲染距离，以方块为单位
int _RenderDistance;

// 视野距离，以方块为单位
int _ViewDistance;

// 光照限制
// x - 最小光照级别 [0, 0.5]
// y - 最大光照级别 [0.5, 1]
half2 _LightLimits;

// 白天世界环境的颜色，方块在 XOZ 平面上距离相机越远，颜色越接近该值
half4 _WorldAmbientColorDay;

// 夜晚世界环境的颜色，方块在 XOZ 平面上距离相机越远，颜色越接近该值
half4 _WorldAmbientColorNight;

#endif // MINECRAFT_INPUT_INCLUDED
#ifndef MINECRAFT_INPUT_INCLUDED
#define MINECRAFT_INPUT_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

// 所有方块纹理（包括法线贴图、PBR 贴图）
TEXTURE2D_ARRAY(_BlockTextures); SAMPLER(sampler_BlockTextures);

// 渲染距离，以方块为单位
int _RenderDistance;

// 视野距离，以方块为单位
int _ViewDistance;

// 光照限制
// x - 最小光照级别 [0, 1]
// y - 最大光照级别 [0, 1]
half2 _LightLimits;

// 世界环境的颜色，方块在 XOZ 平面上距离相机越远，颜色越接近该值
half4 _WorldAmbientColor;

// _TargetBlockPosition
float3 _TargetBlockPosition;

// _DigProgress
int _DigProgress;

#endif // MINECRAFT_INPUT_INCLUDED
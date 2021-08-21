#ifndef MINECRAFT_SHADOW_CASTER_INCLUDED
#define MINECRAFT_SHADOW_CASTER_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
#include "Includes/Minecraft/BlockBRDF.hlsl"

float3 _LightDirection;

struct ShadowVaryings
{
    float2 uv: TEXCOORD0;
    int3 texIndices : TEXCOORD1;
    float4 positionCS: SV_POSITION;
};

float4 GetShadowPositionHClip(BlockAttributes input)
{
    float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
    float3 normalWS = TransformObjectToWorldNormal(input.normalOS);
    float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _LightDirection));

#if UNITY_REVERSED_Z
    positionCS.z = min(positionCS.z, positionCS.w * UNITY_NEAR_CLIP_VALUE);
#else
    positionCS.z = max(positionCS.z, positionCS.w * UNITY_NEAR_CLIP_VALUE);
#endif

    return positionCS;
}

#endif // MINECRAFT_SHADOW_CASTER_INCLUDED
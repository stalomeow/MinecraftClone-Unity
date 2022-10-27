#ifndef MINECRAFT_CORE_INCLUDED
#define MINECRAFT_CORE_INCLUDED

#include "Includes/Minecraft/Input.hlsl"

#define SAMPLE_BLOCK_TEXTURE(uv, index) SAMPLE_TEXTURE2D_ARRAY(_BlockTextures, sampler_BlockTextures, uv, index)
#define SAMPLE_BLOCK_ALBEDO(uv, indices) SAMPLE_BLOCK_TEXTURE(uv, indices.x)
#define SAMPLE_BLOCK_NORMAL(uv, indices) SAMPLE_BLOCK_TEXTURE(uv, indices.y)
#define SAMPLE_BLOCK_MER(uv, indices) SAMPLE_BLOCK_TEXTURE(uv, indices.z)

inline half4 EaseIn(half4 a, half4 b, float w)
{
    return a + (b - a) * w * w * w; // 先慢后快
}

inline void HighlightBlock(float3 blockPos, float2 uv, half4 highlightColor, inout half4 color)
{
    float3 delta = blockPos - _TargetBlockPosition;
    float dist = delta.x * delta.x + delta.y * delta.y + delta.z * delta.z;

    UNITY_BRANCH
    if (dist <= 0.01)
    {
        color += highlightColor;

        UNITY_BRANCH
        if (_DigProgress > -1)
        {
            half4 tex = SAMPLE_TEXTURE2D_ARRAY(_DigProgressTextures, sampler_DigProgressTextures, uv, _DigProgress);
            color.rgb *= tex.rgb;
            color.a = saturate(color.a + tex.a);
        }
    }
}

struct BlockAttributes
{
    float4 positionOS : POSITION;
    float3 normalOS : NORMAL;
    float4 tangentOS : TANGENT;
    float2 uv : TEXCOORD0;
    int3 texIndices : TEXCOORD1; // x: albedo, y: normal, z: mer（对应纹理在纹理数组中的索引）
    float3 lights : TEXCOORD2; // x: emission, y: sky_light, z: block_light（均为 [0, 1] 的数字）
    float3 blockPositionWS : TEXCOORD3;
};

#endif // MINECRAFT_CORE_INCLUDED
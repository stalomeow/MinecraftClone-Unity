#ifndef MINECRAFT_SCREEN_SPACE_REFLECTION_INCLUDED
#define MINECRAFT_SCREEN_SPACE_REFLECTION_INCLUDED

#include "Includes/Minecraft/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

float2 PosToUV(float3 vpos)
{
    float4 projPos = mul(unity_CameraProjection, float4(vpos, 1));
    float3 screenPos = projPos.xyz / projPos.w;
    return float2(screenPos.x, screenPos.y) * 0.5 + 0.5;
}

float compareWithDepth(float3 vpos)
{
    float2 uv = PosToUV(vpos);
    float depth = LinearEyeDepth(SampleSceneDepth(uv), _ZBufferParams);
    int inside = uv.x > 0 && uv.x < 1 && uv.y > 0 && uv.y < 1;
    return lerp(0, abs(vpos.z + depth), inside);
}

bool RayMarching(float3 o, float3 r, out float2 hitUV)
{
    float3 start = o;
    float3 end = o;
    float stepSize = 0.15;
    float thinkness = 0.1;
    float triveled = 0;
    int maxMarching = 256;
    float maxDistance = 500;

    UNITY_LOOP
    for (int i = 1; i <= maxMarching; ++i)
    {
        end += r * stepSize;
        triveled += stepSize;

        if (triveled > maxDistance)
        {
            return false;
        }

        float collied = compareWithDepth(end);

        if (collied > 0)
        {
            hitUV = PosToUV(end);
            return true;
        }
    }

    return false;
}

#endif // MINECRAFT_SCREEN_SPACE_REFLECTION_INCLUDED
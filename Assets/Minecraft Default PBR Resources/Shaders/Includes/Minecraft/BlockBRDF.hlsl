#ifndef MINECRAFT_BLOCK_BRDF_INCLUDED
#define MINECRAFT_BLOCK_BRDF_INCLUDED

#include "Includes/Minecraft/Core.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/BSDF.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

struct BlockBRDFData
{
    half3 albedo;
    half metallic;
    half emission;
    half roughness;
    half skyLight;
    half blockLight;
    float3 positionWS;
    float3 normalWS;
    float3 viewDirWS;
    float4 shadowCoord;
};

inline half3 FresnelTerm(half3 c, half cosA)
{
    half t = pow(1 - cosA, 5);
    return c + (1 - c) * t;
}

inline half3 FresnelLerp(half3 c0, half3 c1, half cosA)
{
    half t = pow(1 - cosA, 5);
    return lerp(c0, c1, t);
}

inline void InitializeBlockBRDFData(half4 albedo, half4 mer, float3 positionWS, float3 normalWS, float3 lights, float3 viewDirWS, float4 shadowCoord, out BlockBRDFData data)
{
    data = (BlockBRDFData)0;
    data.albedo = albedo.rgb;
    data.metallic = mer.r;
    data.emission = mer.g * lights.x;
    data.roughness = mer.b;
    data.skyLight = lights.y;
    data.blockLight = lights.z;
    data.positionWS = positionWS;
    data.normalWS = normalWS;
    data.viewDirWS = viewDirWS;

#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
    data.shadowCoord = shadowCoord;
#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
    data.shadowCoord = TransformWorldToShadowCoord(positionWS);
#else
    data.shadowCoord = float4(0, 0, 0, 0);
#endif
}

inline half4 BlockFragmentPBR(BlockBRDFData input, half alpha)
{
    half oneMinusReflectivity = OneMinusReflectivityMetallic(input.metallic);

    half3 diffColor = input.albedo * oneMinusReflectivity;
    half3 specColor = lerp(kDieletricSpec.rgb, input.albedo, input.metallic);

    float3 normalWS = NormalizeNormalPerPixel(input.normalWS);
    half3 viewDirWS = SafeNormalize(input.viewDirWS);
    half3 reflectVector = reflect(-viewDirWS, normalWS);

    Light mainLight = GetMainLight(input.shadowCoord);
    half3 halfDir = SafeNormalize(mainLight.direction + viewDirWS);
    half nv = saturate(dot(normalWS, viewDirWS));
    half nl = saturate(dot(normalWS, mainLight.direction));
    half nh = saturate(dot(normalWS, halfDir));
    half lv = saturate(dot(mainLight.direction, viewDirWS));
    half lh = saturate(dot(mainLight.direction, halfDir));

    half isDay = saturate(mainLight.direction.y); // 白天为 1，夜晚为 0，昼夜交替时在 (0, 1) 内
    half receiveSkyLight = pow(input.skyLight, 2); // 如果直接被太阳照到就是 1，否则是 [0, 1) 间的一个比较小的数
    half skyLightLevel = lerp(_LightLimits.x, _LightLimits.y, input.skyLight * nl) * isDay; // 白天才受到太阳光照
    half perceptualRoughness = RoughnessToPerceptualRoughness(input.roughness);

    half3 dayDiffuse = diffColor * DisneyDiffuse(nv, nl, lv, perceptualRoughness) * skyLightLevel;
    half3 nightColor = (1 - isDay) * 0.02 * diffColor; // 晚上不能全黑，所以加一点亮度
    half3 diffuseTerm = (dayDiffuse + nightColor) * receiveSkyLight; // 不被太阳照到的地方，让它尽可能暗

    half DV = DV_SmithJointGGX(nh, nl, nv, input.roughness);
    half3 F = FresnelTerm(specColor, lh);
    half3 specularTerm = DV * F * nl * isDay * receiveSkyLight; // 白天并且直接被太阳照到的话才有

    half shadowAttenuation = lerp(1, mainLight.shadowAttenuation, isDay); // 晚上不要阴影

    half3 skyLightTerm = PI * (diffuseTerm + specularTerm) * mainLight.color * mainLight.distanceAttenuation * shadowAttenuation;
    half3 blockLightTerm = (diffColor + specColor) * input.blockLight; // 方块光照不能受阴影影响
    half3 emissionTerm = input.emission * input.albedo; // 自发光

    half4 color = half4(emissionTerm + max(skyLightTerm, blockLightTerm), alpha);

    // fade
    float dis = distance(input.positionWS.xz, GetCameraPositionWS().xz);
    half4 worldAmbientColor = lerp(_WorldAmbientColorNight, _WorldAmbientColorDay, isDay);
    return EaseIn(color, worldAmbientColor, saturate(dis / max(min(_RenderDistance, _ViewDistance), 0.01)));
}

#endif // MINECRAFT_BLOCK_BRDF_INCLUDED
Shader "Minecraft/Water (PBR)"
{
    Properties
    {
		[HDR] _MainColor("Main Color", Color) = (1, 1, 1, 1)
		[HDR] _DeepColor("Deep Color", Color) = (1, 1, 1, 1)
		_Depth("Depth", Float) = 1
		_BumpMap("Normal", 2D) = "bump" {}
		_BumpScale("Bump Scale", Float) = 1.0
		_FlowSpeed("Flow Speed", Range(0, 1)) = 1
		_FlowDirection1("Flow Direction 1", Vector) = (0.1, 0, -0.15, 0)
		_FlowDirection2("Flow Direction 2", Vector) = (-0.15, 0, 0.245, 0)
    }
    SubShader
    {
		HLSLINCLUDE
		#include "Includes/Minecraft/BlockBRDF.hlsl"

		CBUFFER_START(UnityPerMaterial)
			half4 _MainColor;
			half4 _DeepColor;
			half _Depth;
			TEXTURE2D(_BumpMap);
			float4 _BumpMap_ST;
			SAMPLER(sampler_BumpMap);
			half _BumpScale;
			half _FlowSpeed;
			half4 _FlowDirection1;
			half4 _FlowDirection2;
		CBUFFER_END
		ENDHLSL

		Tags
		{
			"RenderPipeline" = "UniversalPipeline"
			"UniversalMaterialType" = "Lit"
			"RenderType" = "Transparent"
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"ShaderModel"="4.5"
		}

        Pass
        {
			Tags { "LightMode" = "UniversalForward" }

			LOD 200
			Cull Off
			ZTest Less
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
			#pragma exclude_renderers gles gles3 glcore
            #pragma target 4.5

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareOpaqueTexture.hlsl"
			//#include "Includes/Minecraft/ScreenSpaceReflection.hlsl"

			#pragma vertex vert
            #pragma fragment frag

			struct Varyings
			{
				float2 uv : TEXCOORD0;
				int3 texIndices : TEXCOORD1;
				float3 positionWS : TEXCOORD2;
				float3 normalWS : TEXCOORD3;
				float4 tangentWS : TEXCOORD4;
				float3 lights : TEXCOORD5;
				float3 viewDirWS : TEXCOORD6;
				float4 shadowCoord : TEXCOORD7;
				float4 screenPos : TEXCOORD8;
				float4 positionCS : SV_POSITION;
			};

			Varyings vert(BlockAttributes input)
			{
				Varyings output = (Varyings)0;
				VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
				VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);

				half3 viewDirWS = GetWorldSpaceViewDir(vertexInput.positionWS);

				output.uv = input.uv;
				output.texIndices = input.texIndices;
				output.positionWS = vertexInput.positionWS;
				output.normalWS = normalInput.normalWS;
				real sign = input.tangentOS.w * GetOddNegativeScale();
				output.tangentWS = half4(normalInput.tangentWS.xyz, sign);
				output.lights = input.lights;
				output.viewDirWS = viewDirWS;

#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				output.shadowCoord = GetShadowCoord(vertexInput);
#endif
				output.screenPos = ComputeScreenPos(vertexInput.positionCS);
				output.positionCS = vertexInput.positionCS;
				return output;
			}

			float4 frag(Varyings input) : SV_TARGET
			{
				float depth = Linear01Depth(SampleSceneDepth(input.screenPos.xy / input.screenPos.w), _ZBufferParams);
				float depthValue = saturate(depth * _ProjectionParams.z - (input.screenPos.w + _Depth));
				half4 albedo = half4(SAMPLE_BLOCK_ALBEDO(input.uv, input.texIndices).rgb, 1) * lerp(_MainColor, _DeepColor, depthValue);

				float2 normalUV1 = input.positionWS.xz * _BumpMap_ST.xy + _BumpMap_ST.zw + _FlowDirection1.xz * _FlowSpeed * _Time.z;
				float2 normalUV2 = input.positionWS.xz * _BumpMap_ST.xy + _BumpMap_ST.zw + _FlowDirection2.xz * _FlowSpeed * _Time.z;
				float3 bitangent = input.tangentWS.w * cross(input.normalWS.xyz, input.tangentWS.xyz);

				float3 normalTS1 = UnpackNormalScale(SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, normalUV1), _BumpScale);
				float3 normalWS1 = TransformTangentToWorld(normalTS1, half3x3(input.tangentWS.xyz, bitangent.xyz, input.normalWS.xyz));
				float3 normalTS2 = UnpackNormalScale(SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, normalUV2), _BumpScale);
				float3 normalWS2 = TransformTangentToWorld(normalTS2, half3x3(input.tangentWS.xyz, bitangent.xyz, input.normalWS.xyz));
				float3 normalWS = normalWS1 + normalWS2;

				half4 mer = SAMPLE_BLOCK_MER(input.uv, input.texIndices);

				BlockBRDFData data;
				InitializeBlockBRDFData(albedo, mer, input.positionWS, normalWS, input.lights, input.viewDirWS, input.shadowCoord, data);
				return BlockFragmentPBR(data, albedo.a);
			}
            ENDHLSL
        }
    }
}

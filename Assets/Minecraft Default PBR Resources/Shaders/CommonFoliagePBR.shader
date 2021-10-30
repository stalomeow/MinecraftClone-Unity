Shader "Minecraft/Common Foliage (PBR)"
{
	Properties
	{
		[HDR] _MainColor("Main Color", Color) = (1, 1, 1, 1)
		[HDR] _HighlightColor("Highlight Color", Color) = (1, 1, 1, 1)
		_AlphaCutoff("Alpha Cutoff", Range(0, 1)) = 0.9
		_BumpScale("Bump Scale", Float) = 1.0
		_TimeScale("Time Scale", Range(0, 2)) = 0.1
		_WindDir("Wind Direction", Vector) = (0, 0.0625, 0, 0)
		_NoiseTex("Noise (RGB)", 2D) = "black" {}
		_NoiseScale("Noise Scale", Range(0, 100)) = 1
		_NoiseMin("Noise Min", Range(0,1)) = 0.5
		_NoiseMax("Noise Max", Range(0,1)) = 1
	}
    SubShader
    {
		HLSLINCLUDE
		#include "Includes/Minecraft/BlockBRDF.hlsl"

		CBUFFER_START(UnityPerMaterial)
			half4 _MainColor;
			half4 _HighlightColor;
			half _AlphaCutoff;
			half _BumpScale;
			float _TimeScale;
			float4 _WindDir;
			TEXTURE2D(_NoiseTex);
			SAMPLER(sampler_NoiseTex);
			float _NoiseScale;
			half _NoiseMin;
			half _NoiseMax;
		CBUFFER_END
		ENDHLSL

		Tags
		{
			"RenderPipeline" = "UniversalPipeline"
			"UniversalMaterialType" = "Lit"
			"RenderType" = "TransparentCutout"
			"Queue" = "AlphaTest"
			"IgnoreProjector" = "True"
			"ShaderModel"="4.5"
		}

        Pass
        {
			Tags { "LightMode" = "UniversalForward" }

			LOD 300
			Cull Back

			HLSLPROGRAM
			#pragma exclude_renderers gles gles3 glcore
            #pragma target 4.5

			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT

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
				float3 blockPositionWS : TEXCOORD8;
				float4 positionCS : SV_POSITION;
			};

			Varyings vert(BlockAttributes input)
			{
				float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
				float2 noiseUV = (positionWS.xz + positionWS.yy) * _NoiseScale + _Time.yy * _TimeScale;
				half noise = SAMPLE_TEXTURE2D_LOD(_NoiseTex, sampler_NoiseTex, noiseUV, 0).r;
				noise = (noise - _NoiseMin) / (_NoiseMax - _NoiseMin);
				input.positionOS += _WindDir * noise;

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
				output.blockPositionWS = input.blockPositionWS;
				output.positionCS = vertexInput.positionCS;
				return output;
			}

			float4 frag(Varyings input) : SV_TARGET
			{
				half4 albedo = SAMPLE_BLOCK_ALBEDO(input.uv, input.texIndices) * _MainColor;
				float3 bitangent = input.tangentWS.w * cross(input.normalWS.xyz, input.tangentWS.xyz);
				float3 normalTS = UnpackNormalScale(SAMPLE_BLOCK_NORMAL(input.uv, input.texIndices), _BumpScale);
				float3 normalWS = TransformTangentToWorld(normalTS, half3x3(input.tangentWS.xyz, bitangent.xyz, input.normalWS.xyz));

				half4 mer = SAMPLE_BLOCK_MER(input.uv, input.texIndices);

				BlockBRDFData data;
				InitializeBlockBRDFData(albedo, mer, input.positionWS, normalWS, input.lights, input.viewDirWS, input.shadowCoord, data);

				half4 col = BlockFragmentPBR(data, albedo.a);
				HighlightBlock(input.blockPositionWS, input.uv, _HighlightColor, col);
				clip(col.a - _AlphaCutoff);
				return col;
			}
            ENDHLSL
        }

		Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            ZWrite On
            ZTest LEqual
            ColorMask 0
			Cull Back

            HLSLPROGRAM
			#pragma exclude_renderers gles gles3 glcore
            #pragma target 4.5

			#include "Includes/Minecraft/ShadowCaster.hlsl"

            #pragma vertex vert
            #pragma fragment frag

            ShadowVaryings vert(BlockAttributes input)
			{
				ShadowVaryings output;
				output.uv = input.uv;
				output.texIndices = input.texIndices;
				output.positionCS = GetShadowPositionHClip(input);
				return output;
			}

			half4 frag(ShadowVaryings input) : SV_TARGET
			{
				half4 albedo = SAMPLE_BLOCK_ALBEDO(input.uv, input.texIndices) * _MainColor;
				clip(albedo.a - _AlphaCutoff);
				return 0;
			}
            ENDHLSL
        }
    }
}

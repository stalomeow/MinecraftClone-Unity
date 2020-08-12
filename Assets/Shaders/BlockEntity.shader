Shader "Minecraft/BlockEntity"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white"{}
		_AlphaTex("Alpha", 2D) = "white"{}
		_Cutoff("Alpha Cutoff", Range(0, 1)) = 0.9
		_Color("Color", Color) = (1, 1, 1, 1)
		_MinLightLevel("Min Light Level", Range(0, 1)) = 0.1
	}
	SubShader
	{
		Tags{"Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout"}
		LOD 100
		Cull Back

		Pass
		{
			CGPROGRAM
			#pragma vertex vertex
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				//float3 normal : NORMAL;
				float3 color : COLOR; // z is light
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				//float3 normal : NORMAL;
				float2 uv : TEXCOORD1;
				fixed light : COLOR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D _AlphaTex;

			fixed _Cutoff;
			fixed4 _Color;
			fixed _MinLightLevel;

			v2f vertex(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				//o.normal = UnityObjectToWorldNormal(v.normal.xyz);
				o.uv = TRANSFORM_TEX(v.color.xy, _MainTex);
				o.light = v.color.z;
				return o;
			}

			fixed4 frag(v2f i) : SV_TARGET
			{
				clip(tex2D(_AlphaTex, i.uv).r - _Cutoff);

				fixed3 color = tex2D(_MainTex, i.uv).rgb;
				color *= _Color.rgb;
				color *= lerp(_MinLightLevel, 1, i.light);

				return fixed4(color, 1);
			}
			ENDCG
		}
	}

	//FallBack "Unlit/Texture"
}

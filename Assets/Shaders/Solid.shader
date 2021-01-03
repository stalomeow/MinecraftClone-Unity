Shader "Minecraft/Solid"
{
	Properties
	{
		_TexArr("Block Textures", 2DArray) = ""{}
		_Cutoff("Alpha Cutoff", Range(0, 1)) = 0.9
		_AmbientColor("Ambient Color", Color) = (0.3632075, 0.6424405, 1, 1)
		_MinLightLevel("Min Light Level", Range(0, 1)) = 0.1
		_RenderRadius("Render Radius", Int) = 192
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
				float4 color : COLOR; // z is texture_index, w is light
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 worldVertex : TEXCOORD0;
				//float3 normal : NORMAL;
				float3 uv : TEXCOORD1;
				fixed light : COLOR;
			};

			UNITY_DECLARE_TEX2DARRAY(_TexArr);
			
			fixed _Cutoff;
			fixed4 _AmbientColor;
			fixed _MinLightLevel;
			int _RenderRadius;

			inline fixed3 easeIn(fixed3 a, fixed3 b, float w)
			{
				return a + (b - a) * w * w * w;//先慢后快
			}

			v2f vertex(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldVertex = mul(unity_ObjectToWorld, v.vertex);
				//o.normal = UnityObjectToWorldNormal(v.normal.xyz);
				o.uv = v.color.xyz;
				o.light = v.color.w;
				return o;
			}

			fixed4 frag(v2f i) : SV_TARGET
			{
				fixed4 tex = UNITY_SAMPLE_TEX2DARRAY(_TexArr, i.uv);
				clip(tex.a - _Cutoff);

				fixed3 color = tex.rgb;
				float dis = distance(i.worldVertex.xz, _WorldSpaceCameraPos.xz);
				color = easeIn(color, _AmbientColor.rgb, saturate(dis / _RenderRadius));
				color *= lerp(_MinLightLevel, 1, i.light);
				
				return fixed4(color, 1);
			}
            ENDCG
        }
    }

	//FallBack "Unlit/Texture"
}

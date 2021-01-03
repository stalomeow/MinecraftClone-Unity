Shader "Minecraft/Skybox"
{
    Properties
    {
		_SunTex("Sun Tex", 2D) = "white"{}
        _SkyColorTop("Sky Color Top", Color) = (0, 0.588235, 1, 1)
        _SkyColorHorizon("Sky Color Horizon", Color) = (0.3632075, 0.6424405, 1, 1)

		[Header(Day Sky Settings)]
		_DayTopColor("Day Sky Color Top", Color) = (0.4,1,1,1)
		_DayBottomColor("Day Sky Color Bottom", Color) = (0,0.8,1,1)

		[Header(Night Sky Settings)]
		_NightTopColor("Night Sky Color Top", Color) = (0,0,0,1)
		_NightBottomColor("Night Sky Color Bottom", Color) = (0,0,0.2,1)
    }
    SubShader
	{
		Pass
		{
			Tags { "Queue" = "Background" "RenderType" = "Background" "PreviewType" = "Skybox" }
			Cull Off ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 worldVertex : COLOR0;
				float3 uv : TEXCOORD0;
				float3 texcoord : TEXCOORD1;
			};

			sampler2D _SunTex;
			fixed4 _SkyColorTop;
			fixed4 _SkyColorHorizon;
			fixed4 _DayBottomColor, _DayTopColor, _NightBottomColor, _NightTopColor;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldVertex = mul(unity_ObjectToWorld, v.vertex);
				o.uv = v.uv;
				o.texcoord = v.vertex.xyz;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float sun = saturate(distance(i.uv, _WorldSpaceLightPos0) / 0.5);
				float sunUV = lerp(0.5, 0, sun);

				fixed4 gradientDay = lerp(_DayBottomColor, _DayTopColor, saturate(i.uv.y));
				fixed4 gradientNight = lerp(_NightBottomColor, _NightTopColor, saturate(i.uv.y));
				fixed4 skyGradients = lerp(gradientNight, gradientDay, saturate(_WorldSpaceLightPos0.y));

				return skyGradients;
			}
			ENDCG
		}
	}
    Fallback "Skybox/Procedural"
}

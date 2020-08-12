Shader "Minecraft/Skybox"
{
    Properties
    {
        _SkyColorTop("Sky Color Top", Color) = (0, 0.588235, 1, 1)
        _SkyColorHorizon("Sky Color Horizon", Color) = (0.3632075, 0.6424405, 1, 1)
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
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 texcoord : TEXCOORD0;
			};

			fixed4 _SkyColorTop;
			fixed4 _SkyColorHorizon;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.vertex.xyz;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// texcoord is in object space
				return lerp(_SkyColorHorizon, _SkyColorTop, saturate(normalize(i.texcoord).y));
			}
			ENDCG
		}
	}
    //Fallback "Skybox/Procedural"
}

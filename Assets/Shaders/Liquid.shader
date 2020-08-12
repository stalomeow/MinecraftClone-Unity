Shader "Minecraft/Liquid"
{
    Properties
    {
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_WindDir("Wind Direction", Vector) = (0, 0.0625, 0, 0)
		_NoiseTex("Noise (RGB)", 2D) = "black" {}
		_NoiseScale("Noise Scale", Range(0, 100)) = 1
		_NoiseMin("Noise Min", Range(0,1)) = 0.5
		_NoiseMax("Noise Max", Range(0,1)) = 1
		_TimeScale("Time Scale", Range(0, 2)) = 0.1
		_Alpha("Alpha", Range(0, 1)) = 0.7

		_AmbientColor("Ambient Color", Color) = (0.3632075, 0.6424405, 1, 1)
		_MinLightLevel("Min Light Level", Range(0, 1)) = 0.1
		_RenderRadius("Render Radius", Int) = 192
    }
    SubShader
    {
		Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 200
		Cull Off
		ZTest Less
		ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 uv : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
				float4 worldVertex : TEXCOORD0;
                float2 uv : TEXCOORD1;
                float light : COLOR;
            };

			sampler2D _MainTex;
            float4 _MainTex_ST;
			sampler2D _NoiseTex;

			fixed4 _Color;
			float4 _WindDir;
			float _NoiseScale;
			float _NoiseMin;
			float _NoiseMax;
			float _TimeScale;
			fixed _Alpha;

			fixed4 _AmbientColor;
			fixed _MinLightLevel;
			int _RenderRadius;

			inline fixed3 easeIn(fixed3 a, fixed3 b, float w)
			{
				return a + (b - a) * w * w * w;//先慢后快
			}

            v2f vert (appdata v)
            {
                v2f o; 

				o.worldVertex = mul(unity_ObjectToWorld, v.vertex);
				float2 uv_NoiseTex = o.worldVertex.xz * _NoiseScale + float2(_Time.y * _TimeScale, _Time.y * _TimeScale);

				float noise = tex2Dlod(_NoiseTex, float4(uv_NoiseTex, 0, 0)).r;
				noise = (noise - _NoiseMin) / (_NoiseMax - _NoiseMin);

				v.vertex += (_WindDir * noise) - 0.01;
				o.vertex = UnityObjectToClipPos(v.vertex);

				o.uv = TRANSFORM_TEX(v.uv.xy, _MainTex);
				o.light = v.uv.z;

                return o;
            }

			fixed4 frag(v2f i) : SV_Target
			{
				fixed3 color = tex2D(_MainTex, i.uv).rgb * _Color.rgb;
				float dis = distance(i.worldVertex.xz, _WorldSpaceCameraPos.xz);
				color = easeIn(color, _AmbientColor.rgb, saturate(dis / _RenderRadius));
				color *= lerp(_MinLightLevel, 1, i.light);

                return fixed4(color.rgb, _Alpha);
            }
            ENDCG
        }
    }
}

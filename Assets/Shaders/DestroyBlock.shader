Shader "Minecraft/DestroyBlock"
{
    Properties
    {
		_DestroyColor("Destroy Color", Color) = (0, 0, 0, 1)
		_DestroyProgress("Destroy Progress", Range(0, 1)) = 0
		_DestroyTex0("Destroy Texture 0", 2D) = "white" {}
		_DestroyTex1("Destroy Texture 1", 2D) = "white" {}
		_DestroyTex2("Destroy Texture 2", 2D) = "white" {}
		_DestroyTex3("Destroy Texture 3", 2D) = "white" {}
		_DestroyTex4("Destroy Texture 4", 2D) = "white" {}
		_DestroyTex5("Destroy Texture 5", 2D) = "white" {}
		_DestroyTex6("Destroy Texture 6", 2D) = "white" {}
		_DestroyTex7("Destroy Texture 7", 2D) = "white" {}
		_DestroyTex8("Destroy Texture 8", 2D) = "white" {}
		_DestroyTex9("Destroy Texture 9", 2D) = "white" {}
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
				float2 uv: TEXCOORD0;
			};

            struct v2f
            {
                float4 vertex : SV_POSITION;
				float2 uv: TEXCOORD0;
            };

			fixed4 _DestroyColor;
			fixed _DestroyProgress;

			sampler2D _DestroyTex0;
			sampler2D _DestroyTex1;
			sampler2D _DestroyTex2;
			sampler2D _DestroyTex3;
			sampler2D _DestroyTex4;
			sampler2D _DestroyTex5;
			sampler2D _DestroyTex6;
			sampler2D _DestroyTex7;
			sampler2D _DestroyTex8;
			sampler2D _DestroyTex9;

			float4 _DestroyTex0_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _DestroyTex0);
                return o;
            }

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 des0 = tex2D(_DestroyTex0, i.uv);
				fixed4 des1 = tex2D(_DestroyTex1, i.uv);
				fixed4 des2 = tex2D(_DestroyTex2, i.uv);
				fixed4 des3 = tex2D(_DestroyTex3, i.uv);
				fixed4 des4 = tex2D(_DestroyTex4, i.uv);
				fixed4 des5 = tex2D(_DestroyTex5, i.uv);
				fixed4 des6 = tex2D(_DestroyTex6, i.uv);
				fixed4 des7 = tex2D(_DestroyTex7, i.uv);
				fixed4 des8 = tex2D(_DestroyTex8, i.uv);
				fixed4 des9 = tex2D(_DestroyTex9, i.uv);

				fixed4 desColor = fixed4(0, 0, 0, 0);
				float progress = _DestroyProgress * 10;

				desColor = lerp(desColor, des0, saturate(progress));
				desColor = lerp(desColor, des1, saturate(progress - 1));
				desColor = lerp(desColor, des2, saturate(progress - 2));
				desColor = lerp(desColor, des3, saturate(progress - 3));
				desColor = lerp(desColor, des4, saturate(progress - 4));
				desColor = lerp(desColor, des5, saturate(progress - 5));
				desColor = lerp(desColor, des6, saturate(progress - 6));
				desColor = lerp(desColor, des7, saturate(progress - 7));
				desColor = lerp(desColor, des8, saturate(progress - 8));
				desColor = lerp(desColor, des9, saturate(progress - 9));

				return desColor * _DestroyColor;
            }
            ENDCG
        }
    }
}

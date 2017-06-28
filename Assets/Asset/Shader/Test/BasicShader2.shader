// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Customize/SimpleVerxShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color)  = (1,1,1,1)

		_DissolveTex("DisTexture", 2D) = "white" {}
		_DissolveValue("DisValue", Range(0,1)) = 1

		_ExtrudeValue("ExtrudeValue", Range(-0.2, 0.2)) = 0

	}
	SubShader
	{
		Tags 
		{ 
			"RenderType"="Transparent"
		}
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work

			#include "UnityCG.cginc"
			sampler2D _MainTex;
			sampler2D _DissolveTex;
			float4 _Color;
			float _DissolveValue;
			float _ExtrudeValue;


			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};


			v2f vert (appdata v)
			{
				v.vertex.xyz += v.normal.xyz * _ExtrudeValue;
						
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
//				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 dis = tex2D(_DissolveTex, i.uv);

				col = fixed4(col.x, col.y, col.z, 1);

				clip( dis.rbg - _DissolveValue);

				return col * _Color;
			}
			ENDCG
		}
	}
}

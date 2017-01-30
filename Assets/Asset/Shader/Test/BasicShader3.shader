Shader "Customize/SimpleVerxShader2"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_SecTex("SecondTexture", 2D) = "white" {}
		_ChangeValue("DisValue", Range(0,1)) = 1
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
			sampler2D _SecTex;
			float _ChangeValue;

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
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
//				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 main = tex2D(_MainTex, i.uv);
				fixed4 sec = tex2D(_SecTex, i.uv);
				float changeValue = sin( _Time.y ) * _ChangeValue;

				fixed4 c = ( ( 1 - changeValue ) * sec + changeValue * main );


				return c;
			}
			ENDCG
		}
	}
}

Shader "Unlit/Terrain"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_HeightMap("HeightMap", 2D) = "black" {}
		_MaxHeight("MaxHeight", float) = 1
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _HeightMap;
			float _MaxHeight;

			v2f vert(appdata v)
			{
				v2f o;
				// TODO:
				//  Use tex2Dlod to get the height map color at the current position.
				//  Use this to offset the vertex in the y-direction.
				float heightOffset = tex2Dlod(_HeightMap, float4(v.uv,0,0));

				v.vertex.y += heightOffset * _MaxHeight;

				o.vertex = UnityObjectToClipPos(v.vertex);
				// manual tiling for now:
				o.uv = v.uv * 4;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}

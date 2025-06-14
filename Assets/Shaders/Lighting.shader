Shader "CustomRenderTexture/Lighting"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
        _AmbientIntensity ("AmbientIntensity", float) = 0.3
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
			#include "UnityLightingCommon.cginc" // NEEDED FOR LightColor0 !

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
			float _AmbientIntensity;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				// TODO: You probably need to pass more information to the fragment shader...
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// TODO: calculate the (1) ambient, (2) diffuse and possibly (3) specular light contribution
				//    for this fragment, and change the fragment color accordingly.

				fixed4 albedo = tex2D(_MainTex, i.uv);

				// ambient
				albedo = albedo * _AmbientIntensity * fixed4(0.8,0.8,1.2,1);


				return albedo;
			}
			ENDCG
		}

		// cast shadows:
		Pass
		{
			Tags{ "LightMode" = "ShadowCaster" }
			CGPROGRAM
			#pragma vertex VSMain
			#pragma fragment PSMain

			float4 VSMain(float4 vertex:POSITION) : SV_POSITION
			{
				return UnityObjectToClipPos(vertex);
			}

			float4 PSMain(float4 vertex:SV_POSITION) : SV_TARGET
			{
				return 0;
			}

			ENDCG
		}
	}
}
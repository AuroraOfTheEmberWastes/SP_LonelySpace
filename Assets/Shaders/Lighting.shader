Shader "CustomRenderTexture/Lighting"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
        _AmbientIntensity ("AmbientIntensity", float) = 0.3
		_DiffuseIntensity ("DiffuseIntensity", float) = 1
		_SpecularIntensity ("SpecularIntensity", float) = 1
		_LightColor ("LightColor", color) = (0.8,0.8,1.2,1)
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
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 normal : TEXCOORD1;
			};

			sampler2D _MainTex;
			float _AmbientIntensity;
			float _DiffuseIntensity;
			float _SpecularIntensity;
			float4 _LightColor;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				// TODO: You probably need to pass more information to the fragment shader...

				o.normal = normalize(mul(unity_ObjectToWorld, float4(v.normal,0)).xyz);

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// TODO: calculate the (1) ambient, (2) diffuse and possibly (3) specular light contribution
				//    for this fragment, and change the fragment color accordingly.

				float4 lightColor = _LightColor; 


				fixed4 albedo = tex2D(_MainTex, i.uv);

				// ambient
				float4 ambient = _AmbientIntensity * lightColor;

				// diffuse

				float diffuseStrength = max(0,dot(i.normal, _WorldSpaceLightPos0.xyz));
				float4 diffuse = diffuseStrength * lightColor * _DiffuseIntensity;


				// specular
				// _WorldSpaceCameraPos
				float4 specular = 0;
				if (diffuseStrength != 0)
				{
					float3 camera = normalize(_WorldSpaceCameraPos);
					float3 reflection = normalize(reflect(-_WorldSpaceLightPos0.xyz, i.normal));
					float specularStrength = max(0,dot(camera, reflection));
					specularStrength = pow(specularStrength,8);
					specular = specularStrength * lightColor * _SpecularIntensity;
				}


				//putting it together
				float4 lighting = ambient + diffuse + specular;
				albedo = albedo * lighting;

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
Shader "Unlit/Billboarding"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
		// Make transparency work:
		Tags
		{ 
			"RenderType"="Transparent"
			"Queue" = "Transparent"
		}
        LOD 100

        Pass
        {
			// Make transparency work:
			Blend SrcAlpha OneMinusSrcAlpha
			//ZWrite Off // Turned on for the balloon cat

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;

				float4x4 MVP = UNITY_MATRIX_MVP;            // Needed to prevent Unity meddling with our shaders!!
                float4x4 modelMatrix = unity_ObjectToWorld;             // grab the transform (if you think about it, object to world just that)
                float xScale = modelMatrix[0][0];                       // it starts counting from the bottom left corner

				// TODO: Experiment with different combinations of space transformations here:
				float4 p = v.vertex;
                p.w = 1;
				o.vertex =
					  mul(MVP, float4(0,p.y,p.z,p.w))                   // y scale is included in here, z doesn't particularly matter but meh
					+ mul(UNITY_MATRIX_P, float4(p.x * xScale,0,0,0));  // here it only scales if you add it in separately           

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);


                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}

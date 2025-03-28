Shader "TESTING"
{
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
        LOD 100
        ZWrite Off Cull Off
        Pass
        {
            Name "ColorBlitPass"

            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            #pragma vertex Vert
            #pragma fragment frag

            TEXTURE2D_X(_CameraOpaqueTexture);
            SAMPLER(sampler_CameraOpaqueTexture);

            float2 _CircleCenter; // TEMPERARY
            float _CircleRadius;   // Radius of the transparent circle
            float _EdgeSmoothness; // Smoothness of the transition at the circle's edge

            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                // Sample the texture color
                float4 color = SAMPLE_TEXTURE2D_X(_CameraOpaqueTexture, sampler_CameraOpaqueTexture, input.texcoord);

                // Calculate distance from the center
                float2 center = float2(0.5, 0.5);
                float distance = length(input.texcoord - center);

                // Calculate alpha using smoothstep for a smooth transition 
                float alpha = 1.0 - smoothstep(_CircleRadius, _CircleRadius + _EdgeSmoothness, distance);

                // Apply the transparency mask
                color.a *= alpha;

                // Apply the intensity effect
                color.rgb *= float3(0, 0, 0);

                return color;
            }

            float GetDistanceFromPoint(float2 position, Varyings input)
            {
                return length(input.texcoord - _CircleCenter);
            }
            ENDHLSL
        }
    }
}




/*



            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            // Properties
            float _CircleRadius;
            float _EdgeSmoothness;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                // Calculate distance from the center of the screen
                float2 center = float2(0.5, 0.5);
                float2 uv = i.uv;
                float distance = length(uv - center);

                // Compute alpha based on distance
                float alpha = 1.0 - smoothstep(_CircleRadius, _CircleRadius + _EdgeSmoothness, distance);

                // Return black color with calculated alpha
                return float4(0, 0, 0, alpha);
            }*/
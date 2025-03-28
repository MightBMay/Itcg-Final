Shader "Custom/ShadowMask"
{
    Properties
    {
        _Pos("Pos", Vector) = (0,0,0,0)
        _CircleRadius("Circle Radius", Float) = 0.3
        _EdgeSmoothness("Edge Smoothness", Float) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
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
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            // Properties
            float4 _Pos;
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
                float2 center = _Pos.xy;
                float2 uv = i.uv;
                float distance = length(uv - center);
                return float4(0,0,0,1);
                if ( _CircleRadius>distance)
                {
                    return float4(0,0,0,1);
                }
                else
                {
                    return float4(1,1,1,1);
                }
                // Compute alpha based on distance
                float alpha = 1.0 - smoothstep(_CircleRadius, _CircleRadius + _EdgeSmoothness, distance);

                // Return black color with calculated alpha
                return float4(0, 0, 0, alpha);
            }
            ENDCG
        }
    }
    FallBack "Unlit/Transparent"
}
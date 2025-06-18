Shader "UI/FullscreenDangerPulse"
{
    Properties
    {
        _Color ("Overlay Color", Color) = (1, 0, 0, 1)
        _VignetteStrength ("Vignette Strength", Float) = 0.5
        _PulseAmount ("Pulse Amount", Float) = 0.2
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
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

            fixed4 _Color;
            float _VignetteStrength;
            float _PulseAmount;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 center = float2(0.5, 0.5);
                float dist = distance(i.uv, center);

                float pulse = 0.5 + 0.5 * sin(_Time.y * 5);
                float vignette = smoothstep(0.4, 0.8, dist) * _VignetteStrength;

                float totalEffect = saturate(pulse * _PulseAmount + vignette);

                // Final color with controlled alpha (visibility)
                float alpha = 0.15; // 15% max opacity
                return fixed4(_Color.rgb, alpha) * totalEffect;
            }
            ENDCG
        }
    }
}

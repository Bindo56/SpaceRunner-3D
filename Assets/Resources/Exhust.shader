Shader "Unlit/Exhust"
{
   Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Glow Color", Color) = (0.2, 0.6, 1, 1)
        _ScrollSpeed ("Scroll Speed", Float) = 1.0
        _GlowStrength ("Glow Strength", Float) = 2.0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Blend SrcAlpha One // Additive glow
        ZWrite Off
        Cull Off
        Lighting Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _Color;
            float _ScrollSpeed;
            float _GlowStrength;

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float2 uvOffset = float2(0, _Time.y * _ScrollSpeed);
                o.uv = v.uv + uvOffset;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 tex = tex2D(_MainTex, i.uv);
                return tex * _Color * _GlowStrength;
            }
            ENDCG
        }
    }
}

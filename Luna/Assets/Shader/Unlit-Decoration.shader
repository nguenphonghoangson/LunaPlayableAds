Shader "Horus/Unlit/Decoration"
{
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, 1)
        _Decor("Decoration", 2D) = "white" {}
        _Mask("Decoration Mask", 2D) = "white" {}
        _MulStrength("Multiply Strength", Range(-10, 10)) = 1.0
        [Enum(Multiply, 1, Blend, 0)] _MulOrBlend("Color Mix Mode", Float) = 1
        _Opacity("Opacity", Range(0,1)) = 1.0
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100

        Pass
        {
            Name "Main"
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            half4 _Color;
            sampler2D _Decor;
            sampler2D _Mask;
            half _MulStrength;
            half _Opacity;

            struct mesh_data
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float2 uv_MainTex : TEXCOORD0;
                float2 uv_Decor : TEXCOORD2;
                float2 uv_Mask : TEXCOORD3;
            };

            float4 _MainTex_ST;
            float4 _Decor_ST;
            float4 _Mask_ST;
            fixed _MulOrBlend;

            Interpolators vert(mesh_data v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv_MainTex = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv_Decor = TRANSFORM_TEX(v.uv, _Decor);
                o.uv_Mask = TRANSFORM_TEX(v.uv, _Mask);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag(Interpolators IN) : SV_Target
            {
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
                c = c * _Color;
                fixed4 decor = tex2D(_Decor, IN.uv_Decor);
                const fixed decor_mask = tex2D(_Mask, IN.uv_Mask).r;
                const half mask = _Opacity * decor_mask * decor.a;

                decor = decor * decor_mask;

                const fixed4 fully_decor = lerp(decor * _MulStrength, c * decor * _MulStrength, _MulOrBlend);
                c = lerp(c, fully_decor, mask);

                UNITY_APPLY_FOG(IN.fogCoord, c);
                return c;
            }
            ENDCG
        }
    }
}
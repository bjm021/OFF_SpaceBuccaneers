Shader "Unlit/TransparentWobble"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _Falloff ("Falloff", Range(0, 100)) = 10
        _Fill ("Fill", Range(0, 1)) = 0.1
        _Speed ("Speed", Float) = 2
        _Frequency ("Frequency", Float) = 10
        _Amplitude ("Amplitude", Float) = 0.1
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent" "Queue"="Transparent"
        }
        Blend One One
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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float3 world_pos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            float4 _Color;
            
            float _Speed;
            float _Frequency;
            float _Amplitude;

            float _Falloff;
            float _Fill;

            v2f vert(appdata v)
            {
                v2f o;
                float wobble = sin(v.vertex.z * _Frequency + _Time.y * _Speed) * _Amplitude;
                float wobble2 = sin(v.vertex.z * _Frequency + _Time.z * _Speed) * _Amplitude;
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.vertex.x += wobble;
                o.vertex.y += wobble2;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = mul(UNITY_MATRIX_M, v.normal);
                o.world_pos = mul(UNITY_MATRIX_M, v.vertex);
                
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);
                col.rgb *= _Color;
                float3 viewDir = i.world_pos - _WorldSpaceCameraPos;
                viewDir = normalize(viewDir);
                i.normal = normalize(i.normal);
                float facing = dot(i.normal, viewDir) * -1;
                float inverseFacing = 1-facing;
                float fresnel = pow(inverseFacing, _Falloff);
                fresnel = lerp(_Fill, 1, fresnel);
                
                return col * fresnel;
            }
            ENDCG
        }
    }
}
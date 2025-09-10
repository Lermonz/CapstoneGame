Shader "Unlit/WeirdWobbleShader5"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
        LOD 100

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
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                //_Color.w *= _CosTime.w;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                //i.uv *= 6;
                //i.uv.y -= _Time.y*0.42;
                //i.uv.y += 0.2*sin(i.uv.y+_Time.y*0.3);
                fixed4 col = tex2D(_MainTex, i.uv);
                float wnoise = noise(float3(i.uv, _Time.y));
                col += wnoise;
                return col*i.color;
            }
            float4 hydranoise(float2 uv, float offset)
            {
                float2 st = uv/_Resolution;
            }
            ENDCG
        }
    }
}

Shader "Unlit/WeirdWobbleShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
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
                i.uv.x += i.uv.y*0.5+i.uv.x*0.5;
                i.uv *= 48;
                i.uv.y += cos(i.uv.x*2.5)+_Time.y*0.025;
                i.uv.x += 0.025+_Time.y*0.015;
                fixed4 col = tex2D(_MainTex, i.uv)*i.color;
                return col;
            }
            ENDCG
        }
    }
}

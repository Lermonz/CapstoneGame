Shader "Unlit/WeirdWobbleShader1"
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
                i.uv *= cos(i.uv.x)*_SinTime.w*0.02+1;
                i.uv.x += dot(_Time.y*0.02+15,i.uv.y)-_CosTime.y*0.013;
                i.uv.y += sin(distance(cos(i.uv.y),i.uv.x))*0.25+_Time.y*0.21;
                fixed4 col = tex2D(_MainTex, i.uv);
                return col*i.color;
            }
            ENDCG
        }
    }
}

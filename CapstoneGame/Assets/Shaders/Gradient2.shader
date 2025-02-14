Shader "Unlit/VertexID"
{
    Properties {
        _Speed ("Speed", Range(0.1,5)) = 1
        _Color1 ("Color 1", Color) = (1,1,1,1)
        _Color2 ("Color 2", Color) = (0,0,0,0)
        _MainTex ("Sprite Texture", 2D) = "white" { }
    }
    SubShader
    {
        Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
        LOD 200
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                fixed4 color: COLOR;
                float2 texcoord : TEXCOORD0;
            };
            
            struct v2f
            {
                float4 color : TEXCOORD0;
                fixed4 mat_color : COLOR;
                float4 vertex : SV_POSITION;
            };
            
            sampler2D _MainTex;
            half _Speed;
            fixed4 _Color1;
            fixed4 _Color2;

            v2f vert (appdata v)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(v.vertex);
                v.texcoord.y += _Time.y*_Speed;
                OUT.color = v.color*lerp(_Color2,_Color1, cos(v.texcoord.y*2.6)*0.18+0.2);
                OUT.color.w = 0;
                return OUT;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return i.color;
            }
            ENDCG

        }
    }
}
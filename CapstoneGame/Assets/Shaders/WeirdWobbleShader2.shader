Shader "Unlit/WeirdWobbleShader2"
{
    Properties {
        _Color1 ("Color 1", Color) = (1,1,1,1)
        _Color2 ("Color 2", Color) = (0,0,0,1)
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
                float4 color: COLOR;
                float2 texcoord : TEXCOORD0;
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                fixed4 color: COLOR;
                float4 vertex : SV_POSITION;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color1;
            fixed4 _Color2;

            v2f vert (appdata v)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(v.vertex);
                OUT.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                OUT.color = v.color;
                return OUT;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                i.uv *= 48;
                i.uv.x -= cos(i.uv.y*2.5)+_Time.y*0.055;
                i.uv.y -= 0.025+_Time.y*0.05;
                fixed4 col = tex2D(_MainTex, i.uv)*i.color;
                return col;
            }
            ENDCG
        }
    }
}

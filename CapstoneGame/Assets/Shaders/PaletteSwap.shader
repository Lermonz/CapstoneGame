Shader "Unlit/PaletteSwap"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" { }
        _Color ("Tint", Color) = (1,1,1,1)
        _Palette ("Palette", 2D) = "" {}
    }
    SubShader
    {
        Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
        LOD 100
        Cull Off

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
                float4 color: COLOR;
                float2 texcoord : TEXCOORD0;
            };
            
            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                fixed4 color: COLOR;
                float4 vertex : SV_POSITION;
            };
            

            sampler2D _MainTex;
            fixed4 _Color;
            float4 _MainTex_ST;
            sampler2D _Palette;
            float4 _Palette_ST;

            v2f vert (appdata v)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(v.vertex);
                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                OUT.color = v.color*_Color;
                return OUT;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.texcoord);
                fixed4 final_color = tex2D(_Palette,float2(col.r+0.2,col.g+0.2));
                final_color.a = col.a;
                return final_color*i.color;
            }
            ENDCG
        }
    }
}

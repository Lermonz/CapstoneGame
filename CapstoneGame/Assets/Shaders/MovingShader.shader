Shader "Unlit/MovingShader"
{
    Properties
    {
        [PerRendererData]  _MainTex ("Sprite Texture", 2D) = "white" { }
        _Color ("Tint", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
        LOD 200

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

            v2f vert (appdata v)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(v.vertex);
                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                OUT.color = v.color*_Color;
                //_Color.w *= _CosTime.w;
                return OUT;
            }

            fixed4 frag (v2f i, UNITY_VPOS_TYPE screenPos : SV_POSITION) : SV_Target
            {
                // sample the texture
                i.texcoord.y -= _Time.y*0.13;
                i.texcoord.x += cos(i.texcoord.y*_CosTime.y)*0.5;
                fixed4 col = tex2D(_MainTex, i.texcoord)*i.color;
                return col;
            }
            ENDCG
        }
    }
}

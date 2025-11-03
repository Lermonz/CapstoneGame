Shader "Custom/MoveSpaceTextureLogo"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _ScaleSize ("Scale Size", Float) = 1
        _XSpeed ("X Speed", Float) = 1
        _YSpeed ("Y Speed", Float) = 1

        _Stencil("Stencil Reference", Float) = 0
        _StencilComp("Stencil Comparison", Float) = 8
        _StencilOp("Stencil Operation", Float) = 0
        _StencilReadMask("Stencil Read Mask", Float) = 255
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _ColorMask("Color Mask", Float) = 15
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Stencil {
                Ref[_Stencil]
                Comp[_StencilComp]
                Pass[_StencilOp]
                ReadMask[_StencilReadMask]
                WriteMask[_StencilWriteMask]
            }
    
            ColorMask[_ColorMask]
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
            float _XSpeed;
            float _YSpeed;
            float _ScaleSize;

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
                i.uv *= _ScaleSize;
                i.uv.x += _Time.y*_XSpeed;
                i.uv.y += _Time.y*_YSpeed;
                fixed4 col = tex2D(_MainTex, i.uv);
                return col*i.color;
            }
            ENDCG
        }
    }
}

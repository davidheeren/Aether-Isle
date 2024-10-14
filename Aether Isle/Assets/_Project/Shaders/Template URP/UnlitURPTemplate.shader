Shader "Custom/UnlitURPTemplate"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        _Tint ("Tint", Color) = (1, 1, 1, 1)

        //[MainTexture] [MainColor] tags for material.MainColor and material.MainTexture
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        //HLSLINCLUDE
        //ENDHLSL // If you want to define variables and structs in the SubShader not the Pass


        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes //appdata, VertexInput
            {
                float4 position : POSITION; // vertex OS
                float2 uv : TEXCOORD0;
            };

            struct Varyings //v2f, VetexOutput
            {
                float4 position : SV_POSITION; //vertex HCS : pixel position
                float2 uv : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
                sampler2D _MainTex;
                float4 _Tint;
            CBUFFER_END

            Varyings vert(Attributes i)
            {
                Varyings o;
                o.position = TransformObjectToHClip(i.position.xyz);
                o.uv = i.uv;
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                half4 col = tex2D(_MainTex, i.uv);
                return col * _Tint * unity_SpriteColor;
            }

            ENDHLSL
        }
    }
}

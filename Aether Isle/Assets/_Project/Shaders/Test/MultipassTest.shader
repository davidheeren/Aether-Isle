Shader "Custom/MultipassTest"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        _Flip ("Flip", Float) = 0
    }

    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Back // Only renders front of object

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
                float _Flip;
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
                if (_Flip > 0.5)
                {
                    if (i.uv.x < 0.6)
                        return float4(1,0,0,1);
                    else
                        discard;
                }
                else
                {
                    if (i.uv.x > 0.4)
                        return float4(0,1,0,1);
                    else
                        discard;
                }

                return float4(0,0,1,1);
            }

            ENDHLSL
        }
    }
}

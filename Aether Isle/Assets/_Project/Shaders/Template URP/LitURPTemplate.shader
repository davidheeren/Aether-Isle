Shader "Custom/LitURPTemplate"
{
    // Source: https://github.com/Unity-Technologies/Graphics/blob/master/Packages/com.unity.render-pipelines.universal/Shaders/2D/Sprite-Lit-Default.shader
    Properties
    {
        [PerRendererData] _MainTex("_MainTex", 2D) = "white" {}
    }

    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/Core2D.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"
        
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/ShapeLightShared.hlsl"
        
        CBUFFER_START(UnityPerMaterial)
            sampler2D _MainTex;
        CBUFFER_END

        struct Attributes
        {
            float3 positionOS : POSITION;
            float2 uv : TEXCOORD0;
            UNITY_SKINNED_VERTEX_INPUTS
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        struct Varyings
        {
            float4  positionCS : SV_POSITION;
            float2  uv : TEXCOORD0;
            half2   lightingUV : TEXCOORD1;
            UNITY_VERTEX_OUTPUT_STEREO
        };

        #if USE_SHAPE_LIGHT_TYPE_0
        SHAPE_LIGHT(0)
        #endif

        #if USE_SHAPE_LIGHT_TYPE_1
        SHAPE_LIGHT(1)
        #endif

        #if USE_SHAPE_LIGHT_TYPE_2
        SHAPE_LIGHT(2)
        #endif

        #if USE_SHAPE_LIGHT_TYPE_3
        SHAPE_LIGHT(3)
        #endif

        #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/CombinedShapeLightShared.hlsl"

        half4 GetLitColor(Varyings i)
        {
            const half4 main = tex2D(_MainTex, i.uv) * unity_SpriteColor;
            SurfaceData2D surfaceData;
            InputData2D inputData;

            InitializeSurfaceData(main.rgb, main.a, surfaceData);
            InitializeInputData(i.uv, i.lightingUV, inputData);

            return CombinedShapeLightShared(surfaceData, inputData);
        }

        ENDHLSL

        Pass
        {
            HLSLPROGRAM
            #pragma vertex CombinedShapeLightVertex
            #pragma fragment CombinedShapeLightFragment

            Varyings CombinedShapeLightVertex(Attributes v)
            {
                Varyings o = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                UNITY_SKINNED_VERTEX_COMPUTE(v);

                v.positionOS = UnityFlipSprite(v.positionOS, unity_SpriteProps.xy);
                o.positionCS = TransformObjectToHClip(v.positionOS);

                o.uv = v.uv;
                o.lightingUV = half2(ComputeScreenPos(o.positionCS / o.positionCS.w).xy);

                return o;
            }

            half4 CombinedShapeLightFragment(Varyings i) : SV_Target
            {
                return GetLitColor(i);
            }
            ENDHLSL
        }
    }
}

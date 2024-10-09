Shader "Custom/Actor"
{
    Properties
    {
        [PerRendererData] _MainTex("_MainTex", 2D) = "white" {}
        _HitFactor ("Hit Factor", Range(0, 1)) = 0
        _HitColor ("Hit Color", Color) = (1, 1, 1, 1)

        _DieFactor ("Die Factor", Range(0, 1)) = 0
        _DieColor ("Die Color", Color) = (1, 0, 0, 1)

        _IceFactor ("Ice Factor", Range(0, 1)) = 0
        _IceColor ("Ice Color", Color) = (0, 0, 1, 1)

        _StencilValue ("Stencil Value", Float) = 15

        _AlphaClip ("Alpha Clip", Float) = 0.5
    }

    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" "LightMode" = "Universal2D"}

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/Core2D.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"
        
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/ShapeLightShared.hlsl"
        
        CBUFFER_START(UnityPerMaterial)
            sampler2D _MainTex;
            float _HitFactor;
            float4 _HitColor;
            float _DieFactor;
            float4 _DieColor;
            float _IceFactor;
            float4 _IceColor;
            float _AlphaClip;
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

        half4 GetLitColor(half4 col, Varyings i)
        {
            SurfaceData2D surfaceData;
            InputData2D inputData;

            InitializeSurfaceData(col.rgb, col.a, surfaceData);
            InitializeInputData(i.uv, i.lightingUV, inputData);

            return CombinedShapeLightShared(surfaceData, inputData);
        }

        half4 GetGrayscale(half4 col) 
        {
            half grayscale = dot(col.rgb, half3(0.3, 0.59, 0.11));
            return half4(grayscale, grayscale, grayscale, col.a);
        }

        ENDHLSL

        Pass
        {
            Stencil
            {
                Ref [_StencilValue]
                Comp Always
                Pass Replace
            }

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
                half4 main = tex2D(_MainTex, i.uv);

                if (main.a < _AlphaClip)
                    discard;


                if (_DieFactor > 0.5)
                {
                    return GetLitColor(_DieColor, i);
                }

                if (_HitFactor > 0.5)
                {
                    return GetLitColor(_HitColor, i);
                }

                if (_IceFactor > 0.5)
                {
                    half4 lit = GetLitColor(main, i);
                    return GetGrayscale(lit) * _IceColor;
                }

                return GetLitColor(main * unity_SpriteColor, i);
            }
            ENDHLSL
        }
    }
}

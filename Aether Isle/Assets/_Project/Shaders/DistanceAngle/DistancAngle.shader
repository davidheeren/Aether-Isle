Shader "Custom/DistanceAngle"
{
    Properties
    {
        _Angle ("Angle", Range(0, 360)) = 90
        _AngleFactor ("Angle Factor", Range(0, 1)) = 0.5
        _ValueIntersection ("Value Intersection", Range(0, 1)) = 0
        _HighContrast ("High Contrast", Range(0, 1)) = 0
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
                float _Angle;
                float _AngleFactor;
                float _ValueIntersection;
                float _HighContrast;
            CBUFFER_END

            Varyings vert(Attributes i)
            {
                Varyings o;
                o.position = TransformObjectToHClip(i.position.xyz);
                o.uv = i.uv;
                return o;
            }

            
            float InverseLerp(float a, float b, float value)
            {
                return (value - a) / (b - a);
            }

            float DeltaAngle(float2 a, float2 b)
            {
                return degrees(acos(dot(normalize(a), normalize(b))));
            }
            
            float DistanceAngleFactor(float2 uv)
            {
                float radius = 0.5;

                float2 distTo = uv - float2(0.5f, 0.5f);
                float dist = clamp(length(distTo), 0, radius);

                float angle = radians(_Angle);
                float2 dir = float2(cos(angle), sin(angle));

                float deltaAngle = DeltaAngle(distTo, dir);
    
                float distFac = InverseLerp(radius, 0, length(distTo)) * 0.5;
                float angleFac = InverseLerp(180.0, 0.0, deltaAngle) * 0.5;

                return (distFac * (1.0 - _AngleFactor) * 2) + (angleFac * _AngleFactor * 2);
            }

            half4 frag(Varyings i) : SV_Target
            {
                float da = DistanceAngleFactor(i.uv);

                if (_ValueIntersection > 0 && abs(da - _ValueIntersection) < 0.01)
                    return half4(1, 0, 0, 1);

                if (_HighContrast > 0.5)
                    da = InverseLerp(_ValueIntersection, 1, da);

                if (da > 1 || da < 0) 
                    return half4(0, 0, 1, 1);

                return half4(da, da, da, 1);
            }

            ENDHLSL
        }
    }
}

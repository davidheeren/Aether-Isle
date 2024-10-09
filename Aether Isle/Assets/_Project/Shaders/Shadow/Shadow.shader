Shader "Custom/Shadow"
{
	Properties
	{
		[PerRendererData] _MainTex ( "Sprite Texture", 2D ) = "white" {}
		_Tint ( "Tint", Color ) = ( 1, 1, 1, 1 )
		_AlphaClip ("Alpha Clip", Float) = 0.5
	}
	 
	SubShader
	{
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Back // Only renders front of object

		Pass
		{
			// Discards pixels whose stencil values are not equal to 5, then sets stencil value to 5
			Stencil
			{
				Ref 5
				Comp NotEqual
				Pass Replace
			}   
	 
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
                sampler2D _MainTex;
                float4 _Tint;
                float _AlphaClip;
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

                 if (col.a < _AlphaClip)
                    discard;

                 return col.a * _Tint * unity_SpriteColor.a;
            }

            ENDHLSL
		}
	}

	Fallback off
}

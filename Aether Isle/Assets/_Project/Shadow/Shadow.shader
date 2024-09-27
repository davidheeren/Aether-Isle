Shader "Custom/Shadow"
{
	Properties
	{
		[PerRendererData] _MainTex ( "Sprite Texture", 2D ) = "white" {}
		_Tint ( "Tint", Color ) = ( 1, 1, 1, 1 )
		_AlphaClip ("AlphaClip", Float) = 0.1
	}
	 
	SubShader
	{
		Tags
		{
			"Queue"="Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "TransparentCutout"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Pass
		{
			// Discards pixels whose stencil values are not equal to 4, then sets stencil value to 4
			Stencil
			{
				Ref 4
				Comp NotEqual
				Pass Replace
			}

			Cull Off
      		Lighting Off
      		ZWrite Off

			Blend SrcAlpha OneMinusSrcAlpha     
	 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			 
			uniform sampler2D _MainTex;

			struct appdata_t
			{
				float4 vertex : POSITION;
				float4 color : COLOR;		// Built-in for vertex color
				float2 texcoord : TEXCOORD0;
			};
			 
			struct v2f
			{
				half4 pos : POSITION;
				half2 uv : TEXCOORD0;
				fixed4 color : COLOR;	// Pass the color to the fragment shader (this will include SpriteRenderer.color)
			};

			fixed4 _Tint;
			float _AlphaClip;

			v2f vert(appdata_t v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				half2 uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
				o.uv = uv;
				o.color = v.color;
				return o;
			}

			half4 frag (v2f i) : COLOR
			{
				half4 color = tex2D(_MainTex, i.uv);

				if (color.a <= _AlphaClip)
					discard;

				color = (1, 1, 1, 1) * color.a * i.color.a;
				//color = color * i.color;

				// Returns texuture color * sprite renderer color * material tint
				return color * _Tint;
			}

			ENDCG
		}
	}

	Fallback off
}

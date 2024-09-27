Shader "Custom/Shadow2"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_RenderTex ("Render Texture", 2D) = "white" {}
		_Tint ("Tint", Color) = (1, 1, 1, 1)
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
			Cull Off
      		Lighting Off
      		ZWrite Off

			Blend SrcAlpha OneMinusSrcAlpha     
	 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			 
			uniform sampler2D _MainTex;
			uniform sampler2D _RenderTex;
			fixed4 _Tint;
			float _AlphaClip;

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

				float alpha = color.a * i.color.a;

				// Returns texuture color * sprite renderer color * material tint
				return (1, 1, 1, alpha) * _Tint;
			}

			ENDCG
		}
	}

	Fallback off
}

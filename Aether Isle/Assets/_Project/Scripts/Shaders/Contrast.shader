Shader "Unlit/Contrast"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LightColor ("LightColor", Color) = (1, 1, 1, 1) 
        _DarkColor ("DarkColor", Color) =  (0, 0, 0, 1)
        _Threshold ("Threshold", float) = 0.5
        _IsContrast ("IsContrast", int) = 0

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _LightColor;
            fixed4 _DarkColor;
            float _Threshold;
            int _IsContrast;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                if (!_IsContrast)
                {
                    //return col;
                }

                float luminance = dot(col.rgb, fixed3(0.299, 0.587, 0.114));
                //float luminance = (col.r + col.g + col.b) / 3.0;

                fixed4 newCol;

                if (luminance > _Threshold)
                {
                    newCol = _DarkColor;
                }
                else
                {
                    newCol = _LightColor;
                }

                newCol.a = col.a;
                  
                return 1 - col;
            }

            ENDCG
        }
    }
}

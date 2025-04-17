Shader "UI/RoundedCornersCSS"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Radius ("Corner Radius (0-0.5)", Range(0,0.5)) = 0.1
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100

        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _Radius;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // Рассчёт расстояния до ближайшего угла (поведение как у border-radius)
                float2 corner = min(uv, 1.0 - uv);
                float dist = min(corner.x, corner.y);

                float radius = _Radius;

                // Если пиксель внутри радиуса — показываем, иначе — обрезаем
                if (corner.x < radius && corner.y < radius)
                {
                    float2 cornerCenter = float2(radius, radius);
                    float2 diff = corner - cornerCenter;
                    if (dot(diff, diff) > radius * radius)
                        discard;
                }

                fixed4 col = tex2D(_MainTex, uv) * _Color;
                return col;
            }
            ENDCG
        }
    }
}
Shader "Hidden/GammaCorrection"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Gamma ("Gamma", Float) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Gamma;

            fixed4 frag(v2f_img i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb = pow(col.rgb, 1.0 / _Gamma); 
                return col;
            }
            ENDCG
        }
    }
}
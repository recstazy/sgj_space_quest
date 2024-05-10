Shader "Unlit/RetrowavePost"
{
    Properties
    {
        [MainTexture]_MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise Tex", 2D) = "gray" {}

        [Space]
        _ChromaticShiftAmt ("Chromatic Shift", Range(0, 1)) = 1
        _ChromaticShiftBias ("Chromatic Shift Bias", Range(0, 0.1)) = 0
        _ChromaticShiftNoise ("Chromatic Shift Noise", Range(0, 0.1)) = 0
        _ChromaticShiftNoiseScale ("Chromatic Shift Noise Scale", Float) = 1
        _ChromaticShiftNoiseSpeed ("Chromatic Shift Noise Speed", Vector) = (1, 0.1, 1, 1)

        [Space]
        _PostHSVAmt ("Post HSV Amount", Range(0, 1)) = 1
        _PostHSV ("Post HSV", Vector) = (1, 1, 1, 1)

        [Space]
        _FilmGrainAmt ("Film Grain", Range(0, 1)) = 0.1
        _FilmGrainScale ("Film Grain Scale", Vector) = (1, 1, 1, 1)
        _FilmGrainSpeed ("Film Grain Speed", Vector) = (1, 0.1, 1, 1)
        _FilmGrainTreshold ("Film Grain Treshold", Range(0, 1)) = 0.5

    }
    SubShader
    {
        ZTest False
        ZWrite Off
        Cull Off

        Pass
        {
            Name "RetrowavePost"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Library/RetrowaveUtils.hlsl"
            #define REFERENCE_ASPECT 0.75 // 3:4

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            sampler2D _NoiseTex;
            half _ChromaticShiftAmt;
            half _ChromaticShiftBias;
            half _ChromaticShiftNoise;
            half _ChromaticShiftNoiseScale;
            half2 _ChromaticShiftNoiseSpeed;

            half3 _PostHSV;
            half _PostHSVAmt;

            half _FilmGrainAmt;
            half2 _FilmGrainScale;
            half2 _FilmGrainSpeed;
            half _FilmGrainTreshold;

            half2 calculateUvAspectCorrection()
            {
                half referenceWidth = _MainTex_TexelSize.w * REFERENCE_ASPECT;
                half xCorrection = referenceWidth/ _MainTex_TexelSize.z;
                return half2(xCorrection, 1);
            }

            half4 frag (v2f i) : SV_Target
            {
                half3 color = 0;
                const half2 uvCorrection = calculateUvAspectCorrection();

                float2 chromaticNoiseUv = i.uv * _ChromaticShiftNoiseScale 
                    + _Time.y * _ChromaticShiftNoiseSpeed * uvCorrection;

                half2 chromaticShiftUvOffset = half2(1, 0) * (_ChromaticShiftBias);

                chromaticShiftUvOffset *= _ChromaticShiftAmt * uvCorrection;
                half4 mainColor = tex2D(_MainTex, i.uv);
                half4 chromaticShiftColorR = tex2D(_MainTex, i.uv + chromaticShiftUvOffset);
                half4 chromaticShiftColorL = tex2D(_MainTex, i.uv - chromaticShiftUvOffset);
                color = half3(chromaticShiftColorR.r, chromaticShiftColorL.g, mainColor.b);

                //half3 colorHSV = RGBToHSV(color);
                //colorHSV *= lerp(1, _PostHSV,_PostHSVAmt);
                //color = HSVToRGB(colorHSV);

                //float2 filmGrainNoiseUv = i.uv * _FilmGrainScale + _Time.y * _FilmGrainSpeed * uvCorrection;
                //half filmGrainNoise = tex2D(_NoiseTex, filmGrainNoiseUv / uvCorrection);
                //filmGrainNoise = saturate(filmGrainNoise - _FilmGrainTreshold) / _FilmGrainTreshold;
                //color = saturate(color + filmGrainNoise * _FilmGrainAmt);

                return half4(color, 1);
            }
            ENDHLSL
        }
    }
}

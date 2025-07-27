Shader "Custom/PixelRadialBlackDissolve"
{
    Properties
    {
        _DissolveAmount ("Dissolve Amount", Range(0, 0.5)) = 0
        _NoiseScale ("Noise Scale", Float) = 10
        _NoiseIntensity ("Noise Intensity", Range(0, 1)) = 0.2
        _PixelSize ("Pixel Size", Int) = 10
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
            #include "Utility/noiseSimplex.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                float3 objectPos : TEXCOORD2;
            };

            float _DissolveAmount;
            float _NoiseScale;
            float _NoiseIntensity;
            int _PixelSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.objectPos = v.vertex.xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Пикселизация координат
                float2 pixelUV = floor(i.uv * _PixelSize) / _PixelSize;
                
                // Радиальное расстояние (на основе пикселизованных UV)
                float2 center = float2(0.5, 0.5);
                float distanceFromCenter = distance(pixelUV, center);
                
                // Генерация шума на основе пикселизованных координат
                float3 noiseCoord = float3(pixelUV * _NoiseScale, _Time.y);
                float noise = snoise(noiseCoord) * 0.5 + 0.5;
                
                // Комбинируем эффекты
                float dissolve = distanceFromCenter + (noise * _NoiseIntensity);
                
                // Применяем растворение с пиксельным эффектом
                clip(dissolve - _DissolveAmount * 2);
                
                // Чёрный цвет
                return fixed4(0, 0, 0, 1);
            }
            ENDCG
        }
    }
}
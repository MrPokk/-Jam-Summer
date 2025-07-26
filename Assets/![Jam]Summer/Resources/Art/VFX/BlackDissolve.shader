Shader "Custom/RadialBlackDissolve"
{
    Properties
    {
        _DissolveAmount ("Dissolve Amount", Range(0, 1)) = 0
        _NoiseScale ("Noise Scale", Float) = 10
        _NoiseIntensity ("Noise Intensity", Range(0, 1)) = 0.2
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.objectPos = v.vertex.xyz; // Позиция в локальном пространстве объекта
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Расстояние от центра объекта (в локальных координатах)
                float distanceFromCenter = length(i.objectPos);
                // Нормализуем расстояние (предполагаем, что объект примерно 1 единицу в размере)
                distanceFromCenter = saturate(distanceFromCenter * 2);
                
                // Генерация шума
                float3 noiseCoord = i.worldPos * _NoiseScale;
                float noise = snoise(noiseCoord) * 0.5 + 0.5;
                
                // Комбинируем радиальное растворение с шумом
                float dissolve = distanceFromCenter + (noise * _NoiseIntensity);
                
                // Основная логика растворения
                clip(dissolve - _DissolveAmount * 2); // Умножаем на 2 для полного диапазона
                
                // Чёрный цвет
                return fixed4(0, 0, 0, 1);
            }
            ENDCG
        }
    }
}
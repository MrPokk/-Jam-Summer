Shader "Sprites/ZeroSaturationOverlay"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _ReplacementColor ("Replacement Color", Color) = (1, 0, 0, 1)
        _Intensity ("Overlay Intensity", Range(0, 1)) = 0.8
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
            "CanUseSpriteAtlas" = "True"
            "PreviewType" = "Plane"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

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
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            fixed4 _ReplacementColor;
            float _Precision;
            int _BlendMode;
            float _Intensity;

            float IsZeroSaturation(fixed3 rgb)
            {
                float minRGB = min(rgb.r, min(rgb.g, rgb.b));
                float maxRGB = max(rgb.r, max(rgb.g, rgb.b));
                return step(maxRGB - minRGB, _Precision);
            }

            // Разные режимы наложения цвета
            fixed3 ApplyBlendMode(fixed3 original, fixed3 replacement, float factor)
            {
                switch(_BlendMode)
                {
                    case 0: // Multiply (Умножение)
                        return lerp(original, original * replacement, factor);
                    
                    case 1: // Overlay (Наложение)
                        return lerp(original, 
                            original < 0.5 ? 2 * original * replacement : 1 - 2 * (1 - original) * (1 - replacement), 
                            factor);
                    
                    case 2: // Screen (Осветление)
                        return lerp(original, 1 - (1 - original) * (1 - replacement), factor);
                    
                    case 3: // Additive (Аддитивный)
                        return lerp(original, original + replacement, factor);
                    
                    case 4: // Color Burn (Затемнение)
                        return lerp(original, 1 - (1 - original) / replacement, factor);
                    
                    default:
                        return original;
                }
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 original = tex2D(_MainTex, i.uv) * i.color;
                if (original.a == 0) return original;

                float isZeroSat = IsZeroSaturation(original.rgb);
                
                fixed3 blendedColor = ApplyBlendMode(
                    original.rgb, 
                    _ReplacementColor.rgb * _Intensity, 
                    isZeroSat
                );

                fixed3 finalColor = lerp(original.rgb, blendedColor, isZeroSat);
                
                return fixed4(finalColor, original.a);
            }
            ENDCG
        }
    }
}
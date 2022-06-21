Shader "Custom/SpriteFillCircle"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _Angle ("Angle", Range(0,180)) = 30
        _NearClip ("Near Clip", Range(0,1)) = 0.1
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex SpriteVert
            #pragma fragment MySpriteFrag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"
            float _Angle;
            float _NearClip;
            fixed GetAngle(fixed2 from, fixed2 to){
                float denominator = sqrt((from.x*from.x + from.y*from.y) * (to.x*to.x + to.y * to.y));
                if (denominator < 0.000001)
                    return 0;

                float dotNum = clamp(dot(from, to) / denominator, -1.0, 1.0);
                return degrees(acos(dotNum));
            }
            fixed4 MySpriteFrag(v2f IN) : SV_Target
            {
                fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
                fixed2 uvCenter = fixed2(0.5, 0.5);
                fixed absAngle = GetAngle(fixed2(0,1), IN.texcoord - uvCenter);
                c.a *= (absAngle <= _Angle) * distance(IN.texcoord, uvCenter)*2 > _NearClip;
                c.rgb *= c.a;
                return c;
            }
        ENDCG
        }
    }
}
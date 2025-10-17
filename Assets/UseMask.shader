Shader "Stencil/UseMask"
{
    Properties
    {
        _ColorA ("Color A", Color) = (1,0,0,1)
        _ColorB ("Color B", Color) = (0,1,0,0.5)
    }

    SubShader
    {
        HLSLINCLUDE
        ENDHLSL

        Tags
        {
            "Queue"="Transparent" "RenderType"="Opaque"
        }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        
        Stencil
        {
            Ref 1
            ReadMask 255
            Comp NotEqual
        }
        
        Pass
        {
            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            #pragma vertex vert
            #pragma fragment frag

            float4 _ColorA;
            
            float4 vert(float4 p:POSITION) : SV_Position {
                return TransformObjectToHClip(p);
            }

            float4 frag() : SV_TARGET {

                return _ColorA;
            }
            
            
            ENDHLSL
        }

    }
}
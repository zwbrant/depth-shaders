Shader "Stencil/MaskWrite"
{
    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque" }
        ColorMask 0
        ZWrite On
        ZTest LEqual

        Stencil
        {
            Ref 1
            Comp Always
            Pass Replace
            ReadMask 254
            WriteMask 254
        }
        
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        ENDHLSL

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            float4 vert(float4 p:POSITION) : SV_Position {
                
                return TransformObjectToHClip(p);

            }
            float4 frag() : SV_Target { return 0; } // color discarded by ColorMask 0
            ENDHLSL
        }
    }
}

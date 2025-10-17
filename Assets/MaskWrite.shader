Shader "Stencil/MaskWrite"
{
    Properties
    {
        _ColorA ("Color A", Color) = (1,0,0,1)
    }
    
    SubShader
    {
        Tags
        {
            "Queue"="Transparent" "RenderType"="Transparent"
        }

//        Stencil
//        {
//            Ref 1
//            WriteMask 255
//        }

        Pass
        {
            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            #pragma vertex vert
            #pragma fragment frag
            
            float4 vert(float4 p:POSITION) : SV_Position
            {
                return TransformObjectToHClip(p);
            }

            float4 _ColorA;
            // color discarded by ColorMask 0
            float4 frag() : SV_Target
            {
                return _ColorA;
            }
            
            ENDHLSL
        }
    }
}
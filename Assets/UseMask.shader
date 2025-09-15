Shader "Stencil/UseMask"
{
    Properties
    {
        _ColorA ("Masked Color", Color) = (1,0,0,1)
        _ColorB ("Global Color", Color) = (0,1,0,0.5)
    }

    SubShader
    {
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        ENDHLSL

        Tags
        {
            "Queue"="Transparent" "RenderType"="Transparent"
        }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        // Pass 1: inside the mask
        Stencil
        {
            Ref 1 Comp Equal Pass Keep ReadMask 254 WriteMask 0
        }
        Pass
        {

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            float4 vert(float4 p:POSITION) : SV_Position { return TransformObjectToHClip(p); }
            float4 _ColorA;
            float4 frag() : SV_Target { return _ColorA; }
            ENDHLSL
        }

        // Pass 2: outside the mask
        Stencil
        {
            Ref 1 Comp Always Pass Keep ReadMask 255 WriteMask 0
        }
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            float4 vert(float4 p:POSITION) : SV_Position { return TransformObjectToHClip(p); }
            float4 _ColorB;
            float4 frag() : SV_Target { return _ColorB; }
            ENDHLSL
        }





    }
}
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;

public class BlurRenderPass : ScriptableRenderPass
{
    
    private Material _material;
    private BlurRendererFeature.BlurSettings _defaultSettings;
    private TextureDesc _blurTextureDescriptor;
    
    private static readonly int horizontalBlurId = Shader.PropertyToID("_HorizontalBlur");
    private static readonly int verticalBlurId = Shader.PropertyToID("_VerticalBlur");
    private const string k_BlurTextureName = "_BlurTexture";
    private const string k_VerticalPassName = "VerticalBlurRenderPass";
    private const string k_HorizontalPassName = "HorizontalBlurRenderPass";

    
    public BlurRenderPass(Material material, BlurRendererFeature.BlurSettings defaultSettings)
    {
        _material = material;
        _defaultSettings = defaultSettings;
    }
    
    public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
    {
        var resourceData = frameData.Get<UniversalResourceData>();
        var cameraData = frameData.Get<UniversalCameraData>();
        
        TextureHandle srcCamColor = resourceData.cameraColor;
        _blurTextureDescriptor = srcCamColor.GetDescriptor(renderGraph);
        _blurTextureDescriptor.name = k_BlurTextureName;
        _blurTextureDescriptor.depthBufferBits = 0;
        
        TextureHandle blurTexture = renderGraph.CreateTexture(_blurTextureDescriptor);
        
        // The following line ensures that the render pass doesn't blit
        // from the back buffer.
        if (resourceData.isActiveTargetBackBuffer)
            return;
        
        // Update the blur settings in the material
        UpdateBlurSettings();
        
        // This check is to avoid an error from the material preview in the scene
        if (!srcCamColor.IsValid() || !blurTexture.IsValid())
            return;
        
        // The AddBlitPass method adds a vertical blur render graph pass that blits from the source texture (camera color in this case) to the destination texture using the first shader pass (the shader pass is defined in the last parameter).
        RenderGraphUtils.BlitMaterialParameters paraVertical = new(srcCamColor, blurTexture, _material, 0);
        renderGraph.AddBlitPass(paraVertical, k_VerticalPassName);
        
        // The AddBlitPass method adds a horizontal blur render graph pass that blits from the texture written by the vertical blur pass to the camera color texture. The method uses the second shader pass.
        RenderGraphUtils.BlitMaterialParameters paraHorizontal = new(blurTexture, srcCamColor, _material, 1);
        renderGraph.AddBlitPass(paraHorizontal, k_HorizontalPassName);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        base.Execute(context, ref renderingData);
        
    }

    private void UpdateBlurSettings()
    {
        if (_material == null) return;

        _material.SetFloat(horizontalBlurId, _defaultSettings.horizontalBlur);
        _material.SetFloat(verticalBlurId, _defaultSettings.verticalBlur);
    }

}

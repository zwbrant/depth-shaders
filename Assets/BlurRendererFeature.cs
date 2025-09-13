using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BlurRendererFeature : ScriptableRendererFeature
{
    [SerializeField] private BlurSettings settings;
    [SerializeField] private Shader shader;
    
    private Material _material;
    private BlurRenderPass _blurRenderPass;
    
    [Serializable]
    public class BlurSettings
    {
        [Range(0,0.4f)] public float horizontalBlur;
        [Range(0,0.4f)] public float verticalBlur;
    }

    
    public override void Create()
    {
        if (shader == null)
        {
            return;
        }
        _material = new Material(shader);
        _blurRenderPass = new BlurRenderPass(_material, settings);

        _blurRenderPass.renderPassEvent = RenderPassEvent.AfterRenderingSkybox;
    }


    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (_blurRenderPass == null)
        { 
            return;
        }                
        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            renderer.EnqueuePass(_blurRenderPass);
        }
    }
    protected override void Dispose(bool disposing)
    {
        if (Application.isPlaying)
        {
            Destroy(_material);
        }
        else
        {
            DestroyImmediate(_material);
        }
    }
    
}

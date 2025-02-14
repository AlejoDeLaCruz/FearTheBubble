using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PixelationPostProcess : ScriptableRendererFeature
{
    [System.Serializable]
    public class PixelationSettings
    {
        public Material material = null;
        [Range(1, 500)] public float pixelDensity = 100;
    }

    public PixelationSettings settings = new PixelationSettings();

    private PixelationRenderPass renderPass;

    public override void Create()
    {
        renderPass = new PixelationRenderPass(settings);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.material != null)
        {
            renderer.EnqueuePass(renderPass);
        }
    }
}

public class PixelationRenderPass : ScriptableRenderPass
{
    private PixelationPostProcess.PixelationSettings settings;
    private RenderTargetIdentifier source;
    private RenderTargetHandle tempTexture;

    public PixelationRenderPass(PixelationPostProcess.PixelationSettings settings)
    {
        this.settings = settings;
        tempTexture.Init("_TemporaryColorTexture");
    }

    public void Setup(RenderTargetIdentifier source)
    {
        this.source = source;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get("Pixelation");

        RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
        cmd.GetTemporaryRT(tempTexture.id, opaqueDesc);

        // Aplicar el shader de pixelación
        cmd.Blit(source, tempTexture.Identifier(), settings.material);
        cmd.Blit(tempTexture.Identifier(), source);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
}
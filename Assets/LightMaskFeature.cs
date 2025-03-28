using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LightMaskFeature : ScriptableRendererFeature
{
    class LightMaskPass : ScriptableRenderPass
    {
        private Material material;
        private RTHandle cameraColorTarget;

        public LightMaskPass(Material mat)
        {
            this.material = mat;
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        }

        public void Setup(RTHandle colorTargetHandle)
        {
            cameraColorTarget = colorTargetHandle;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (material == null) return;

            CommandBuffer cmd = CommandBufferPool.Get("LightMaskPass");

            // Render using the updated cameraColorTargetHandle
            Blit(cmd, cameraColorTarget, cameraColorTarget, material);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    public Material material;
    private LightMaskPass pass;

    public override void Create()
    {
        pass = new LightMaskPass(material);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (material == null) return;

        // Use the updated cameraColorTargetHandle
        pass.Setup(renderer.cameraColorTargetHandle);
        renderer.EnqueuePass(pass);
    }
}

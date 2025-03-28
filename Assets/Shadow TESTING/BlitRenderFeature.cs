using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

internal class BlitRenderFeature : ScriptableRendererFeature
{
    public Shader m_Shader;
    public float m_CircleRadius = 0.2f;
    [Range(0.0f, 0.5f)] public float m_EdgeSmoothness = 0.05f;
    public Vector2 m_LightPosition = new Vector2(0.5f, 0.5f);

    private Material m_Material;
    private BlitPass m_RenderPass;

    public override void Create()
    {
        m_Material = CoreUtils.CreateEngineMaterial(m_Shader);
        m_RenderPass = new BlitPass(m_Material)
        {
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            m_RenderPass.SetParameters( m_CircleRadius, m_EdgeSmoothness, m_LightPosition);
            renderer.EnqueuePass(m_RenderPass);
        }
    }

    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(m_Material);
    }

    private class BlitPass : ScriptableRenderPass
    {
        private Material m_Material;
        private float m_CircleRadius;
        private float m_EdgeSmoothness;
        private Vector2 m_LightPosition;

        public BlitPass(Material material)
        {
            m_Material = material;
        }

        public void SetParameters(float circleRadius, float edgeSmoothness, Vector2 lightPosition)
        {
            m_CircleRadius = circleRadius;
            m_EdgeSmoothness = edgeSmoothness;
            m_LightPosition = lightPosition;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("BlitPass");

            // Set material parameters
            m_Material.SetFloat("_CircleRadius", m_CircleRadius);
            m_Material.SetFloat("_EdgeSmoothness", m_EdgeSmoothness);
            m_Material.SetVector("_CircleCenter", m_LightPosition);

            // Ensure the current render target is valid
            var source = renderingData.cameraData.renderer.cameraColorTargetHandle;

            // Perform the blit operation
            Blit(cmd, source, source, m_Material);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }
}

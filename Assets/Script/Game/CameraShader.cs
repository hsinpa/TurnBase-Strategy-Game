using UnityEngine;

[ExecuteInEditMode]
public class CameraShader : MonoBehaviour {
    public Material EffectMaterial;

    void OnRenderImage(RenderTexture src, RenderTexture dst) {
        if (EffectMaterial != null)
            Graphics.Blit(src, dst, EffectMaterial);
    }
}
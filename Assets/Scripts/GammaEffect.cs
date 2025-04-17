using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class GammaEffect : MonoBehaviour
{
    [Range(0.5f, 2.5f)]
    public float gamma = 1.0f;

    [SerializeField] private Shader gammaShader;
    private Material gammaMaterial;

    void Start()
    {
        if (gammaShader == null)
        {
            gammaShader = Shader.Find("Hidden/GammaCorrection");
        }

        if (gammaShader != null && gammaMaterial == null)
        {
            gammaMaterial = new Material(gammaShader);
            gammaMaterial.hideFlags = HideFlags.HideAndDontSave;
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (gammaMaterial != null)
        {
            gammaMaterial.SetFloat("_Gamma", gamma);
            Graphics.Blit(src, dest, gammaMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }

    public void SetGamma(float value)
    {
        gamma = Mathf.Clamp(value, 0.5f, 2.5f);
    }
}
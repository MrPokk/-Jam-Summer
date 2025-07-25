using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ShaderColorController : MonoBehaviour
{
    [Header("Shader Settings")]
    [SerializeField] private Color _replacementColor = Color.white;

    private SpriteRenderer _spriteRenderer;
    private Material _material;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _material = new Material(_spriteRenderer.sharedMaterial);
        _spriteRenderer.material = _material;

        UpdateShaderProperties();
    }

    public void UpdateShaderProperties()
    {
        _material?.SetColor("_ReplacementColor", _replacementColor);
    }

    public void SetReplacementColor(Color newColor)
    {
        _replacementColor = newColor;
        UpdateShaderProperties();
    }

    private void OnValidate()
    {
        if (_material != null)
        {
            UpdateShaderProperties();
        }
    }
}

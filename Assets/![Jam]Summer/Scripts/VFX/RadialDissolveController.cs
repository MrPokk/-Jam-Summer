using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
public class RadialDissolveController : MonoBehaviour
{
    private static readonly int DissolveAmountID = Shader.PropertyToID("_DissolveAmount");
    private static readonly int NoiseScaleID = Shader.PropertyToID("_NoiseScale");
    private static readonly int NoiseIntensityID = Shader.PropertyToID("_NoiseIntensity");

    [Header("Dissolve Settings")]
    [Range(0f, 1f)] public float dissolveAmount = 0f;
    public float dissolveSpeed = 0.5f;

    [Header("Noise Settings")]
    public float noiseScale = 1f;
    [Range(0f, 1f)] public float noiseIntensity = 0.2f;

    [Header("Animation")]
    public bool playOnStart = false;
    public bool loopAnimation = false;
    public AnimationCurve dissolveCurve = AnimationCurve.Linear(0, 0, 1, 1);

    private Material _material;
    private Renderer _renderer;
    private Coroutine _dissolveCoroutine;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        if (_renderer == null)
        {
            Debug.LogError("No Renderer component found!", this);
            return;
        }

        // Создаем новый материал на основе sharedMaterial для избежания модификации исходного
        _material = new Material(_renderer.sharedMaterial);
        _renderer.material = _material;
    }

    private void Start()
    {
        UpdateShaderProperties();

        if (playOnStart)
        {
            StartDissolveAnimation();
        }
    }

    private void UpdateShaderProperties()
    {
        if (_material == null) return;

        _material.SetFloat(DissolveAmountID, dissolveAmount);
        _material.SetFloat(NoiseScaleID, noiseScale);
        _material.SetFloat(NoiseIntensityID, noiseIntensity);
    }

    public void SetDissolveAmount(float amount)
    {
        dissolveAmount = Mathf.Clamp01(amount);
        UpdateShaderProperties();
    }

    public Coroutine StartDissolveAnimation(float duration = -1f)
    {
        if (_material == null && !InitializeMaterial())
            return null;

        StopCurrentAnimation();

        float animDuration = duration > 0 ? duration : (1f / dissolveSpeed);
        _dissolveCoroutine = StartCoroutine(AnimateDissolve(animDuration));
        return _dissolveCoroutine;
    }

    public void StopDissolveAnimation() => StopCurrentAnimation();

    public void ResetDissolve()
    {
        StopCurrentAnimation();
        dissolveAmount = 0f;
        UpdateShaderProperties();
    }

    public void SetNoiseParameters(float scale, float intensity)
    {
        noiseScale = scale;
        noiseIntensity = Mathf.Clamp01(intensity);
        UpdateShaderProperties();
    }

    private bool InitializeMaterial()
    {
        _renderer = GetComponent<Renderer>();
        if (_renderer == null)
        {
            Debug.LogError("No Renderer component found!", this);
            return false;
        }

        _material = new Material(_renderer.sharedMaterial);
        _renderer.material = _material;
        return true;
    }

    private void StopCurrentAnimation()
    {
        if (_dissolveCoroutine != null)
        {
            StopCoroutine(_dissolveCoroutine);
            _dissolveCoroutine = null;
        }
    }

    private IEnumerator AnimateDissolve(float duration)
    {
        float timer = 0f;
        float startAmount = dissolveAmount;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / duration);

            dissolveAmount = Mathf.Lerp(startAmount, 1f, dissolveCurve.Evaluate(progress));
            UpdateShaderProperties();

            yield return null;
        }

        dissolveAmount = 1f;
        UpdateShaderProperties();

        if (loopAnimation)
        {
            yield return new WaitForSeconds(0.5f);
            StartDissolveAnimation(duration);
        }
        else
        {
            _dissolveCoroutine = null;
        }
    }

    private void OnDestroy()
    {
        StopCurrentAnimation();

        if (_material != null)
        {
            if (Application.isPlaying)
                Destroy(_material);
            else
                DestroyImmediate(_material);
        }
    }
}

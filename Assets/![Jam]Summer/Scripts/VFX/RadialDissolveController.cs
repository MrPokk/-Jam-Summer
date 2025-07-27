using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Renderer))]
public class RadialDissolveController : MonoBehaviour
{
    private static readonly int DissolveAmountID = Shader.PropertyToID("_DissolveAmount");
    private static readonly int NoiseScaleID = Shader.PropertyToID("_NoiseScale");
    private static readonly int NoiseIntensityID = Shader.PropertyToID("_NoiseIntensity");

    private const float MaxDissolveAmount = 0.5f;
    private float dissolveAmount = 0f;
    private float dissolveSpeed = 0.5f;

    [Header("Noise Settings")]
    [SerializeField] private float noiseScale = 1f;
    [SerializeField, Range(0f, 1f)] private float noiseIntensity = 0.2f;

    [Header("Animation Settings")]
    [SerializeField] private AnimationCurve dissolveCurve = AnimationCurve.Linear(0, 0, 1, 1);

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

        _material = new Material(_renderer.sharedMaterial);
        _renderer.material = _material;
    }

    private void Start()
    {
        UpdateShaderProperties();
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
        dissolveAmount = Mathf.Clamp(amount, 0f, MaxDissolveAmount);
        UpdateShaderProperties();
    }

    public Coroutine DissolveAnimation(float duration = -1f, DissolveType dissolveType = DissolveType.None)
    {
        gameObject.SetActive(true);

        if (_material == null && !InitializeMaterial())
            return null;

        StopCurrentAnimation();

        var animDuration = duration > 0 ? duration : (1f / dissolveSpeed);

        if (dissolveType == DissolveType.Increase)
            _dissolveCoroutine = StartCoroutine(AnimateIncrease(animDuration));
        else if (dissolveType == DissolveType.Decrease)
            _dissolveCoroutine = StartCoroutine(AnimateDecrease(animDuration));
        else
            throw new ArgumentException("DissolveType is not valid");

        return _dissolveCoroutine;
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

    private IEnumerator AnimateDecrease(float duration)
    {
        SetDissolveAmount(0);
        UpdateShaderProperties();

        float timer = 0f;
        float startAmount = dissolveAmount;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / duration);

            dissolveAmount = Mathf.Lerp(startAmount, MaxDissolveAmount, dissolveCurve.Evaluate(progress));
            UpdateShaderProperties();

            yield return null;
        }

        SetDissolveAmount(MaxDissolveAmount);
        UpdateShaderProperties();
    }

    private IEnumerator AnimateIncrease(float duration)
    {
        SetDissolveAmount(MaxDissolveAmount);
        UpdateShaderProperties();

        float timer = 0f;
        float startAmount = dissolveAmount;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / duration);

            dissolveAmount = Mathf.Lerp(startAmount, 0f, dissolveCurve.Evaluate(progress));
            UpdateShaderProperties();

            yield return null;
        }

        SetDissolveAmount(0);
        UpdateShaderProperties();
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

public enum DissolveType
{
    None,
    Increase,
    Decrease
}

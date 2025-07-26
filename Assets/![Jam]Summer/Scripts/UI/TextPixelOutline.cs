using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
[DisallowMultipleComponent]
public class TextPixelOutline : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> _textOutlines = new();
    private Dictionary<TMP_Text, (Vector3 positionOffset, Vector3 scaleFactor)> _outlineTransforms = new();

    [SerializeField] private bool _isEditing;

    [Header("MainText")]
    [SerializeField] private string _text;
    [SerializeField] private TMP_Text _mainText;

    [Header("Colors")]
    [SerializeField] private Color _textColor = Color.white;
    [SerializeField] private Color _outlineColor = Color.black;

    private void Start()
    {
        if (_mainText == null)
            throw new Exception("MainText is null");

        _textOutlines.RemoveAll(x => x == null);

        _isEditing = false;

        StoreOutlineTransforms();
    }

    private void OnValidate()
    {
        if (!_mainText)
            return;

        _mainText.color = _textColor;

        SetContent(_text);
        UpdateOutlineColor();
        UpdateOutlineTransforms();
    }
    
    private void Update()
    {
        UpdateOutlineTransforms();
    }

    private void UpdateOutlineTransforms()
    {
        if (_isEditing)
            return;

        if (_mainText == null)
            return;

        foreach (var outline in _textOutlines)
        {
            if (_outlineTransforms.TryGetValue(outline, out var transformData))
            {
                outline.transform.localPosition = _mainText.transform.localPosition + transformData.positionOffset;

                outline.transform.localScale = new Vector3(
                    _mainText.transform.localScale.x * transformData.scaleFactor.x,
                    _mainText.transform.localScale.y * transformData.scaleFactor.y,
                    _mainText.transform.localScale.z * transformData.scaleFactor.z);
            }
        }
    }

    private void StoreOutlineTransforms()
    {
        _outlineTransforms.Clear();
        foreach (var outline in _textOutlines)
        {
            if (outline != null && _mainText != null)
            {
                Vector3 positionOffset = outline.transform.localPosition - _mainText.transform.localPosition;
                Vector3 scaleFactor = new Vector3(
                    outline.transform.localScale.x / _mainText.transform.localScale.x,
                    outline.transform.localScale.y / _mainText.transform.localScale.y,
                    outline.transform.localScale.z / _mainText.transform.localScale.z);

                _outlineTransforms[outline] = (positionOffset, scaleFactor);
            }
        }
    }

    public Transform GetTransform() => _mainText != null ? _mainText.transform : throw new Exception("MainText is null");
    public Vector3 GetPosition() => _mainText != null ? _mainText.transform.localPosition : throw new Exception("MainText is null");
    public Vector3 GetScale() => _mainText != null ? _mainText.transform.localScale : throw new Exception("MainText is null");

    public TextPixelOutline SetContent(string text)
    {
        if (_mainText == null)
            throw new Exception("MainText is null");

        _mainText.text = text;
        foreach (var outline in _textOutlines)
        {
            if (outline != null) outline.text = text;
        }

        return this;
    }

    public TextPixelOutline SetAlpha(float targetAlpha)
    {
        if (_mainText == null)
            throw new Exception("MainText is null");

        _mainText.alpha = targetAlpha;
        foreach (var outline in _textOutlines)
        {
            outline.alpha = targetAlpha;
        }

        return this;
    }

    public TextPixelOutline SetTextColor(Color color)
    {
        _textColor = color;
        if (_mainText != null) _mainText.color = color;
        return this;
    }

    public TextPixelOutline SetOutlineColor(Color color)
    {
        _outlineColor = color;
        UpdateOutlineColor();
        return this;
    }

    public TextPixelOutline SetLocalPosition(Vector3 position)
    {
        if (_mainText == null)
            throw new Exception("MainText is null");

        _mainText.transform.localPosition = position;
        UpdateOutlineTransforms();
        return this;
    }

    public TextPixelOutline SetLocalScale(Vector3 scale)
    {
        if (_mainText == null)
            throw new Exception("MainText is null");

        _mainText.transform.localScale = scale;
        UpdateOutlineTransforms();
        return this;
    }

    public TextPixelOutline SetAll(
        string text,
        Vector3 position,
        Vector3 scale,
        Color textColor,
        Color outlineColor,
        float alpha = 1f)
    {
        return SetTextColor(textColor)
            .SetOutlineColor(outlineColor)
            .SetAlpha(alpha)
            .SetContent(text)
            .SetLocalPosition(position)
            .SetLocalScale(scale);
    }

    private void UpdateOutlineColor()
    {
        foreach (var outline in _textOutlines)
        {
            if (outline != null) outline.color = _outlineColor;
        }
    }

    internal void SetAll(Color textColor, float alpha, string text, Vector3 scale, Vector3 position)
    {
        throw new NotImplementedException();
    }
}

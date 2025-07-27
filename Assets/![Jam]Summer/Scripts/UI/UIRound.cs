using BitterCMS.UnityIntegration;
using TMPro;
using UnityEngine;

public class UIRound : MonoBehaviour
{
    private Root _root;
    private TMP_Text _textRound;
    private int _currentRound = 0;
    private void Start()
    {
        _textRound = GetComponent<TMP_Text>();
        _root = GlobalState.GetRoot<Root>();

        OnRoundChanged();
    }

    public void Update()
    {
        OnRoundChanged();
    }

    private void OnRoundChanged()
    {
        if (_root.RoundCurrent != _currentRound || _currentRound == 0)
        {
            _currentRound = _root.RoundCurrent;
            var roundFromString = _currentRound + 1;
            _textRound.text = "Round: " + roundFromString.ToString();
        }
    }
}

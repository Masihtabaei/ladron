using UnityEngine;
using TMPro;
using System;

public class InteractionUserInterfaceManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _hint;

    [SerializeField]
    private TextMeshProUGUI _dialogue;

    [SerializeField]
    private TextMeshProUGUI _countdownDisplay;

    [SerializeField]
    private TimeManager _timeManager;

    private void Awake()
    {
        _timeManager.TimeUpdated += OnTimeUpdated;
    }
    public void UpdateHint(string message)
    {
        _hint.text = message;
    }

    public void UpdateDialogue(string message)
    {
        _dialogue.text = message;
    }

    private void OnTimeUpdated(TimeSpan newValue)
    {
        int wrappedHours = (int)newValue.TotalHours % 24;
        _countdownDisplay.text = $"{wrappedHours:00}:{newValue.Minutes:00}";
    }
}

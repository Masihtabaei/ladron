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
        _countdownDisplay.text = $"CLOCK\n{wrappedHours:00}:{newValue.Minutes:00}";
    }

    private void OndDeadLineApproaches()
    {
        _countdownDisplay.color = Color.red;
    }
    private void Awake()
    {
        _timeManager.TimeUpdated += OnTimeUpdated;
        _timeManager.DeadLineApproaches += OndDeadLineApproaches;
    }

    private void OnDestroy()
    {
        _timeManager.TimeUpdated -= OnTimeUpdated;
        _timeManager.DeadLineApproaches -= OndDeadLineApproaches;
    }
}

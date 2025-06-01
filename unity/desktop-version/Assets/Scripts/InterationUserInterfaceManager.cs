using UnityEngine;
using TMPro;

public class InteractionUserInterfaceManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _hint;
    
    [SerializeField]
    private TextMeshProUGUI _countdownDisplay;

    [SerializeField]
    private CountdownClock _countdownClock;

    private void Awake()
    {
        _countdownClock.TimeUpdated += OnTimeUpdated;
    }
    public void UpdateHint(string message)
    {
        _hint.text = message;
    }

    private void OnTimeUpdated(float newValue)
    {
        float minutes = Mathf.FloorToInt(newValue / 60);
        float seconds = Mathf.FloorToInt(newValue % 60);
        _countdownDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds); ;
    }
}

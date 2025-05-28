using System;
using UnityEngine;

public class CountdownClock : MonoBehaviour
{
    public event Action TimeOut;
    public event Action<float> TimeUpdated;

    private float _counterInSeconds;

    [SerializeField]
    private int _multiplier = 1;

    [SerializeField]
    private float _counterInMinutes = 5f;

    private void Start()
    {
        _counterInSeconds = (float)TimeSpan.FromMinutes(_counterInMinutes).TotalSeconds;
    }
    private void Update()
    {
        if (_counterInSeconds <= 0)
        {
            TimeOut?.Invoke();
            TimeUpdated?.Invoke(0);
            return;
        }

        _counterInSeconds -= _multiplier * Time.deltaTime;
        TimeUpdated?.Invoke(_counterInSeconds);
    }
}

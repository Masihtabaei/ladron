using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public event Action TimeOut;
    public event Action<TimeSpan> TimeUpdated;

    private TimeSpan _counter;
    private TimeSpan _deadLine;

    [SerializeField]
    private byte _multiplier = 1;

    [SerializeField]
    private byte _startingHour = 22;

    [SerializeField]
    private byte _deadLineHour = 8;

    private void Start()
    {
        _counter = TimeSpan.FromHours(_startingHour);
        _deadLine = TimeSpan.FromHours(_deadLineHour);

        if (_deadLine <= _counter)
        {
            _deadLine = _deadLine.Add(TimeSpan.FromDays(1));
        }
    }
    private void Update()
    {
        var comparisonResult = _deadLine.CompareTo(_counter);
        if (comparisonResult <= 0)
        {
            TimeOut?.Invoke();
            TimeUpdated?.Invoke(_deadLine);
            return;
        }

        _counter = _counter.Add(TimeSpan.FromSeconds(_multiplier * Time.deltaTime));
        TimeUpdated?.Invoke(_counter);
    }
}

using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public UnityAction OnTimerEnd;
    [SerializeField] private float _duration;
    private bool _isRunning = false;
    private float _time = 0f;
    [SerializeField] private bool _loop = false;

    private void Update()
    {
        if (_isRunning)
        {
            _time += Time.deltaTime;
            if (_time >= _duration)
            {
                EndTimer();
                OnTimerEnd?.Invoke();
                if (_loop) StartTimer();
            }
        }
    }

    public void StartTimer()
    {
        _time = 0f;
        _isRunning = true;
    }

    public void EndTimer()
    {
        _isRunning = false;
    }

    public float GetDuration()
    {
        return _duration;
    }

    public float GetRemainingTime()
    {
        if (!_isRunning)
        {
            return 0f;
        }
        return _duration - _time;
    }

    public bool GetIsRunning()
    {
        return _isRunning;
    }
}
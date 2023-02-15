using System;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{
    public static KitchenGameManager Instance { get; private set; }
    public event EventHandler OnStateChanged;
    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    private State _currentState;
    private float _waitingToStartTimer = 1f;
    private float _countdownToStartTimer = 3f;
    private float _gamePlayingTimer;
    private float _gamePlayingTimerMax = 10f;

    public float GetCountdownToStartTimer() => _countdownToStartTimer;

    private void Awake()
    {
        Instance = this;
        _currentState = State.WaitingToStart;
        _gamePlayingTimer = _gamePlayingTimerMax;
    }

    private void Update()
    {
        switch (_currentState)
        {
            case State.WaitingToStart:
                _waitingToStartTimer -= Time.deltaTime;
                if (_waitingToStartTimer < 0f)
                {
                    _currentState = State.CountdownToStart;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.CountdownToStart:
                _countdownToStartTimer -= Time.deltaTime;
                if (_countdownToStartTimer < 0f)
                {
                    _currentState = State.GamePlaying;
                    
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                _gamePlayingTimer -= Time.deltaTime;
                if (_gamePlayingTimer < 0f)
                {
                    _currentState = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
    }

    public bool IsGamePlaying()
    {
        return _currentState == State.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    {
        return _currentState == State.CountdownToStart;
    }

    public bool IsGameOver()
    {
        return _currentState == State.GameOver;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return (_gamePlayingTimer / _gamePlayingTimerMax);
    }
}
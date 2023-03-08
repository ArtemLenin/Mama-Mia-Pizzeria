using System;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{
    public static KitchenGameManager Instance { get; private set; }
    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    private State _currentState;
    private float _countdownToStartTimer = 1f;
    private float _gamePlayingTimer;
    private float _gamePlayingTimerMax = 300f;
    private bool _isGamePause = false;

    public float GetCountdownToStartTimer() => _countdownToStartTimer;

    private void Awake()
    {
        Instance = this;
        _currentState = State.WaitingToStart;
        _gamePlayingTimer = _gamePlayingTimerMax;
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;

        // DEBUG TRIGGER GAME START AUTOMATICALLY
        _currentState = State.CountdownToStart;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (_currentState == State.WaitingToStart)
        {
            _currentState = State.CountdownToStart;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    public void TogglePauseGame()
    {
        _isGamePause = !_isGamePause;
        Time.timeScale = _isGamePause ? 0f : 1f;

        if (_isGamePause) OnGamePaused?.Invoke(this, EventArgs.Empty);
        else OnGameUnpaused?.Invoke(this, EventArgs.Empty);


    }

    private void Update()
    {
        switch (_currentState)
        {
            case State.WaitingToStart:
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
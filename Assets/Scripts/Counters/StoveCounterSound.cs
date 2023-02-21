using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter _stoveCounter;

    private float _warningSoundTimer;
    private AudioSource _audioSource;
    private bool _playWarningSound;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        _stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {

        float burnShowProgressAmount = .5f;
        _playWarningSound = _stoveCounter.IsFried() && e.ProgressNormalized >= burnShowProgressAmount;

    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool condition = e.state == StoveCounter.State.Fried || e.state == StoveCounter.State.Frying;
        if (condition) _audioSource.Play();
        else _audioSource.Pause();
    }

    private void Update()
    {
        if (!_playWarningSound) return;
        _warningSoundTimer -= Time.deltaTime;
        if (_warningSoundTimer < 0)
        {
            float warningSoundTimerMax = .2f;
            _warningSoundTimer = warningSoundTimerMax;

            SoundManager.Instance.PlayWarningSound(_stoveCounter.transform.position);
        }
    }
}
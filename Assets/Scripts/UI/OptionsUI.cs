using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    [SerializeField] private Button _soundEffectButton;
    [SerializeField] private Button _musicButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private TextMeshProUGUI _soundEffectsText;
    [SerializeField] private TextMeshProUGUI _musicText;
    [SerializeField] private GameObject _rebindWindow;
    
    [Header("Player Inputs")]
    [SerializeField] private Button _moveUpButton;
    [SerializeField] private Button _moveDownButton;
    [SerializeField] private Button _moveLeftButton;
    [SerializeField] private Button _moveRightButton;
    [SerializeField] private Button _interactiveButton;
    [SerializeField] private Button _interactiveAlternateButton;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private TextMeshProUGUI _moveUpText;
    [SerializeField] private TextMeshProUGUI _moveDownText;
    [SerializeField] private TextMeshProUGUI _moveLeftText;
    [SerializeField] private TextMeshProUGUI _moveRightText;
    [SerializeField] private TextMeshProUGUI _interactiveText;
    [SerializeField] private TextMeshProUGUI _interactiveAlternateText;
    [SerializeField] private TextMeshProUGUI _pauseText;
    



    private void Awake()
    {
        Instance = this;

        _soundEffectButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolue();
            UpdateVisual();
        });
        _musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        _closeButton.onClick.AddListener(() =>
        {
            Hide();
        });

        _moveUpButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.MoveUp); });
        _moveUpButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.MoveDown); });
        _moveUpButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.MoveLeft); });
        _moveUpButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.MoveRight); });
        _moveUpButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Interact); });
        _moveUpButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.InteractAlternate); });
        _moveUpButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Pause); });
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;
        HideRebindWindow();
        Hide();
        UpdateVisual();
    }

    private void KitchenGameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        _soundEffectsText.text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
        _musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume   () * 10f);

        _moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveUp);
        _moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveDown);
        _moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveLeft);
        _moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveRight);
        _interactiveText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        _interactiveAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        _pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowRebindWindow()
    {
        _rebindWindow.SetActive(true);
    }

    private void HideRebindWindow()
    {
        _rebindWindow.SetActive(false);
    }

    private void RebindBinding(GameInput.Binding binding)
    {
        ShowRebindWindow();
        GameInput.Instance.RebindBinding(binding, () =>
        {
            HideRebindWindow();
            UpdateVisual();
        } );
    }
}
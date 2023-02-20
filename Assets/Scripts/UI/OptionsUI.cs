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
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;
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
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
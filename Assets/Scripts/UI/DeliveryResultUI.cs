using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    private const string POPUP = "PopUp";

    [SerializeField] private Image _background;
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _message;
    [SerializeField] private Color _successColor;
    [SerializeField] private Color _failedColor;
    [SerializeField] private Sprite _successSprite;
    [SerializeField] private Sprite _failureSprite;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed; ;

        gameObject.SetActive(false);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
        _animator.SetTrigger(POPUP);
        _background.color = _failedColor;
        _icon.sprite = _failureSprite;
        _message.text = "Delivery\nFailed";
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
        _animator.SetTrigger(POPUP);
        _background.color = _successColor;
        _icon.sprite = _successSprite;
        _message.text = "Delivery\nSuccess";
    }
}
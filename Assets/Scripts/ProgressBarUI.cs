using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private Image _barImage;
    [SerializeField] private GameObject _hasProgressGameObject;
    
    private IHasProgress _hasProgress;

    private void Start()
    {
        _hasProgress = _hasProgressGameObject.GetComponent<IHasProgress>();
        if (_hasProgress == null) Debug.LogError("Game Object " + _hasProgressGameObject + " doesn't have a component that implements IHasProgress!");

        _hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
        _barImage.fillAmount = 0f;
        Hide();
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        _barImage.fillAmount = e.ProgressNormalized;

        if (e.ProgressNormalized == 0 || e.ProgressNormalized == 1) Hide();
        else Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
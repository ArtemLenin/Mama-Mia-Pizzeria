using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter _baseCounter;
    [SerializeField] private GameObject[] _visualGameObjectArray;

    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == _baseCounter) Show();
        else Hide();
    }

    private void Show()
    {
        foreach (GameObject item in _visualGameObjectArray)
        {
            item.SetActive(true);
        }
    }

    private void Hide()
    {
        foreach (GameObject item in _visualGameObjectArray)
        {
            item.SetActive(false);
        }
    }
}
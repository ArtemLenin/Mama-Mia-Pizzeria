using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TestingNetcodeUI : MonoBehaviour
{
    [SerializeField] private Button _startHostButton;
    [SerializeField] private Button _startClientButton;

    private void Awake()
    {
        _startHostButton.onClick.AddListener(() =>
        {
            print("host");
            NetworkManager.Singleton.StartHost();
            Hide();
        });

        _startClientButton.onClick.AddListener(() =>
        {
            print("client");
            NetworkManager.Singleton.StartClient();
            Hide();
        });
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
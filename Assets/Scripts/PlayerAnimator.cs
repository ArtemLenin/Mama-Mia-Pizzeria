using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";

    [SerializeField] private Animator _animator;
    [SerializeField] private Player _player;

    private void Awake()
    {
        if (!_animator) _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetBool(IS_WALKING, _player.IsWalking());
    }
}

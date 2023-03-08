using Unity.Netcode;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
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
        if (!IsOwner) return;
        _animator.SetBool(IS_WALKING, _player.IsWalking());
    }
}

using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    private PlayerInputActions actions;

    private void Awake()
    {
        actions = new PlayerInputActions();
        actions.Player.Enable();

        actions.Player.Interaction.performed += Interaction_performed;
    }

    private void Interaction_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementNormalizedVector()
    {
        Vector2 inputVector = actions.Player.Move.ReadValue<Vector2>();
        inputVector.Normalize();

        return inputVector;
    }
}
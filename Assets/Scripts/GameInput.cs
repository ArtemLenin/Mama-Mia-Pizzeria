using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternativeAction;
    public event EventHandler OnPauseAction;

    private PlayerInputActions actions;

    private void Awake()
    {
        Instance = this;
        actions = new PlayerInputActions();
        actions.Player.Enable();

        actions.Player.Interaction.performed += Interaction_performed;
        actions.Player.InteractionAlternative.performed += InteractionAlternative_performed; ;
        actions.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy()
    {
        actions.Player.Interaction.performed -= Interaction_performed;
        actions.Player.InteractionAlternative.performed -= InteractionAlternative_performed; ;
        actions.Player.Pause.performed -= Pause_performed;

        actions.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractionAlternative_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternativeAction?.Invoke(this, EventArgs.Empty);
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